

namespace CoopPuzzle.Npc
{
    abstract class NPC
    {
        protected Color color;
        protected float depth;
        protected Texture2D tex, bubbleTex;
        protected Vector2 pos, bubblePos, textPos;
        protected string[] text;
        protected bool playerInRange, editmode = false;
        protected Rectangle[] srcRecArray;
        protected Rectangle bubbleSrcRec, range;
        protected int frame = 0, textNum, frameCount;
        protected double frameTimer, frameInterval = 500; 
        public object Value { get; }
        public Rectangle Range { get { return range; } }

        public NPC(Vector2 pos, int textNum)
        {
            this.pos = pos;
            this.textNum = textNum;
            this.bubbleTex = Assets.bubbleTex;
            
        }

        public virtual void Update(GameTime gT, Player player, Player otherPlayer, Game1 game1) 
        {
            if (game1.editmode)
                editmode = true;
            depth = Math.Clamp((pos.Y + 32 - game1.camera.Position.Y) / Assets.ScreenHeight, 0, 1);
            if (range.Intersects(player.hitbox) || range.Intersects(otherPlayer.hitbox))
            {
                if (!playerInRange)
                    playerInRange = true;

                Animation(gT);
            }
            else
            {
                frame = 0;
                playerInRange = false;
            }
        }
        public virtual void Draw(SpriteBatch sb)
        {
            if (editmode)
            {
                sb.DrawLine(new Vector2(range.Left, range.Top), new Vector2(range.Right, range.Top), 1, Color.White);
                sb.DrawLine(new Vector2(range.Left, range.Top), new Vector2(range.Left, range.Bottom), 1, Color.White);
                sb.DrawLine(new Vector2(range.Left, range.Bottom), new Vector2(range.Right, range.Bottom), 1, Color.White);
                sb.DrawLine(new Vector2(range.Right, range.Bottom), new Vector2(range.Right, range.Top), 1, Color.White);
            }            
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
    }
}
