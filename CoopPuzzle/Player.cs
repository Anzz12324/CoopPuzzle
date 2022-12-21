using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;

namespace CoopPuzzle
{
    internal class Player : GameObject
    {
        AnimatedSprite sprite;
        ParticleSystem particles;
        float speed = 100;

        Vector2 start, oldPos, velocity;

        Vector2 spritePos { get { return new Vector2(Pos.X + 16, Pos.Y - 8); } }
        Vector2 emitterPos { get { return new Vector2(Pos.X + 16, Pos.Y + 16); } }
        
        public Vector2 Pos { get { return position; } set { position = value; } }
        public Vector2 Vel { get { return velocity; } set { velocity = value; } }
        public Vector2 CheckPos { get; set; }

        public Player(Vector2 position, Color color, AnimatedSprite sprite)
        {
            this.color = color;
            this.position = position;
            this.start = position;
            this.sprite = sprite;
            particles = new ParticleSystem(position);
        }

        public override void Update(GameTime gt, List<GameObject> objects, Game1 game1)
        {
            velocity = Vector2.Zero;
            oldPos = Pos;
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;
            var animation = "idleDown";
            
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                velocity.X -= dt * speed;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                velocity.X += dt * speed;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                velocity.Y -= dt * speed;

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
                velocity.Y += dt * speed;

            if (Vel.X > 0)
                animation = "walkRight";
            if (Vel.X < 0)
                animation = "walkLeft";
            if (Vel.Y > 0)
                animation = "walkDown";
            if (Vel.Y < 0)
                animation = "walkUp";
            if (Vel == Vector2.Zero)
                animation = "idleDown";

            position += Vel;

            for (int i = 0; i < objects.Count; i++)
            {
                if (this.hitbox.Intersects(objects[i].hitbox))
                {
                    if (objects[i] is WeighedSwitch)
                        break;

                    if (objects[i] is Door)
                    {
                        Door door = (Door)objects[i];
                        if (!door.Open)
                            HandleCollision();
                        else
                            return;
                    }
                        HandleCollision();
                }
            }

            sprite.Play(animation);

            particles.EmitterLocation = emitterPos;
            Color groundColor = game1.GetColorOfPixel(emitterPos);
            particles.Update(dt, Vel, groundColor);

            sprite.Update(dt);
        }

        public void UpdateOther(GameTime gt, List<GameObject> objects, Game1 game1)
        {
            oldPos = Pos;
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;
            var animation = "idleDown";

            if (Vel.X > 0)
                animation = "walkRight";
            if (Vel.X < 0)
                animation = "walkLeft";
            if (Vel.Y > 0)
                animation = "walkDown";
            if (Vel.Y < 0)
                animation = "walkUp";
            if (Vel == Vector2.Zero)
                animation = "idleDown";

            position += Vel;

            for (int i = 0; i < objects.Count; i++)
            {
                if (this.hitbox.Intersects(objects[i].hitbox))
                {
                    if (objects[i] is WeighedSwitch)
                        break;

                    if (objects[i] is Door)
                    {
                        Door door = (Door)objects[i];
                        if (!door.Open)
                            HandleCollision();
                        else
                            return;
                    }
                    HandleCollision();
                }
            }

            sprite.Play(animation);

            particles.EmitterLocation = emitterPos;
            Color groundColor = game1.GetColorOfPixel(emitterPos);
            particles.Update(dt, Vel, groundColor);

            sprite.Update(dt);
        }

        public override void Draw(SpriteBatch sb)
        {
            //base.Draw(sb);
            particles.Draw(sb);
            sb.Draw(sprite, spritePos, 0f, Vector2.One * 2f);
        }

        private void HandleCollision()
        {
            Pos = oldPos;
        }
    }
}
