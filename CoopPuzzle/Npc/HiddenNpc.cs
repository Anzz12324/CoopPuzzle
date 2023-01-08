using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle.Npc
{
    internal class HiddenNpc : NPC
    {
        public HiddenNpc(Texture2D tex, Texture2D bubbleTex, Vector2 pos, Vector2 bubblePos) : base(tex, bubbleTex, pos, bubblePos)
        {
        }

        public override void Update(GameTime gT, Player[] players)
        {
            
        }
        public override void Draw(SpriteBatch sb)
        {
            
        }
    }
}
