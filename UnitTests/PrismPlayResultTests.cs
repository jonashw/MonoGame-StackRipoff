using Microsoft.Xna.Framework;
using MonoGame_StackRipoff;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class PrismPlayResultTests
    {
        [Test]
        public void Perfect()
        {
            var r1 = RectangularPrismFactory.MakeStandard(new Vector3());
            var r2 = RectangularPrismFactory.MakeStandard(new Vector3());
            var result = Stack.TryPlace(r1, r2);
            assertResultType(result, ResultType.Perfect);
        }

        [Test]
        public void TotalMiss()
        {
            var r1 = RectangularPrismFactory.Make(new Size3(1,1,1), new Vector3(-1,0,0), Color.White);
            var r2 = RectangularPrismFactory.Make(new Size3(1,1,1), new Vector3(0,0,0), Color.White);
            var result = Stack.TryPlace(r1, r2);
            assertResultType(result, ResultType.TotalMiss);
        }

        private static void assertResultType(PrismPlayResult result, ResultType expectedType)
        {
            var type = result.Match(
                perfect => ResultType.Perfect,
                totalMiss => ResultType.TotalMiss,
                mixed => ResultType.Mixed);

            Assert.That(type,Is.EqualTo(expectedType));
        }
    }

    public enum ResultType
    {
        Perfect,
        TotalMiss,
        Mixed
    }
}