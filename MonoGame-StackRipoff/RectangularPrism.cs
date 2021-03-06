﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public class RectangularPrism
    {
        public const float PerfectPlayTolerance = 0.5f;
        private readonly VertexPositionNormalTexture[] _vertices;
        public readonly Size3 Size;
        public float YVelocity;
        public float Opacity = 1f;
        public Vector3 Position;
        public readonly Color Color;

        public Matrix WorldMatrix
        {
            get { return Matrix.CreateTranslation(Position); }
        }

        public float Left
        {
            get { return Position.X - Size.X/2f; }
        }

        public float Right
        {
            get { return Position.X + Size.X/2f; }
        }
        public float Front
        {
            get { return Position.Z + Size.Z/2f; }
        }
        public float Back
        {
            get { return Position.Z - Size.Z/2f; }
        }

        public float Bottom
        {
            get { return Position.Y - Size.Y/2f; }
        }

        public RectangularPrism(VertexPositionNormalTexture[] vertices, Vector3 position, Color color, Size3 size)
        {
            _vertices = vertices;
            Position = position;
            Color = color;
            Size = size;
        }

        public RectangularPrism(VertexPositionNormalTexture[] vertices, Color color, Size3 size)
        {
            _vertices = vertices;
            Color = color;
            Size = size;
        }

        public PrismOverlapResult OverlapWith(RectangularPrism other, PrismBounceAxis prismBounceAxis)
        {
            if (prismBounceAxis == PrismBounceAxis.X)
            {
                if (Math.Abs(Right - other.Right) <= PerfectPlayTolerance)
                {
                    return new PrismOverlapResult.PerfectLanding(
                        copyWithPosition(new Vector3(other.Position.X, Position.Y, Position.Z)));
                }
                if (Left < other.Left && Right <= other.Left)
                {
                    return new PrismOverlapResult.TotalMiss();
                }
                if (other.Right < Right && other.Right <= Left)
                {
                    return new PrismOverlapResult.TotalMiss();
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
                    return new PrismOverlapResult.Mixed(
                        RectangularPrismFactory.Make(
                            hangerSize,
                            new Vector3(Left + hangerSize.X / 2f, Position.Y, Position.Z),
                            Color),
                        RectangularPrismFactory.Make(
                            landedSize,
                            new Vector3(Position.X + hangerSize.X / 2f, Position.Y, Position.Z),
                            Color));
                } else {
                    var hangerSize = new Size3(
                        Right - other.Right,
                        RectangularPrismFactory.TileHeight,
                        Size.Z);
                    var landedSize = new Size3(
                        Size.X - hangerSize.X,
                        RectangularPrismFactory.TileHeight,
                        Size.Z);
                    return new PrismOverlapResult.Mixed(
                        RectangularPrismFactory.Make(
                            hangerSize,
                            new Vector3(Right - hangerSize.X / 2f, Position.Y, Position.Z),
                            Color),
                        RectangularPrismFactory.Make(
                            landedSize,
                            new Vector3(Position.X - hangerSize.X / 2f, Position.Y, Position.Z),
                            Color));
                }
            }
            //Front/Back
            if (Math.Abs(Front - other.Front) <= PerfectPlayTolerance)
            {
                return new PrismOverlapResult.PerfectLanding(
                    copyWithPosition(new Vector3(Position.X, Position.Y, other.Position.Z)));
            }
            if (Front < other.Front && Front <= other.Back)
            {
                return new PrismOverlapResult.TotalMiss();
            }
            if (other.Front < Front && other.Front <= Back)
            {
                return new PrismOverlapResult.TotalMiss();
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
                return new PrismOverlapResult.Mixed(
                    RectangularPrismFactory.Make(
                        hangerSize,
                        new Vector3(Position.X, Position.Y, Front - hangerSize.Z / 2f),
                        Color),
                    RectangularPrismFactory.Make(
                        landedSize,
                        new Vector3(Position.X, Position.Y, Position.Z - hangerSize.Z / 2f),
                        Color));
            } else {
                var hangerSize = new Size3(
                    Size.X,
                    RectangularPrismFactory.TileHeight,
                    other.Back - Back);
                var landedSize = new Size3(
                    Size.X,
                    RectangularPrismFactory.TileHeight,
                    Size.Z - hangerSize.Z);
                return new PrismOverlapResult.Mixed(
                    RectangularPrismFactory.Make(
                        hangerSize,
                        new Vector3(Position.X, Position.Y, Back + hangerSize.Z/2f),
                        Color),
                    RectangularPrismFactory.Make(
                        landedSize,
                        new Vector3(Position.X, Position.Y, Position.Z + hangerSize.Z/2f),
                            Color));
            }
        }

        private RectangularPrism copyWithPosition(Vector3 position)
        {
            return new RectangularPrism(_vertices, position, Color, Size);
        }

        public void Draw(GraphicsDevice graphics, BasicEffect effect)
        {
            effect.World = WorldMatrix;
            effect.DiffuseColor = Color.ToVector3();
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                effect.Alpha = Opacity;
                pass.Apply();
                graphics.DrawUserPrimitives(PrimitiveType.TriangleList, _vertices, 0, _vertices.Length / 3);
            }
        }
    }
}