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

namespace CoopPuzzle
{

    public class Game1 : Game
    {
        bool active = false, host = false, connected = false, editmodePlayer = false;
        NetManager netManager;
        enum DiffCam { SnapMove, FollowPlayer, KeyInput, FullScreenMove }
        DiffCam diffCam = DiffCam.SnapMove;

        Player player, otherPlayer;

        List<GameObject> objects;

        RenderTarget2D renderTarget;

        Editor editor;

        KeyboardState kbState, kbPreviousState;

        public OrthographicCamera camera { get; private set; }
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        Color[] colorData;
        public int ScreenWidth { get { return 1280; } }
        public int ScreenHeight { get { return 720; } }

        private int latency;
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

            objects = new List<GameObject>()
            {
                new WeighedSwitch(new Vector2(200, 100), Color.Green, 0),
                new Door(new Vector2(300, 100), Color.Green, 0),
                new Block(new Vector2(500), Vector2.One * 32, Color.Red),
                new Trap(new Vector2(550,500), Color.White),
                new MovableBlock(new Vector2(400, 500), Color.White)
            };

            LoadLevel();
        }

        private void Shotcuts()
        {
            kbPreviousState = kbState;
            kbState = Keyboard.GetState();
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
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
                diffCam = DiffCam.FollowPlayer;
            if (kbState.IsKeyDown(Keys.P) && kbPreviousState.IsKeyUp(Keys.P))
                diffCam = DiffCam.KeyInput;
            if (kbState.IsKeyDown(Keys.OemCloseBrackets) && kbPreviousState.IsKeyUp(Keys.OemCloseBrackets))
                diffCam = DiffCam.FullScreenMove;
        }
        protected override void Update(GameTime gameTime)
        {
            Shotcuts();

            if (editmode)
            {
                editor.Update(ref objects, camera.Position);
                
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

                UpdateObjects(gameTime);
            }

            if (active)
            {
                if (connected)
                {
                    player.Update(gameTime, objects, this);
                    otherPlayer.UpdateOther(gameTime, objects, this);
                }

                UpdateObjects(gameTime);

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
                if (objects[i] is not WeighedSwitch)
                    objects[i].Draw(spriteBatch);
                if (objects[i] is Door)
                {
                    if (editmode)
                    {
                        Door door = (Door)objects[i];
                        spriteBatch.DrawString(Assets.font, door.id.ToString(), new Vector2(door.Pos.X, door.Pos.Y - 16), Color.Black);
                    }
                }
            }

            if (!connected && !editmode)
                spriteBatch.DrawString(Assets.bigFont, "Waiting on your friend to join!", new Vector2(100,360), Color.Black);                
            if (active)
                spriteBatch.DrawString(Assets.font, (host) ? "Server   P1" : "Client   P2", new Vector2(100,0), Color.Black);


            spriteBatch.DrawString(Assets.font, "P1", new Vector2(player.Pos.X, player.Pos.Y - 64), Color.Black);
            spriteBatch.DrawString(Assets.font, "P2", new Vector2(otherPlayer.Pos.X, otherPlayer.Pos.Y - 64), Color.Black);
            spriteBatch.DrawString(Assets.font, $"FPS:{(int)(1 / gameTime.ElapsedGameTime.TotalSeconds)}", new Vector2(500, 0), Color.Black);
            spriteBatch.DrawString(Assets.font, $"Pos:{player.Pos}  otherPos:{otherPlayer.Pos}", new Vector2(camera.Position.X + 600,camera.Position.Y), Color.Yellow);
            spriteBatch.DrawString(Assets.font, $"PlayerEdit: {editmodePlayer}", new Vector2(300,0), Color.Black);
            spriteBatch.DrawString(Assets.font, $"latency: {latency}", new Vector2(300, 50), Color.Black);
            spriteBatch.DrawString(Assets.font, $"Camera Pos; {camera.Position}", new Vector2(camera.Position.X, camera.Position.Y + 60), Color.Yellow);
            spriteBatch.DrawString(Assets.font, $"Camera Move; {diffCam}", new Vector2(camera.Position.X, camera.Position.Y + 80), Color.Yellow);
            spriteBatch.DrawString(Assets.font, "Switch Camera; I, O, P, {", new Vector2(camera.Position.X, camera.Position.Y + 100), Color.Yellow);
            if (diffCam == DiffCam.KeyInput || diffCam == DiffCam.FullScreenMove)
                spriteBatch.DrawString(Assets.font, "Move Camera in KeyInput\nUp: U\nDown: J\nLeft: H\nRight: K", new Vector2(camera.Position.X, camera.Position.Y + 120), Color.Yellow);

            otherPlayer.Draw(spriteBatch);
            player.Draw(spriteBatch);

            if (editmode)
            {
                spriteBatch.DrawString(Assets.font, "Switch between player : L", new Vector2(0, 40), Color.Black);
                spriteBatch.DrawString(Assets.font, "Place block: Left-Click\nChange size of block: Scroll (+ Ctrl)\nChange door and switch id: Scroll", new Vector2(0, 500), Color.Black);

                editor.Draw(spriteBatch, transformMatrix);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        public void UpdateObjects(GameTime gameTime)
        {
            Player[] players = new Player[] { player, otherPlayer };
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is WeighedSwitch)
                    objects[i].Update(gameTime, objects, players);
                else if (objects[i] is Trap)
                    objects[i].Update(gameTime);
                else if (objects[i] is Door)
                    objects[i].Update(gameTime, objects);
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
                this.latency = latency;
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
            try
            {
                return colorData[(int)position.X + (int)position.Y * ScreenWidth];
            }
            catch (IndexOutOfRangeException)
            {
                return Color.Black;
            }
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
                case DiffCam.FollowPlayer:
                    camera.LookAt(player.Pos);
                    break;
                case DiffCam.KeyInput:
                    const float movementSpeed = 200;
                    camera.Move(GetMovementDirection() * movementSpeed * gameTime.GetElapsedSeconds());
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

        private Vector2 GetMovementDirection()
        {
            var movementDirection = Vector2.Zero;
            var state = Keyboard.GetState();
            if (state.IsKeyDown(Keys.J))
            {
                movementDirection += Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.U))
            {
                movementDirection -= Vector2.UnitY;
            }
            if (state.IsKeyDown(Keys.H))
            {
                movementDirection -= Vector2.UnitX;
            }
            if (state.IsKeyDown(Keys.K))
            {
                movementDirection += Vector2.UnitX;
            }
            return movementDirection;
        }

        void LoadLevel()
        {
            string level = "../../../Content/level.json";
            JsonParser.GetJObjectFromFile(level);

            player = new Player(JsonParser.GetPos("player"), Color.White, new AnimatedSprite(Assets.spriteSheet));
            otherPlayer = new Player(JsonParser.GetPos("otherPlayer"), Color.White, new AnimatedSprite(Assets.spriteSheet2));

            //objects = new List<GameObject>();
            List<Rectangle> blockRects = JsonParser.GetRectangleList(level, "block");
            for (int i = 0; i < blockRects.Count; i++)
            {
                objects.Add(new Block(new Vector2(blockRects[i].X, blockRects[i].Y), new Vector2(blockRects[i].Width, blockRects[i].Height), Color.White));
            }
        }
    }
}