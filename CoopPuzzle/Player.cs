using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;

namespace CoopPuzzle
{
    internal class Player : GameObject
    {
        AnimatedSprite sprite;
        ParticleSystem particles;
        float speed = 100;
        string animation;

        Vector2 start, oldPos, velocity, checkPos;

        Vector2 spritePos { get { return new Vector2(Pos.X + 12, Pos.Y - 16); } }
        Vector2 emitterPos { get { return new Vector2(Pos.X + 12, Pos.Y + 8); } }

        public Vector2 CheckPos { get { return checkPos; } set { checkPos = value; } }
        public Vector2 Pos { get { return position; } set { position = value; } }
        public Vector2 Vel { get { return velocity; } set { velocity = value; } }
        
        public override Rectangle hitbox { get { return new Rectangle((int)position.X, (int)position.Y, 24, 16); } }

        public Player(Vector2 position, Color color, AnimatedSprite sprite)
        {
            this.color = color;
            this.position = position;
            this.start = position;
            this.sprite = sprite;
            particles = new ParticleSystem(position);
            this.checkPos = Vector2.Zero;
            animation = "idleDown";
        }

        public override void Update(GameTime gt, List<GameObject> objects, Game1 game1)
        {
            velocity = Vector2.Zero;
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;
            
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
                velocity.X -= dt * speed;

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
                velocity.X += dt * speed;

            if (Keyboard.GetState().IsKeyDown(Keys.Up))
                velocity.Y -= dt * speed;

            if (Keyboard.GetState().IsKeyDown(Keys.Down))
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
                        break;
                    if (objects[i] is Trap)
                        TrapCollision();
                    if (objects[i] is Door)
                    {
                        Door door = (Door)objects[i];
                        if (!door.Open)
                            HandleCollision();
                        else
                            return;
                    }
                    if (objects[i] is Block)
                        HandleCollision();
                }
            }
            sprite.Play(animation);
            sprite.Update(dt);
            particles.EmitterLocation = emitterPos;
            particles.Update(dt, Vel, game1.GetColorOfPixel(emitterPos));
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            particles.Draw(sb);
            sb.Draw(sprite, spritePos, 0f, Vector2.One * 2f);
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
