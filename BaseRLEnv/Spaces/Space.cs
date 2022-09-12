using BaseRLEnv.Utils;

namespace BaseRLEnv.Spaces;

/// <summary>
/// A Space is the set of all possible values for an observation or action.
/// </summary>
public abstract class Space
{
    public shape Shape { get; private init; } = null!;
    public dtype Type { get; private init; } = null!;
    public np.random NpRandom { get; private set; } = null!;

    public Space(shape shape, dtype type, np.random npRandom)
    {
        CheckInitParameter(shape, type, npRandom);
        (Shape, Type, NpRandom) = (shape, type, npRandom);
    }

    public Space(shape shape, dtype type, uint? seed = null)
    {
        CheckInitParameter(shape, type);
        (Shape, Type) = (shape, type);
        Seed(seed);
    }

    /// <summary>
    /// Returns a sample from the space.
    /// </summary>
    /// <returns></returns>
    public abstract ndarray Sample();

    /// <summary>
    /// Generates an ndarray whose shape and type are consistent with the space definition.
    /// The default content is 0.
    /// </summary>
    /// <returns></returns>
    public virtual ndarray Generate()
        => np.zeros(Shape, Type);

    /// <summary>
    /// Check if the sample is valid.
    /// </summary>
    /// <param name="array"> sample </param>
    /// <returns></returns>
    public virtual Result CheckConditions(ndarray array)
    {
        if (array.shape != Shape)
            return Result.Fail(new string[] { array.ToString() , "The sample does not match the shape defined by the condition." });
        if (array.Dtype != Type)
            return Result.Fail(new string[] { array.ToString(), "The sample does not match the type defined by the condition." });
        return Result.Ok();
    }

    /// <summary>
    /// Seed the PRNG of this space and possibly the PRNGs of subspaces.
    /// </summary>
    /// <param name="seed"></param>
    /// <returns></returns>
    public uint Seed(uint? seed = null)
    {
        (NpRandom, uint rndSeed) = Seeding.NpRandom(seed);
        return rndSeed;
    }

    private static void CheckInitParameter(shape shape, dtype type)
    {
        ArgumentNullException.ThrowIfNull(shape);
        ArgumentNullException.ThrowIfNull(type);
    }

    private static void CheckInitParameter(shape shape, dtype type, np.random npRandom)
    {
        CheckInitParameter(shape, type);
        ArgumentNullException.ThrowIfNull(npRandom);
    }
}
