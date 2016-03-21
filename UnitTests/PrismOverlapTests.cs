using Microsoft.Xna.Framework;
using MonoGame_StackRipoff;
using NUnit.Framework;

namespace UnitTests
{
    [TestFixture]
    public class PrismOverlapTests
    {
        [Test]
        public void Perfect()
        {
            var r1 = RectangularPrismFactory.MakeStandard(new Vector3());
            var r2 = RectangularPrismFactory.MakeStandard(new Vector3());
            var result = r1.OverlapWith(r2, PrismBounceAxis.X);
            assertResultType(result, ResultType.Perfect);
        }

        [Test]
        public void PerfectWithinToleranceRight()
        {
            var r1 = RectangularPrismFactory.MakeStandard(new Vector3(RectangularPrism.PerfectPlayTolerance,0,0));
            var r2 = RectangularPrismFactory.MakeStandard(new Vector3());
            var result = r1.OverlapWith(r2, PrismBounceAxis.X);
            assertResultType(result, ResultType.Perfect);
        }

        [Test]
        public void PerfectWithinToleranceLeft()
        {
            var r1 = RectangularPrismFactory.MakeStandard(new Vector3(-RectangularPrism.PerfectPlayTolerance,0,0));
            var r2 = RectangularPrismFactory.MakeStandard(new Vector3());
            var result = r1.OverlapWith(r2, PrismBounceAxis.X);
            assertResultType(result, ResultType.Perfect);
        }

        [Test]
        public void PerfectWithinToleranceFront()
        {
            var r1 = RectangularPrismFactory.MakeStandard(new Vector3(0,0,RectangularPrism.PerfectPlayTolerance));
            var r2 = RectangularPrismFactory.MakeStandard(new Vector3());
            var result = r1.OverlapWith(r2, PrismBounceAxis.Z);
            assertResultType(result, ResultType.Perfect);
        }

        [Test]
        public void PerfectWithinToleranceBack()
        {
            var r1 = RectangularPrismFactory.MakeStandard(new Vector3(0,0,-RectangularPrism.PerfectPlayTolerance));
            var r2 = RectangularPrismFactory.MakeStandard(new Vector3());
            var result = r1.OverlapWith(r2, PrismBounceAxis.Z);
            assertResultType(result, ResultType.Perfect);
        }

        [Test]
        public void TotalMissLeft()
        {
            var r1 = RectangularPrismFactory.Make(new Size3(1,1,1), new Vector3(-1,0,0), Color.White);
            var r2 = RectangularPrismFactory.Make(new Size3(1,1,1), new Vector3(0,0,0), Color.White);
            var result = r1.OverlapWith(r2, PrismBounceAxis.X);
            assertResultType(result, ResultType.TotalMiss);
        }

        [Test]
        public void TotalMissRight()
        {
            var r1 = RectangularPrismFactory.Make(new Size3(2,1,2), new Vector3(2,0,0), Color.White);
            var r2 = RectangularPrismFactory.Make(new Size3(2,1,2), new Vector3(0,0,0), Color.White);
            var result = r1.OverlapWith(r2, PrismBounceAxis.X);
            assertResultType(result, ResultType.TotalMiss);
        }

        [Test]
        public void TotalMissFront()
        {
            var r1 = RectangularPrismFactory.Make(new Size3(2,1,2), new Vector3(0,0,2), Color.White);
            var r2 = RectangularPrismFactory.Make(new Size3(2,1,2), new Vector3(0,0,0), Color.White);
            var result = r1.OverlapWith(r2, PrismBounceAxis.Z);
            assertResultType(result, ResultType.TotalMiss);
        }

        [Test]
        public void TotalMissBack()
        {
            var r1 = RectangularPrismFactory.Make(new Size3(2,1,2), new Vector3(0,0,-2), Color.White);
            var r2 = RectangularPrismFactory.Make(new Size3(2,1,2), new Vector3(0,0,0), Color.White);
            var result = r1.OverlapWith(r2, PrismBounceAxis.Z);
            assertResultType(result, ResultType.TotalMiss);
        }

