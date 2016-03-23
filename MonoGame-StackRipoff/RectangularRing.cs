using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public class RectangularRing
    {
        public Vector3 Position;
        public float InnerSizeZ;
        public float InnerSizeX;
        public float OuterSize;
        public float Opacity = 1;

        public void Draw(GraphicsDevice graphics, BasicEffect effect)
        {
            var isx = InnerSizeX/2f;
            var isz = InnerSizeZ/2f;
            var osx = OuterSize + isx;
            var osz = OuterSize + isz;
            // ReSharper disable InconsistentNaming
            var imm = new Vector3(Position.X - isx, 0, Position.Z - isz);
            var iMm = new Vector3(Position.X + isx, 0, Position.Z - isz);
            var imM = new Vector3(Position.X - isx, 0, Position.Z + isz);
            var iMM = new Vector3(Position.X + isx, 0, Position.Z + isz);
            var omm = new Vector3(Position.X - osx, 0, Position.Z - osz);
            var oMm = new Vector3(Position.X + osx, 0, Position.Z - osz);
            var omM = new Vector3(Position.X - osx, 0, Position.Z + osz);
            var oMM = new Vector3(Position.X + osx, 0, Position.Z + osz);
            // ReSharper restore InconsistentNaming
            var triangles = new[]
            {
                omm, imm, iMm,
                omm, imM, imm,
                omm, omM, imM,
                omm, iMm, oMm,
                oMM, oMm, iMm,
                oMM, iMm, iMM,
                oMM, iMM, imM,
                oMM, imM, omM
            }.Select(v => new VertexPositionNormalTexture(v, Vector3.UnitY, new Vector2())).ToArray();

            effect.World = Matrix.CreateTranslation(Position);
            effect.DiffuseColor = Color.White.ToVector3();
            foreach (var pass in effect.CurrentTechnique.Passes)
            {
                effect.Alpha = Opacity;
                pass.Apply();
                graphics.DrawUserPrimitives(PrimitiveType.TriangleList, triangles, 0, triangles.Length/3, VertexPositionNormalTexture.VertexDeclaration);
            }
        }
    }
}