using RL.Env.Spaces;
using RL.Env.Utils;

namespace RL.Env.Envs;

public abstract class BaseEnv<T>
    where T : Space
{
    public np.random NpRandom { get { if (npRandom == null) (npRandom, uint _) = Seeding.NpRandom(null); return npRandom; } }

    private np.random npRandom = null!;
    /// <summary>
    /// This attribute gives the format of valid actions.
    /// It is of datatype Space provided by Gym. For example,
    /// if the action space is of type Discrete and gives the value Discrete(2),
    /// this means there are two valid discrete actions: 0 & 1.
    /// </summary>
    public T ActionSpace { get; protected init; } = null!;
    /// <summary>
    /// This attribute gives the format of valid observations.
    /// 
    /// It is of datatype Space provided by Gym. For example,
    /// if the observation space is of type Box and the shape of the object is (4,),
    /// this denotes a valid observation will be an array of 4 numbers.
    /// 
    /// We can check the box bounds as well with attributes.
    /// </summary>
    public T ObservationSpace { get; protected init; } = null!;
    /// <summary>
    /// This attribute is a tuple corresponding to min and max possible rewards. Default range is set to (-inf,+inf).
    /// You can set it if you want a narrower range.
    /// </summary>
    public RewardRange RewardRange { get; protected init; } = new(double.PositiveInfinity, double.NegativeInfinity);

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
    public virtual ResetResult Reset(uint? seed = null, Dictionary<string, object>? options = null)
    {
        if (seed is not null)
            (npRandom, uint _) = Seeding.NpRandom(seed);
        return null!;
    }

    /// <summary>
    /// Run one timestep of the environment’s dynamics.
    ///
    /// When end of episode is reached, you are responsible for calling reset() to reset this environment’s state.Accepts an action and returns either a tuple (observation, reward, terminated, truncated, info).
    /// </summary>
    /// <param name="action"> an action provided by the agent </param>
    /// <returns></returns>
    public abstract StepResult Step(ndarray action);

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
    public abstract ndarray? Render(RanderMode randerMode);

    /// <summary>
    /// Override close in your subclass to perform any necessary cleanup.
    ///
    /// Environments will automatically close() themselves when garbage collected or when the program exits.
    /// </summary>
    public virtual void Close() { }

    public override string ToString()
        => $"<{GetType().Name}>ActionSpace: {ActionSpace}\nObservationSpace: {ObservationSpace}\nRewardRange: {RewardRange}";
}

public enum RanderMode
{
    None = 0,
    Human,
    RGBArray,
    RGBArrayList,
    Ansi
}
