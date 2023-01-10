using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoopPuzzle.Npc;

namespace CoopPuzzle
{
    internal class Editor
    {
        MouseState mouse, prevMouse;
        KeyboardState board, prevBoard;

        Rectangle ghostRectangle = new Rectangle(0,0,Assets.tileSize,Assets.tileSize);

        int HUDHeight = Assets.ScreenHeight - Assets.tileSize * 2;
        List<GameObject> HUDobjects;
        List<NPC> HUDNpcs;

        string placeType = "Block";
        int id, currentColor;
        bool canPlace;

        public Editor()
        {
            HUDobjects = new List<GameObject>()
            {
                new Block(new Vector2(Assets.tileSize * 1, HUDHeight), Vector2.One * Assets.tileSize, Color.White),
                new Door(new Vector2(Assets.tileSize * 2, HUDHeight), Color.Green, -1),
                new MovableBlock(new Vector2(Assets.tileSize * 3, HUDHeight), Vector2.One * Assets.tileSize, Color.White),
                new Trap(new Vector2(Assets.tileSize * 4, HUDHeight), Color.White),
                new WeighedSwitch(new Vector2(Assets.tileSize * 5, HUDHeight), Color.White, -1),
                new CheckPoint(new Vector2(Assets.tileSize * 6, HUDHeight), Vector2.One * Assets.tileSize, Color.White)
            };
            HUDNpcs= new List<NPC>()
            {
                new StoryNpc(new Vector2(550,400), 1),
                new HintNpc(new Vector2(900,500), 1),
                new HiddenNpc(new Vector2(900,300),1,1),
                new HiddenNpc(new Vector2(700,400),2,1),
                new HiddenNpc(new Vector2(700,100),3,1),
            };
        }

        public void Update(ref List<GameObject> objects, ref List<NPC> npcs, Player[] players, Vector2 camera)
        {
            prevMouse = mouse;
            mouse = Mouse.GetState();
            prevBoard = board;
            board = Keyboard.GetState();

            int extraX = (mouse.X < -camera.X) ? 1 : 0;
            int extraY = (mouse.Y < -camera.Y) ? 1 : 0;

            ghostRectangle.X = ((mouse.X + (int)camera.X) / Assets.tileSize - extraX) * Assets.tileSize;
            ghostRectangle.Y = ((mouse.Y + (int)camera.Y) / Assets.tileSize - extraY) * Assets.tileSize;

            int scroll = mouse.ScrollWheelValue - prevMouse.ScrollWheelValue;
            if (placeType == "Door" || placeType == "WeighedSwitch")
            {
                ghostRectangle.Size = new Point(Assets.tileSize, Assets.tileSize);
                id += Math.Clamp(scroll, -1, 1);
            }
            else if(placeType == "Block" || placeType == "MovableBlock" || placeType == "CheckPoint")
            {
                if (board.IsKeyDown(Keys.LeftShift))
                    currentColor += Math.Clamp(scroll, -1, 1);
                else
                {
                    if (board.IsKeyDown(Keys.LeftControl))
                        ghostRectangle.Height += Assets.tileSize * Math.Clamp(scroll, -1, 1);
                    else
                        ghostRectangle.Width += Assets.tileSize * Math.Clamp(scroll, -1, 1);
                }

                if (currentColor >= Assets.colors.Length)
                    currentColor = 0;
                if (currentColor < 0)
                    currentColor = Assets.colors.Length - 1;

                if (ghostRectangle.Height < Assets.tileSize)
                    ghostRectangle.Height = Assets.tileSize;
                if (ghostRectangle.Width < Assets.tileSize)
                    ghostRectangle.Width = Assets.tileSize;
            }

            if (mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released && canPlace)
            {
                for (int i = 0; i < HUDobjects.Count; i++)
                {
                    if (HUDobjects[i].HUDhitbox.Contains(mouse.Position))
                    {
                        if (placeType.Contains("Npc"))
                        {
                            currentColor = 0;
                            id = 0;
                        }

                        placeType = HUDobjects[i].GetType().Name;
                        return;
                    }
                }
                for (int i = 0; i < npcs.Count; i++)
                {
                    if (npcs[i].Range.Contains(mouse.Position))
                    {
                        placeType = npcs[i].GetType().Name;
                        if (placeType == "HiddenNpc")
                        {
                            HiddenNpc hidden = (HiddenNpc)npcs[i];
                            //placeType = "HiddenNpc " + hidden.Npc;
                            currentColor = hidden.Npc;
                        }
                        return;
                    }
                }

                switch (placeType)
                {
                    case "Block":
                        objects.Add(new Block(new Vector2(ghostRectangle.X, ghostRectangle.Y), new Vector2(ghostRectangle.Width, ghostRectangle.Height), Assets.colors[currentColor]));
                        break;
                    case "Door":
                        objects.Add(new Door(new Vector2(ghostRectangle.X, ghostRectangle.Y), Assets.colors[currentColor], id));
                        break;
                    case "MovableBlock":
                        objects.Add(new MovableBlock(new Vector2(ghostRectangle.X, ghostRectangle.Y), new Vector2(ghostRectangle.Width, ghostRectangle.Height), Assets.colors[currentColor]));
                        break;
                    case "Trap":
                        objects.Add(new Trap(new Vector2(ghostRectangle.X, ghostRectangle.Y), Assets.colors[currentColor]));
                        break;
                    case "WeighedSwitch":
                        objects.Add(new WeighedSwitch(new Vector2(ghostRectangle.X, ghostRectangle.Y), Assets.colors[currentColor], id));
                        break;
                    case "CheckPoint":
                        objects.Add(new CheckPoint(new Vector2(ghostRectangle.X, ghostRectangle.Y), new Vector2(ghostRectangle.Width, ghostRectangle.Height), Assets.colors[currentColor]));
                        break;
                    case "HintNpc":
                        npcs.Add(new HintNpc(new Vector2(ghostRectangle.X, ghostRectangle.Y), 0));
                        break;
                    case "StoryNpc":
                        npcs.Add(new StoryNpc(new Vector2(ghostRectangle.X, ghostRectangle.Y), 0));
                        break;
                    case "HiddenNpc":
                        npcs.Add(new HiddenNpc(new Vector2(ghostRectangle.X, ghostRectangle.Y), currentColor, 0));
                        break;
                }
            }

            if (mouse.RightButton == ButtonState.Pressed && prevMouse.RightButton == ButtonState.Released)
            {
                for (int i = 0; i < objects.Count; i++)
                {
                    if (objects[i].HUDhitbox.Contains(new Vector2(mouse.Position.X, mouse.Position.Y) + camera))
                    {
                        objects.RemoveAt(i);
                    }
                }
                for (int i = 0; i < npcs.Count; i++)
                {
                    if (npcs[i].Range.Contains(new Vector2(mouse.Position.X, mouse.Position.Y) + camera))
                    {
                        npcs.RemoveAt(i);
                        return;
                    }
                }
            }

            canPlace = true;
            for (int i = 0; i < objects.Count; i++)                                                           
            {
                if (objects[i].HUDhitbox.Contains(new Vector2(mouse.Position.X, mouse.Position.Y) + camera))
                {
                    objects[i].TempColor = Color.Lerp(objects[i].Color, Color.Black, 0.5f);
                    canPlace = false;
                }
                else
                    objects[i].TempColor = objects[i].Color;
            }
            //for (int i = 0; i < npcs.Count; i++)
            //{
            //    if (npcs[i].Range.Contains(new Vector2(mouse.Position.X, mouse.Position.Y) + camera))
            //        canPlace = false;
            //}

            if (board.IsKeyDown(Keys.R) && prevBoard.IsKeyUp(Keys.R))
                SaveLevel(objects, players);
        }

