﻿

namespace CoopPuzzle.Npc
{
    abstract class NPC
    {
        protected Color color;
        protected float depth;
        protected Texture2D tex, bubbleTex;
        protected Vector2 pos, bubblePos;
        protected string[] text;
        protected bool playerInRange;
        protected Rectangle[] srcRecArray;
        protected Rectangle bubbleSrcRec, range;
        protected int frame = 0, Npc, textNum, frameCount;
        protected double frameTimer, frameInterval = 500; 
        public object Value { get; }

        public NPC(Texture2D tex, Vector2 pos, int frameCount, int Npc, int textNum)
        {
            this.tex = tex;
            this.pos = pos;
            this.Npc = Npc;
            this.frameCount= frameCount;
            this.textNum = textNum;
            this.bubbleTex = Assets.bubbleTex;
            
        }

        public virtual void Update(GameTime gT, Player player, Player otherPlayer, Game1 game1) { }
        public abstract void Draw(SpriteBatch sb);
    }
}
