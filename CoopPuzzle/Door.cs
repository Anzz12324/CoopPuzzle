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

        public Door(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
        }

        public override void Update(GameTime gT, List<GameObject> objects)
        {
            for (int i = 0; i < objects.Count; i++)
            {
                if (objects[i] is WeighedSwitch)
                {
                    WeighedSwitch ws = (WeighedSwitch)objects[i];
                    Open = ws.Weight;
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if(!Open) base.Draw(sb);
        }
    }
}
