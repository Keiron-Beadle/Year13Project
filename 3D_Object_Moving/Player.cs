using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace _3D_Object_Moving
{
    public class Player : GameComponent
    {
        //Attributes
        public Vector3 cameraPosition;
        private Vector3 cameraRotation;
        private Vector3 cameraLookAt;
        Vector3 weaponPos;
        private Vector3 mouseRotationBuffer;
        private MouseState currentMouseState;
        private MouseState previousMouseState;
        Vector3 lookAtOffset;
        KeyboardState _prevstate;
        public BoundingBox cameraBox;
        int health = 100;
        public bool cameraMouseEnabled = false;
        private float cameraSpeed;

        public Vector3 GetLookAtOffSet()
        {         
            return lookAtOffset;
        }

        public Vector3 GetLookAt() { return cameraLookAt; }

        public Vector3 GetWeaponPosition()
        {
            weaponPos = new Vector3(Position.X, 0.62f, Position.Z);
            return weaponPos;
        }

        public Vector3 GetCameraRotation() { return cameraRotation; }

        //MAP LOADING
#pragma warning disable CS0414 // The field 'Player.moveScale' is assigned but its value is never used
        private float moveScale;
#pragma warning restore CS0414 // The field 'Player.moveScale' is assigned but its value is never used
        public Vector3 comparisonVector { get; set; }

        //Constructor
        public Player(Game game, Vector3 position, Vector3 rotation, float speed)
            : base(game)
        {
            cameraSpeed = speed;
            Projection = Matrix.CreatePerspectiveFieldOfView(MathHelper.PiOver4,
                Game.GraphicsDevice.Viewport.AspectRatio,
                0.05f,
                1000.0f);

            //Set camera position and rotation
            MoveTo(position, rotation);
            mouseRotationBuffer = new Vector3(1.55f, 0, 0);
            previousMouseState = Mouse.GetState();
        }

        public Point ClosestNode(List<Node> nodes)
        {
            Dictionary<Node, int> closestNode = new Dictionary<Node, int>();
            for (int i = 0; i < nodes.Count; i++)
            {
                closestNode.Add(nodes[i], DistanceBetweenNodeAndPlayer(nodes[i]));
            }
            Node closestNodeFound = (from n in closestNode orderby n.Value ascending select n.Key).First();
            return new Point((int)(closestNodeFound.Position.X - 5) / 10, (int)(closestNodeFound.Position.Z - 5) / 10);
        }

        public int GetPlayerHealth() { return health; }

        public void SetPlayerHealth(int newHealth) { health = newHealth; }

        public void RemovePlayerHealth(int decrement) { health = health - decrement; }


        int DistanceBetweenNodeAndPlayer(Node inNode)
        {
            int Px = (int)cameraPosition.X;
            int Pz = (int)cameraPosition.Z;
            int Nx = (int)inNode.Position.X;
            int Nz = (int)inNode.Position.Z;

            double distance = Math.Sqrt(((Px - Nx) * (Px - Nx)) + ((Pz - Nz) * (Pz - Nz)));
            return (int)distance;
        }

        //Properties
        public Vector3 Position
        {
            get { return cameraPosition; }
            set
            {
                cameraPosition = value;
                UpdateLookAt();
            }
        }

        void UpdateBoundingBox()
        {
            cameraBox = new BoundingBox(cameraPosition + new Vector3(-0.4f, -0.4f, -0.4f),
                cameraPosition + new Vector3(0.4f, 0.4f, 0.4f));
        }

        public Vector3 Rotation
        {
            get { return cameraRotation; }
            set
            {
                cameraRotation = value;
                UpdateLookAt();
            }
        }

        public Vector3 GetModelRotation(Vector3 rotation) { return rotation; }

        public Matrix Projection
        {
            get;
            protected set;
        }

        public Matrix View
        {
            get
            {
                return Matrix.CreateLookAt(cameraPosition, cameraLookAt, Vector3.Up);
            }
        }

        //Set camera pos]ition and rotation
        private void MoveTo(Vector3 position, Vector3 rotation)
        {
            Position = position;
            Rotation = rotation;
            
        }

        //Update the lookat vector
        private void UpdateLookAt()
        {
            Matrix rotationMatrix = Matrix.CreateRotationX(cameraRotation.X) * Matrix.CreateRotationY(cameraRotation.Y);

            //Create a lookat offset vector
            lookAtOffset = Vector3.Transform(Vector3.UnitZ, rotationMatrix);
            //Update the camera's look at vector
            cameraLookAt = cameraPosition + lookAtOffset;
        }

        //Simulate movement
        private Vector3 PreviewMove(Vector3 amount)
        {
            //Create a rotation matrix
            Matrix rotate = Matrix.CreateRotationY(cameraRotation.Y);
            //Create a movement vector
            Vector3 movement = new Vector3(amount.X, amount.Y, amount.Z);
            movement = Vector3.Transform(movement, rotate);
            //Return the value of the camera position
            comparisonVector = cameraPosition;
            return cameraPosition + movement;
        }

        //Actually move the camera
        private void Move(Vector3 scale)
        {
            MoveTo(PreviewMove(scale), Rotation);
        }

        //Update method
        public void UpdateCamera(GameTime gameTime)
        {
            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            
            currentMouseState = Mouse.GetState();

            KeyboardState state = Keyboard.GetState();
            //Handle key presses:
            Vector3 moveVector = Vector3.Zero;
            moveScale = 1;
            if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
                moveScale = 2f;
            if (state.IsKeyDown(Keys.W))
            {
                moveVector.Z = 5;
            }
            if (state.IsKeyDown(Keys.S))
            {
                moveVector.Z  = -5;
            }
            if (state.IsKeyDown(Keys.A))
            {
                moveVector.X = 5;
            }
            if (state.IsKeyDown(Keys.D))
            {
                moveVector.X = -5;
            }

            if (moveVector != Vector3.Zero)
            {
                //Normalise the vector - to stop us moving faster diagonally
                moveVector.Normalize();
                //Add smooth and speed
                moveVector *= deltaTime * cameraSpeed;

                //Move camera
                Move(moveVector);

            }
            UpdateBoundingBox();
            if (cameraMouseEnabled)
            {
                //Handle mouse movement
                float deltaX;
                float deltaY;

                if (currentMouseState != previousMouseState)
                {
                    //Save mouse location
                    deltaX = currentMouseState.X - (Game.GraphicsDevice.Viewport.Width / 2);
                    deltaY = currentMouseState.Y - (Game.GraphicsDevice.Viewport.Height / 2);

                    //Determines the speed of the rotation 0.1 is pretty quick and looks just about right
                    mouseRotationBuffer.X -= 0.1f * deltaX * deltaTime;
                    mouseRotationBuffer.Y -= 0.1f * deltaY * deltaTime;

                    if (mouseRotationBuffer.Y < MathHelper.ToRadians(-75.0f))
                    {
                        mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(-75.0f));
                    }
                    if (mouseRotationBuffer.Y > MathHelper.ToRadians(75.0f))
                    {
                        mouseRotationBuffer.Y = mouseRotationBuffer.Y - (mouseRotationBuffer.Y - MathHelper.ToRadians(75.0f));
                    }

                    Rotation = new Vector3(-MathHelper.Clamp(mouseRotationBuffer.Y, MathHelper.ToRadians(-75.0f), MathHelper.ToRadians(75.0f)),
                        MathHelper.WrapAngle(mouseRotationBuffer.X), 0);

                    deltaX = 0;
                    deltaY = 0;
                }

                Mouse.SetPosition(Game.GraphicsDevice.Viewport.Width / 2, Game.GraphicsDevice.Viewport.Height / 2);
            }
            //Debugging, allows player to toggle the camera from setting cursor position
            if (Keyboard.GetState().IsKeyDown(Keys.K) && _prevstate != state)
            {
                cameraMouseEnabled = !cameraMouseEnabled;
            }
            previousMouseState = currentMouseState;
            _prevstate = state;
            base.Update(gameTime);
        }


    }
    
}
