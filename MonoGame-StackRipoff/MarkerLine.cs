using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public class MarkerLine
    {
        public Vector3 StartPoint;
        public Vector3 EndPoint;

        public void Draw(GraphicsDevice graphics, BasicEffect effect)
        {
            effect.World = Matrix.Identity;
            effect.DiffuseColor = Color.White.ToVector3();
            effect.CurrentTechnique.Passes[0].Apply();
            var vertices = new[]
            {
                new VertexPositionNormalTexture(StartPoint,  Vector3.UnitX, new Vector2()),
                new VertexPositionNormalTexture(EndPoint, Vector3.UnitX, new Vector2())
            };
            graphics.DrawUserPrimitives(PrimitiveType.LineList, vertices, 0, 1, VertexPositionNormalTexture.VertexDeclaration);
        }
    }
}