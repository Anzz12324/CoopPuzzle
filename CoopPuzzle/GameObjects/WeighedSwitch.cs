using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle
{
    internal class WeighedSwitch : GameObject
    {
        public bool Weight { get; set; }

        public WeighedSwitch(Vector2 position, Color color) : base(position, color)
        {
        }

        public override void Update(GameTime gT, Player[] players)
        {
            Weight = false;
            for (int i = 0; i < players.Length; i++)
            {
                Weight = players[i].hitbox.Intersects(this.hitbox) ? true : Weight;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Weight)
            {
                sb.Draw(Assets.undertaleRuins, hitbox, new Rectangle(23, 837, 20, 20), Color.White);
            }
            else
            {
                sb.Draw(Assets.undertaleRuins, hitbox, new Rectangle(1, 837, 20, 20), Color.White);

            }
        }
    }
}
