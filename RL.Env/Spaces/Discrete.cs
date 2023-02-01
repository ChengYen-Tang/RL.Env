using System.Text.Json.Serialization;

namespace RL.Env.Spaces;

/// <summary>
/// This class represents a finite subset of integers, more specifically a set of the form :math:`\{ a, a+1, \dots, a+n-1 \}`.
/// 
/// Example::
/// 
/// >>> Discrete(2)            # {0, 1}
/// >>> Discrete(3, start=-1)  # {-1, 0, 1}
/// </summary>
public class Discrete : DigitalSpace
{
    private static readonly dtype defaultType = np.Int32;
    [JsonInclude]
    public long N { get; init; }

    public Discrete(long n, long start = 0, Union<np.random, uint>? seed = null)
        : base(np.full(new shape(1), start, defaultType), np.full(new shape(1), n - 1 + start, defaultType), new(1), defaultType, seed)
        => N = n;

    public Discrete(long n, dtype type, long start = 0, Union<np.random, uint>? seed = null)
        : base(np.full(new shape(1), start, type), np.full(new shape(1), n - 1 + start, type), new(1), type, seed)
        => N = n;

    [Newtonsoft.Json.JsonConstructor]
    [JsonConstructor]
    public Discrete(long n, ndarray low, ndarray high, ndarray boundedBelow, ndarray boundedAbove, shape shape, dtype type, np.random npRandom)
        : base(low, high, boundedBelow, boundedAbove, shape, type, npRandom)
        => N = n;

    /// <summary>
    /// Checks whether this space can be flattened to a :class:`spaces.Box`.
    /// </summary>
    public override bool IsNpFlattenable => true;

    public override long FlatDim()
        => N;

    protected override Result CheckType(dtype type)
    {
        if (type == np.Int8 || type == np.Int16 || type == np.Int32 || type == np.Int64 ||
            type == np.UInt8 || type == np.UInt16 || type == np.UInt32 || type == np.UInt64)
            return Result.Ok();
        return Result.Fail("Discrete only supports int and uint types.");
    }

    public static bool operator ==(Discrete obj1, Discrete obj2)
        => obj1.Equals(obj2);

    public static bool operator !=(Discrete obj1, Discrete obj2)
        => !obj1.Equals(obj2);

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not Discrete)
            return false;
        Discrete space = (obj as Discrete)!;
        if (N != space.N)
            return false;
        if (Type != space.Type)
            return false;
        if (Shape != space.Shape)
            return false;
        if (!np.array_equal(High, space.High))
            return false;
        if (!np.array_equal(Low, space.Low))
            return false;
        if (!np.array_equal(BoundedBelow, space.BoundedBelow))
            return false;
        if (!np.array_equal(BoundedAbove, space.BoundedAbove))
            return false;
        return true;
    }
}
