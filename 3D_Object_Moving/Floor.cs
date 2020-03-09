using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3D_Object_Moving
{
    class Floor
    {
        //Attributes
        private int floorWidth; //Width of the plane generated to act as a virtual 3-dimensional floor. 
        private int floorDepth; //Depth of the plane generated to act as a virtual 3-dimensional floor. 
        private VertexBuffer floorBuffer;
        private GraphicsDevice device;
        private Color[] floorColours = new Color[1] { new Color(4, 53, 19) }; //Colour of floor

        //Constructor
        public Floor(GraphicsDevice device, int width, int depth)
        {
            this.device = device;
            floorWidth = width;
            floorDepth = depth;
            BuildFloorBuffer();
        }

        //Build vertex buffer
        private void BuildFloorBuffer()
        {
            List<VertexPositionColor> vertexList = new List<VertexPositionColor>();
            int counter = 0;

            //Create the floor
            for (int i = 0; i < floorWidth; i++)
            {
                counter++;
                for (int j = 0; j < floorDepth; j++)
                {
                    counter++;

                    foreach (VertexPositionColor vertex in FloorTile(i, j, floorColours[counter % 1]))
                    {
                        vertexList.Add(vertex);
                    }
                }
            }

            //Create the buffer
            floorBuffer = new VertexBuffer(device, VertexPositionColor.VertexDeclaration, vertexList.Count, BufferUsage.None);
            floorBuffer.SetData(vertexList.ToArray());
        }

        //Defines a single floor tile
        private List<VertexPositionColor> FloorTile(int xOffset, int zOffset, Color tileColour)
        {
            List<VertexPositionColor> vertexList = new List<VertexPositionColor>();
            vertexList.Add(new VertexPositionColor(new Vector3(0 + xOffset, 0, 0 + zOffset), tileColour));
            vertexList.Add(new VertexPositionColor(new Vector3(1 + xOffset, 0, 0 + zOffset), tileColour));
            vertexList.Add(new VertexPositionColor(new Vector3(0 + xOffset, 0, 1 + zOffset), tileColour));
            vertexList.Add(new VertexPositionColor(new Vector3(1 + xOffset, 0, 0 + zOffset), tileColour));
            vertexList.Add(new VertexPositionColor(new Vector3(1 + xOffset, 0, 1 + zOffset), tileColour));
            vertexList.Add(new VertexPositionColor(new Vector3(0 + xOffset, 0, 1 + zOffset), tileColour));

            return vertexList;
        }

        //Draw
        public void Draw(Player camera, BasicEffect effect)
        {
            effect.VertexColorEnabled = true;
            effect.View = camera.View;
            effect.Projection = camera.Projection;
            effect.World = Matrix.Identity;
        
            //Loop through and draw each tile
            foreach (EffectPass pass in effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                device.SetVertexBuffer(floorBuffer);
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, floorBuffer.VertexCount / 3);
            }
        }
    }
}
