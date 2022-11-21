using RL.Env.Utils;

namespace RL.Env.Tests.Utils;

[TestClass]
public class SeedingTests
{
    [TestMethod]
    public void TestSeedIsNull()
    {
        (np.random npRandom, uint seed) = Seeding.NpRandom();
        Assert.IsNotNull(npRandom);
        Assert.AreNotEqual<uint>(0, seed);
        var a = npRandom.rand();
        Assert.AreNotEqual(0.5488, a, 0.0001);
    }

    [TestMethod]
    public void TestSeed()
    {
        (np.random npRandom, uint seed) = Seeding.NpRandom(0);
        Assert.IsNotNull(npRandom);
        Assert.AreEqual<uint>(0, seed);
        Assert.AreEqual(0.5488, npRandom.rand(), 0.0001);
    }
}
