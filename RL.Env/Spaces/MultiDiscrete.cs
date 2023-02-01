using RL.Env.Utils.Serialization;
using System.Text.Json.Serialization;

namespace RL.Env.Spaces;

/// <summary>
/// It is useful to represent game controllers or keyboards where each key can be represented as a discrete action space.
/// 
/// Example::
/// >> d = MultiDiscrete(np.array([[1, 2], [3, 4]]))
/// >> d.sample()
/// array([[0, 0],
///     [2, 3]])
/// </summary>
public class MultiDiscrete : DigitalSpace
{
    [JsonConverter(typeof(NdarrayJsonConverter))]
    public ndarray Nvec { get; init; }

    public MultiDiscrete(ndarray nvec, Union<np.random, uint>? seed = null)
        : base(np.full(nvec.shape, 0, nvec.Dtype), nvec - 1, nvec.shape, nvec.Dtype, seed)
        => Nvec = nvec;

    public MultiDiscrete(ndarray nvec, dtype type, Union<np.random, uint>? seed = null)
        : base(np.full(nvec.shape, 0, type), nvec - 1, nvec.shape, type, seed)
        => Nvec = nvec;

    [Newtonsoft.Json.JsonConstructor]
    [JsonConstructor]
    public MultiDiscrete(ndarray nvec, ndarray low, ndarray high, ndarray boundedBelow, ndarray boundedAbove, shape shape, dtype type, np.random npRandom)
        : base(low, high, boundedBelow, boundedAbove, shape, type, npRandom)
        => Nvec = nvec;

    /// <summary>
    /// Checks whether this space can be flattened to a :class:`spaces.Box`.
    /// </summary>
    public override bool IsNpFlattenable => true;

    public override long FlatDim()
        => Nvec.AsInt64Array().Sum();

    protected override Result CheckType(dtype type)
    {
        if (type == np.UInt8 || type == np.UInt16 || type == np.UInt32 || type == np.UInt64)
            return Result.Ok();
        return Result.Fail("MultiDiscrete only supports uint types.");
    }

    public static bool operator ==(MultiDiscrete obj1, MultiDiscrete obj2)
        => obj1.Equals(obj2);

    public static bool operator !=(MultiDiscrete obj1, MultiDiscrete obj2)
        => !obj1.Equals(obj2);

    public override bool Equals(object? obj)
    {
        if (obj == null || obj is not MultiDiscrete)
            return false;
        MultiDiscrete space = (obj as MultiDiscrete)!;
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
        if (!np.array_equal(Nvec, space.Nvec))
            return false;
        return true;
    }
}
