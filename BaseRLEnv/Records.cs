namespace BaseRLEnv;

/// <summary></summary>
/// <param name="Observation"> Observation of the initial state. This will be an element of observation_space (typically a numpy array) and is analogous to the observation returned by step(). </param>
/// <param name="Info"> This dictionary contains auxiliary information complementing observation. It should be analogous to the info returned by step(). </param>
public record ResetResult(ndarray Observation, Dictionary<string, dynamic> Info);
/// <summary></summary>
/// <param name="Observation"> this will be an element of the environment’s observation_space. This may, for instance, be a numpy array containing the positions and velocities of certain objects. </param>
/// <param name="Reward"> The amount of reward returned as a result of taking the action. </param>
/// <param name="Terminated"> whether a terminal state (as defined under the MDP of the task) is reached. In this case further step() calls could return undefined results. </param>
/// <param name="Truncated">
/// whether a truncation condition outside the scope of the MDP is satisfied.
/// Typically a timelimit, but could also be used to indicate agent physically going out of bounds.
/// Can be used to end the episode prematurely before a terminal state is reached.
/// </param>
/// <param name="Info">
/// info contains auxiliary diagnostic information (helpful for debugging, learning, and logging). This might, for instance,
/// contain: metrics that describe the agent’s performance state, variables that are hidden from observations, or individual reward terms that are combined to produce the total reward.
/// It also can contain information that distinguishes truncation and termination, however this is deprecated in favour of returning two booleans, and will be removed in a future version.
/// </param>
public record StepResult(ndarray Observation, double Reward, bool Terminated, bool Truncated, Dictionary<string, dynamic> Info);
