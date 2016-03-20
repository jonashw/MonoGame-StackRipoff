using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public class RectangularPrism
    {
        public readonly VertexPositionNormalTexture[] Vertices;
        public Vector3 Position;
        public readonly Color Color;

        public Matrix WorldMatrix
        {
            get { return Matrix.CreateTranslation(Position); }
        }

        public RectangularPrism(VertexPositionNormalTexture[] vertices, Vector3 position, Color color)
        {
            Vertices = vertices;
            Position = position;
            Color = color;
        }

        public RectangularPrism(VertexPositionNormalTexture[] vertices, Color color)
        {
            Vertices = vertices;
            Color = color;
        }
    }
}