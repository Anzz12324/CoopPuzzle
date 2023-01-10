using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle.Npc
{
    internal class HiddenNpc : NPC
    {
        private int Npc;
        public HiddenNpc(Vector2 pos, int Npc, int textNum) : base(pos, textNum)
        {
            this.Npc = Npc;
            LoadSrcRec();
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, srcRecArray[frame], Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, depth);
            if (playerInRange)
            {
                sb.Draw(bubbleTex, bubblePos, bubbleSrcRec, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth);
                sb.DrawString(Assets.font, text[textNum], new Vector2(bubblePos.X + 5, bubblePos.Y + 5), color, 0, Vector2.Zero, 1, SpriteEffects.None, depth + 0.01f);
            }
        }
        private void LoadSrcRec()
        {
            if (Npc == 1)
            {
                //Nice Cream Guy
                tex = Assets.creamGuy;
                frameCount = 2;
                srcRecArray = new Rectangle[frameCount];
                srcRecArray[0] = new Rectangle(163,21,74,60);
                srcRecArray[1] = new Rectangle(84, 21, 74, 60);

            }
            else if (Npc == 2)
            {
                //Asgore Dreemurr
                tex = Assets.asgore;
                frameCount = 2;
                srcRecArray = new Rectangle[frameCount];
                srcRecArray[0] = new Rectangle(5,432,55,61);
                srcRecArray[1] = new Rectangle(65, 432, 55, 61);
            }
            else if (Npc == 3)
            {
                //Grillby
                tex = Assets.grillby;
                frameCount = 5;
                srcRecArray = new Rectangle[frameCount];
                srcRecArray[0] = new Rectangle(8,364,24,53);
                srcRecArray[1] = new Rectangle(38, 364, 24, 53);
                srcRecArray[2] = new Rectangle(68, 364, 24, 53);
                srcRecArray[3] = new Rectangle(98, 364, 24, 53);
                srcRecArray[4] = new Rectangle(128, 364, 24, 53);
            }
        }
    }
}
