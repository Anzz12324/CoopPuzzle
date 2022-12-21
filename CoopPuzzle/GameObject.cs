namespace CoopPuzzle
{
    internal class GameObject
    {
        protected Texture2D tex;
        protected Color color;
        protected Vector2 position;
        protected Rectangle srcRec;
        public Rectangle hitbox { get { return new Rectangle((int)position.X, (int)position.Y, 32, 32); } }


        public virtual void Update(GameTime gT) { }
        public virtual void Update(GameTime gT, Block b) { }
        public virtual void Update(GameTime gT, Player[] players) { }
        public virtual void Update(GameTime gT, WeighedSwitch weighedSwitch) { }
        public virtual void Update(GameTime gT, List<GameObject> objects) { }
        public virtual void Update(GameTime gT, List<GameObject> objects, Game1 game1) { }
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Assets.white, hitbox, color);
        }
    }
}
