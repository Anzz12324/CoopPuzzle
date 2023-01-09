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
        Color color;
        public StoryNpc(Texture2D tex, Vector2 pos, int Npc, int textNum) : base(tex, pos, Npc, textNum)
        {
            srcRecArray = new Rectangle[2];
            srcRecArray[0] = new Rectangle(5, 17, 21, 21);
            srcRecArray[1] = new Rectangle(29, 17, 21, 21);
            bubbleSrcRec = new Rectangle(20, 315, 174, 105);
            range = new Rectangle((int)pos.X-30, (int)pos.Y-30, 100, 100);
            bubblePos = new Vector2(pos.X - (bubbleSrcRec.Width / 2) + srcRecArray[0].Width, pos.Y - bubbleSrcRec.Height);
            color = Color.Transparent;
            text = "Hello my travelers!";
        }

        public override void Update(GameTime gT, Player player, Player otherPlayer, Game1 game1)
        {
            depth = Math.Clamp((pos.Y + 32 - game1.camera.Position.Y) / game1.ScreenHeight, 0, 1);
            if (range.Intersects(player.hitbox) || range.Intersects(otherPlayer.hitbox))
            {
                if (!playerInRange)
                {
                    playerInRange = true;
                    color = Color.Black;
                }

                frameTimer -= gT.ElapsedGameTime.TotalMilliseconds;
                if (frameTimer <= 0)
                {
                    frameTimer = frameInterval;
                    frame++;
                    if (frame == 2)
                    {
                        frame = 0;
                    }
                }
            }
            else
            {
                frame = 0;
                playerInRange = false;
                color = Color.Transparent;
            }

        }
        public override void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, srcRecArray[frame], Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, depth);
            if (playerInRange)
            {
                sb.Draw(bubbleTex, bubblePos, bubbleSrcRec, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth);
                sb.DrawString(Assets.font, text, new Vector2(bubblePos.X+10, bubblePos.Y+5), color, 0, Vector2.Zero, 1, SpriteEffects.None, depth+0.01f);
            }
            sb.DrawLine(new Vector2(range.Left, range.Top), new Vector2(range.Right, range.Top), 1, Color.White);
            sb.DrawLine(new Vector2(range.Left, range.Top), new Vector2(range.Left, range.Bottom), 1, Color.White);
            sb.DrawLine(new Vector2(range.Left, range.Bottom), new Vector2(range.Right, range.Bottom), 1, Color.White);
            sb.DrawLine(new Vector2(range.Right, range.Bottom), new Vector2(range.Right, range.Top), 1, Color.White);
        }
    }
}
