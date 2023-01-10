using MonoGame.Extended;

namespace CoopPuzzle
{
    abstract class GameObject
    {
        protected Texture2D tex;
        protected Color color;
        protected Vector2 position;
        protected Vector2 size = Vector2.One * 40;
        protected float depth;

        public Vector2 Pos { get { return position; } set { position = value; } }
        public Vector2 Size { get { return size; } }
        public virtual Rectangle hitbox { get { return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y); } }
        public Rectangle HUDhitbox { get { return new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y); } }
        public Color TempColor { set; get; }
        public Color Color { set { color = value; } get { return color; } }

        public GameObject(Vector2 position, Color color)
        {
            this.position = position;
            this.color = color;
            TempColor = color;
        }

        public virtual void Update(GameTime gT) { }
        public virtual void Update(GameTime gT, Block b) { }
        public virtual void Update(GameTime gT, Player[] players) { }
        public virtual void Update(GameTime gT, WeighedSwitch weighedSwitch) { }
        public virtual void Update(GameTime gT, List<GameObject> objects) { }
        public virtual void Update(GameTime gT, List<GameObject> objects, Player[] players) { }
        public virtual void Update(GameTime gT, List<GameObject> objects, Game1 game1) { }
        public virtual void Update(GameTime gT, Game1 game1) { depth = (Pos.Y + 20 - game1.camera.Position.Y) / Assets.ScreenHeight; }
        public virtual void Draw(SpriteBatch sb)
        {
            sb.Draw(Assets.white, hitbox, TempColor);
        }
    }
}
