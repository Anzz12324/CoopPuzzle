using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle
{
    internal class Trap : GameObject
    {
        private int frame;
        private double frameTimer, frameInterval;
        public Trap(Vector2 position)
        {
            this.position = position;
            srcRecArray = new Rectangle[2];
            srcRecArray[0] = new Rectangle(1,860,20,20);
            srcRecArray[1] = new Rectangle(23, 860, 20, 20);
            frameTimer = 100;
            frameInterval = 1000;
        }
        public override void Update(GameTime gT, List<GameObject> objects)
        {
            frameTimer -= gT.ElapsedGameTime.TotalMilliseconds;
            if (frameTimer <= 0)
            {
                frameTimer = frameInterval;
                frame++;
                if (frame == 2)
                {
                    frame = 0;
                }
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Assets.undertaleRuins, hitbox, srcRecArray[frame], Color.White);
        }
    }
}
