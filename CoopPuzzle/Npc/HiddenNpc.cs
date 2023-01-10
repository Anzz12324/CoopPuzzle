

namespace CoopPuzzle.Npc
{
    internal class HiddenNpc : NPC
    {
        public int Npc { get; private set; }
        private Rectangle grillbyTable;
        public HiddenNpc(Vector2 pos, int Npc, int textNum) : base(pos, textNum)
        {
            this.Npc = Npc;
            LoadSrcRec();
        }


        public override void Update(GameTime gT, Player player, Player otherPlayer, Game1 game1)
        {
            if (Npc == 3)
            {
                Animation(gT);
                if (range.Intersects(player.hitbox) || range.Intersects(otherPlayer.hitbox))
                {
                    if (!playerInRange)
                        playerInRange = true;
                }
                else
                {
                    playerInRange = false;
                }
            }
            else
            {
                base.Update(gT, player, otherPlayer, game1);
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Npc == 3)
            {
                sb.Draw(tex, new Vector2(pos.X-70, pos.Y + 80), grillbyTable, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, depth+0.01f);
                sb.DrawRectangle(range, Color.White);
            }
            sb.Draw(tex, pos, srcRecArray[frame], Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, depth + 0.02f);
            if (playerInRange)
            {
                sb.Draw(bubbleTex, bubblePos, bubbleSrcRec, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth+0.03f);
                sb.DrawString(Assets.font, text[textNum], textPos, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, depth + 0.04f);
            }
            base.Draw(sb);
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
                bubblePos = new Vector2(pos.X + 130, pos.Y-20);
                bubbleSrcRec = new Rectangle(21, 18, 99, 108);
                range = new Rectangle((int)pos.X, (int)pos.Y + srcRecArray[0].Height, 200, 100);
                textPos = new Vector2(bubblePos.X + 15, bubblePos.Y + 5);
                text = new string[5]
                {
                    "Lorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum",
                    "Lorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum",
                    "Lorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum",
                    "Lorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum",
                    "Lorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum"
                };

            }
            else if (Npc == 2)
            {
                //Asgore Dreemurr
                tex = Assets.asgore;
                frameCount = 2;
                srcRecArray = new Rectangle[frameCount];
                srcRecArray[0] = new Rectangle(5,432,55,61);
                srcRecArray[1] = new Rectangle(65, 432, 55, 61);
                bubblePos = new Vector2(pos.X-30, pos.Y-100);
                bubbleSrcRec = new Rectangle(20, 315, 174, 105);
                range = new Rectangle((int)pos.X, (int)pos.Y+100, 110, 100);
                textPos = new Vector2(bubblePos.X + 5, bubblePos.Y + 10);
                text = new string[5]
                {
                    "Lorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum",
                    "Lorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum",
                    "Lorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum",
                    "Lorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum",
                    "Lorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum\nLorem ipsum Lorem ipsum"
                };
            }
            else if (Npc == 3)
            {
                //Grillby
                tex = Assets.grillby;
                frameInterval = 150;
                frameCount = 5;
                srcRecArray = new Rectangle[frameCount];
                srcRecArray[0] = new Rectangle(8,364,24,53);
                srcRecArray[1] = new Rectangle(38, 364, 24, 53);
                srcRecArray[2] = new Rectangle(68, 364, 24, 53);
                srcRecArray[3] = new Rectangle(98, 364, 24, 53);
                srcRecArray[4] = new Rectangle(128, 364, 24, 53);
                grillbyTable = new Rectangle(8,441,134,35);
                bubblePos = new Vector2(pos.X+50,pos.Y);
                bubbleSrcRec = new Rectangle(21,18,99,108);
                range = new Rectangle((int)pos.X-40,(int)pos.Y + 149, 130,40);
                textPos = new Vector2(bubblePos.X + 15, bubblePos.Y + 5);
                text = new string[5]
                {
                    "Lorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum",
                    "Lorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum",
                    "Lorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum",
                    "Lorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum",
                    "Lorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum\nLorem ipsum"
                };
            }
        }
    }
}
