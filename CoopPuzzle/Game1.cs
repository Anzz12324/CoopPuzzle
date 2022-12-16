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

namespace CoopPuzzle
{

    public class Game1 : Game
    {
        bool active = false, host = false, connected = false;
        NetManager netManager;

        Player player, otherPlayer;

        SpriteFont font, bigFont;
        Block block;
        List<GameObject> objects;

        
        private GraphicsDeviceManager graphics;
        private SpriteBatch spriteBatch;

        public int ScreenWidth { get { return 1280; } }
        public int ScreenHeight { get { return 720; } }

        public string ip, password; 
        public int port;

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

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);

            Assets.LoadTextures(Content);
            font = Content.Load<SpriteFont>("font");
            bigFont = Content.Load<SpriteFont>("bigFont");
            block = new Block(new Vector2(500), Color.Red);
            var spriteSheet = Content.Load<SpriteSheet>("frisk.sf", new JsonContentLoader());

            player = new Player(new Vector2(100, 200), Color.White, new AnimatedSprite(spriteSheet));
            otherPlayer = new Player(new Vector2(100, 300), Color.Black, new AnimatedSprite(spriteSheet));

            objects = new List<GameObject>()
            {
                new WeighedSwitch(new Vector2(200, 100), Color.Green),
                new Door(new Vector2(300, 100), Color.Green),
                new Block(new Vector2(500), Color.Red)
            };
        }


        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.GetState().IsKeyDown(Keys.Escape))
            {
                netManager?.Stop();
                Exit();
            }


            if (active)
            {
                if (connected)
                {
                    player.Update(gameTime, objects);
                    otherPlayer.UpdateOther(gameTime, objects);
                }
                
                Player[] players = new Player[] { player, otherPlayer };
                for (int i = 0; i < objects.Count; i++)
                {
                    if (objects[i] is WeighedSwitch)
                        objects[i].Update(gameTime, players);

                    else
                        objects[i].Update(gameTime, objects);
                }

                NetDataWriter writer = new NetDataWriter();
                netManager.PollEvents();
                writer.PutArray(new float[] { player.Vel.X, player.Vel.Y });
                netManager.SendToAll(writer, DeliveryMethod.ReliableOrdered);
            }

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin(samplerState: SamplerState.PointWrap);

            if (!connected)
                spriteBatch.DrawString(bigFont, "Waiting on your friend to join!", new Vector2(100,360), Color.Black);
            if (active)
                spriteBatch.DrawString(font, (host) ? "Server   P1" : "Client   P2", new Vector2(100,0), Color.Black);

            spriteBatch.DrawString(font, "Server: J \nClient: K", new Vector2(), Color.Black);
            spriteBatch.DrawString(font, "P1", new Vector2(player.Pos.X, player.Pos.Y - 20), Color.Black);
            spriteBatch.DrawString(font, "P2", new Vector2(otherPlayer.Pos.X, otherPlayer.Pos.Y - 20), Color.Black);
            spriteBatch.DrawString(font, $"FPS:{(int)(1 / gameTime.ElapsedGameTime.TotalSeconds)}", new Vector2(500, 0), Color.Black);

            block.Draw(spriteBatch);

            for (int i = 0; i < objects.Count; i++)
            {
                objects[i].Draw(spriteBatch);
            }

            player.Draw(spriteBatch);
            otherPlayer.Draw(spriteBatch);




            spriteBatch.End();
            base.Draw(gameTime);
        }

        public void ConnectionSetup(out EventBasedNetListener listener)
        {
            listener = new EventBasedNetListener();
            netManager = new NetManager(listener);

            listener.NetworkReceiveEvent += (fromPeer, dataReader, deliveryMethod) =>
            {
                float[] array = dataReader.GetFloatArray();
                otherPlayer.Vel = new Vector2(array[0], array[1]);

                dataReader.Recycle();
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
                    if (netManager.ConnectedPeersCount < 2 /* max connections */)
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
    }
}