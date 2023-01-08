﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CoopPuzzle
{
    public static class Assets
    {
        public static Texture2D white, undertaleRuins, box;
        public static SpriteFont font, bigFont;

        public static void LoadTextures(ContentManager gd)
        {
            white = gd.Load<Texture2D>("white");
            undertaleRuins = gd.Load<Texture2D>("UndertaleRuins");
            box = gd.Load<Texture2D>("box");

            font = gd.Load<SpriteFont>("font");
            bigFont = gd.Load<SpriteFont>("bigFont");
        }
    }
}
