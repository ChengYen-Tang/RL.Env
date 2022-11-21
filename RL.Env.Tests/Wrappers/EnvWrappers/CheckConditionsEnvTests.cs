using RL.Env.Envs;
using RL.Env.Wrappers.EnvWrappers;

namespace RL.Env.Tests.Wrappers.EnvWrappers;

[TestClass]
public class CheckConditionsEnvTests
{
    private class MockEnv : BaseEnv<DigitalSpace>
    {
        public Func<uint?, Dictionary<string, object>?, ResetResult> ResetCallback { get; set; } = null!;
        public Func<ndarray, StepResult> StepCallback { get; set; } = null!;

        public MockEnv()
        {
            ActionSpace = new Discrete(3, -1);
            ObservationSpace = new Box(2, 6, new(2, 3), np.Float32);
            RewardRange = new(5, 2);
        }

        public override ndarray? Render(RanderMode randerMode)
            => throw new NotImplementedException();

        public override ResetResult Reset(uint? seed = null, Dictionary<string, object>? options = null)
            => ResetCallback(seed, options);

        public override StepResult Step(ndarray action)
            => StepCallback(action);
    }

    [TestMethod]
    public void TestAllStepCorrect()
    {
        MockEnv mockEnv = new();
        mockEnv.ResetCallback = (uint? _, Dictionary<string, object>? _) => new(mockEnv.ObservationSpace.Sample(), new());
        mockEnv.StepCallback = (ndarray _) => new(mockEnv.ObservationSpace.Sample(), 5, false, false, new());
        CheckConditionsEnv<DigitalSpace> env = new(mockEnv);
        env.Reset();
        for (int i = 0; i < 100; i++)
        {
            var action = env.ActionSpace.Sample();
            env.Step(action);
        }
    }

    [ExpectedException(typeof(Error), "RewardRange exception: Reward must be >= 2, and reward must be <= 5.")]
    [TestMethod]
    public void TestRewardError()
    {
        MockEnv mockEnv = new();
        mockEnv.ResetCallback = (uint? _, Dictionary<string, object>? _) => new(mockEnv.ObservationSpace.Sample(), new());
        mockEnv.StepCallback = (ndarray _) => new(mockEnv.ObservationSpace.Sample(), 6, false, false, new());
        CheckConditionsEnv<DigitalSpace> env = new(mockEnv);
        env.Reset();
        for (int i = 0; i < 100; i++)
        {
            var action = env.ActionSpace.Sample();
            env.Step(action);
        }
    }

    [ExpectedException(typeof(Error))]
    [TestMethod]
    public void TestResetObservationError()
    {
        MockEnv mockEnv = new();
        mockEnv.ResetCallback = (uint? _, Dictionary<string, object>? _) => new(new(), new());
        mockEnv.StepCallback = (ndarray _) => new(mockEnv.ObservationSpace.Sample(), 5, false, false, new());
        CheckConditionsEnv<DigitalSpace> env = new(mockEnv);
        env.Reset();
    }

    [ExpectedException(typeof(Error))]
    [TestMethod]
    public void TestStepObservationError()
    {
        MockEnv mockEnv = new();
        mockEnv.ResetCallback = (uint? _, Dictionary<string, object>? _) => new(mockEnv.ObservationSpace.Sample(), new());
        mockEnv.StepCallback = (ndarray _) => new(new(), 5, false, false, new());
        CheckConditionsEnv<DigitalSpace> env = new(mockEnv);
        env.Reset();
        for (int i = 0; i < 100; i++)
        {
            var action = env.ActionSpace.Sample();
            env.Step(action);
        }
    }
}
