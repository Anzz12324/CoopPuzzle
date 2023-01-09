using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Sprites;
using MonoGame.Extended.VectorDraw;
using System.Linq;

namespace CoopPuzzle
{
    internal class Player : GameObject
    {
        AnimatedSprite sprite;
        ParticleSystem particles;
        float speed = 100;
        string animation;

        Vector2 start, oldPos, velocity;

        Vector2 spritePos { get { return new Vector2(Pos.X + 12, Pos.Y - 16); } }
        Vector2 emitterPos { get { return new Vector2(Pos.X + 12, Pos.Y + 8); } }

        public Vector2 Vel { get { return velocity; } set { velocity = value; } }
        
        public override Rectangle hitbox { get { return new Rectangle((int)position.X, (int)position.Y, 24, 16); } }

        public Player(Vector2 position, Color color, AnimatedSprite sprite) : base(position, color)
        {
            this.start = position;
            this.sprite = sprite;
            particles = new ParticleSystem(position);
            animation = "idleDown";
        }

        public override void Update(GameTime gt, List<GameObject> objects, Game1 game1)
        {
            velocity = Vector2.Zero;
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;
            
            if (Keyboard.GetState().IsKeyDown(Keys.Left) || Keyboard.GetState().IsKeyDown(Keys.A))
                velocity.X -= dt * speed;

            if (Keyboard.GetState().IsKeyDown(Keys.Right) || Keyboard.GetState().IsKeyDown(Keys.D))
                velocity.X += dt * speed;

            if (Keyboard.GetState().IsKeyDown(Keys.Up) || Keyboard.GetState().IsKeyDown(Keys.W))
                velocity.Y -= dt * speed;

            if (Keyboard.GetState().IsKeyDown(Keys.Down) || Keyboard.GetState().IsKeyDown(Keys.S))
                velocity.Y += dt * speed;

            UpdateOther(gt, objects, game1);
        }

        public void UpdateOther(GameTime gt, List<GameObject> objects, Game1 game1)
        {
            oldPos = Pos;
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;

            if (Vel.Y > 0)
                animation = "walkDown";
            else if (Vel.Y < 0)
                animation = "walkUp";
            else if (Vel.X > 0)
                animation = "walkRight";
            else if (Vel.X < 0)
                animation = "walkLeft";
            else if (animation == "walkDown")
                animation = "idleDown";
            else if (animation == "walkUp")
                animation = "idleUp";
            else if (animation == "walkRight")
                animation = "idleRight";
            else if (animation == "walkLeft")
                animation = "idleLeft";

            position += Vel;

            for (int i = 0; i < objects.Count; i++)
            {
                if (this.hitbox.Intersects(objects[i].hitbox))
                {
                    if (objects[i] is WeighedSwitch)
                        continue;

                    if (objects[i] is Trap)
                        TrapCollision();

                    if (objects[i] is Door)
                    {
                        Door door = (Door)objects[i];
                        if (!door.Open)
                            HandleCollision();
                        else
                            continue;
                    }

                    if (objects[i] is Block)
                        HandleCollision();

                    if (objects[i] is MovableBlock)
                    {
                        MovableBlock movable = (MovableBlock)objects[i];
                        if(Vel != Vector2.Zero)
                        {
                            movable.Push(this, objects, out bool stuck);
                            if (stuck)
                                HandleCollision();
                        }
                    }
                }
            }

            sprite.Play(animation);
            sprite.Update(dt);
            sprite.Depth = Math.Clamp((Pos.Y - game1.camera.Position.Y) / game1.ScreenHeight, 0, 1);
            particles.EmitterLocation = emitterPos;
            particles.Update(dt, Vel, game1, emitterPos);
        }

        public override void Draw(SpriteBatch sb)
        {
            //base.Draw(sb);
            particles.Draw(sb);
            sb.Draw(sprite, spritePos, 0f, Vector2.One * 2f);
            sb.DrawLine(new Vector2(hitbox.Left, hitbox.Top), new Vector2(hitbox.Right, hitbox.Top), 1, Color.White);
            sb.DrawLine(new Vector2(hitbox.Left, hitbox.Top), new Vector2(hitbox.Left, hitbox.Bottom), 1, Color.White);
            sb.DrawLine(new Vector2(hitbox.Left, hitbox.Bottom), new Vector2(hitbox.Right, hitbox.Bottom), 1, Color.White);
            sb.DrawLine(new Vector2(hitbox.Right, hitbox.Bottom), new Vector2(hitbox.Right, hitbox.Top), 1, Color.White);
        }

        private void HandleCollision()
        {
            Pos = oldPos;
        }
        private void TrapCollision()
        {
            Pos = start;
        }
    }
}
