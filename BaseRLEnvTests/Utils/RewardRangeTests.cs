using BaseRLEnv.Utils;
using Error = BaseRLEnv.Error;

namespace BaseRLEnvTests.Utils;

[TestClass]
public class RewardRangeTests
{
    [TestMethod]
    public void TestInit()
    {
        RewardRange rewardRange = new(1, 0);
        Assert.AreEqual(0, rewardRange.Min);
        Assert.AreEqual(1, rewardRange.Max);
    }

    [DataRow(1d, 1d)]
    [DataRow(0d, 1d)]
    [ExpectedException(typeof(Error), "Min must be less than Max.")]
    [TestMethod]
    public void TestInitError(double max, double min)
    {
        RewardRange _ = new(max, min);
    }

    [DataRow(100)]
    [DataRow(5)]
    [DataRow(0)]
    [TestMethod]
    public void TestCheckConditions(double reward)
    {
        RewardRange rewardRange = new(100, 0);
        rewardRange.CheckConditions(reward);
    }

    [DataRow(101)]
    [DataRow(-1)]
    [ExpectedException(typeof(Error), "Reward must be >=Min, and reward must be <=Max.")]
    [TestMethod]
    public void TestCheckConditionsError(double reward)
    {
        RewardRange rewardRange = new(100, 0);
        rewardRange.CheckConditions(reward);
    }
}
