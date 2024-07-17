using BeyondNet.Ddd.ValueObjects;

namespace BeyondNet.Ddd.Test
{
    [TestClass]
    public class PriorityTest
    {

        [TestMethod]
        public void PriorityAddTest()
        {
            Priority priority = 1;
            priority += 1;
            Assert.AreEqual(2, priority.Value);
        }

        [TestMethod]
        public void PrioritySubtractTest()
        {
            Priority priority = 1;
            priority -= 1;
            Assert.AreEqual(0, priority.Value);
        }

        [TestMethod]
        public void PriorityIncrementTest()
        {
            Priority priority = 1;
            priority++;
            Assert.AreEqual(2, priority.Value);
        }

        [TestMethod]
        public void PriorityDecrementTest()
        {
            Priority priority = 1;
            priority--;
            Assert.AreEqual(0, priority.Value);
        }

        [TestMethod]
        public void PriorityEqualityTest()
        {
            Priority priority1 = 1;
            Priority priority2 = 1;
            Assert.AreEqual(priority1, priority2);
        }
    }
}