        [Test]
        public void MixedLeft()
        {
            var r1 = RectangularPrismFactory.Make(new Size3(4,1,4), new Vector3(-1,0,0), Color.White);
            var r2 = RectangularPrismFactory.Make(new Size3(4,1,4), new Vector3(0,0,0), Color.White);
            var result = r1.OverlapWith(r2, PrismBounceAxis.X);
            assertResultType(result, ResultType.Mixed);
            result.Do(
                perfect => { },
                totalMiss => { },
                mixed =>
                {
                    Assert.That(mixed.Missed.Size.X, Is.EqualTo(1));
                    Assert.That(mixed.Missed.Position.X, Is.EqualTo(-2.5f));
                    Assert.That(mixed.Landed.Size.X, Is.EqualTo(3));
                    Assert.That(mixed.Landed.Position.X, Is.EqualTo(-0.5f));
                });
        }

        [Test]
        public void MixedFront()
        {
            var r1 = RectangularPrismFactory.Make(new Size3(4,1,4), new Vector3(0,0,1), Color.White);
            var r2 = RectangularPrismFactory.Make(new Size3(4,1,4), new Vector3(0,0,0), Color.White);
            var result = r1.OverlapWith(r2, PrismBounceAxis.Z);
            assertResultType(result, ResultType.Mixed);
            result.Do(
                perfect => { },
                totalMiss => { },
                mixed =>
                {
                    Assert.That(mixed.Missed.Size.Z, Is.EqualTo(1f), "Missed.Size.Z");
                    Assert.That(mixed.Missed.Position.Z, Is.EqualTo( 2.5f), "Missed.Position.Z");
                    Assert.That(mixed.Landed.Size.Z, Is.EqualTo(3f), "Landed.Size.Z");
                    Assert.That(mixed.Landed.Position.Z, Is.EqualTo( 0.5f), "Landed.Position.Z");
                });
        }

        [Test]
        public void MixedBack()
        {
            var r1 = RectangularPrismFactory.Make(new Size3(4,1,4), new Vector3(0,0,-3), Color.White);
            var r2 = RectangularPrismFactory.Make(new Size3(4,1,4), new Vector3(0,0,0), Color.White);
            var result = r1.OverlapWith(r2, PrismBounceAxis.Z);
            assertResultType(result, ResultType.Mixed);
            result.Do(
                perfect => { },
                totalMiss => { },
                mixed =>
                {
                    Assert.That(mixed.Missed.Size.Z, Is.EqualTo(3f), "Missed.Size.Z");
                    Assert.That(mixed.Missed.Position.Z, Is.EqualTo( -3.5f), "Missed.Position.Z");
                    Assert.That(mixed.Landed.Size.Z, Is.EqualTo(1f), "Landed.Size.Z");
                    Assert.That(mixed.Landed.Position.Z, Is.EqualTo( -1.5f), "Landed.Position.Z");
                });
        }

        [Test]
        public void MixedRight()
        {
            var r1 = RectangularPrismFactory.Make(new Size3(4,1,4), new Vector3(3,0,0), Color.White);
            var r2 = RectangularPrismFactory.Make(new Size3(4,1,4), new Vector3(0,0,0), Color.White);
            var result = r1.OverlapWith(r2, PrismBounceAxis.X);
            assertResultType(result, ResultType.Mixed);
            result.Do(
                perfect => { },
                totalMiss => { },
                mixed =>
                {
                    Assert.That(mixed.Missed.Size.X, Is.EqualTo(3));
                    Assert.That(mixed.Missed.Position.X, Is.EqualTo(3.5f));
                    Assert.That(mixed.Landed.Size.X, Is.EqualTo(1));
                    Assert.That(mixed.Landed.Position.X, Is.EqualTo(1.5f));
                });
        }

        private static void assertResultType(PrismOverlapResult result, ResultType expectedType)
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