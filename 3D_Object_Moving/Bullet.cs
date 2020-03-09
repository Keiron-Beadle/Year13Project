using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace _3D_Object_Moving
{
    class Bullet : GameModel
    {
        const float BULLETSIZE = 0.032f; //Sets the size of bullets
        const float BULLETSPEED = 0.11f; //Sets speed of bullets
        Vector3 lookAt = new Vector3(); //Initialised the lookAt which is saved at the specific time the bullet
                                        //is created to get a path the bullet will follow.
        List<Vector3> directions = new List<Vector3>(); //List of directions the bullet took to get to said LookAt.
                                                       //This is replayed after it reaches LookAt to continue path.
        Vector3 tempDirection; //Temporary used for saving into 'directions'.
        int currentDirectionIndex = -1; //Used to determine which index I need to add to 'directions'
        bool finishedPath, finishedX, finishedZ; //If I've finished the X coordinates, Z coordinates -> Implying after those, I've finished the full path.
        //int lifespan = 800; //Disabled, used to be for testing - now uses collision-based death
        public Bullet(Vector3 inPosition, GraphicsDevice inDevice) //Constructor
            : base(inPosition, inDevice)
        {
            device = inDevice;
            position = inPosition;

            SetUpVertices(Vector3.Zero, BULLETSIZE, BULLETSIZE, Color.Purple); //Sets up bullet cube
            SetUpIndices();
            world = Matrix.CreateTranslation(0, 0, 0);
            bEffect = new BasicEffect(device)
            {
                World = world,
                VertexColorEnabled = true
            };
        }

        public Vector3 GetLookAt() { return lookAt; }
        public void SetLookAt(Vector3 inLookAt) { lookAt = inLookAt ; } //Used to access variables outside of class.

        public void Update()
        {            
            if (position.X - BULLETSPEED < lookAt.X && position.X + BULLETSPEED > lookAt.X) { finishedX = true; }
            if (position.Z - BULLETSPEED < lookAt.Z && position.Z + BULLETSPEED > lookAt.Z) { finishedZ = true; } 
            //Basically, if we're within a given amount of the lookAt's coordinates in both X and Z we've finished the path.
            if (!finishedPath)
            {
                if (finishedX && finishedZ)
                {
                    finishedPath = true;
                    currentDirectionIndex = 0;
                    //I had to include the extra 'if (!finishedPath)' otherwise it wouldn't do what I expected as it would go into the
                    //!finishedPath below, then do all of that code after executing the above lines
                }
            }

            if (!finishedPath)
            {
                currentDirectionIndex++;
                tempDirection = Vector3.Zero;
                tempDirection = position;

                if (!finishedX) //If we're not finished with X yet.
                {
                    if (position.X < lookAt.X) //If we're less than lookAt's X we increase X.
                    {
                        tempDirection.X += BULLETSPEED;
                        directions.Add(position - tempDirection);
                        position.X += BULLETSPEED;
                    }
                    else if (position.X > lookAt.X) //else decrease X
                    {
                        tempDirection.X -= BULLETSPEED;
                        directions.Add(position - tempDirection);
                        position.X -= BULLETSPEED;
                    }
                    tempDirection = Vector3.Zero; //Resets tempDirection
                    tempDirection = position;
                }

                if (!finishedZ) //If we're not finished with Z yet.
                {
                    if (position.Z < lookAt.Z) //If we're less than LookAt Z we increase Z
                    {
                        tempDirection.Z += BULLETSPEED;
                        directions.Add(position - tempDirection);
                        position.Z += BULLETSPEED;
                    }
                    else if (position.Z > lookAt.Z) // else decrease Z
                    {
                        tempDirection.Z -= BULLETSPEED;
                        directions.Add(position - tempDirection);
                        position.Z -= BULLETSPEED;
                    }
                }

            }
            else
            {
                position -= directions[currentDirectionIndex]; //This means we've 'finishedpath'
                currentDirectionIndex++;                       //so we loop through 'directions' using 'currentdirectionindex'
                if (currentDirectionIndex == directions.Count) // over and over.
                {
                    currentDirectionIndex = 0;
                }
            }

            SetUpVertices(Vector3.Zero, BULLETSIZE, BULLETSIZE, Color.Purple);  //SetsUpvertices again to show a new Cube
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
                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12); //Draws cube
             }
        }
    }
}
