using RL.Env.Utils;
using RL.Env.Utils.Serialization;
using System.Text.Json.Serialization;
using JsonConverter = System.Text.Json.Serialization.JsonConverter;
using JsonConverterAttribute = System.Text.Json.Serialization.JsonConverterAttribute;

namespace RL.Env.Spaces;

/// <summary>
/// A Space is the set of all possible values for an observation or action.
/// </summary>
[JsonDerivedType(typeof(Box), typeDiscriminator: nameof(Box))]
[JsonDerivedType(typeof(Discrete), typeDiscriminator: nameof(Discrete))]
[JsonDerivedType(typeof(MultiBinary), typeDiscriminator: nameof(MultiBinary))]
[JsonDerivedType(typeof(MultiDiscrete), typeDiscriminator: nameof(MultiDiscrete))]
public abstract partial class Space
{
    public shape Shape { get; private init; } = null!;
    [JsonConverter(typeof(DtypeJsonConverter))]
    public dtype Type { get; private init; } = null!;
    [JsonInclude]
    [JsonConverter(typeof(RandomJsonConverter))]
    public np.random NpRandom { get; private set; } = null!;
    public abstract bool IsNpFlattenable { get; }

    public Space(shape shape, dtype type, Union<np.random, uint>? seed = null)
    {
        if (seed is null)
        {
            CheckInitParameter(shape, type);
            Seed(null);
        }
        else
        {
            seed.MatchAction(
                (n) => { CheckInitParameter(shape, type, n); NpRandom = n; },
                (u) => { CheckInitParameter(shape, type); Seed(u); });
        }
        (Shape, Type) = (shape, type);
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
            return Result.Fail(new string[] { array.ToString(), "The sample does not match the shape defined by the condition." });
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

    /// <summary>
    /// Return the number of dimensions a flattened equivalent of this space would have.
    /// </summary>
    /// <returns> The number of dimensions for the flattened spaces </returns>
    public abstract long FlatDim();

    public override string ToString()
        => $"Space Type: {GetType().Name}\nShape: {Shape}\ndType: {Type}";

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

    public static bool operator ==(Space obj1, Space obj2)
        => obj1.Equals(obj2);

    public static bool operator !=(Space obj1, Space obj2)
        => !obj1.Equals(obj2);
}