        public void Draw(SpriteBatch sb, Matrix transformMatrix)
        {
            sb.End();

            sb.Begin(sortMode: SpriteSortMode.Deferred, samplerState: SamplerState.PointWrap, transformMatrix: transformMatrix);

            //sb.Draw(Assets.white, ghostRectangle, colors[currentColor] * 0.5f);

            switch (placeType)
            {
                case "Block":
                    new Block(new Vector2(ghostRectangle.X, ghostRectangle.Y), new Vector2(ghostRectangle.Width, ghostRectangle.Height), Assets.colors[currentColor] * 0.5f).Draw(sb);
                    break;
                case "Door":
                    new Door(new Vector2(ghostRectangle.X, ghostRectangle.Y), Assets.colors[currentColor] * 0.5f, id).Draw(sb);
                    break;
                case "MovableBlock":
                    new MovableBlock(new Vector2(ghostRectangle.X, ghostRectangle.Y), new Vector2(ghostRectangle.Width, ghostRectangle.Height), Assets.colors[currentColor] * 0.5f).Draw(sb);
                    break;
                case "Trap":
                    new Trap(new Vector2(ghostRectangle.X, ghostRectangle.Y), Assets.colors[currentColor] * 0.5f).Draw(sb);
                    break;
                case "WeighedSwitch":
                    new WeighedSwitch(new Vector2(ghostRectangle.X, ghostRectangle.Y), Color.White, id).Draw(sb);
                    break;
                case "HintNpc":
                    new HintNpc(new Vector2(ghostRectangle.X, ghostRectangle.Y), id).Draw(sb);
                    break;
                case "StoryNpc":
                    new StoryNpc(new Vector2(ghostRectangle.X, ghostRectangle.Y), id).Draw(sb);
                    break;
                case "HiddenNpc":
                    new HiddenNpc(new Vector2(ghostRectangle.X, ghostRectangle.Y), currentColor, id).Draw(sb);
                    break;
            }

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
                int lineWidth = (HUDobjects[i].GetType().Name == placeType) ? 4 : 2;
                sb.DrawLine(HUDobjects[i].Pos, new Vector2(0, 1), Assets.tileSize, lineWidth, Color.Black);
                sb.DrawLine(HUDobjects[i].Pos, new Vector2(1, 0), Assets.tileSize, lineWidth, Color.Black);
                sb.DrawLine(new Vector2(HUDobjects[i].Pos.X + Assets.tileSize, HUDobjects[i].Pos.Y), new Vector2(0, 1), Assets.tileSize, lineWidth, Color.Black);
                sb.DrawLine(new Vector2(HUDobjects[i].Pos.X, HUDobjects[i].Pos.Y + Assets.tileSize), new Vector2(1, 0), Assets.tileSize, lineWidth, Color.Black);
            }
            sb.DrawString(Assets.font, placeType + $"\nSkin: {currentColor}", new Vector2(Assets.tileSize * 1, HUDHeight + Assets.tileSize), Color.Black);
            sb.End();

            sb.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointWrap, transformMatrix: transformMatrix);
        }

        public void SaveLevel(List<GameObject> objects, Player[] players)
        {
            JsonParser.WriteJsonToFile("../../../Content/level.json", objects, players);
        }
    }
}
