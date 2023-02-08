using RL.Env.Envs;
using RL.Env.Spaces;

namespace RL.Env.Wrappers;

public abstract class Wrapper<T> : BaseEnv<T>
    where T : Space
{
    protected readonly BaseEnv<T> baseEnv;

    public Wrapper(BaseEnv<T> baseEnv)
    {
        ArgumentNullException.ThrowIfNull(baseEnv);
        this.baseEnv = baseEnv;
        ActionSpace = baseEnv.ActionSpace;
        ObservationSpace = baseEnv.ObservationSpace;
        RewardRange = baseEnv.RewardRange;
    }

    /// <summary>
    /// Steps through the environment with action.
    /// </summary>
    /// <param name="action"></param>
    /// <returns></returns>
    public override StepResult Step(ndarray action)
        => baseEnv.Step(action);

    /// <summary>
    /// Resets the environment with kwargs.
    /// </summary>
    /// <param name="seed"></param>
    /// <param name="options"></param>
    /// <returns></returns>
    public override ResetResult Reset(uint? seed = null, Dictionary<string, object>? options = null)
        => baseEnv.Reset(seed, options);

    /// <summary>
    /// Renders the environment.
    /// </summary>
    /// <param name="randerMode"></param>
    /// <returns></returns>
    public override ndarray? Render(RanderMode randerMode)
        => baseEnv.Render(randerMode);

    /// <summary>
    /// Closes the environment.
    /// </summary>
    public override void Close()
        => baseEnv.Close();

    public override string ToString()
        => $"<{GetType().Name}>{baseEnv}";
}
