using Sharply.PublicSample;
using Xunit;

namespace Sharply.PublicSample.Tests;

public sealed class BayesianMasteryModelTests
{
    private readonly BayesianMasteryModel _model = new();
    private readonly MasteryParameters _parameters = new(0.1, 0.1, 0.2);

    [Fact]
    public void Correct_answer_increases_mastery()
    {
        var updated = _model.Update(0.4, true, _parameters);
        Assert.True(updated > 0.4);
    }

    [Fact]
    public void Incorrect_answer_reduces_belief_before_learning_transition()
    {
        var updated = _model.Update(0.8, false, _parameters);
        Assert.True(updated < 0.8);
    }

    [Fact]
    public void Rejects_invalid_parameters()
    {
        Assert.Throws<ArgumentException>(() =>
            _model.Update(0.5, true, new MasteryParameters(0.1, 0.6, 0.5)));
    }
}
