namespace Sharply.PublicSample;

public readonly record struct MasteryParameters(
    double LearnProbability,
    double SlipProbability,
    double GuessProbability)
{
    public void Validate()
    {
        ValidateProbability(LearnProbability, nameof(LearnProbability));
        ValidateProbability(SlipProbability, nameof(SlipProbability));
        ValidateProbability(GuessProbability, nameof(GuessProbability));
        if (SlipProbability + GuessProbability >= 1)
            throw new ArgumentException("Slip and guess probabilities must sum to less than one.");
    }

    private static void ValidateProbability(double value, string name)
    {
        if (value is < 0 or > 1) throw new ArgumentOutOfRangeException(name);
    }
}

public interface IMasteryModel
{
    double Update(double priorMastery, bool correct, MasteryParameters parameters);
}

public sealed class BayesianMasteryModel : IMasteryModel
{
    public double Update(double priorMastery, bool correct, MasteryParameters parameters)
    {
        if (priorMastery is < 0 or > 1)
            throw new ArgumentOutOfRangeException(nameof(priorMastery));
        parameters.Validate();

        var evidence = correct
            ? priorMastery * (1 - parameters.SlipProbability)
            : priorMastery * parameters.SlipProbability;
        var alternative = correct
            ? (1 - priorMastery) * parameters.GuessProbability
            : (1 - priorMastery) * (1 - parameters.GuessProbability);
        var posterior = evidence / (evidence + alternative);

        return posterior + (1 - posterior) * parameters.LearnProbability;
    }
}
