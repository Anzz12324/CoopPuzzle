global using System;
global using System.IO;
global using System.Diagnostics;
global using System.Collections.Generic;
global using Microsoft.Xna.Framework;
global using Microsoft.Xna.Framework.Graphics;
global using Microsoft.Xna.Framework.Input;
global using Microsoft.Xna.Framework.Content;
global using LiteNetLib;
global using LiteNetLib.Utils;
global using MonoGame.Extended.Sprites;
global using MonoGame.Extended.Content;
global using MonoGame.Extended.Serialization;
global using MonoGame.Extended.Timers;
global using MonoGame.Extended;
global using MonoGame.Extended.ViewportAdapters;
global using CoopPuzzle.Npc;

namespace CoopPuzzle
{

    public class Game1 : Game
    {
        bool active = false, host = false, connected = false, editmodePlayer = false, netStats = false, fps = true, EditmodeUI = true;
        NetManager netManager;
        enum DiffCam { SnapMove, FullScreenMove, FollowPlayer }
        DiffCam diffCam = DiffCam.SnapMove;

        Player player, otherPlayer;
        List<GameObject> objects;
        List<NPC> npcs;

        RenderTarget2D renderTarget;

        Editor editor;
        SoundManager sound;

        KeyboardState kbState, kbPreviousState;

        public OrthographicCamera camera { get; private set; }
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Color[] colorData;

        private int ping;
        public string ip, password;
        public int port;
        public bool editmode = false;
        public Game1()
        {
            Assets.ScreenWidth = 1280;
            Assets.ScreenHeight = 720;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = Assets.ScreenWidth;
            graphics.PreferredBackBufferHeight = Assets.ScreenHeight;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            TargetElapsedTime = TimeSpan.FromSeconds(1f / 144f);
            renderTarget = new RenderTarget2D(graphics.GraphicsDevice, Assets.ScreenWidth, Assets.ScreenHeight);
            colorData = new Color[Assets.ScreenWidth * Assets.ScreenHeight];

            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, Assets.ScreenWidth, Assets.ScreenHeight);
            camera = new OrthographicCamera(viewportAdapter);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            DebugDraw.Init(GraphicsDevice);
            Assets.LoadTextures(Content);
            editor = new Editor();
            sound = new SoundManager();
            npcs = new List<NPC>();
            objects = new List<GameObject>();

            LoadLevel();
        }

