using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle
{
    internal class Block : GameObject
    {

        public Block(Vector2 position, Vector2 size, Color color) : base(position, color)
        {
            this.size = size;
            tex = Assets.brick;
        }

        public override void Update(GameTime gT)
        {
            base.Update(gT);
        }

        public override void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < size.X; i+=40)
            {
                for (int j = 0; j < size.Y; j += 40)
                {
                    sb.Draw(tex, new Rectangle((int)Pos.X + i, (int)Pos.Y + j, 40, 40), TempColor);
                }
            }
        }
    }
}
