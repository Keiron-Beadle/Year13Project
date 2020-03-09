using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;


namespace _3D_Object_Moving
{
    class Items : GameModel
    {
        const float CUBESIZE = 0.47f;
        float SCALE = 0.02f;
        Matrix rotationMatrix;
        SpriteBatch TwoDimensionItemDraw;
        ItemState _state = new ItemState();

        enum ItemState
        {
            Fixed,
            Dynamic,
        }

        public void SetRotationMatrix(Matrix rotationX, Matrix rotationY) { rotationMatrix = rotationX * rotationY; }

        public Items(Vector3 inPosition, GraphicsDevice inDevice, Grid inGrid, Model inMod)
            :base(inPosition, inDevice, inGrid, inMod)
        {
            device = inDevice;
            grid = inGrid;
            position = inPosition;
            SetUpVertices(new Vector3(-0.2f, 0, -0.14f), CUBESIZE, CUBESIZE, Color.Orange);
            SetUpIndices();
            world = Matrix.CreateTranslation(0, 0, 0);
            _state = ItemState.Fixed;
            TwoDimensionItemDraw = new SpriteBatch(device);
            bEffect = new BasicEffect(device)
            {
                World = world,
                VertexColorEnabled = true
            };
        }

        public void SetPosition(Vector3 newPosition)
        {
            position = newPosition;
            _state = ItemState.Dynamic;
             SCALE = 0.02f;
        }

        public void Render(Matrix view, Matrix projection, int index)
        {
            bEffect.View = view;
            bEffect.Projection = projection;

            if (_state == ItemState.Fixed)
            {
                foreach (var mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.World = Matrix.CreateScale(SCALE, SCALE, SCALE) * Matrix.CreateRotationX(MathHelper.ToRadians(10)) * Matrix.CreateTranslation(position);
                        effect.View = view;
                        effect.Projection = projection;
                    }
                    mesh.Draw();
                }
            }
            else if (_state == ItemState.Dynamic)
            {               
                foreach (var mesh in model.Meshes)
                {
                    foreach (BasicEffect effect in mesh.Effects)
                    {
                        effect.EnableDefaultLighting();
                        effect.PreferPerPixelLighting = true;
                        effect.World = Matrix.CreateScale(SCALE, SCALE, SCALE) * rotationMatrix * 
                            Matrix.CreateTranslation(position);
                        effect.View = view;
                        effect.Projection = projection;
                    }
                    mesh.Draw();
                }
            }


            device.SetVertexBuffer(vBuffer);
            device.Indices = IBuffer;
            
             //foreach (EffectPass pass in bEffect.CurrentTechnique.Passes)
             //{
             //    pass.Apply();
             //    device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, 12);
           // }
        }
    }
}
