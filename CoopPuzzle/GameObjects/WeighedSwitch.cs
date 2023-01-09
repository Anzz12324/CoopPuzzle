using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle
{
    internal class WeighedSwitch : GameObject
    {
        public bool Weight { get; private set; }
        public int id { get; private set; }
        public override Rectangle hitbox { get { return new Rectangle((int)position.X + 4, (int)position.Y + 6, (int)size.X - 4 * 2, (int)size.Y - 16); } }

        public Player[] players { get; private set; }

        public WeighedSwitch(Vector2 position, Color color, int id) : base(position, color)
        {
            this.id = id;
        }

        public override void Update(GameTime gT, List<GameObject> objects, Player[] players)
        {
            this.players = players;
            Weight = false;
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is MovableBlock)
                    Weight = objects[i].hitbox.Intersects(hitbox) ? true : Weight;
            }
            for (int i = 0; i < players.Length; i++)
            {
                Weight = players[i].hitbox.Intersects(hitbox) ? true : Weight;
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            //sb.Draw(Assets.white, hitbox, Color.Red);
            if (Weight)
            {
                sb.Draw(Assets.undertaleRuins, Pos, new Rectangle(23, 837, 20, 20), TempColor, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);
            }
            else
            {
                sb.Draw(Assets.undertaleRuins, Pos, new Rectangle(1, 837, 20, 20), TempColor, 0f, Vector2.Zero, 2f, SpriteEffects.None, 0f);

            }
        }
    }
}
