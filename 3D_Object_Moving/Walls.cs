using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _3D_Object_Moving
{
    class Walls
    {
        //Transform later to have static v and i buffers.
        protected VertexBuffer vBuffer;

        public List<BoundingBox> collisionBox;

        public VertexBuffer VBuffer
        { get { return vBuffer; } set { vBuffer = value; } }

        private IndexBuffer iBuffer;
        public IndexBuffer IBuffer
        { get { return iBuffer; } set { iBuffer = value; } }

        protected BasicEffect bEffect;
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

        protected Vector3 position;
        public Vector3 Position
        { get { return position; } set { position = value; } }

        //Need to change this eventually to use textures.
        protected VertexPositionColor[] vertices;
        short[] indices;

        /// <summary>
        /// Used to find the minimum point of the needed BoundingBox
        /// </summary>
        protected Vector3 MinResult(Vector3 u, Vector3 v)
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
        /// <summary>
        /// Used to find max point of the needed BoundingBox.
        /// </summary>
        protected Vector3 MaxResult(Vector3 u, Vector3 v)
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

        protected GraphicsDevice device;
        //constructors!
        public Walls(Vector3 inPosition, GraphicsDevice inDevice)
        {
            device = inDevice;

            collisionBox = new List<BoundingBox>();
            this.position = inPosition;
            SetUpIndices();
            world = Matrix.CreateTranslation(0, 0, 0);
            bEffect = new BasicEffect(device);
            bEffect.World = world;
            bEffect.PreferPerPixelLighting = true;
            bEffect.VertexColorEnabled = true;
        }

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
