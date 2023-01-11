using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoopPuzzle.Npc;
using static System.Net.Mime.MediaTypeNames;

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
        Rectangle bgButton;

        string placeType = "Block";
        int id, currentColor;
        bool canPlace;

        public Editor()
        {
            HUDobjects = new List<GameObject>()
            {
                new Block(new Vector2(Assets.tileSize * 1, HUDHeight), Vector2.One * Assets.tileSize, Color.White),
                new Door(new Vector2(Assets.tileSize * 2, HUDHeight), Color.White, -1, 0),
                new MovableBlock(new Vector2(Assets.tileSize * 3, HUDHeight), Vector2.One * Assets.tileSize, Color.White),
                new Trap(new Vector2(Assets.tileSize * 4, HUDHeight), Color.White, -1),
                new WeighedSwitch(new Vector2(Assets.tileSize * 5, HUDHeight), Color.White, -1),
                new CheckPoint(new Vector2(Assets.tileSize * 6, HUDHeight), Vector2.One * Assets.tileSize, Color.White)
            };
            HUDNpcs = new List<NPC>()
            {
                new StoryNpc(new Vector2(Assets.tileSize * 8, HUDHeight), 1),
                new HintNpc(new Vector2(Assets.tileSize * 9, HUDHeight), 1),
                new HiddenNpc(new Vector2(Assets.tileSize * 10, HUDHeight), 1, 1),
                new HiddenNpc(new Vector2(Assets.tileSize * 11, HUDHeight), 2, 1),
                new HiddenNpc(new Vector2(Assets.tileSize * 12, HUDHeight), 3, 1),
            };
            for (int i = 0; i < HUDNpcs.Count; i++)
            {
                HUDNpcs[i].IsButton();
            }

            bgButton = new Rectangle(Assets.tileSize * 14, HUDHeight, Assets.tileSize, Assets.tileSize);
        }

        public void Update(ref List<GameObject> objects, ref List<NPC> npcs, ref List<BGTile> bgtiles, Player[] players, Vector2 camera)
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
            if (placeType == "WeighedSwitch" || placeType.Contains("Npc") || placeType == "Trap")
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
            }
            else if (placeType == "Door")
            {
                ghostRectangle.Size = new Point(Assets.tileSize, Assets.tileSize);

                if (board.IsKeyDown(Keys.LeftShift))
                    currentColor += Math.Clamp(scroll, -1, 1);
                else
                    id += Math.Clamp(scroll, -1, 1);

            }
            else if (placeType == "BGTile")
            {
                if (board.IsKeyDown(Keys.LeftShift))
                    id += Math.Clamp(scroll, -1, 1);
                else
                {
                    if (board.IsKeyDown(Keys.LeftControl))
                        ghostRectangle.Height += Assets.tileSize * Math.Clamp(scroll, -1, 1);
                    else
                        ghostRectangle.Width += Assets.tileSize * Math.Clamp(scroll, -1, 1);
                }
            }

            if (currentColor >= Assets.colors.Length)
                currentColor = 0;
            if (currentColor < 0)
                currentColor = Assets.colors.Length - 1;

            if (ghostRectangle.Height < Assets.tileSize)
                ghostRectangle.Height = Assets.tileSize;
            if (ghostRectangle.Width < Assets.tileSize)
                ghostRectangle.Width = Assets.tileSize;

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
                for (int i = 0; i < HUDNpcs.Count; i++)
                {
                    if (HUDNpcs[i].Range.Contains(mouse.Position))
                    {
                        if (!placeType.Contains("Npc"))
                            id = 0;

                        placeType = HUDNpcs[i].GetType().Name;
                        if (placeType == "HiddenNpc")
                        {
                            HiddenNpc hidden = (HiddenNpc)HUDNpcs[i];
                            currentColor = hidden.Npc;
                        }
                        return;
                    }
                }

                if (bgButton.Contains(mouse.Position))
                {
                    placeType = "BGTile";
                    id = 0;
                    return;
                }

                switch (placeType)
                {
                    case "Block":
                        objects.Add(new Block(new Vector2(ghostRectangle.X, ghostRectangle.Y), new Vector2(ghostRectangle.Width, ghostRectangle.Height), Assets.colors[currentColor]));
                        break;
                    case "Door":
                        objects.Add(new Door(new Vector2(ghostRectangle.X, ghostRectangle.Y), Color.White, id, currentColor));
                        break;
                    case "MovableBlock":
                        objects.Add(new MovableBlock(new Vector2(ghostRectangle.X, ghostRectangle.Y), new Vector2(ghostRectangle.Width, ghostRectangle.Height), Assets.colors[currentColor]));
                        break;
                    case "Trap":
                        objects.Add(new Trap(new Vector2(ghostRectangle.X, ghostRectangle.Y), Assets.colors[currentColor], id));
                        break;
                    case "WeighedSwitch":
                        objects.Add(new WeighedSwitch(new Vector2(ghostRectangle.X, ghostRectangle.Y), Assets.colors[currentColor], id));
                        break;
                    case "CheckPoint":
                        objects.Add(new CheckPoint(new Vector2(ghostRectangle.X, ghostRectangle.Y), new Vector2(ghostRectangle.Width, ghostRectangle.Height), Assets.colors[currentColor]));
                        break;
                    case "HintNpc":
                        npcs.Add(new HintNpc(new Vector2(ghostRectangle.X, ghostRectangle.Y), id));
                        break;
                    case "StoryNpc":
                        npcs.Add(new StoryNpc(new Vector2(ghostRectangle.X, ghostRectangle.Y), id));
                        break;
                    case "HiddenNpc":
                        npcs.Add(new HiddenNpc(new Vector2(ghostRectangle.X, ghostRectangle.Y), currentColor, id));
                        break;
                }
            }

            if(mouse.LeftButton == ButtonState.Pressed && prevMouse.LeftButton == ButtonState.Released && placeType == "BGTile")
            {
                for (int i = 0; i < ghostRectangle.Width; i += 40)
                {
                    for (int j = 0; j < ghostRectangle.Height; j += 40)
                    {
                        Vector2 pos = new Vector2(ghostRectangle.X + i, ghostRectangle.Y + j);
                        for (int k = 0; k < bgtiles.Count; k++)
                        {
                            if (bgtiles[k].Pos == pos)
                                bgtiles.RemoveAt(k);
                        }
                        bgtiles.Add(new BGTile(pos, id % 2, Assets.random.Next(10)));
                    }
                }
            }

            if (mouse.RightButton == ButtonState.Pressed && prevMouse.RightButton == ButtonState.Released)
            {
                for (int i = 0; i < objects.Count; i++)
                {
                    if (objects[i].HUDhitbox.Contains(new Vector2(mouse.Position.X, mouse.Position.Y) + camera))
                    {
                        objects.RemoveAt(i);
                        return;
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

            if (mouse.RightButton == ButtonState.Pressed && placeType == "BGTile")
            {
                for (int i = 0; i < bgtiles.Count; i++)
                {
                    if (new Rectangle((int)bgtiles[i].Pos.X, (int)bgtiles[i].Pos.Y, Assets.tileSize, Assets.tileSize).Contains(new Vector2(mouse.Position.X, mouse.Position.Y) + camera))
                        bgtiles.RemoveAt(i);
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
            for (int i = 0; i < npcs.Count; i++)
            {
                if (npcs[i].Range.Contains(new Vector2(mouse.Position.X, mouse.Position.Y) + camera))
                    canPlace = false;
            }

            if (board.IsKeyDown(Keys.R) && prevBoard.IsKeyUp(Keys.R))
                SaveLevel(objects, bgtiles, players, npcs);
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
                    //sb.Draw(Assets.white, ghostRectangle, Color.SaddleBrown);
                    new Door(new Vector2(ghostRectangle.X, ghostRectangle.Y), Color.White * 0.5f, id, currentColor).Draw(sb);
                    break;
                case "MovableBlock":
                    new MovableBlock(new Vector2(ghostRectangle.X, ghostRectangle.Y), new Vector2(ghostRectangle.Width, ghostRectangle.Height), Assets.colors[currentColor] * 0.5f).Draw(sb);
                    break;
                case "Trap":
                    new Trap(new Vector2(ghostRectangle.X, ghostRectangle.Y), Assets.colors[currentColor] * 0.5f, id).Draw(sb);
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

            sb.DrawRectangle(ghostRectangle, Color.Black, 1, 1);

            if (placeType == "Door" || placeType == "WeighedSwitch" || placeType.Contains("Npc") || placeType == "Trap" || placeType == "BGTile")
                sb.DrawString(Assets.font, id.ToString(), new Vector2(ghostRectangle.X, ghostRectangle.Y - 16), Color.Black); 
            sb.End();

            sb.Begin(samplerState: SamplerState.PointWrap);
            for (int i = 0; i < HUDobjects.Count; i++)
            {
                HUDobjects[i].Draw(sb);
                int lineWidth = (HUDobjects[i].GetType().Name == placeType) ? 3 : 1;
                sb.DrawRectangle(new Rectangle((int)HUDobjects[i].Pos.X, (int)HUDobjects[i].Pos.Y, Assets.tileSize, Assets.tileSize), Color.Black, lineWidth);
            }
            for (int i = 0; i < HUDNpcs.Count; i++)
            {
                int lineWidth = (HUDNpcs[i].GetType().Name == placeType) ? 3 : 1;
                if(HUDNpcs[i].GetType().Name == "HiddenNpc" && placeType == "HiddenNpc")
                    lineWidth = (currentColor == HUDNpcs[i].Npc) ? 3 : 1;

                sb.DrawRectangle(HUDNpcs[i].Range, Color.Black, lineWidth, 1);

                string npcName = "";
                string[] hiddenNames = new string[] { "", "nice", "asgo", "grill" };
                if (HUDNpcs[i].GetType().Name == "StoryNpc")
                    npcName = "flow";
                else if (HUDNpcs[i].GetType().Name == "HintNpc")
                    npcName = "mett";
                else if (HUDNpcs[i].GetType().Name == "HiddenNpc")
                {
                    HiddenNpc hidden = (HiddenNpc)HUDNpcs[i];
                    npcName = hiddenNames[hidden.Npc];
                }

                sb.DrawString(Assets.font, npcName, new Vector2(HUDNpcs[i].Range.X + 2, HUDNpcs[i].Range.Y), Color.Black);
            }

            sb.DrawRectangle(bgButton, Color.Black, (placeType == "BGTile") ? 3 : 1, 1);
            sb.DrawString(Assets.font, "BG\nTile", new Vector2(bgButton.X + 2, bgButton.Y), Color.Black);

            sb.DrawString(Assets.font, placeType, new Vector2(Assets.tileSize * 1, HUDHeight + Assets.tileSize), Color.Black);
            sb.End();

            sb.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointWrap, transformMatrix: transformMatrix);
        }

        public void SaveLevel(List<GameObject> objects, List<BGTile> bgtiles, Player[] players, List<NPC> npcs)
        {
            JsonParser.WriteJsonToFile("../../../Content/level.json", objects, bgtiles, players, npcs);
        }
    }
}
