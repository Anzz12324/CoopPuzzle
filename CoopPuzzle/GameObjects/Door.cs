using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle
{
    internal class Door : GameObject
    {
        public bool Open { get; private set; }

        Rectangle[] srcRects = new Rectangle[]
        {
            new Rectangle(0, 0, 40, 40),
            new Rectangle(0, 40, 40, 40)
        };
        Rectangle srcRect;

        public int rotation { get; protected set; }

        float rotate;

        public Door(Vector2 position, Color color, int id, int rotation) : base(position, color)
        {
            this.id = id;
            //Color = Color.Green;
            tex = Assets.door;

            this.rotation = rotation;
            rotate = MathHelper.ToRadians(90 * (rotation % 4));

            srcRect = srcRects[0];
        }

        public override void Update(GameTime gT, List<GameObject> objects, Game1 game1)
        {
            Open = false;
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is WeighedSwitch)
                {
                    WeighedSwitch ws = (WeighedSwitch)objects[i];
                    if(ws.id == id && ws.Weight) Open = true;
                }
            }

            srcRect = Open ? srcRects[1] : srcRects[0];

            base.Update(gT, game1);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, Pos + Size / 2, srcRect, Color, rotate, Size / 2, 1f, SpriteEffects.None, 1f);
        }
    }
}
