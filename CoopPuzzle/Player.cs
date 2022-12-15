﻿using Microsoft.Xna.Framework;
using MonoGame.Extended.Sprites;

namespace CoopPuzzle
{
    internal class Player : GameObject
    {
        AnimatedSprite sprite;
        float speed = 100;
        Vector2 oldPos;
        Vector2 spritePos { get { return new Vector2(Pos.X + 16, Pos.Y - 8); } }
        
        public Vector2 Pos { get { return position; } set { position = value; } }
        private Vector2 start;
        public Player(Vector2 position, Color color, AnimatedSprite sprite)
        {
            this.color = color;
            this.position = position;
            this.start = position;
            this.sprite = sprite;
        }

        public override void Update(GameTime gt, List<GameObject> objects)
        {
            oldPos = Pos;
            float dt = (float)gt.ElapsedGameTime.TotalSeconds;
            var animation = "idleDown";
            
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                animation = "walkLeft";
                position.X-= dt * speed;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                animation = "walkRight";
                position.X += dt * speed; ;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                animation = "walkUp";
                position.Y -= dt * speed; ;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                animation = "walkDown";
                position.Y += dt * speed; ;
            }

            for (int i = 0; i < objects.Count; i++)
            {
                if (this.hitbox.Intersects(objects[i].hitbox))
                {
                    if (objects[i] is WeighedSwitch)
                        return;

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
            sprite.Update(dt);
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);
            sb.Draw(sprite, spritePos, 0f, Vector2.One * 2f);
        }

        private void HandleCollision()
        {
            Pos = oldPos;
        }
    }
}