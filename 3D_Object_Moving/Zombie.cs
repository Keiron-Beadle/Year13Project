using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _3D_Object_Moving
{
    class Zombie : GameModel
    {
        public float ZOMBIESPEED = 0.06f; //Speed of Zombie
        string prevMovement = ""; //The previous direction which the zombie was coming from
        const float ZOMBIEWIDTH = 2f; //Width of zombie's collision box
        const float ZOMBIEHEIGHT = 2f; //Height of zombie's collision box
        Vector3 collisionOffset = new Vector3(-0.7f, 0f, -0.37f); //Offset of box from corner
        int health = 100; //Zombie health

        public Zombie(Vector3 inPosition, GraphicsDevice inDevice, Grid inGrid, Model inMod)
            : base(inPosition, inDevice, inGrid, inMod)
        {
            device = inDevice;
            grid = inGrid;
            position = inPosition;
            for (int i = 0; i < Game1.NUMBEROFZOMBIES; i++) { directionRotation[i] = Matrix.CreateRotationY(MathHelper.PiOver2); } //Sets rotation of zombie, 
                                                                               //needed to set at the start of instantiation to correctly work.
            SetUpVertices(collisionOffset, ZOMBIEWIDTH, ZOMBIEHEIGHT, Color.Red); //Sets up vertices of collision box
            SetUpIndices(); //Same for indices
            world = Matrix.CreateTranslation(0, 0, 0);
            bEffect = new BasicEffect(device)
            {
                World = world,
                VertexColorEnabled = true 
            };
        }

        public int GetZombieHealth() { return health; } //Return health of zombie

        public void SetZombieHealth(int newHealth) { health = newHealth; } //Sets health of zombie

        public string GetMovement(Vector3 inCurrentNode, Vector3 inNextNode)
        {
            float currentX = inCurrentNode.X;
            float currentZ = inCurrentNode.Z;
            float nextX = inNextNode.X; //This whole subroutine is used to find which direction (relatively) the zomibe
            float nextZ = inNextNode.Z; //is headed next. For rotation purposes.

            string nextDirection = "";

            if (currentX > nextX)
                nextDirection = "back"; 

            else if (nextX > currentX)
                nextDirection = "forward"; 

            else if (nextZ < currentZ)
                nextDirection = "left"; 

            else if (currentZ < nextZ)
                nextDirection = "right"; 


            return nextDirection;
        }

        public void MoveZombie(string inMovement, List<Point> inPath, int index)
        {
            List<Vector3> ThreeDimensionPath = new List<Vector3>();

            for (int i = 0; i < inPath.Count; i++)          
                ThreeDimensionPath.Add(new Vector3((inPath[i].X + 0.5f) * 10, 1, (inPath[i].Y + 0.5f) * 10));
            //This will actually move the zombie by following the path.
            if (inMovement == "forward")
            {
                position.X = position.X + ZOMBIESPEED;
                switch (prevMovement)
                {
                    case "left":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(-MathHelper.PiOver2);
                        break;
                    case "right":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(MathHelper.PiOver2);
                        break;
                    case "back":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(MathHelper.Pi);
                        break;
                    default:
                        break;
                }

            }
            else if (inMovement == "back")
            {
                position.X = position.X - ZOMBIESPEED; //X AND Z flipped here
                switch (prevMovement)
                {
                    case "left":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(MathHelper.PiOver2);
                        break;
                    case "right":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(-MathHelper.PiOver2);
                        break;
                    case "forward":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(MathHelper.Pi);
                        break;
                    default:
                        break;
                }
            }
            else if (inMovement == "left")
            {
                position.Z = position.Z - ZOMBIESPEED;
                switch (prevMovement)
                {
                    case "forward":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(MathHelper.PiOver2);
                        break;
                    case "right":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(MathHelper.Pi);
                        break;
                    case "back":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(-MathHelper.PiOver2);
                        break;
                    default:
                        break;
                }

            }
            else if (inMovement == "right")
            {
                position.Z = position.Z + ZOMBIESPEED; 
                switch (prevMovement)
                {
                    case "left":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(MathHelper.Pi);
                        break;
                    case "forward":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(-MathHelper.PiOver2);
                        break;
                    case "back":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(MathHelper.PiOver2);
                        break;
                    default:
                        break;
                }
            }

            if (prevMovement == "")
            {
                switch (inMovement)
                {
                    case "left":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(MathHelper.PiOver2);
                        break;
                    case "back":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(MathHelper.Pi);
                        break;
                    case "right":
                        directionRotation[index] = directionRotation[index] * Matrix.CreateRotationY(-MathHelper.PiOver2);
                        break;
                    default:
                        break;
                }
            }

            if (prevMovement != inMovement)
                prevMovement = inMovement;
            SetUpVertices(collisionOffset, ZOMBIEWIDTH, ZOMBIEHEIGHT, Color.Red);
            
        }

        public void Render(Matrix view, Matrix projection, int index)
        {
            bEffect.View = view;
            bEffect.Projection = projection;
           
            foreach (var mesh in model.Meshes)
            {
                foreach (BasicEffect effect in mesh.Effects)
                {
                    effect.EnableDefaultLighting();
                    effect.PreferPerPixelLighting = true;
                    effect.World = Matrix.CreateScale(0.64f, 0.32f, 0.32f) * Matrix.CreateRotationX(-MathHelper.PiOver2) * directionRotation[index] * Matrix.CreateTranslation(position);
                    effect.View = view; //I used an 'off' X scale for comedic purposes + fill room length with zombie 
                    effect.Projection = projection;
                }
                mesh.Draw();
            }

            device.SetVertexBuffer(vBuffer);
            device.Indices = IBuffer;

             /*foreach (EffectPass pass in bEffect.CurrentTechnique.Passes)
             {
                 pass.Apply();
                 device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
             }*/
        }
    }
}
