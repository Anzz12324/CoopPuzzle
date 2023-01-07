using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle
{
    internal class MovableBlock : GameObject
    {

        public MovableBlock(Vector2 position, Color color) : base(position, color)
        {
            tex = Assets.white;
        }

        public override void Update(GameTime gT)
        {
            base.Update(gT);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, position, hitbox, color);
        }
    }
}
