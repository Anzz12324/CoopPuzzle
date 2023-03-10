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
        private int frame = 0;
        Rectangle[] srcRecArray;
        new float depth = 0.01f;

        public override Rectangle hitbox { get { return new Rectangle((int)position.X + 4, (int)position.Y + 6, (int)size.X - 4 * 2, (int)size.Y - 16); } }

        public Trap(Vector2 position, Color color, int id) : base(position, color)
        {
            srcRecArray = new Rectangle[2];
            srcRecArray[0] = new Rectangle(23, 860, 20, 20);
            srcRecArray[1] = new Rectangle(1, 860, 20, 20);
            this.id = id;
        }
        public override void Update(GameTime gT, Player[] players)
        {

            if (players[0].Range.Intersects(hitbox) || players[1].Range.Intersects(hitbox))
            {
                if (id == 0)
                {
                    frame = 1;

                }
                else
                {
                    frame = 0;
                }
            }
            else
            {
                frame = 0;
            }
        }
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(Assets.undertaleRuins, hitbox, srcRecArray[frame], TempColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }
    }
}
