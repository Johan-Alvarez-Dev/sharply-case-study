namespace Sharply.PublicSample;

public sealed record SubmitReviewCommand(
    Guid UserId,
    Guid KnowledgeComponentId,
    ReviewRating Rating,
    DateTimeOffset ReviewedAt);

public interface IReviewStateStore
{
    Task<ReviewState> GetAsync(Guid userId, Guid knowledgeComponentId, CancellationToken cancellationToken);
    Task SaveAsync(Guid userId, Guid knowledgeComponentId, ReviewState state, CancellationToken cancellationToken);
}

public sealed class SubmitReviewHandler(
    IReviewStateStore store,
    IReviewScheduler scheduler)
{
    public async Task<ReviewState> HandleAsync(
        SubmitReviewCommand command,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(command);
        if (command.UserId == Guid.Empty || command.KnowledgeComponentId == Guid.Empty)
            throw new ArgumentException("User and knowledge-component identifiers are required.");

        var current = await store.GetAsync(
            command.UserId, command.KnowledgeComponentId, cancellationToken);
        var next = scheduler.Schedule(current, command.Rating, command.ReviewedAt);
        await store.SaveAsync(
            command.UserId, command.KnowledgeComponentId, next, cancellationToken);
        return next;
    }
}
