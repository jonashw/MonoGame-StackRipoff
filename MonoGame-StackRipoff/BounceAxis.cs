using System;
using System.Diagnostics;
using Microsoft.Xna.Framework;

namespace MonoGame_StackRipoff
{
    public abstract class AxisDirection
    {
        public static readonly AxisDirection Positive = new PositiveDirection();
        public static readonly AxisDirection Negative = new NegativeDirection();
        public abstract float Apply(float value);
        private AxisDirection(){}

        private class PositiveDirection : AxisDirection
        {
            public override float Apply(float value)
            {
                return value;
            }
        }

        private class NegativeDirection : AxisDirection
        {
            public override float Apply(float value)
            {
                return -value;
            }
        }

        public AxisDirection Opposite
        {
            get
            {
                return this == Positive
                    ? Negative
                    : Positive;
            }
        }
    }

    [DebuggerDisplay("{Id}")]
    public abstract class BounceAxis
    {
        public static readonly BounceAxis X = new XAxis();
        public static readonly BounceAxis Z = new ZAxis();

        public readonly string Id;
        public abstract float Extreme(RectangularPrism prism, AxisDirection direction);

        public override string ToString()
        {
            return Id;
        }

        public float PositiveExtreme(RectangularPrism prism)
        {
            return Extreme(prism, AxisDirection.Positive);
        }

        public float NegativeExtreme(RectangularPrism prism)
        {
            return Extreme(prism, AxisDirection.Negative);
        }

        private class XAxis : BounceAxis
        {
            public XAxis() : base(
                size => size.WithX,
                size => size.X,
                (vector,v) => new Vector3(v, vector.Y, vector.Z),
                vector => vector.X,
                "X") { }

            public override float Extreme(RectangularPrism prism, AxisDirection direction)
            {
                return direction == AxisDirection.Positive
                    ? prism.Right
                    : prism.Left;
            }
        }

        private class ZAxis : BounceAxis
        {
            public ZAxis() : base(
                size => size.WithZ,
                size => size.Z,
                (vector, v) => new Vector3(vector.X, vector.Y, v),
                vector => vector.Z,
                "Z") { }

            public override float Extreme(RectangularPrism prism, AxisDirection direction)
            {
                return direction == AxisDirection.Positive
                    ? prism.Front
                    : prism.Back;
            }
        }

        private readonly Func<Size3, Func<float, Size3>> _getUpdatedSize;
        private readonly Func<Size3, float> _getSizeComponent;
        private readonly Func<Vector3, float, Vector3> _getUpdatedVector;
        private readonly Func<Vector3, float> _getVectorComponent;
        private BounceAxis(
            Func<Size3, Func<float, Size3>> getUpdatedSize,
            Func<Size3, float> getSizeComponent,
            Func<Vector3, float, Vector3> getUpdatedVector,
            Func<Vector3, float> getVectorComponent, string id)
        {
            _getUpdatedSize = getUpdatedSize;
            _getSizeComponent = getSizeComponent;
            _getUpdatedVector = getUpdatedVector;
            _getVectorComponent = getVectorComponent;
            Id = id;
        }

        public Size3 HangerSize(RectangularPrism a, RectangularPrism b, AxisDirection direction)
        {
            return _getUpdatedSize(a.Size)(Extreme(a, direction) - Extreme(b, direction));
        }

        public Size3 LandedSize(RectangularPrism prism, Size3 hangerSize)
        {
            return _getUpdatedSize(prism.Size)(_getSizeComponent(prism.Size) - _getSizeComponent(hangerSize));
        }

        public Vector3 HangerPosition(RectangularPrism a, Size3 hangerSize, AxisDirection direction)
        {
            return _getUpdatedVector(a.Position, Extreme(a, direction.Opposite) + direction.Apply(_getSizeComponent(hangerSize)/2f));
        }

        public Vector3 LandedPosition(RectangularPrism prism, Size3 hangerSize, AxisDirection direction)
        {
            return _getUpdatedVector(prism.Position, _getVectorComponent(prism.Position) + direction.Apply(_getSizeComponent(hangerSize)/2f));
        }

        //
        public Size3 NegativeHangerSize(RectangularPrism a, RectangularPrism b)
        {
            return HangerSize(a, b, AxisDirection.Negative);
        }

        public Size3 PositiveHangerSize(RectangularPrism a, RectangularPrism b)
        {
            return HangerSize(a, b, AxisDirection.Positive);
        }

        public Vector3 NegativeHangerPosition(RectangularPrism a, Size3 hangerSize)
        {
            return HangerPosition(a, hangerSize, AxisDirection.Positive);
        }

        public Vector3 PositiveHangerPosition(RectangularPrism a, Size3 hangerSize)
        {
            return HangerPosition(a, hangerSize, AxisDirection.Negative);
        }

        public Vector3 NegativeLandedPosition(RectangularPrism prism, Size3 hangerSize)
        {
            return LandedPosition(prism, hangerSize, AxisDirection.Positive);
        }

        public Vector3 PositiveLandedPosition(RectangularPrism prism, Size3 hangerSize)
        {
            return LandedPosition(prism, hangerSize, AxisDirection.Negative);
        }
    }
}