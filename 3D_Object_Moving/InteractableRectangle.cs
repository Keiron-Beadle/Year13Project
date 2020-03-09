using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace _3D_Object_Moving
{
    public class InteractableRectangle
    {
        int timer; //Timer for checking how long mouse is over the box. 
        Texture2D rectTex; //Texture
        Rectangle rect; //Rectangle

        public int GetTimer() { return timer; } //Getter for timer (used in fading the box)
        public void SetTimer(int i) { timer= timer + i; } //Sets the timer (used for fading)

        public Rectangle GetRectangle() { return rect; } //Getter for the rectangle, used in checking for collision with mouse.

        public InteractableRectangle(Rectangle inRect, Texture2D inRectTex, int inTimer)
        {
            timer = inTimer;
            rectTex = inRectTex;
            rect = inRect;

        } 

        //Draws texture. And uses timer to change opacity.
        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(rectTex, rect, Color.White * (0.5f * ((float)timer / (float)25)));
        }
        //Draws the text on the box. 
        public void DrawText(SpriteBatch spriteBatch, string inString, SpriteFont inFont)
        {
            spriteBatch.DrawString(inFont, inString, new Vector2(rect.X + 120, rect.Y + 28), Color.White);
        }


    }
}
