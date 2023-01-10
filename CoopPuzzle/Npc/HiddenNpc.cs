using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle.Npc
{
    internal class HiddenNpc : NPC
    {
        public HiddenNpc(Texture2D tex, Vector2 pos, int frameCount, int Npc, int textNum) : base(tex, pos, frameCount, Npc, textNum)
        {
            srcRecArray = new Rectangle[frameCount];
            
            LoadSrcRec();
        }

        public override void Update(GameTime gT, Player player, Player otherPlayer, Game1 game1)
        {

        }
        public override void Draw(SpriteBatch sb)
        {

        }
        private void LoadSrcRec()
        {
            if (Npc == 1)
            {

            }
        }
    }
}
