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
    public readonly ndarray Nvec;

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

    protected override Result CheckType(dtype type)
    {
        if (type == np.UInt8 || type == np.UInt16 || type == np.UInt32 || type == np.UInt64)
            return Result.Ok();
        return Result.Fail("MultiDiscrete only supports uint types.");
    }
}
