namespace BaseRLEnvTests.Spaces;

[TestClass]
public class SpaceTests
{
    private MockSpace space = null!;
    [TestInitialize]
    public void Init()
        => space = new(new(2, 3), np.Int32);

    [TestMethod]
    public void TestCheckConditionsIsOK()
    {
        ndarray array = np.array(new int[,] { { 1, 2, 3 }, { 4, 5, 6 } });
        Result result = space.CheckConditions(array);
        Assert.IsTrue(result.IsSuccess);
    }

    [TestMethod]
    public void TestCheckConditionsIsFailedByShape()
    {
        ndarray array = np.array(new int[,] { { 1, 2, 3, 4 }, { 4, 5, 6, 7 } });
        Result result = space.CheckConditions(array);
        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("The sample does not match the shape defined by the condition.", result.Errors[1].Message);
    }

    [TestMethod]
    public void TestCheckConditionsIsFailedByType()
    {
        ndarray array = np.array(new double[,] { { 1, 2, 3 }, { 4, 5, 6 } });
        Result result = space.CheckConditions(array);
        Assert.IsTrue(result.IsFailed);
        Assert.AreEqual("The sample does not match the type defined by the condition.", result.Errors[1].Message);
    }

    [TestMethod]
    public void TestGenerate()
    {
        ndarray array = space.Generate();
        Assert.IsTrue(space.CheckConditions(array).IsSuccess);
        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 3; j++)
                Assert.AreEqual(0, array[i, j]);
    }
}

public class MockSpace : Space
{
    public MockSpace(shape shape, dtype type)
        : base(shape, type) { }

    public override ndarray Sample()
    {
        throw new NotImplementedException();
    }
}
