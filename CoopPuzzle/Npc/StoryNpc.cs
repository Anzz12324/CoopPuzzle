using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CoopPuzzle.Npc
{
    internal class StoryNpc : NPC
    {
        public StoryNpc(Texture2D tex, Vector2 pos, int frameCount, int Npc, int textNum) : base(tex, pos, frameCount, Npc, textNum)
        {
            srcRecArray = new Rectangle[frameCount];
            srcRecArray[0] = new Rectangle(5, 17, 21, 21);
            srcRecArray[1] = new Rectangle(29, 17, 21, 21);
            bubbleSrcRec = new Rectangle(20, 315, 174, 105);
            range = new Rectangle((int)pos.X-30, (int)pos.Y-30, 100, 100);
            bubblePos = new Vector2(pos.X - (bubbleSrcRec.Width / 2) + srcRecArray[0].Width, pos.Y - bubbleSrcRec.Height);
            color = Color.Black;
            text = new string[5] 
            {   
                "0 Hello there travelers!\nYou seem to be lost.\n",
                "1 Lorem ipsum dolor sit \namet, consectetur \nadipiscing elit, sed do\neiusmod tempor \nincididunt ut labore et.",
                "2 Lorem ipsum dolor sit \namet, consectetur \nadipiscing elit, sed do\neiusmod tempor \nincididunt ut labore et.",
                "3 Lorem ipsum dolor sit \namet, consectetur \nadipiscing elit, sed do\neiusmod tempor \nincididunt ut labore et.",
                "4 Lorem ipsum dolor sit \namet, consectetur \nadipiscing elit, sed do\neiusmod tempor \nincididunt ut labore et."
            };
        }

        public override void Update(GameTime gT, Player player, Player otherPlayer, Game1 game1)
        {
            depth = Math.Clamp((pos.Y + 32 - game1.camera.Position.Y) / Assets.ScreenHeight, 0, 1);
            if (range.Intersects(player.hitbox) || range.Intersects(otherPlayer.hitbox))
            {
                if (!playerInRange)
                {
                    playerInRange = true;
                }

                frameTimer -= gT.ElapsedGameTime.TotalMilliseconds;
                if (frameTimer <= 0)
                {
                    frameTimer = frameInterval;
                    frame++;
                    if (frame == frameCount)
                    {
                        frame = 0;
                    }
                }
            }
            else
            {
                frame = 0;
                playerInRange = false;
            }

        }
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, srcRecArray[frame], Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, depth);
            if (playerInRange)
            {
                sb.Draw(bubbleTex, bubblePos, bubbleSrcRec, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth);
                sb.DrawString(Assets.font, text[textNum], new Vector2(bubblePos.X+5, bubblePos.Y+5), color, 0, Vector2.Zero, 1, SpriteEffects.None, depth+0.01f);
            }
            sb.DrawLine(new Vector2(range.Left, range.Top), new Vector2(range.Right, range.Top), 1, Color.White);
            sb.DrawLine(new Vector2(range.Left, range.Top), new Vector2(range.Left, range.Bottom), 1, Color.White);
            sb.DrawLine(new Vector2(range.Left, range.Bottom), new Vector2(range.Right, range.Bottom), 1, Color.White);
            sb.DrawLine(new Vector2(range.Right, range.Bottom), new Vector2(range.Right, range.Top), 1, Color.White);
        }
    }
}
