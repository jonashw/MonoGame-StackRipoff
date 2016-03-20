using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public static class RectangularPrismFactory
    {
        public const int TileHeight = 2;
        public static RectangularPrism MakeStandard(Vector3 position)
        {
            return new RectangularPrism(
                Create(new Size3(10, TileHeight, 10)),
                position,
                SceneColors.NextPrismColor());
        }

        public static RectangularPrism Make(Size3 size, Vector3 position, Color color)
        {
            return new RectangularPrism(Create(size), position, color);
        }

        public static VertexPositionNormalTexture[] Create(Size3 size)
        {
            var x = size.X/2;
            var y = size.Y/2;
            var z = size.Z/2;

            var mainVertices = new Vector3[8];
            mainVertices[0] = new Vector3(-x, y, z);//Front Top    Left
            mainVertices[1] = new Vector3(-x,-y, z);//Front Bottom Left
            mainVertices[2] = new Vector3( x, y, z);//Front Top    Right
            mainVertices[3] = new Vector3( x,-y, z);//Front Bottom Right
            mainVertices[4] = new Vector3( x, y,-z);//Back  Top    Right
            mainVertices[5] = new Vector3( x,-y,-z);//Back  Bottom Right
            mainVertices[6] = new Vector3(-x, y,-z);//Back  Top    Left
            mainVertices[7] = new Vector3(-x,-y,-z);//Back  Bottom Left

            return new[]
            {
                //Front
                new
                {
                    normal = Vector3.UnitZ,
                    triplets = new []
                    {
                        new[] {0, 1, 2},
                        new[] {1, 3, 2}
                    }
                },
                //Right
                new
                {
                    normal = Vector3.UnitX,
                    triplets = new[]
                    {
                        new[] {2, 3, 4},
                        new[] {3, 5, 4},
                    }
                },
                //Back
                new
                {
                    normal = -Vector3.UnitZ,
                    triplets = new[]
                    {
                        new[] {4, 5, 6},
                        new[] {5, 7, 6},
                    }
                },
                //Left
                new
                {
                    normal = -Vector3.UnitX,
                    triplets = new[]
                    {
                        new[] {6, 7, 0},
                        new[] {7, 1, 0},
                    }
                },
                //Top
                new
                {
                    normal = Vector3.UnitY,
                    triplets = new[]
                    {
                        new[] {6, 0, 4},
                        new[] {0, 2, 4},
                    }
                },
                //Bottom
                new
                {
                    normal = -Vector3.UnitY,
                    triplets = new[]
                    {
                        new[] {5, 3, 7},
                        new[] {3, 1, 7}
                    }
                }
            }.SelectMany(face =>
                face.triplets.SelectMany(triplet =>
                    triplet.Select(i =>
                        new VertexPositionNormalTexture(
                            mainVertices[i],
                            face.normal,
                            new Vector2())))).ToArray();
        }
    }
}