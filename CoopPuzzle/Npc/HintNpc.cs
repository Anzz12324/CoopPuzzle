using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle.Npc
{
    internal class HintNpc : NPC
    {
        public HintNpc(Vector2 pos, int textNum) : base(pos, textNum)
        {
            tex = Assets.mettaton;
            frameCount = 2;
            srcRecArray = new Rectangle[frameCount];
            srcRecArray[0] = new Rectangle(9,324,52,46);
            srcRecArray[1] = new Rectangle(62, 324, 58, 46);
            bubbleSrcRec = new Rectangle(20, 315, 174, 105);
            range = new Rectangle((int)pos.X-10, (int)pos.Y+50, 120, 100);
            bubblePos = new Vector2(pos.X - (bubbleSrcRec.Width / 2) + srcRecArray[0].Width-5, pos.Y - bubbleSrcRec.Height);
            textPos = new Vector2(bubblePos.X + 5, bubblePos.Y + 5);
            color = Color.Black;
            dialogueArray = new string[][]
            {
            new string[]
            {
                "Lorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum",
                "Lorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum",
                "Lorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum",
                "Lorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum",
                "Lorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum"
            }
            };
            hitbox = new Rectangle((int)pos.X, (int)pos.Y + (srcRecArray[0].Height * 2 - 10), srcRecArray[0].Width * 2, 10);
        }


        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, srcRecArray[frame], Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, depth);
            if (playerInRange)
            {
                sb.Draw(bubbleTex, bubblePos, bubbleSrcRec, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth);
                sb.DrawString(Assets.font, dialogueArray[dialogueNum][textNum], textPos , color, 0, Vector2.Zero, 1, SpriteEffects.None, depth + 0.01f);
            }

            base.Draw(sb);
        }
    }
}