        private void Shotcuts()
        {
            kbPreviousState = kbState;
            kbState = Keyboard.GetState();
            if (kbState.IsKeyDown(Keys.Escape))
            {
                netManager?.Stop();
                Exit();
            }
            if (kbState.IsKeyDown(Keys.L) && kbPreviousState.IsKeyUp(Keys.L))
                editmodePlayer = !editmodePlayer;
            if (kbState.IsKeyDown(Keys.I) && kbPreviousState.IsKeyUp(Keys.I))
            {
                camera.Position = Vector2.Zero;
                diffCam = DiffCam.SnapMove;
            }
            if (kbState.IsKeyDown(Keys.O) && kbPreviousState.IsKeyUp(Keys.O))
                diffCam = DiffCam.FullScreenMove;
            if (kbState.IsKeyDown(Keys.P) && kbPreviousState.IsKeyUp(Keys.P))
                diffCam = DiffCam.FollowPlayer;
            if (kbState.IsKeyDown(Keys.F2) && kbPreviousState.IsKeyUp(Keys.F2))
                fps = !fps;
            if (kbState.IsKeyDown(Keys.F3) && kbPreviousState.IsKeyUp(Keys.F3))
                netStats = !netStats;
            if (kbState.IsKeyDown(Keys.F1) && kbPreviousState.IsKeyUp(Keys.F1))
                EditmodeUI = !EditmodeUI;
        }
        protected override void Update(GameTime gameTime)
        {
            Player[] players = new Player[] { player, otherPlayer };
            Shotcuts();

            if (editmode)
            {
                editor.Update(ref objects, ref npcs, players, camera.Position);

                if (editmodePlayer)
                {
                    otherPlayer.Update(gameTime, objects, this);
                    player.UpdateOther(gameTime, objects, this);
                }
                else
                {
                    player.Update(gameTime, objects, this);
                    otherPlayer.UpdateOther(gameTime, objects, this);
                }

                UpdateObjects(gameTime, players);
            }

            if (active)
            {
                if (connected)
                {
                    player.Update(gameTime, objects, this);
                    otherPlayer.UpdateOther(gameTime, objects, this);
                }

                UpdateObjects(gameTime, players);

                NetDataWriter writer = new NetDataWriter();
                netManager.PollEvents();
                writer.PutArray(new float[] { player.Vel.X, player.Vel.Y });
                writer.PutArray(new float[] { player.Pos.X, player.Pos.Y });
                netManager.SendToAll(writer, DeliveryMethod.ReliableOrdered);
            }

            CameraMove();

            renderTarget.GetData(colorData);
            base.Update(gameTime);
        }
        protected override void Draw(GameTime gameTime)
        {
            var transformMatrix = camera.GetViewMatrix();
            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(samplerState: SamplerState.PointWrap, transformMatrix: transformMatrix);
            spriteBatch.Draw(Content.Load<Texture2D>("ExamplePNG"), new Vector2(400, 100), Color.White);
            spriteBatch.Draw(Assets.GrassTileSet, new Vector2(1280, 0), null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            spriteBatch.Draw(Assets.StoneTileSet, new Vector2(1280 + Assets.GrassTileSet.Width * 2, 0), null, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is WeighedSwitch)
                {
                    objects[i].Draw(spriteBatch);
                    if (editmode)
                    {
                        WeighedSwitch ws = (WeighedSwitch)objects[i];
                        spriteBatch.DrawString(Assets.font, ws.id.ToString(), new Vector2(ws.Pos.X, ws.Pos.Y - 16), Color.Black);
                    }
                }
            }
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(sortMode: SpriteSortMode.FrontToBack, samplerState: SamplerState.PointWrap, transformMatrix: transformMatrix);
            spriteBatch.Draw(renderTarget, camera.Position, Color.White);
            
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is not WeighedSwitch and not CheckPoint)
                    objects[i].Draw(spriteBatch);
                if (editmode)
                {
                    if (objects[i] is CheckPoint)
                        objects[i].Draw(spriteBatch);

                    if (objects[i] is Door)
                    {
                        Door door = (Door)objects[i];
                        spriteBatch.DrawString(Assets.font, door.id.ToString(), new Vector2(door.Pos.X, door.Pos.Y - 16), Color.Black);
                    }
                    if (objects[i] is Trap)
                    {
                        Trap trap = (Trap)objects[i];
                        if (trap.id != 0)
                            spriteBatch.FillRectangle(trap.hitbox, Color.Green * 0.5f);
                        spriteBatch.DrawString(Assets.font, trap.id.ToString(), new Vector2(trap.Pos.X, trap.Pos.Y - 16), Color.Black);
                    }
                }
            }

            if (!connected && !editmode)
                spriteBatch.Draw(Assets.wait, new Vector2(Assets.ScreenWidth / 2 - Assets.wait.Width / 2, Assets.ScreenHeight / 2 - Assets.wait.Height),
                    new Rectangle(0, 0, Assets.wait.Width, Assets.wait.Height), Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, 1);


            otherPlayer.Draw(spriteBatch);
            player.Draw(spriteBatch);
            for (int i = 0; i < npcs.Count; i++)
            {
                npcs[i].Draw(spriteBatch);
            }

            if (fps)
            {
                spriteBatch.FillRectangle(new Rectangle((int)camera.Position.X, (int)camera.Position.Y, 60, 20), new Color(Color.Black, 0.5f), 0.9f);
                spriteBatch.DrawString(Assets.font, $"{(int)(1 / gameTime.ElapsedGameTime.TotalSeconds)} FPS", new Vector2(camera.Position.X + 2, camera.Position.Y), Color.LightGreen, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                if (netStats)
                {
                    spriteBatch.FillRectangle(new Rectangle((int)camera.Position.X + 60, (int)camera.Position.Y, 140, 20), new Color(Color.Black, 0.5f), 0.9f);
                    spriteBatch.DrawString(Assets.font, $"{ping} Ping", new Vector2(camera.Position.X + 67, camera.Position.Y), Color.LightGreen, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                    if (active)
                        spriteBatch.DrawString(Assets.font, host ? "Server   P1" : "Client   P2", new Vector2(camera.Position.X + 120, camera.Position.Y), Color.LightGreen, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                }
            }

            if (editmode)
            {
                spriteBatch.DrawString(Assets.font, "P1", new Vector2(player.Pos.X, player.Pos.Y - 64), Color.Black);
                spriteBatch.DrawString(Assets.font, "P2", new Vector2(otherPlayer.Pos.X, otherPlayer.Pos.Y - 64), Color.Black);

                if (diffCam == DiffCam.FullScreenMove)
                {
                    spriteBatch.FillRectangle(new Rectangle((int)camera.Position.X, (int)camera.Position.Y + 290, 100, 90), new Color(Color.Gray, 0.5f), 0.9f);
                    spriteBatch.DrawString(Assets.font, "Move Camera \nUp: U\nDown: J\nLeft: H\nRight: K", new Vector2(camera.Position.X + 2, camera.Position.Y + 290), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                }

                spriteBatch.FillRectangle(new Rectangle((int)camera.Position.X, (int)camera.Position.Y + 381, 250, 230), new Color(Color.Gray, 0.5f), 0.9f);
                spriteBatch.DrawString(Assets.font, $"Camera Pos: X:{camera.Position.X.ToString("0.00")} Y:{camera.Position.Y.ToString("0.00")}", new Vector2(camera.Position.X + 2, camera.Position.Y + 381), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(Assets.font, $"Camera Move: {diffCam}", new Vector2(camera.Position.X + 2, camera.Position.Y + 398), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(Assets.font, "Switch Camera: I, O, P", new Vector2(camera.Position.X + 2, camera.Position.Y + 415), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(Assets.font, $"P1 Pos:X:{player.Pos.X.ToString("0.00")} Y:{player.Pos.Y.ToString("0.00")}\nP2 Pos:X:{otherPlayer.Pos.X.ToString("0.00")} Y:{otherPlayer.Pos.Y.ToString("0.00")}", new Vector2(camera.Position.X + 2, camera.Position.Y + 432), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(Assets.font, "Switch between player : L", new Vector2(camera.Position.X + 2, camera.Position.Y + 466), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(Assets.font, $"Player 2: {editmodePlayer}", new Vector2(camera.Position.X + 2, camera.Position.Y + 483), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(Assets.font, "Place block: Left-Click\nRemove block: Right-Click\nChange size of block: Scroll (+ Ctrl)\nChange door and switch id: Scroll\nChange color: Shift + Scroll\nSave level: R", new Vector2(camera.Position.X + 2, camera.Position.Y + 500), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                editor.Draw(spriteBatch, transformMatrix);

                for (int i = 0; i < npcs.Count; i++)
                {
                    npcs[i].DebugDraw(spriteBatch);
                }
            }

            

            spriteBatch.End();

            base.Draw(gameTime);
        }

        void UpdateObjects(GameTime gameTime, Player[] players)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is WeighedSwitch)
                    objects[i].Update(gameTime, objects, players);
                else if (objects[i] is Trap)
                    objects[i].Update(gameTime, players);
                else if (objects[i] is Door)
                    objects[i].Update(gameTime, objects, this);
                else if (objects[i] is Block or MovableBlock)
                    objects[i].Update(gameTime, this);
            }
            for (int i = 0; i < npcs.Count; i++)
            {
                npcs[i].Update(gameTime, player, otherPlayer, this);
            }
        }
        public void ConnectionSetup(out EventBasedNetListener listener)
        {
            listener = new EventBasedNetListener();
            netManager = new NetManager(listener);

            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                float[] array = dataReader.GetFloatArray();
                otherPlayer.Vel = new Vector2(array[0], array[1]);
                float[] array2 = dataReader.GetFloatArray();
                otherPlayer.Pos = new Vector2(array2[0], array2[1]);
                dataReader.Recycle();
            };
            listener.NetworkLatencyUpdateEvent += (fromPeer, latency) =>
            {
                this.ping = latency;
            };
            active = true;
        }
        public void Host()
        {
            if (!active)
            {
                ConnectionSetup(out EventBasedNetListener listener);

                netManager.Start(port /* port */);

                listener.ConnectionRequestEvent += request =>
                {
                    if (netManager.ConnectedPeersCount < 1 /* max connections */)
                        request.AcceptIfKey(password);
                    else
                        request.Reject();
                };

                listener.PeerConnectedEvent += peer =>
                {
                    Debug.WriteLine("We got connection: {0}", peer.EndPoint); // Show peer ip
                    connected = true;
                };
                host = true;
            }
        }
        public void Join()
        {
            if (!active)
            {
                (player, otherPlayer) = (otherPlayer, player);

                ConnectionSetup(out EventBasedNetListener listener);

                netManager.Start();
                netManager.Connect(ip /* host ip */, port /* port */, password /* Password */);
                listener.PeerConnectedEvent += peer =>
                {
                    Debug.WriteLine("We got connection: {0}", peer.EndPoint); // Show peer ip
                    connected = true;
                };
            }
            //Andreas ip: 217.210.114.224
            //Johannes ip: 84.217.51.42
            //Localhost: localhost eller 127.0.0.1
            //Port: 27960
            //192.168.1.207
        }
        public Color GetColorOfPixel(Vector2 position)
        {
            //try
            //{
            return colorData[(int)position.X + (int)position.Y * Assets.ScreenWidth];
            //}
            //catch (IndexOutOfRangeException)
            //{
            //    return Color.Black;
            //}
        }
        private void CameraMove()
        {
            switch (diffCam)
            {
                case DiffCam.SnapMove:
                    if (player.Pos.X+2 < camera.Position.X)
                        camera.Move(new Vector2(-Assets.ScreenWidth, 0));
                    if (player.Pos.X > camera.Position.X + Assets.ScreenWidth)
                        camera.Move(new Vector2(Assets.ScreenWidth, 0));
                    if (player.Pos.Y < camera.Position.Y)
                        camera.Move(new Vector2(0, -Assets.ScreenHeight));
                    if (player.Pos.Y > camera.Position.Y + Assets.ScreenHeight)
                        camera.Move(new Vector2(0, Assets.ScreenHeight));
                    break;
                case DiffCam.FullScreenMove:
                    if (kbState.IsKeyDown(Keys.H) && kbPreviousState.IsKeyUp(Keys.H))
                        camera.Move(new Vector2(-Assets.ScreenWidth, 0));
                    if (kbState.IsKeyDown(Keys.K) && kbPreviousState.IsKeyUp(Keys.K))
                        camera.Move(new Vector2(Assets.ScreenWidth, 0));
                    if (kbState.IsKeyDown(Keys.U) && kbPreviousState.IsKeyUp(Keys.U))
                        camera.Move(new Vector2(0, -Assets.ScreenHeight));
                    if (kbState.IsKeyDown(Keys.J) && kbPreviousState.IsKeyUp(Keys.J))
                        camera.Move(new Vector2(0, Assets.ScreenHeight));
                    break;
                case DiffCam.FollowPlayer:
                    camera.LookAt(player.Pos);
                    break;
            }
        }
        public void LoadLevel()
        {
            string level = "Content/level.json";
            JsonParser.GetJObjectFromFile(level);

            player = new Player(JsonParser.GetPos(level, "player"), Color.White, new AnimatedSprite(Assets.spriteSheet));
            otherPlayer = new Player(JsonParser.GetPos(level, "otherPlayer"), Color.White, new AnimatedSprite(Assets.spriteSheet2));

            //objects = new List<GameObject>();
            List<Rectangle> blockRects = JsonParser.GetRectangleList(level, "block");
            List<int> blockColors = JsonParser.GetIdList(level, "block");
            for (int i = 0; i < blockRects.Count; i++)
            {
                objects.Add(new Block(new Vector2(blockRects[i].X, blockRects[i].Y), new Vector2(blockRects[i].Width, blockRects[i].Height), Assets.colors[blockColors[i]]));
            }

            List<Rectangle> checkPointRects = JsonParser.GetRectangleList(level, "checkpoint");
            for (int i = 0; i < checkPointRects.Count; i++)
            {
                objects.Add(new CheckPoint(new Vector2(checkPointRects[i].X, checkPointRects[i].Y), new Vector2(checkPointRects[i].Width, checkPointRects[i].Height), Color.White));
            }

            List<Vector2> doorPosList = JsonParser.GetPosList(level, "door");
            List<int> doorIdList = JsonParser.GetIdList(level, "door");
            List<int> doorRotationList = JsonParser.GetRotationList(level, "door");
            for (int i = 0; i < doorPosList.Count; i++)
            {
                objects.Add(new Door(doorPosList[i], Color.White, doorIdList[i], doorRotationList[i]));
            }

            List<Vector2> switchPosList = JsonParser.GetPosList(level, "switch");
            List<int> switchIdList = JsonParser.GetIdList(level, "switch");
            for (int i = 0; i < switchPosList.Count; i++)
            {
                objects.Add(new WeighedSwitch(switchPosList[i], Color.White, switchIdList[i]));
            }

            List<Rectangle> movableRects = JsonParser.GetRectangleList(level, "movable");
            for (int i = 0; i < movableRects.Count; i++)
            {
                objects.Add(new MovableBlock(new Vector2(movableRects[i].X, movableRects[i].Y), new Vector2(movableRects[i].Width, movableRects[i].Height), Color.White));
            }

            List<Vector2> trapPosList = JsonParser.GetPosList(level, "trap");
            List<int> trapIdList = JsonParser.GetIdList(level, "trap");
            for (int i = 0; i < trapPosList.Count; i++)
            {
                objects.Add(new Trap(trapPosList[i], Color.White, trapIdList[i]));
            }

            List<Vector2> hiddenNpcPosList = JsonParser.GetPosList(level, "hiddenNpc");
            List<int> hiddenNpcSkinList = JsonParser.GetSkinList(level, "hiddenNpc");
            List<int> hiddenNpcIdList = JsonParser.GetIdList(level, "hiddenNpc");
            for (int i = 0; i < hiddenNpcPosList.Count; i++)
            {
                npcs.Add(new HiddenNpc(hiddenNpcPosList[i], hiddenNpcSkinList[i], hiddenNpcIdList[i]));
            }

            List<Vector2> hintNpcPosList = JsonParser.GetPosList(level, "hintNpc");
            List<int> hintNpcIdList = JsonParser.GetIdList(level, "hintNpc");
            for (int i = 0; i < hintNpcPosList.Count; i++)
            {
                npcs.Add(new HintNpc(hintNpcPosList[i], hintNpcIdList[i]));
            }

            List<Vector2> storyNpcPosList = JsonParser.GetPosList(level, "storyNpc");
            List<int> storyNpcIdList = JsonParser.GetIdList(level, "storyNpc");
            for (int i = 0; i < storyNpcPosList.Count; i++)
            {
                npcs.Add(new StoryNpc(storyNpcPosList[i], storyNpcIdList[i]));
            }
        }
    }
}