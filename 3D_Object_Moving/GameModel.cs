using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace _3D_Object_Moving
{
    class GameModel
    {
        protected VertexBuffer vBuffer;

        public BoundingBox collisionBox;

        public VertexBuffer VBuffer
        { get { return vBuffer; } set { vBuffer = value; } }

        protected IndexBuffer iBuffer;
        public IndexBuffer IBuffer
        { get { return iBuffer; } set { iBuffer = value; } }

        protected BasicEffect bEffect;
        public BasicEffect BEffect
        { get { return bEffect; } set { bEffect = value; } }

        protected Matrix world;
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

        protected GraphicsDevice device;
        protected Grid grid;
        bool atNode = false;
        readonly Random rnd = new Random();
        protected Model model;
        protected Matrix[] directionRotation = new Matrix[Game1.NUMBEROFZOMBIES];
        private VertexPositionColor[] vertices;
        short[] indices;
        //Finds min point needed for bounding box
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
        //Same for max point
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
        //3 Constructors needed for varying objects.
        public GameModel(Vector3 inPosition, GraphicsDevice inDevice, Grid inGrid, Model inMod)
        {
            position = inPosition;
            device = inDevice;
            grid = inGrid;
            model = inMod;
        }
        
        public GameModel(GraphicsDevice inDevice, Grid inGrid, Model inMod)
        {
            device = inDevice;
            grid = inGrid;
            model = inMod;
        }

        public GameModel(Vector3 inPosition, GraphicsDevice inDevice)
        {
            position = inPosition;
            device = inDevice;
            bEffect = new BasicEffect(device)
            {
                World = world,
                VertexColorEnabled = true
            };
        }
        //Used for collisionbox. 
        protected void SetUpVertices(Vector3 offSet, float WIDTH, float HEIGHT, Color inColor)
        {
            vertices = new VertexPositionColor[8];

            //front left bottom corner
            vertices[0] = new VertexPositionColor(new Vector3(0, 0, 0) + position + offSet, inColor);
            //front left upper corner
            vertices[1] = new VertexPositionColor(new Vector3(0, HEIGHT, 0) + position + offSet, inColor);
            //front right upper corner
            vertices[2] = new VertexPositionColor(new Vector3(0, HEIGHT, WIDTH) + position + offSet, inColor);
            //front lower right corner
            vertices[3] = new VertexPositionColor(new Vector3(0, 0, WIDTH) + position + offSet, inColor);
            //back left lower corner
            vertices[4] = new VertexPositionColor(new Vector3(WIDTH, 0, 0) + position + offSet, inColor);
            //back left upper corner
            vertices[5] = new VertexPositionColor(new Vector3(WIDTH, HEIGHT, 0) + position + offSet, inColor);
            //back right upper corner
            vertices[6] = new VertexPositionColor(new Vector3(WIDTH, HEIGHT, WIDTH) + position + offSet, inColor);
            //back right lower corner
            vertices[7] = new VertexPositionColor(new Vector3(WIDTH, 0, WIDTH) + position + offSet, inColor);

            Vector3 max = vertices[0].Position;
            Vector3 min = vertices[0].Position;

            foreach (VertexPositionColor vc in vertices)
            {
                min = MinResult(min, vc.Position);
                max = MaxResult(max, vc.Position);
            }

            collisionBox = new BoundingBox(min, max);


            vBuffer = new VertexBuffer(device, typeof(VertexPositionColor), 8, BufferUsage.WriteOnly);
            vBuffer.SetData<VertexPositionColor>(vertices);
        }

        protected void SetUpIndices()
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
        //Method to see if the object has intersected another collisionbox. 
        public bool IntersectNode(BoundingBox boxToIntersect)
        {
            if (collisionBox.Intersects(boxToIntersect))
            {
                atNode = true;
            }
            else
            {
                atNode = false;
            }
            return atNode;
        }

    }
}
