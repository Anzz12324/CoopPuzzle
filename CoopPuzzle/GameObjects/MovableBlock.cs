using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle
{
    internal class MovableBlock : GameObject
    {
        float depth;

        public MovableBlock(Vector2 position, Color color) : base(position, color)
        {
            tex = Assets.white;
        }

        public override void Update(GameTime gT, Game1 game1)
        {
            depth = (Pos.Y - game1.camera.Position.Y) / game1.ScreenHeight;

            base.Update(gT);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, hitbox, null, color, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public void Push(Player player, List<GameObject> objects, out bool stuck)
        {
            Vector2 up = new Vector2(0, player.hitbox.Top - hitbox.Bottom);
            Vector2 down = new Vector2(0, player.hitbox.Bottom - hitbox.Top);
            Vector2 left = new Vector2(player.hitbox.Left - hitbox.Right, 0);
            Vector2 right = new Vector2(player.hitbox.Right - hitbox.Left, 0);
            Vector2[] vectors = new Vector2[] { up, down, left, right };
            IEnumerable<Vector2> sortedVectors = vectors.OrderBy(v => v.Length());
            vectors = sortedVectors.ToArray();

            Pos += vectors[0] * 0.75f; //gå kortaste vägen ur objektet du kolliderade med

            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] == this || objects[i] is WeighedSwitch)
                    continue;

                if (hitbox.Intersects(objects[i].hitbox))
                {
                    Pos -= vectors[0] * 0.75f;
                    stuck = true;
                    return;
                }
            }
            stuck = false;
        }
    }
}
