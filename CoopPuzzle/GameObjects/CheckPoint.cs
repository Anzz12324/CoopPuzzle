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
            sb.DrawLine(new Vector2(hitbox.Left, hitbox.Top), new Vector2(hitbox.Right, hitbox.Top), 1, Color.Black);
            sb.DrawLine(new Vector2(hitbox.Left, hitbox.Top), new Vector2(hitbox.Left, hitbox.Bottom), 1, Color.Black);
            sb.DrawLine(new Vector2(hitbox.Left, hitbox.Bottom), new Vector2(hitbox.Right, hitbox.Bottom), 1, Color.Black);
            sb.DrawLine(new Vector2(hitbox.Right, hitbox.Bottom), new Vector2(hitbox.Right, hitbox.Top), 1, Color.Black);
            sb.DrawString(Assets.font, "CHECKPOINT", Pos, Color.Black);
        }
    }
}
