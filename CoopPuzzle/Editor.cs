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

        Rectangle ghostRectangle = new Rectangle(0,0,40,40);

        List<GameObject> HUDobjects = new List<GameObject>()
        {
            new Block(new Vector2(256, 640), Vector2.One * 40, Color.White),
            new Door(new Vector2(256 + 45, 640), Color.Green, -1),
            new MovableBlock(new Vector2(256 + 45 * 2, 640), Color.SaddleBrown),
            new Trap(new Vector2(256 + 45 * 3, 640), Color.White),
            new WeighedSwitch(new Vector2(256 + 45 * 4, 640), Color.White, -1)
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

            ghostRectangle.X = ((mouse.X + (int)camera.X) / 40 - extraX) * 40;
            ghostRectangle.Y = ((mouse.Y + (int)camera.Y) / 40 - extraY) * 40;

            int scroll = mouse.ScrollWheelValue - prevMouse.ScrollWheelValue;
            if (placeType == "Door" || placeType == "WeighedSwitch")
            {
                ghostRectangle.Size = new Point(40, 40);
                id += Math.Clamp(scroll, -1, 1);
            }
            else
            {
                if (board.IsKeyDown(Keys.LeftControl))
                    ghostRectangle.Height += 40 * Math.Clamp(scroll, -1, 1);
                else
                    ghostRectangle.Width += 40 * Math.Clamp(scroll, -1, 1);
            }

            if (ghostRectangle.Height < 40)
                ghostRectangle.Height = 40;
            if (ghostRectangle.Width < 40)
                ghostRectangle.Width = 40;

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

            //for (int i = 0; i < objects.Count; i++)                                                           //<--Ta bort kommentar när vi lagt till texturer
            //{
            //    if (objects[i].hitbox.Contains(new Vector2(mouse.Position.X, mouse.Position.Y) + camera))
            //        objects[i].setColor = Color.Red;
            //    else
            //        objects[i].setColor = Color.White;
            //}
        }

        public void Draw(SpriteBatch sb, Matrix transformMatrix)
        {
            sb.End();
            sb.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointWrap, transformMatrix: transformMatrix);
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
                int lineWidth = (HUDobjects[i].GetType().Name == placeType) ? 3 : 1;
                sb.DrawLine(HUDobjects[i].Pos, new Vector2(0, 1), 40, lineWidth, Color.Black);
                sb.DrawLine(HUDobjects[i].Pos, new Vector2(1, 0), 40, lineWidth, Color.Black);
                sb.DrawLine(new Vector2(HUDobjects[i].Pos.X + 40, HUDobjects[i].Pos.Y), new Vector2(0, 1), 40, lineWidth, Color.Black);
                sb.DrawLine(new Vector2(HUDobjects[i].Pos.X, HUDobjects[i].Pos.Y + 40), new Vector2(1, 0), 40, lineWidth, Color.Black);
            }
            sb.End();

            sb.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointWrap, transformMatrix: transformMatrix);
        }
    }

}
