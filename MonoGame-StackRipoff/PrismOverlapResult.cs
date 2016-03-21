using System;

namespace MonoGame_StackRipoff
{
    public abstract class PrismOverlapResult
    {
        public abstract void Do(Action<PerfectLanding> perfectAction, Action<TotalMiss> totalMissAction, Action<Mixed> mixedAction);
        public abstract T Match<T>(Func<PerfectLanding,T> perfectFunc, Func<TotalMiss,T> totalMissFunc, Func<Mixed,T> mixedFunc);

        public class PerfectLanding : PrismOverlapResult
        {
            public readonly RectangularPrism Landed;

            public PerfectLanding(RectangularPrism landed)
            {
                Landed = landed;
            }

            public override void Do(Action<PerfectLanding> perfectAction, Action<TotalMiss> totalMissAction,
                Action<Mixed> mixedAction)
            {
                perfectAction(this);
            }

            public override T Match<T>(Func<PerfectLanding, T> perfectFunc, Func<TotalMiss, T> totalMissFunc,
                Func<Mixed, T> mixedFunc)
            {
                return perfectFunc(this);
            }
        }
        public class TotalMiss : PrismOverlapResult
        {
            public override void Do(Action<PerfectLanding> perfectAction, Action<TotalMiss> totalMissAction,
                Action<Mixed> mixedAction)
            {
                totalMissAction(this);
            }

            public override T Match<T>(Func<PerfectLanding, T> perfectFunc, Func<TotalMiss, T> totalMissFunc,
                Func<Mixed, T> mixedFunc)
            {
                return totalMissFunc(this);
            }
        }
        public class Mixed : PrismOverlapResult
        {
            public readonly RectangularPrism Missed;
            public readonly RectangularPrism Landed;

            public Mixed(RectangularPrism missed, RectangularPrism landed)
            {
                Missed = missed;
                Landed = landed;
            }

            public override void Do(Action<PerfectLanding> perfectAction, Action<TotalMiss> totalMissAction,
                Action<Mixed> mixedAction)
            {
                mixedAction(this);
            }

            public override T Match<T>(Func<PerfectLanding, T> perfectFunc, Func<TotalMiss, T> totalMissFunc,
                Func<Mixed, T> mixedFunc)
            {
                return mixedFunc(this);
            }
        }
    }
}