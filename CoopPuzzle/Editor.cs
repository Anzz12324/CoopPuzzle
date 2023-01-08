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

        List<GameObject> HUDobjects = new List<GameObject>()
        {
            new Block(new Vector2(128, 640), Vector2.One * 32, Color.White),
            new Door(new Vector2(256, 640), Color.Green, -1),
            new MovableBlock(new Vector2(384, 640), Color.SaddleBrown),
            new Trap(new Vector2(512, 640), Color.White),
            new WeighedSwitch(new Vector2(640, 640), Color.White, -1)
        };

        string placeType = "Block";
        int id = 0;

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

            if (board.IsKeyDown(Keys.R) && prevBoard.IsKeyUp(Keys.R))
                id++;
            if (board.IsKeyDown(Keys.F) && prevBoard.IsKeyUp(Keys.F))
                id--;

            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released)
            {
                for (int i = 0; i < HUDobjects.Count; i++)
                {
                    if (HUDobjects[i].hitbox.Contains(mouse.Position))
                    {
                        placeType = HUDobjects[i].GetType().Name;
                        return;
                    }
                }
                switch (placeType)
                {
                    case "Block":
                        objects.Add(new Block(new Vector2(ghostRectangle.X, ghostRectangle.Y), new Vector2(ghostRectangle.Width, ghostRectangle.Height), Color.White));
                        break;
                    case "Door":
                        objects.Add(new Door(new Vector2(ghostRectangle.X, ghostRectangle.Y), Color.Green, id));
                        break;
                    case "MovableBlock":
                        objects.Add(new MovableBlock(new Vector2(ghostRectangle.X, ghostRectangle.Y), Color.SaddleBrown));
                        break;
                    case "Trap":
                        objects.Add(new Trap(new Vector2(ghostRectangle.X, ghostRectangle.Y), Color.White));
                        break;
                    case "WeighedSwitch":
                        objects.Add(new WeighedSwitch(new Vector2(ghostRectangle.X, ghostRectangle.Y), Color.White, id));
                        break;

                }
            }

            if (mouse.RightButton == ButtonState.Pressed && prevMouse.RightButton == ButtonState.Released)
            {
                for (int i = 0; i < objects.Count; i++)
                {
                    if (objects[i].hitbox.Contains(new Vector2(mouse.Position.X, mouse.Position.Y) + camera))
                    {
                        objects.RemoveAt(i);
                    }
                }
            }
        }

        public void Draw(SpriteBatch sb, Matrix transformMatrix)
        {
            //sb.Draw(Assets.white, ghostRectangle, Color.White * 0.5f);
            sb.DrawLine(new Vector2(ghostRectangle.Left, ghostRectangle.Top), new Vector2(ghostRectangle.Right, ghostRectangle.Top), 1, Color.Black);
            sb.DrawLine(new Vector2(ghostRectangle.Left, ghostRectangle.Top), new Vector2(ghostRectangle.Left, ghostRectangle.Bottom), 1, Color.Black);
            sb.DrawLine(new Vector2(ghostRectangle.Left, ghostRectangle.Bottom), new Vector2(ghostRectangle.Right, ghostRectangle.Bottom), 1, Color.Black);
            sb.DrawLine(new Vector2(ghostRectangle.Right, ghostRectangle.Bottom), new Vector2(ghostRectangle.Right, ghostRectangle.Top), 1, Color.Black);
            if (placeType == "Door" || placeType == "WeighedSwitch")
                sb.DrawString(Assets.font, id.ToString(), new Vector2(ghostRectangle.X, ghostRectangle.Y - 16), Color.Black); 
            sb.End();

            sb.Begin(samplerState: SamplerState.PointWrap);
            for (int i = 0; i < HUDobjects.Count; i++)
            {
                HUDobjects[i].Draw(sb);
                sb.DrawLine(new Vector2(HUDobjects[i].hitbox.Left, HUDobjects[i].hitbox.Top), new Vector2(HUDobjects[i].hitbox.Right, HUDobjects[i].hitbox.Top), 1, Color.Black);
                sb.DrawLine(new Vector2(HUDobjects[i].hitbox.Left, HUDobjects[i].hitbox.Top), new Vector2(HUDobjects[i].hitbox.Left, HUDobjects[i].hitbox.Bottom), 1, Color.Black);
                sb.DrawLine(new Vector2(HUDobjects[i].hitbox.Left, HUDobjects[i].hitbox.Bottom), new Vector2(HUDobjects[i].hitbox.Right, HUDobjects[i].hitbox.Bottom), 1, Color.Black);
                sb.DrawLine(new Vector2(HUDobjects[i].hitbox.Right, HUDobjects[i].hitbox.Bottom), new Vector2(HUDobjects[i].hitbox.Right, HUDobjects[i].hitbox.Top), 1, Color.Black);
            }
            sb.End();

            sb.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointWrap, transformMatrix: transformMatrix);
        }
    }

}
