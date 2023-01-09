using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using MonoGame.Extended.Serialization;

namespace CoopPuzzle
{
    public static class Assets
    {
        public static Texture2D white, undertaleRuins, box;
        public static SpriteFont font, bigFont;
        public static SpriteSheet spriteSheet, spriteSheet2;
        public static Song song;

        public static void LoadTextures(ContentManager gd)
        {
            white = gd.Load<Texture2D>("white");
            undertaleRuins = gd.Load<Texture2D>("UndertaleRuins");
            box = gd.Load<Texture2D>("box");
            spriteSheet = gd.Load<SpriteSheet>("frisk.sf", new JsonContentLoader());
            spriteSheet2 = gd.Load<SpriteSheet>("frisk2.sf", new JsonContentLoader());

            font = gd.Load<SpriteFont>("font");
            bigFont = gd.Load<SpriteFont>("bigFont");

            song = gd.Load<Song>("FrogShop");
        }
    }
}
