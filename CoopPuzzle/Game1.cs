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
using MonoGame.Extended.Serialization;
using MonoGame.Extended.Timers;
using MonoGame.Extended;
using MonoGame.Extended.ViewportAdapters;
using System.Runtime.CompilerServices;
using CoopPuzzle.Npc;

namespace CoopPuzzle
{

    public class Game1 : Game
    {
        bool active = false, host = false, connected = false, editmodePlayer = false, netStats = false, fps = true;
        NetManager netManager;
        enum DiffCam { SnapMove, FullScreenMove }
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
        public int ScreenWidth { get { return 1280; } }
        public int ScreenHeight { get { return 720; } }

        private int ping;
        public string ip, password; 
        public int port;
        public bool editmode = false;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            graphics.PreferredBackBufferWidth = ScreenWidth;
            graphics.PreferredBackBufferHeight = ScreenHeight;
            graphics.ApplyChanges();
        }

        protected override void Initialize()
        {
            TargetElapsedTime = TimeSpan.FromSeconds(1f / 144f);
            renderTarget = new RenderTarget2D(graphics.GraphicsDevice, ScreenWidth, ScreenHeight);
            colorData = new Color[ScreenWidth * ScreenHeight];

            var viewportAdapter = new BoxingViewportAdapter(Window, GraphicsDevice, ScreenWidth, ScreenHeight);
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

            npcs = new List<NPC>()
            {
                new StoryNpc(new Vector2(200,400), 1),
                new HintNpc(new Vector2(-300,300), 1),
                new HiddenNpc(new Vector2(-900,300),1,1),
                new HiddenNpc(new Vector2(-700,300),2,1),
                new HiddenNpc(new Vector2(-500,300),3,1),
            };

            objects = new List<GameObject>()
            //{
            //    new WeighedSwitch(new Vector2(200, 100), Color.White, 0),
            //    new Door(new Vector2(300, 100), Color.Green, 0),
            //    new Block(new Vector2(500), Vector2.One * 32, Color.Red),
            //    new Trap(new Vector2(550, 500), Color.White),
            //    new MovableBlock(new Vector2(400, 500), new Vector2(40, 80), Color.White)
            //}
            ;

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
            if (kbState.IsKeyDown(Keys.F2) && kbPreviousState.IsKeyUp(Keys.F2))
                fps = !fps;
            if (kbState.IsKeyDown(Keys.F3) && kbPreviousState.IsKeyUp(Keys.F3))
                netStats = !netStats;
        }
        protected override void Update(GameTime gameTime)
        {
            Player[] players = new Player[] { player, otherPlayer };
            Shotcuts();

            if (editmode)
            {
                editor.Update(ref objects, players, camera.Position);
                
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

            CameraMove(gameTime);

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
                }
            }

            if (!connected && !editmode)
                spriteBatch.Draw(Assets.wait, new Vector2(ScreenWidth/2-Assets.wait.Width/2,ScreenHeight/2-Assets.wait.Height),
                    new Rectangle(0,0, Assets.wait.Width, Assets.wait.Height) , Color.White, 0, Vector2.Zero, 1, SpriteEffects.None,1);                
            
            
            otherPlayer.Draw(spriteBatch);
            player.Draw(spriteBatch);
            for (int i = 0; i < npcs.Count; i++)
            {
                npcs[i].Draw(spriteBatch);
            }
            if (fps)
            {
                spriteBatch.FillRectangle(new Rectangle((int)camera.Position.X, (int)camera.Position.Y,60,20),new Color(Color.Black, 0.5f),0.9f);
                spriteBatch.DrawString(Assets.font, $"{(int)(1 / gameTime.ElapsedGameTime.TotalSeconds)} FPS", new Vector2(camera.Position.X+2, camera.Position.Y), Color.LightGreen, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
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
                spriteBatch.DrawString(Assets.font, "Switch Camera: I, O", new Vector2(camera.Position.X + 2, camera.Position.Y + 415), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(Assets.font, $"P1 Pos:X:{player.Pos.X.ToString("0.00")} Y:{player.Pos.Y.ToString("0.00")}\nP2 Pos:X:{otherPlayer.Pos.X.ToString("0.00")} Y:{otherPlayer.Pos.Y.ToString("0.00")}", new Vector2(camera.Position.X + 2, camera.Position.Y + 432), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(Assets.font, "Switch between player : L", new Vector2(camera.Position.X + 2, camera.Position.Y + 466), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(Assets.font, $"Player 2: {editmodePlayer}", new Vector2(camera.Position.X + 2, camera.Position.Y + 483), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);
                spriteBatch.DrawString(Assets.font, "Place block: Left-Click\nRemove block: Right-Click\nChange size of block: Scroll (+ Ctrl)\nChange door and switch id: Scroll\nChange color: Shift + Scroll\nSave level: R", new Vector2(camera.Position.X + 2, camera.Position.Y + 500), Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, 1);

                editor.Draw(spriteBatch, transformMatrix);
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
                    objects[i].Update(gameTime);
                else if (objects[i] is Door)
                    objects[i].Update(gameTime, objects);
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
                return colorData[(int)position.X + (int)position.Y * ScreenWidth];
            //}
            //catch (IndexOutOfRangeException)
            //{
            //    return Color.Black;
            //}
        }
        private void CameraMove(GameTime gameTime)
        {
            switch (diffCam)
            {
                case DiffCam.SnapMove:
                    if (player.Pos.X < camera.Position.X)
                        camera.Move(new Vector2(-ScreenWidth, 0));
                    if (player.Pos.X > camera.Position.X + ScreenWidth)
                        camera.Move(new Vector2(ScreenWidth, 0));
                    if (player.Pos.Y < camera.Position.Y)
                        camera.Move(new Vector2(0, -ScreenHeight));
                    if (player.Pos.Y > camera.Position.Y + ScreenHeight)
                        camera.Move(new Vector2(0, ScreenHeight));
                    break;
                case DiffCam.FullScreenMove:
                    if (kbState.IsKeyDown(Keys.H) && kbPreviousState.IsKeyUp(Keys.H))
                        camera.Move(new Vector2(-ScreenWidth, 0));
                    if (kbState.IsKeyDown(Keys.K) && kbPreviousState.IsKeyUp(Keys.K))
                        camera.Move(new Vector2(ScreenWidth, 0));
                    if (kbState.IsKeyDown(Keys.U) && kbPreviousState.IsKeyUp(Keys.U))
                        camera.Move(new Vector2(0, -ScreenHeight));
                    if (kbState.IsKeyDown(Keys.J) && kbPreviousState.IsKeyUp(Keys.J))
                        camera.Move(new Vector2(0, ScreenHeight));
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
            for (int i = 0; i < doorPosList.Count; i++)
            {
                objects.Add(new Door(doorPosList[i], Color.Green, doorIdList[i]));
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
            for (int i = 0; i < trapPosList.Count; i++)
            {
                objects.Add(new Trap(trapPosList[i], Color.White));
            }
        }
    }
}