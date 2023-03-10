

namespace CoopPuzzle.Npc
{
    internal class HiddenNpc : NPC
    {
        private Rectangle grillbyTable;
        private Vector2 grillbyTablePos;
        public HiddenNpc(Vector2 pos, int Npc, int textNum) : base(pos, textNum)
        {
            this.Npc = Npc;
            LoadSrcRec();
        }


        public override void Update(GameTime gT, Player player, Player otherPlayer, Game1 game1)
        {
            if (Npc == 3)
                Animation(gT);
            
            base.Update(gT, player, otherPlayer, game1);
            
        }

        public override void Draw(SpriteBatch sb)
        {
            if (Npc == 3)
                sb.Draw(tex, grillbyTablePos, grillbyTable, Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, depth+0.01f);

            sb.Draw(tex, pos, srcRecArray[frame], Color.White, 0, Vector2.Zero, 2, SpriteEffects.None, depth + 0.02f);

            if (playerInRange)
            {
                sb.Draw(bubbleTex, bubblePos, bubbleSrcRec, Color.White, 0, Vector2.Zero, 1, SpriteEffects.None, depth+0.03f);
                sb.DrawString(Assets.font, dialogueArray[dialogueNum][textNum], textPos, Color.Black, 0, Vector2.Zero, 1, SpriteEffects.None, depth + 0.04f);
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
                dialogueArray = new string[][]
                {
                    new string[]
                    {
                        "It takes 12\npounds of \nmilk to\nproduce just\n1 gallon of\nice cream",
                        "The most \npopular \nflavor is \nvanilla, then\nchocolate",
                        "Chocolate\nice cream\nwas invented\nbefore \nvanilla"
                    }
                };
                hitbox = new Rectangle((int)pos.X+20, (int)pos.Y + (srcRecArray[0].Height * 2 - 30), srcRecArray[0].Width * 2-25 , 20);
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
                dialogueArray = new string[][]
                {
                    new string[]
                    {
                        "Hello, thank you for\ngetting the letter\nto me. lets see what\nit says.",
                        "Hmm... ok... hmm...\nok... hmm... ok...\nhmm... ok... hmm...\nok... hmm... ok...",
                        "I see this was just \na letter to inform on\nthat i am\n",
                        "Behind on the bills\nand that the goverment\nwill come and take the\ncastle"
                    }
                };
                hitbox = new Rectangle((int)pos.X, (int)pos.Y + (srcRecArray[0].Height * 2 - 10), srcRecArray[0].Width * 2, 10);
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
                grillbyTablePos = new Vector2(pos.X - 68, pos.Y + 80);
                bubblePos = new Vector2(pos.X+50,pos.Y);
                bubbleSrcRec = new Rectangle(21,18,99,108);
                range = new Rectangle((int)pos.X-40,(int)pos.Y + 149, 130,40);
                textPos = new Vector2(bubblePos.X + 15, bubblePos.Y + 5);
                dialogueArray = new string[][]
                {
                    new string[]
                    {
                        "\nI'm afraid for\nthe calendar.\nIts days are\nnumbered.",
                        "I only know\n25 letters of\nthe alphabet.\nI don't\nknow y.",
                        "I asked my\ndog what's \ntwo minus\ntwo. He said\nnothing.",
                    }
                };
                hitbox = new Rectangle((int)grillbyTablePos.X, (int)grillbyTablePos.Y + (grillbyTable.Height * 2 - 10), grillbyTable.Width * 2, 10);
            }
        }
    }
}
