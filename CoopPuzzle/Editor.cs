using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle
{
    internal class Editor
    {
        MouseState mouse, prevMouse;
        KeyboardState board, prevBoard;

        Rectangle ghostRectangle = new Rectangle(0,0,32,32);

        public Editor()
        {

        }

        public void Update(ref List<GameObject> objects, Vector2 camera)
        {
            prevMouse = mouse;
            mouse = Mouse.GetState();
            prevBoard = board;
            board = Keyboard.GetState();

            int extraX = (mouse.X < -camera.X) ? 1 : 0;
            int extraY = (mouse.Y < -camera.Y) ? 1 : 0;
            Debug.WriteLine(extraX);

            ghostRectangle.X = ((mouse.X + (int)camera.X) / 32 - extraX) * 32;
            ghostRectangle.Y = ((mouse.Y + (int)camera.Y) / 32 - extraY) * 32;

            int scroll = mouse.ScrollWheelValue - prevMouse.ScrollWheelValue;
            if (board.IsKeyDown(Keys.LeftControl))
                ghostRectangle.Height += 32 * Math.Clamp(scroll, -1, 1);
            else
                ghostRectangle.Width += 32 * Math.Clamp(scroll, -1, 1);

            if (ghostRectangle.Height < 32)
                ghostRectangle.Height = 32;
            if (ghostRectangle.Width < 32)
                ghostRectangle.Width = 32;

            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
                objects.Add(new Block(new Vector2(ghostRectangle.X, ghostRectangle.Y), new Vector2(ghostRectangle.Width, ghostRectangle.Height), Color.White));
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(Assets.white, ghostRectangle, Color.White * 0.5f);
        }
    }

}
