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

    public MultiDiscrete(ndarray nvec, uint? seed = null)
        : base(np.full(nvec.shape, 0, nvec.Dtype), nvec - 1, nvec.shape, nvec.Dtype, seed)
        => Nvec = nvec;

    public MultiDiscrete(ndarray nvec, np.random npRandom)
        : base(np.full(nvec.shape, 0, nvec.Dtype), nvec - 1, nvec.shape, nvec.Dtype, npRandom)
        => Nvec = nvec;

    public MultiDiscrete(ndarray nvec, dtype type, uint? seed = null)
        : base(np.full(nvec.shape, 0, type), nvec - 1, nvec.shape, type, seed)
        => Nvec = nvec;

    public MultiDiscrete(ndarray nvec, dtype type, np.random npRandom)
        : base(np.full(nvec.shape, 0, type), nvec - 1, nvec.shape, type, npRandom)
        => Nvec = nvec;

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
}
