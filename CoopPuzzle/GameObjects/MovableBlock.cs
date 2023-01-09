using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle
{
    internal class MovableBlock : GameObject
    {
        float depth;

        public MovableBlock(Vector2 position, Vector2 size, Color color) : base(position, color)
        {
            this.size = size;
            tex = Assets.box;
        }

        public override void Update(GameTime gT, Game1 game1)
        {
            depth = (Pos.Y - game1.camera.Position.Y) / game1.ScreenHeight;

            base.Update(gT);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, hitbox, null, TempColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public void Push(GameObject pusher, List<GameObject> objects, out bool stuck)
        {
            stuck = false;
            Vector2 up = new Vector2(0, pusher.hitbox.Top - hitbox.Bottom);
            Vector2 down = new Vector2(0, pusher.hitbox.Bottom - hitbox.Top);
            Vector2 left = new Vector2(pusher.hitbox.Left - hitbox.Right, 0);
            Vector2 right = new Vector2(pusher.hitbox.Right - hitbox.Left, 0);
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
                    if (objects[i] is MovableBlock)
                    {
                        MovableBlock mb = (MovableBlock)objects[i];
                        mb.Push(this, objects, out stuck);
                        if (stuck)
                            Pos -= vectors[0] * 0.75f;
                        return;
                    }
                    else if (objects[i] is Door)
                    {
                        Door door = (Door)objects[i];
                        if (door.Open)
                            continue;
                    }
                    Pos -= vectors[0] * 0.75f;
                    stuck = true;
                    return;
                }
            }
        }
    }
}
