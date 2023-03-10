namespace CoopPuzzle.Npc
{
    abstract class NPC
    {
        protected Color color;
        protected float depth;
        protected Texture2D tex, bubbleTex;
        protected Vector2 pos, bubblePos, textPos;
        protected string[][] dialogueArray;
        protected bool playerInRange;
        protected Rectangle[] srcRecArray;
        protected Rectangle bubbleSrcRec, range;
        protected int frame = 0, dialogueNum, textNum, frameCount;
        public int Npc { get; protected set; }
        protected double frameTimer, frameInterval = 500;
        protected Rectangle hitbox;
        public virtual Rectangle Hitbox { get { return hitbox; } }
        public object Value { get; }
        public Rectangle Range { get { return range; } }
        public Vector2 Pos { get { return pos; } }
        public int TextNum { get { return dialogueNum; } }

        KeyboardState board, prevBoard;

        public NPC(Vector2 pos, int textNum)
        {
            this.pos = pos;
            this.dialogueNum = textNum;
            this.bubbleTex = Assets.bubbleTex;
            
        }

        public virtual void Update(GameTime gT, Player player, Player otherPlayer, Game1 game1)
        {
            prevBoard = board;
            board = Keyboard.GetState();

            Collision(player, otherPlayer);

            depth = Math.Clamp((hitbox.Top - 6 - game1.camera.Position.Y) / Assets.ScreenHeight, 0, 1);
            if (range.Intersects(player.hitbox))
            {
                if (!playerInRange)
                    playerInRange = true;
                if (Npc != 3)
                    Animation(gT);
                if (board.IsKeyDown(Keys.Space) && prevBoard.IsKeyUp(Keys.Space))
                {
                    textNum++;
                    if (textNum >= dialogueArray[dialogueNum].Length)
                        textNum = 0;
                }
            }
            else 
            {
                if (Npc != 3)
                    frame = 0;
                playerInRange = false;
            }
        }
        public virtual void Draw(SpriteBatch sb)
        {
        }

        public virtual void DebugDraw(SpriteBatch sb)
        {
            sb.DrawRectangle(Range, Color.White, 1, 1);
            sb.FillRectangle(Hitbox, Color.HotPink, 1);

            sb.DrawString(Assets.font, dialogueNum.ToString(), new Vector2(pos.X, pos.Y - 16), Color.Black);
        }

        protected void Animation(GameTime gT)
        {
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

        protected void Collision(Player p1, Player p2)
        {
            if (hitbox.Intersects(p1.hitbox))
            {
                p1.HandleCollision(hitbox);
            }
            if (hitbox.Intersects(p1.hitbox))
            {
                p2.HandleCollision(hitbox);
            }
        }

        public void IsButton()
        {
            range = new Rectangle((int)pos.X, (int)pos.Y, Assets.tileSize, Assets.tileSize);
        }
    }
}
