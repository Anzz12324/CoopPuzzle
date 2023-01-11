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
        float speed, speedValue = 200;
        string animation;

        Vector2 start, oldPos, velocity;

        Vector2 spritePos { get { return new Vector2(Pos.X + 12, Pos.Y - 16); } }
        Vector2 emitterPos { get { return new Vector2(Pos.X + 12, Pos.Y + 8); } }

        public Vector2 Vel { get { return velocity; } set { velocity = value; } }
        
        public override Rectangle hitbox { get { return new Rectangle((int)position.X, (int)position.Y, 24, 16); } }
        public Rectangle Range { get { return new Rectangle((int)position.X-48, (int)position.Y-55, 120, 120); } }

        public Player(Vector2 position, Color color, AnimatedSprite sprite) : base(position, color)
        {
            this.start = position;
            this.sprite = sprite;
            particles = new ParticleSystem(position);
            animation = "idleDown";
        }

        public override void Update(GameTime gt, List<GameObject> objects, Game1 game1)
        {
            speed = game1.editmode ? speedValue * 2 : speedValue;
            
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

            speed = speedValue;
            for (int i = 0; i < objects.Count; i++)
            {
                if (this.hitbox.Intersects(objects[i].hitbox))
                {
                    if (objects[i] is WeighedSwitch)
                        continue;

                    if (objects[i] is Trap && objects[i].id == 0)
                        TrapCollision();

                    if (objects[i] is Door)
                    {
                        Door door = (Door)objects[i];
                        if (!door.Open)
                            HandleCollision(objects[i].hitbox);
                        else
                            continue;
                    }

                    if (objects[i] is Block)
                        HandleCollision(objects[i].hitbox);

                    if (objects[i] is MovableBlock)
                    {
                        MovableBlock movable = (MovableBlock)objects[i];
                        if (Vel != Vector2.Zero)
                        {
                            int movables = 0;
                            movable.Push(this, objects, out bool stuck, out float divideSpeedBy, ref movables);
                            speed = speed / divideSpeedBy;
                            if (stuck)
                                HandleCollision(objects[i].hitbox);
                        }
                    }

                    if (objects[i] is CheckPoint)
                        start = Pos;
                }
            }

            sprite.Play(animation);
            sprite.Update(dt);
            sprite.Depth = Math.Clamp((Pos.Y - game1.camera.Position.Y) / Assets.ScreenHeight, 0, 1);
            particles.EmitterLocation = emitterPos;
            particles.Update(dt, Vel, game1, emitterPos);
        }

        public override void Draw(SpriteBatch sb)
        {
            //base.Draw(sb);
            particles.Draw(sb);
            sb.Draw(sprite, spritePos, 0f, Vector2.One * 2f);

            //sb.DrawRectangle(hitbox, Color.White, 1, 1);
            //sb.DrawRectangle(Range, Color.HotPink, 1,1);
        }

        public void HandleCollision()
        {
            Pos = oldPos;
        }

        public void HandleCollision(Rectangle hitbox)
        {
            Vector2 up = new Vector2(0, hitbox.Top - this.hitbox.Bottom);
            Vector2 down = new Vector2(0, hitbox.Bottom - this.hitbox.Top);
            Vector2 left = new Vector2(hitbox.Left - this.hitbox.Right, 0);
            Vector2 right = new Vector2(hitbox.Right - this.hitbox.Left, 0);
            Vector2[] vectors = new Vector2[] { up, down, left, right };
            IEnumerable<Vector2> sortedVectors = vectors.OrderBy(v => v.Length());
            vectors = sortedVectors.ToArray();

            //Pos += vectors[0] * 0.95f; //gå kortaste vägen ur objektet du kolliderade med
            if(vectors[0].Y == 0)
                Pos = (vectors[0].X > 0) ? new Vector2(hitbox.Right, Pos.Y) : new Vector2(hitbox.Left - this.hitbox.Width, Pos.Y);

            if (vectors[0].X == 0)
                Pos = (vectors[0].Y > 0) ? new Vector2(Pos.X, hitbox.Bottom) : new Vector2(Pos.X, hitbox.Top - this.hitbox.Height);
        }
        private void TrapCollision()
        {
            Pos = start;
        }
    }
}
