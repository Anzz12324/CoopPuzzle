using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle.Npc
{
    internal class HintNpc : NPC
    {
        public HintNpc(Texture2D tex, Vector2 pos, int Npc, int textNum) : base(tex, pos, Npc, textNum)
        {
        }

        public override void Update(GameTime gT, Player[] players)
        {
            
        }
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, Color.White);
        }
    }
}
