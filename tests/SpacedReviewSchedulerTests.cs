using Sharply.PublicSample;
using Xunit;

namespace Sharply.PublicSample.Tests;

public sealed class SpacedReviewSchedulerTests
{
    private static readonly DateTimeOffset Now = new(2026, 8, 18, 12, 0, 0, TimeSpan.Zero);
    private readonly SpacedReviewScheduler _scheduler = new();

    [Fact]
    public void Good_review_increases_stability_and_reduces_difficulty()
    {
        var current = new ReviewState(5, 3, Now.AddDays(-3), Now);
        var next = _scheduler.Next(current, ReviewRating.Good, Now);
        Assert.Equal(4.85, next.Difficulty, 2);
        Assert.Equal(6, next.StabilityDays);
        Assert.Equal(Now.AddDays(6), next.DueAt);
    }

    [Fact]
    public void Again_review_never_schedules_less_than_one_day()
    {
        var current = new ReviewState(5, 1, Now.AddDays(-1), Now);
        var next = _scheduler.Next(current, ReviewRating.Again, Now);
        Assert.Equal(Now.AddDays(1), next.DueAt);
    }

    [Fact]
    public void Rejects_reviews_that_move_backwards_in_time()
    {
        var current = new ReviewState(5, 3, Now, Now.AddDays(3));
        Assert.Throws<ArgumentOutOfRangeException>(() =>
            _scheduler.Next(current, ReviewRating.Good, Now.AddMinutes(-1)));
    }
}
