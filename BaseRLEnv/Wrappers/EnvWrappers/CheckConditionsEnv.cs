using BaseRLEnv.Spaces;

namespace BaseRLEnv.Wrappers.EnvWrappers;

/// <summary>
/// When you have an error in a strange place, this Wrapper is a good debugging tool. It will check whether the Observation, Action, and Reward meet the defined conditions and types, and throw an exception when an exception is found.
///
///Note: This Wrapper will check the content in each iteration, thus increasing the time complexity.
/// </summary>
/// <typeparam name="T"></typeparam>
public class CheckConditionsEnv<T> : BaseEnv<T>
    where T : Space
{
    private readonly BaseEnv<T> baseEnv;

    public CheckConditionsEnv(BaseEnv<T> baseEnv)
    {
        ArgumentNullException.ThrowIfNull(baseEnv);
        this.baseEnv = baseEnv;
        ActionSpace = baseEnv.ActionSpace;
        ObservationSpace = baseEnv.ObservationSpace;
        RewardRange = baseEnv.RewardRange;
    }

    /// <summary>
    /// Compute the render frames as specified by render_mode attribute during initialization of the environment.
    /// </summary>
    /// <param name="randerMode">
    /// The set of supported modes varies per environment. (And some third-party environments may not support rendering at all.) By convention, if render_mode is:
    /// 
    /// None(default) : no render is computed.
    /// human: render return None.The environment is continuously rendered in the current display or terminal.Usually for human consumption.
    /// rgb_array: return a single frame representing the current state of the environment. A frame is a numpy.ndarray with shape (x, y, 3) representing RGB values for an x-by-y pixel image.
    /// rgb_array_list: return a list of frames representing the states of the environment since the last reset.Each frame is a numpy.ndarray with shape (x, y, 3), as with rgb_array.
    /// ansi: Return a strings (str) or StringIO.StringIO containing a terminal-style text representation for each time step.The text can include newlines and ANSI escape sequences (e.g. for colors).
    /// </param>
    /// <returns></returns>
    public override ndarray? Render(RanderMode randerMode)
        => baseEnv.Render(randerMode);

    /// <summary>
    /// Resets the environment to an initial state and returns the initial observation.
    ///
    /// This method can reset the environment’s random number generator(s) if seed is an integer or if the environment has not yet initialized a random number generator.
    /// If the environment already has a random number generator and reset() is called with seed= None, the RNG should not be reset.
    /// Moreover, reset() should (in the typical use case) be called with an integer seed right after initialization and then never again.
    /// </summary>
    /// <param name="seed"> The seed that is used to initialize the environment’s PRNG.
    /// If the environment does not already have a PRNG and seed=None (the default option) is passed, a seed will be chosen from some source of entropy (e.g. timestamp or /dev/urandom).
    /// However, if the environment already has a PRNG and seed=None is passed, the PRNG will not be reset.
    /// If you pass an integer, the PRNG will be reset even if it already exists.
    /// Usually, you want to pass an integer right after the environment has been initialized and then never again.
    /// Please refer to the minimal example above to see this paradigm in action.
    /// </param>
    /// <param name="options"> Additional information to specify how the environment is reset (optional, depending on the specific environment) </param>
    /// <returns></returns>
    public override ResetResult Reset(uint? seed = null, Dictionary<string, dynamic>? options = null)
    {
        ResetResult result =  baseEnv.Reset(seed, options);
        try
        {
            ObservationSpace.CheckConditions(result.Observation);
        }
        catch (Exception ex)
        {
            throw new Error($"ObservationSpace exception: {ex.Message}");
        }
        return result;
    }

    /// <summary>
    /// Run one timestep of the environment’s dynamics.
    ///
    /// When end of episode is reached, you are responsible for calling reset() to reset this environment’s state.Accepts an action and returns either a tuple (observation, reward, terminated, truncated, info).
    /// </summary>
    /// <param name="action"> an action provided by the agent </param>
    /// <returns></returns>
    public override StepResult Step(ndarray action)
    {
        try
        {
            ActionSpace.CheckConditions(action);
        }
        catch (Exception ex)
        {
            throw new Error($"ActionSpace exception: {ex.Message}");
        }
        StepResult result = baseEnv.Step(action);
        try
        {
            ObservationSpace.CheckConditions(result.Observation);
        }
        catch (Exception ex)
        {
            throw new Error($"ObservationSpace exception: {ex.Message}");
        }
        try
        {
            RewardRange.CheckConditions(result.Reward);
        }
        catch (Exception ex)
        {
            throw new Error($"RewardRange exception: {ex.Message}");
        }
        return result;
    }
}
