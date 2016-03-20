using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public class RectangularPrism
    {
        public readonly VertexPositionNormalTexture[] Vertices;
        public Vector3 Position;
        public Color Color;

        public Matrix WorldMatrix
        {
            get { return Matrix.CreateTranslation(Position); }
        }

        public RectangularPrism(VertexPositionNormalTexture[] vertices, Vector3 position)
        {
            Vertices = vertices;
            Position = position;
        }

        public RectangularPrism(VertexPositionNormalTexture[] vertices)
        {
            Vertices = vertices;
        }
    }
}