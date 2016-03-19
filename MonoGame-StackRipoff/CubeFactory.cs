using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MonoGame_StackRipoff
{
    public static class CubeFactory
    {
        public static VertexPositionNormalTexture[] Create()
        {
            var face = new Vector3[6];
            face[0] = new Vector3(-1f, 01f, 0.0f); //TopLeft
            face[1] = new Vector3(-1f, -1f, 0.0f); //BottomLeft
            face[2] = new Vector3(01f, 01f, 0.0f); //TopRight
            face[3] = new Vector3(-1f, -1f, 0.0f); //BottomLeft
            face[4] = new Vector3(01f, -1f, 0.0f); //BottomRight
            face[5] = new Vector3(01f, 01f, 0.0f); //TopRight

            var textureCoords = new Vector2(0f, 0f);
            var vertices = new VertexPositionNormalTexture[36];

            //front face
            for (var i = 0; i <= 2; i++)
            {
                vertices[i] = new VertexPositionNormalTexture(
                    face[i] + Vector3.UnitZ,
                    Vector3.UnitZ, textureCoords);
                vertices[i + 3] = new VertexPositionNormalTexture(
                    face[i + 3] + Vector3.UnitZ,
                    Vector3.UnitZ, textureCoords);
            }

            vertices[0].TextureCoordinate = new Vector2(0, 0);//Top Left
            vertices[1].TextureCoordinate = new Vector2(0, 1);//Bottom Left
            vertices[2].TextureCoordinate = new Vector2(1, 0);//Top Right
            vertices[3].TextureCoordinate = new Vector2(0, 1);//Bottom Left
            vertices[4].TextureCoordinate = new Vector2(1, 1);//Bottom Right
            vertices[5].TextureCoordinate = new Vector2(1, 0);//TopRight

            //Back face
            for (var i = 0; i <= 2; i++)
            {
                vertices[i + 6] = new VertexPositionNormalTexture(
                    face[2 - i] - Vector3.UnitZ,
                    -Vector3.UnitZ, textureCoords);
                vertices[i + 6 + 3] = new VertexPositionNormalTexture(
                    face[5 - i] - Vector3.UnitZ,
                    -Vector3.UnitZ, textureCoords);
            }

            vertices[6].TextureCoordinate = new Vector2(1, 0);//Top Right
            vertices[7].TextureCoordinate = new Vector2(0, 1);//Bottom Left
            vertices[8].TextureCoordinate = new Vector2(0, 0);//Top Left
            vertices[9].TextureCoordinate = new Vector2(1, 0);//TopRight
            vertices[10].TextureCoordinate = new Vector2(1, 1);//Bottom Right
            vertices[11].TextureCoordinate = new Vector2(0, 1);//Bottom Left

            //left face
            var rotY90 = Matrix.CreateRotationY(-MathHelper.PiOver2);
            for (var i = 0; i <= 2; i++)
            {
                vertices[i + 12] = new VertexPositionNormalTexture(
                    Vector3.Transform(face[i], rotY90) - Vector3.UnitX,
                    -Vector3.UnitX, textureCoords);
                vertices[i + 12 + 3] = new VertexPositionNormalTexture(
                    Vector3.Transform(face[i + 3], rotY90) - Vector3.UnitX,
                    -Vector3.UnitX, textureCoords);
            }

            vertices[14].TextureCoordinate = new Vector2(1, 0);//Top Right
            vertices[13].TextureCoordinate = new Vector2(0, 1);//Bottom Left
            vertices[12].TextureCoordinate = new Vector2(0, 0);//Top Left
            vertices[15].TextureCoordinate = new Vector2(0, 1);//Bottom Left
            vertices[16].TextureCoordinate = new Vector2(1, 1);//Bottom Right
            vertices[17].TextureCoordinate = new Vector2(1, 0);//TopRight

            //Right face
            for (var i = 0; i <= 2; i++)
            {
                vertices[i + 18] = new VertexPositionNormalTexture(
                    Vector3.Transform(face[2 - i], rotY90) + Vector3.UnitX,
                    Vector3.UnitX, textureCoords);
                vertices[i + 18 + 3] = new VertexPositionNormalTexture(
                    Vector3.Transform(face[5 - i], rotY90) + Vector3.UnitX,
                    Vector3.UnitX, textureCoords);
            }

            vertices[18].TextureCoordinate = new Vector2(1, 0);//Top Right
            vertices[19].TextureCoordinate = new Vector2(0, 1);//Bottom Left
            vertices[20].TextureCoordinate = new Vector2(0, 0);//Top Left
            vertices[21].TextureCoordinate = new Vector2(1, 0);//TopRight
            vertices[22].TextureCoordinate = new Vector2(1, 1);//Bottom Right
            vertices[23].TextureCoordinate = new Vector2(0, 1);//Bottom Left

            //Top face
            var rotX90 = Matrix.CreateRotationX(-MathHelper.PiOver2);
            for (var i = 0; i <= 2; i++)
            {
                vertices[i + 24] = new VertexPositionNormalTexture(
                    Vector3.Transform(face[i], rotX90) + Vector3.UnitY,
                    Vector3.UnitY, textureCoords);
                vertices[i + 24 + 3] = new VertexPositionNormalTexture(
                    Vector3.Transform(face[i + 3], rotX90) + Vector3.UnitY,
                    Vector3.UnitY, textureCoords);
            }

            vertices[26].TextureCoordinate = new Vector2(1, 0);//Top Right
            vertices[25].TextureCoordinate = new Vector2(0, 1);//Bottom Left
            vertices[24].TextureCoordinate = new Vector2(0, 0);//Top Left
            vertices[27].TextureCoordinate = new Vector2(0, 1);//Bottom Left
            vertices[28].TextureCoordinate = new Vector2(1, 1);//Bottom Right
            vertices[29].TextureCoordinate = new Vector2(1, 0);//TopRight

            //Bottom face
            for (var i = 0; i <= 2; i++)
            {
                vertices[i + 30] = new VertexPositionNormalTexture(
                    Vector3.Transform(face[2 - i], rotX90) - Vector3.UnitY,
                    -Vector3.UnitY, textureCoords);
                vertices[i + 30 + 3] = new VertexPositionNormalTexture(
                    Vector3.Transform(face[5 - i], rotX90) - Vector3.UnitY,
                    -Vector3.UnitY, textureCoords);
            }

            vertices[30].TextureCoordinate = new Vector2(1, 0);//Top Right
            vertices[31].TextureCoordinate = new Vector2(0, 1);//Bottom Left
            vertices[32].TextureCoordinate = new Vector2(0, 0);//Top Left
            vertices[33].TextureCoordinate = new Vector2(1, 0);//TopRight
            vertices[34].TextureCoordinate = new Vector2(1, 1);//Bottom Right
            vertices[35].TextureCoordinate = new Vector2(0, 1);//Bottom Left

            return vertices;
        }
    }
}