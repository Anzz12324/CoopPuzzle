

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
        protected int frame = 0, textNum, frameCount, Npc;
        protected double frameTimer, frameInterval = 500;
        protected Rectangle hitbox;
        public virtual Rectangle Hitbox { get { return hitbox; } }
        public object Value { get; }

        public NPC(Vector2 pos, int textNum)
        {
            this.pos = pos;
            this.textNum = textNum;
            this.bubbleTex = Assets.bubbleTex;
            
        }

        public virtual void Update(GameTime gT, Player player, Player otherPlayer, Game1 game1) 
        {
            Collision(player, otherPlayer);
            if (game1.editmode)
                editmode = true;
            depth = Math.Clamp((pos.Y + 32 - game1.camera.Position.Y) / Assets.ScreenHeight, 0, 1);
            if (range.Intersects(player.hitbox) || range.Intersects(otherPlayer.hitbox))
            {
                if (!playerInRange)
                    playerInRange = true;
                if (Npc != 3)
                    Animation(gT);
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
            if (editmode)
            {
                sb.DrawLine(new Vector2(range.Left, range.Top), new Vector2(range.Right, range.Top), 1, Color.White);
                sb.DrawLine(new Vector2(range.Left, range.Top), new Vector2(range.Left, range.Bottom), 1, Color.White);
                sb.DrawLine(new Vector2(range.Left, range.Bottom), new Vector2(range.Right, range.Bottom), 1, Color.White);
                sb.DrawLine(new Vector2(range.Right, range.Bottom), new Vector2(range.Right, range.Top), 1, Color.White);
            }
            sb.FillRectangle(Hitbox, Color.HotPink, 1);
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
                p1.HandleCollision();
            }
            if (hitbox.Intersects(p1.hitbox))
            {
                p2.HandleCollision();
            }
        }
    }
}
