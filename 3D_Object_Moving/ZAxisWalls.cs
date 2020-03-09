using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3D_Object_Moving
{
    class ZAxisWalls : Walls
    {
        public ZAxisWalls(Vector3 inPosition, GraphicsDevice inDevice)
            : base(inPosition, inDevice) { SetUpVertices(); }

        private void SetUpVertices()
        {
            vertices = new VertexPositionColor[8];

            //front left bottom corner
            vertices[0] = new VertexPositionColor(new Vector3(2, 0, 0) + position, new Color(12, 111, 0));//
                                                                                                          //front left upper corner
            vertices[1] = new VertexPositionColor(new Vector3(2, 10, 0) + position, new Color(13, 103, 2));//
                                                                                                           //front right upper corner
            vertices[2] = new VertexPositionColor(new Vector3(2, 10, 10) + position, new Color(14, 109, 2));//
                                                                                                            //front lower right corner
            vertices[3] = new VertexPositionColor(new Vector3(2, 0, 10) + position, new Color(12, 111, 0));//
                                                                                                           //back left lower corner
            vertices[4] = new VertexPositionColor(new Vector3(0, 0, 0) + position, new Color(14, 102, 2));//
                                                                                                          //back left upper corner
            vertices[5] = new VertexPositionColor(new Vector3(0, 10, 0) + position, new Color(14, 102, 2));//
                                                                                                           //back right upper corner
            vertices[6] = new VertexPositionColor(new Vector3(0, 10, 10) + position, new Color(13, 103, 2));//
                                                                                                            //back right lower corner
            vertices[7] = new VertexPositionColor(new Vector3(0, 0, 10) + position, new Color(12, 111, 0));//

            Vector3 max = vertices[0].Position;
            Vector3 min = vertices[0].Position;

            foreach (VertexPositionColor vc in vertices)
            {
                min = MinResult(min, vc.Position);
                max = MaxResult(max, vc.Position);
            }

            collisionBox.Add(new BoundingBox(min, max));


            vBuffer = new VertexBuffer(device, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            vBuffer.SetData<VertexPositionColor>(vertices);
        }

        public void Render(Matrix view, Matrix projection)
        {
            bEffect.View = view;
            bEffect.Projection = projection;
            device.SetVertexBuffer(vBuffer);
            device.Indices = IBuffer;

            foreach (EffectPass pass in bEffect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
            }
        }
    }
}
