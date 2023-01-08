

namespace CoopPuzzle.Npc
{
    abstract class NPC
    {

        protected Texture2D tex;
        protected Texture2D bubbleTex;
        protected Vector2 pos;
        protected Vector2 bubblePos;
        protected string text;
        protected Rectangle range;
        protected bool playerInRange;
        protected Rectangle[] srcRecArray;
        protected int frame = 0;
        protected double frameTimer, frameInterval;

        public NPC(Texture2D tex, Texture2D bubbleTex, Vector2 pos, Vector2 bubblePos)
        {
            this.tex = tex;
            this.bubbleTex = bubbleTex;
            this.pos = pos;
            this.bubblePos = bubblePos;
        }

        public abstract void Update(GameTime gT, Player[] players);
        public abstract void Draw(SpriteBatch sb);
    }
}
