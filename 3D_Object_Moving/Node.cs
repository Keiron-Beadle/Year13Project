using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace _3D_Object_Moving
{
    public class Node
    {
        //This whole class is pretty easy to understand, it's just creating Cubes to represent nodes,
        //along with a few methods to draw them.
        private VertexBuffer vBuffer;

        public BoundingBox collisionBox; 

        public VertexBuffer VBuffer
        { get { return vBuffer; } set { vBuffer = value; } }

        private IndexBuffer iBuffer;
        public IndexBuffer IBuffer
        { get { return iBuffer; } set { iBuffer = value; } }

        private BasicEffect bEffect;
        public BasicEffect BEffect
        { get { return bEffect; } set { bEffect = value; } }

        private Matrix world;
        public Matrix World
        { get { return world; } set { world = value; } }

        private Matrix view;
        public Matrix View
        { get { return view; } set { view = value; } }

        private Matrix projection;
        private Matrix Projection
        { get { return projection; } set { projection = value; } }

        private Vector3 position;
        public Vector3 Position
        { get { return position; } set { position = value; } }

        public const float SIZE = 0.5f;
        private GraphicsDevice device;
        private VertexPositionColor[] vertices;

        public Color color = new Color(255, 255, 13);

        short[] indices;
        public bool changed = false; 

        Vector3 MinResult(Vector3 u, Vector3 v)
        {
            Vector3 minVec = v;

            if (u.X < v.X)
            {
                minVec.X = u.X;
            }

            if (u.Y < v.Y)
            {
                minVec.Y = u.Y;
            }

            if (u.Z < v.Z)
            {
                minVec.Z = u.Z;
            }

            return minVec;
        }

        Vector3 MaxResult(Vector3 u, Vector3 v)
        {
            Vector3 maxVec = v;

            if (u.X > v.X)
            {
                maxVec.X = u.X;
            }

            if (u.Y > v.Y)
            {
                maxVec.Y = u.Y;
            }

            if (u.Z > v.Z)
            {
                maxVec.Z = u.Z;
            }

            return maxVec;
        }
         
        public Node(Vector3 inPosition, GraphicsDevice inDevice)
        {
            device = inDevice;

            this.position = inPosition;
            SetUpVertices();
            SetUpIndices();
            world = Matrix.CreateTranslation(0, 0, 0);
            bEffect = new BasicEffect(device);
            bEffect.World = world;
            bEffect.VertexColorEnabled = true;
        }
        //end constructors!

        // >.<
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

        public void UpdateNode()
        {
            SetUpVertices();
        }

        /// <summary>
        /// Sets up the vertices for a cube using 8 unique vertices.
        /// Build order is front to back, left to up to right to down.
        /// </summary>
        private void SetUpVertices()
        {
            vertices = new VertexPositionColor[8];

            //front left bottom corner
            vertices[0] = new VertexPositionColor(new Vector3(0, 0, 0) + position, color);
            //front left upper corner
            vertices[1] = new VertexPositionColor(new Vector3(0, SIZE, 0) + position, color);
            //front right upper corner
            vertices[2] = new VertexPositionColor(new Vector3(0, SIZE, SIZE) + position, color);
            //front lower right corner
            vertices[3] = new VertexPositionColor(new Vector3(0, 0, SIZE) + position, color);
            //back left lower corner
            vertices[4] = new VertexPositionColor(new Vector3(SIZE, 0, 0) + position, color);
            //back left upper corner
            vertices[5] = new VertexPositionColor(new Vector3(SIZE, SIZE, 0) + position, color);
            //back right upper corner
            vertices[6] = new VertexPositionColor(new Vector3(SIZE, SIZE, SIZE) + position, color);
            //back right lower corner
            vertices[7] = new VertexPositionColor(new Vector3(SIZE, 0, SIZE) + position, color);

            Vector3 max = vertices[0].Position;
            Vector3 min = vertices[0].Position;

            foreach (VertexPositionColor vc in vertices)
            {
                min = MinResult(min, vc.Position);
                max = MaxResult(max, vc.Position);
            }

            collisionBox = new BoundingBox(min, max);


            vBuffer = new VertexBuffer(device, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            vBuffer.SetData(vertices);
        }

        /// <summary>
        /// Sets up the indices for a cube. Has 36 positions that match up
        /// to the element numbers of the vertices created earlier.
        /// Valid range is 0-7 for each value.
        /// </summary>
        private void SetUpIndices()
        {
            indices = new short[36];

            //Front face
            //bottom right triangle
            indices[0] = 0;
            indices[1] = 3;
            indices[2] = 2;
            //top left triangle
            indices[3] = 2;
            indices[4] = 1;
            indices[5] = 0;
            //back face
            //bottom right triangle
            indices[6] = 4;
            indices[7] = 7;
            indices[8] = 6;
            //top left triangle
            indices[9] = 6;
            indices[10] = 5;
            indices[11] = 4;
            //Top face
            //bottom right triangle
            indices[12] = 1;
            indices[13] = 2;
            indices[14] = 6;
            //top left triangle
            indices[15] = 6;
            indices[16] = 5;
            indices[17] = 1;
            //bottom face
            //bottom right triangle
            indices[18] = 4;
            indices[19] = 7;
            indices[20] = 3;
            //top left triangle
            indices[21] = 3;
            indices[22] = 0;
            indices[23] = 4;
            //left face
            //bottom right triangle
            indices[24] = 4;
            indices[25] = 0;
            indices[26] = 1;
            //top left triangle
            indices[27] = 1;
            indices[28] = 5;
            indices[29] = 4;
            //right face
            //bottom right triangle
            indices[30] = 3;
            indices[31] = 7;
            indices[32] = 6;
            //top left triangle
            indices[33] = 6;
            indices[34] = 2;
            indices[35] = 3;

            iBuffer = new IndexBuffer(device, IndexElementSize.SixteenBits, sizeof(short) * indices.Length, BufferUsage.WriteOnly);

            iBuffer.SetData(indices);
        }
    }
}
