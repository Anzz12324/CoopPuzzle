using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle
{
    internal class CheckPoint : GameObject
    {
        public CheckPoint(Vector2 position, Vector2 size, Color color) : base(position, color)
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
            sb.DrawRectangle(hitbox, Color.Black, 1, 1);
            sb.DrawString(Assets.font, "CHECK\nPOINT", new Vector2(Pos.X + 2, Pos.Y), Color.Black);
        }
    }
}
