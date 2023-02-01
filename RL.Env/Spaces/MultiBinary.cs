using System.Text.Json.Serialization;

namespace RL.Env.Spaces;

/// <summary>
/// Elements of this space are binary arrays of a shape that is fixed during construction.
/// 
/// Example Usage::
/// 
/// >>> observation_space = MultiBinary(5)
/// >>> observation_space.sample()
///     array([0, 1, 0, 1, 0], dtype=int8
/// >>> observation_space = MultiBinary([3, 2])
/// >>> observation_space.sample()
///     array([[0, 0],
///         [0, 1],
///         [1, 1]], dtype=int8)
/// </summary>
public class MultiBinary : DigitalSpace
{
    private static readonly dtype defaultType = np.UInt8;
    public long N { get; init; }

    public MultiBinary(Union<shape, int> shape, Union<np.random, uint>? seed = null)
        : this(ConvertShape(shape), defaultType, seed) { }

    public MultiBinary(Union<shape, int> shape, dtype type, Union<np.random, uint>? seed = null)
        : base(np.full(ConvertShape(shape), 0, type), np.full(ConvertShape(shape), 1, type), ConvertShape(shape), type, seed)
        => N = Shape.iDims.Aggregate((total, next) => total * next);

    [Newtonsoft.Json.JsonConstructor]
    [JsonConstructor]
    public MultiBinary(ndarray low, ndarray high, ndarray boundedBelow, ndarray boundedAbove, shape shape, dtype type, np.random npRandom, long n)
        : base(low, high, boundedBelow, boundedAbove, shape, type, npRandom) => N = n;

    /// <summary>
    /// Checks whether this space can be flattened to a :class:`spaces.Box`.
    /// </summary>
    public override bool IsNpFlattenable => true;

    protected override Result CheckType(dtype type)
    {
        if (type == np.Int8 || type == np.Int16 || type == np.Int32 || type == np.Int64 ||
            type == np.UInt8 || type == np.UInt16 || type == np.UInt32 || type == np.UInt64)
            return Result.Ok();
        return Result.Fail("MultiBinary only supports int and uint types.");
    }

    public override long FlatDim()
        => N;

    public static bool operator ==(MultiBinary obj1, MultiBinary obj2)
        => obj1.Equals(obj2);

    public static bool operator !=(MultiBinary obj1, MultiBinary obj2)
        => !obj1.Equals(obj2);

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not MultiBinary)
            return false;
        MultiBinary space = (obj as MultiBinary)!;
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

    public static ndarray Flatten(ndarray obs)
        => obs.flatten();

    public ndarray ToMultiBinaryShape(ndarray actions)
        => actions.reshape(Shape);

    private static shape ConvertShape(Union<shape, int> shape)
        => shape.MatchFunc((s) => s, (i) => new shape(i));
}
