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
        public override Rectangle hitbox { get { return new Rectangle((int)position.X, (int)position.Y + 20, (int)size.X, (int)size.Y - 20); } }

        public MovableBlock(Vector2 position, Vector2 size, Color color) : base(position, color)
        {
            this.size = size;
            tex = Assets.box;
        }

        public override void Update(GameTime gT, Game1 game1)
        {
            base.Update(gT, game1);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, HUDhitbox, null, TempColor, 0f, Vector2.Zero, SpriteEffects.None, depth);
        }

        public void Push(GameObject pusher, List<GameObject> objects, out bool stuck, out float divideSpeedBy, ref int movables)
        {
            divideSpeedBy = (float)Math.Sqrt(Size.X * Size.Y) / 20;
            movables++;
            stuck = (movables > 3) ? true : false;

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
                        mb.Push(this, objects, out stuck, out float alsoDivideSpeedBy, ref movables);
                        divideSpeedBy *= alsoDivideSpeedBy * alsoDivideSpeedBy;
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
                    else if (objects[i] is CheckPoint)
                        continue;
                    Pos -= vectors[0] * 0.75f;
                    stuck = true;
                    return;
                }
            }
        }
    }
}
