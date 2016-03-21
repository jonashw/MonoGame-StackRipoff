using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public class RectangularPrism
    {
        public readonly VertexPositionNormalTexture[] Vertices;
        public readonly Size3 Size;
        public Vector3 Position;
        public readonly Color Color;

        public Matrix WorldMatrix
        {
            get { return Matrix.CreateTranslation(Position); }
        }

        public float Left
        {
            get { return Position.X - Size.X/2; }
        }

        public float Right
        {
            get { return Position.X + Size.X/2; }
        }
        public float Front
        {
            get { return Position.Z + Size.Z/2; }
        }
        public float Back
        {
            get { return Position.Z - Size.Z/2; }
        }

        public RectangularPrism(VertexPositionNormalTexture[] vertices, Vector3 position, Color color, Size3 size)
        {
            Vertices = vertices;
            Position = position;
            Color = color;
            Size = size;
        }

        public RectangularPrism(VertexPositionNormalTexture[] vertices, Color color, Size3 size)
        {
            Vertices = vertices;
            Color = color;
            Size = size;
        }

        public PrismPlayResult OverlapWith(RectangularPrism other, PrismBounceAxis prismBounceAxis)
        {
            switch (prismBounceAxis)
            {
                case PrismBounceAxis.X:
                    if (Left < other.Left && Right <= other.Left)
                    {
                        return new PrismPlayResult.TotalMiss();
                    }
                    if (other.Right < Right && other.Right <= Left)
                    {
                        return new PrismPlayResult.TotalMiss();
                    }
                    if (Left < other.Left)
                    {
                        var hangerSize = new Size3(
                            other.Left - Left,
                            RectangularPrismFactory.TileHeight,
                            Size.Z);
                        var landedSize = new Size3(
                            Size.X - hangerSize.X,
                            RectangularPrismFactory.TileHeight,
                            Size.Z);
                        return new PrismPlayResult.Mixed(
                            RectangularPrismFactory.Make(
                                hangerSize,
                                new Vector3(Left + hangerSize.X / 2f, Position.Y, Position.Z),
                                Color),
                            RectangularPrismFactory.Make(
                                landedSize,
                                new Vector3(Position.X + hangerSize.X / 2f, Position.Y, Position.Z),
                                Color));
                    }
                    if (other.Right < Right)
                    {
                        var hangerSize = new Size3(
                            Right - other.Right,
                            RectangularPrismFactory.TileHeight,
                            Size.Z);
                        var landedSize = new Size3(
                            Size.X - hangerSize.X,
                            RectangularPrismFactory.TileHeight,
                            Size.Z);
                        return new PrismPlayResult.Mixed(
                            RectangularPrismFactory.Make(
                                hangerSize,
                                new Vector3(Right - hangerSize.X / 2f, Position.Y, Position.Z),
                                Color),
                            RectangularPrismFactory.Make(
                                landedSize,
                                new Vector3(Position.X - hangerSize.X / 2f, Position.Y, Position.Z),
                                Color));
                    }
                    return new PrismPlayResult.PerfectLanding();
                case PrismBounceAxis.Z:
                    if (Front < other.Front && Front <= other.Back)
                    {
                        return new PrismPlayResult.TotalMiss();
                    }
                    if (other.Front < Front && other.Front <= Back)
                    {
                        return new PrismPlayResult.TotalMiss();
                    }
                    if (other.Front < Front)
                    {
                        var hangerSize = new Size3(
                            Size.X,
                            RectangularPrismFactory.TileHeight,
                            Front - other.Front);
                        var landedSize = new Size3(
                            Size.X,
                            RectangularPrismFactory.TileHeight,
                            Size.Z - hangerSize.Z);
                        return new PrismPlayResult.Mixed(
                            RectangularPrismFactory.Make(
                                hangerSize,
                                new Vector3(Position.X, Position.Y, Front - hangerSize.Z / 2f),
                                Color),
                            RectangularPrismFactory.Make(
                                landedSize,
                                new Vector3(Position.X, Position.Y, Position.Z - hangerSize.Z / 2f),
                                Color));
                    }
                    if (Back < other.Back)
                    {
                        var hangerSize = new Size3(
                            Size.X,
                            RectangularPrismFactory.TileHeight,
                            other.Back - Back);
                        var landedSize = new Size3(
                            Size.X,
                            RectangularPrismFactory.TileHeight,
                            Size.Z - hangerSize.Z);
                        return new PrismPlayResult.Mixed(
                            RectangularPrismFactory.Make(
                                hangerSize,
                                new Vector3(Position.X, Position.Y, Back + hangerSize.Z/2f),
                                Color),
                            RectangularPrismFactory.Make(
                                landedSize,
                                new Vector3(Position.X, Position.Y, Position.Z + hangerSize.Z/2f),
                                Color));
                    }
                    break;
            }
            return new PrismPlayResult.PerfectLanding();
        }

    }
}