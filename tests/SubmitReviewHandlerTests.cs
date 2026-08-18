using Sharply.PublicSample;
using Xunit;

namespace Sharply.PublicSample.Tests;

public sealed class SubmitReviewHandlerTests
{
    [Fact]
    public async Task Loads_calculates_and_persists_the_next_state()
    {
        var now = new DateTimeOffset(2026, 8, 18, 12, 0, 0, TimeSpan.Zero);
        var store = new InMemoryStore(new ReviewState(5, 2, now.AddDays(-2), now));
        var handler = new SubmitReviewHandler(store, new SpacedReviewScheduler());
        var command = new SubmitReviewCommand(Guid.NewGuid(), Guid.NewGuid(), ReviewRating.Good, now);

        var result = await handler.HandleAsync(command);

        Assert.Same(result, store.Saved);
        Assert.True(result.DueAt > now);
    }

    private sealed class InMemoryStore(ReviewState state) : IReviewStateStore
    {
        public ReviewState? Saved { get; private set; }
        public Task<ReviewState> GetAsync(Guid userId, Guid knowledgeComponentId, CancellationToken cancellationToken)
            => Task.FromResult(state);
        public Task SaveAsync(Guid userId, Guid knowledgeComponentId, ReviewState next, CancellationToken cancellationToken)
        {
            Saved = next;
            return Task.CompletedTask;
        }
    }
}
