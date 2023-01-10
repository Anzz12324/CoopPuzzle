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
        public static Texture2D white, undertaleRuins, box, brick, flowey, bubbleTex, wait, grillby, creamGuy, asgore, mettaton, GrassTileSet, StoneTileSet;
        public static SpriteFont font, bigFont;
        public static SpriteSheet spriteSheet, spriteSheet2;
        public static Song song;

        public static int ScreenWidth, ScreenHeight, tileSize = 40;

        public static Color[] colors = new Color[]
        {
            Color.White,
            Color.Tomato,
            Color.PaleGreen,
            Color.DodgerBlue,
            Color.Khaki,
            Color.Orchid
        };

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
            brick = GetTextureFromTileset(undertaleRuins, new Rectangle(201, 685, 20, 20));

            bubbleTex = gd.Load<Texture2D>("UndertaleTextBubbles");
            flowey = gd.Load<Texture2D>("UndertaleFlowey");
            wait = gd.Load<Texture2D>("Wait");
            grillby = gd.Load<Texture2D>("UndertaleGrillby");
            creamGuy = gd.Load<Texture2D>("UndertaleNiceCreamGuy");
            asgore = gd.Load<Texture2D>("UndertaleAsgoreDreemurr");
            mettaton = gd.Load<Texture2D>("UndertaleMettaton");

            GrassTileSet = gd.Load<Texture2D>("TX Tileset Grass");
            StoneTileSet = gd.Load<Texture2D>("TX Tileset Stone Ground");
        }

        static Texture2D GetTextureFromTileset(Texture2D originalTexture, Rectangle srcRect)
        {
            // Create a new texture that is the same size as the original texture
            Texture2D newTexture = new Texture2D(originalTexture.GraphicsDevice, srcRect.Width, srcRect.Height);

            // Retrieve a portion of the pixel data from the original texture
            Color[] pixelData = new Color[srcRect.Width * srcRect.Height];
            originalTexture.GetData(0, srcRect, pixelData, 0, pixelData.Length);

            // Set the pixel data for the new texture
            newTexture.SetData(pixelData);
            return newTexture;
        }
    }
}
