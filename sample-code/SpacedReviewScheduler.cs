namespace Sharply.PublicSample;

public enum ReviewRating { Again, Hard, Good, Easy }

public sealed record ReviewState(
    double Difficulty,
    double StabilityDays,
    DateTimeOffset LastReviewedAt,
    DateTimeOffset DueAt);

public interface IReviewScheduler
{
    ReviewState Schedule(ReviewState current, ReviewRating rating, DateTimeOffset reviewedAt);
}

public sealed class SpacedReviewScheduler : IReviewScheduler
{
    public ReviewState Schedule(ReviewState current, ReviewRating rating, DateTimeOffset reviewedAt)
    {
        ArgumentNullException.ThrowIfNull(current);
        if (current.Difficulty is < 1 or > 10)
            throw new ArgumentOutOfRangeException(nameof(current), "Difficulty must be between 1 and 10.");
        if (current.StabilityDays <= 0)
            throw new ArgumentOutOfRangeException(nameof(current), "Stability must be positive.");
        if (reviewedAt < current.LastReviewedAt)
            throw new ArgumentOutOfRangeException(nameof(reviewedAt), "Reviews cannot move backwards in time.");

        var difficultyDelta = rating switch
        {
            ReviewRating.Again => 1.0,
            ReviewRating.Hard => 0.35,
            ReviewRating.Good => -0.15,
            ReviewRating.Easy => -0.5,
            _ => throw new ArgumentOutOfRangeException(nameof(rating))
        };

        var stabilityFactor = rating switch
        {
            ReviewRating.Again => 0.25,
            ReviewRating.Hard => 1.2,
            ReviewRating.Good => 2.0,
            ReviewRating.Easy => 3.0,
            _ => throw new ArgumentOutOfRangeException(nameof(rating))
        };

        var difficulty = Math.Clamp(current.Difficulty + difficultyDelta, 1, 10);
        var stability = Math.Max(0.25, current.StabilityDays * stabilityFactor);
        var intervalDays = Math.Max(1, (int)Math.Round(stability, MidpointRounding.AwayFromZero));

        return new ReviewState(difficulty, stability, reviewedAt, reviewedAt.AddDays(intervalDays));
    }
}
