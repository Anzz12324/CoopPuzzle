﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle
{
    internal class Door : GameObject
    {
        public bool Open { get; private set; }

        public Door(Vector2 position, Color color, int id) : base(position, color)
        {
            this.id = id;
            Color = Color.Green;
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
            base.Update(gT, game1);
        }

        public override void Draw(SpriteBatch sb)
        {
            if(!Open) base.Draw(sb);
        }
    }
}
