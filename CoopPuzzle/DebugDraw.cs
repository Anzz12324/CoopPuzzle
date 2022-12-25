using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace CoopPuzzle
{
    public static class DebugDraw
    {

        private static Texture2D texture;
        private static Texture2D texCrircle;
        public static void Init(GraphicsDevice graphicsDevice)
        {
            texture = new Texture2D(graphicsDevice, 1, 1);
            Color[] colors = new Color[1] { Color.White };
            texture.SetData(colors);
            GenerateCircleTexture(graphicsDevice, 64);
        }

        public static Texture2D GenerateCircleTexture(GraphicsDevice graphicsDevice, int radius)
        {
            int diameter = radius * 2 + 1;
            Vector2 origin = new Vector2(radius, radius);
            Texture2D tex = new Texture2D(graphicsDevice, diameter, diameter);
            Color[] colors = new Color[diameter * diameter];

            int index = 0;
            for (int i = 0; i < diameter; i++)
            {
                for (int j = 0; j < diameter; j++)
                {
                    float distSq = Vector2.DistanceSquared(origin, new Vector2(i, j));
                    if (distSq < radius * radius)
                        colors[index] = Color.White;
                    else
                        colors[index] = Color.Transparent;
                    index++;
                }
            }
            tex.SetData(colors);
            texCrircle = tex;
            return tex;
        }

        public static void FillCircle(this SpriteBatch spriteBatch, Vector2 position, int radius, Color color)
        {
            Rectangle dest = new Rectangle((int)position.X - radius, (int)position.Y - radius, radius + radius, radius + radius);
            spriteBatch.Draw(texCrircle, dest, color);
        }

        public static void FillRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }
        public static void FillRectangle(this SpriteBatch spriteBatch, Vector2 position, int halfWidth, int halfHeight, float rotation, Color color)
        {
            int x = (int)position.X;
            int y = (int)position.Y;
            int width = halfWidth + halfWidth;
            int height = halfHeight + halfHeight;

            spriteBatch.Draw(texture, new Rectangle(x, y, width, height), new Rectangle(0, 0, width, height), color, rotation, new Vector2(halfWidth, halfHeight), SpriteEffects.None, 0f);
            //FillCircle(spriteBatch, position, 2, Color.Black);
            //DrawRectangle(spriteBatch, position, halfWidth, halfHeight, Color.Black);
        }
        public static void FillRectangle(this SpriteBatch spriteBatch, Vector2 position, int halfWidth, int halfHeight, Color color)
        {
            int x = (int)position.X - halfWidth;
            int y = (int)position.Y - halfHeight;
            int width = halfWidth + halfWidth;
            int height = halfHeight + halfHeight;

            spriteBatch.Draw(texture, new Rectangle(x, y, width, height), color);
        }


        public static void FillRectangle(this SpriteBatch spriteBatch, Vector2 position, Vector2 extents, Color color)
        {
            spriteBatch.FillRectangle(position, (int)extents.X, (int)extents.Y, color);
        }
        public static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 position, Vector2 extents, Color color)
        {
            spriteBatch.DrawRectangle(position, (int)extents.X, (int)extents.Y, color);
        }

        public static void FillRectangle(this SpriteBatch spriteBatch, Vector2 position, Vector2 extents, Color color, float rotation)
        {
            int x = (int)(position.X - extents.X);
            int y = (int)(position.Y - extents.Y);
            int width = (int)(extents.X + extents.X);
            int height = (int)(extents.Y + extents.Y);
            spriteBatch.Draw(texture, position, new Rectangle(x, y, width, height), color, rotation, extents, 1f, SpriteEffects.None, 0f);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Vector2 position, int halfWidth, int halfHeight, Color color)
        {
            int x = (int)position.X;
            int y = (int)position.Y;
            int width = halfWidth + halfWidth;
            int height = halfHeight + halfHeight;

            spriteBatch.Draw(texture, new Rectangle(x - halfWidth, y - halfHeight, width, 1), color);
            spriteBatch.Draw(texture, new Rectangle(x - halfWidth, y - halfHeight, 1, height), color);

            spriteBatch.Draw(texture, new Rectangle(x - halfWidth, y + halfHeight - 1, width, 1), color);
            spriteBatch.Draw(texture, new Rectangle(x + halfWidth - 1, y - halfHeight, 1, height), color);
        }

        public static void DrawRectangle(this SpriteBatch spriteBatch, Rectangle rectangle, Color color)
        {
            int x = rectangle.X;
            int y = rectangle.Y;
            int width = rectangle.Width;
            int height = rectangle.Height;
            spriteBatch.Draw(texture, new Rectangle(x, y, 1, height), color);
            spriteBatch.Draw(texture, new Rectangle(x, y, width, 1), color);
            spriteBatch.Draw(texture, new Rectangle(x, y + height - 1, width, 1), color);
            spriteBatch.Draw(texture, new Rectangle(x + width - 1, y, 1, height), color);
        }
        /// <summary>
        /// Draws a line between point p1 to p2 with width in pixels and color
        /// </summary>
        /// <param name="p1">Starting point as a Vector2</param>
        /// <param name="p2">Ending point as a Vector2</param>
        /// <param name="width">Width of the line in pixels</param>
        /// <param name="color">Color of the line</param>
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 p1, Vector2 p2, int width, Color color)
        {
            Vector2 v = p2 - p1;
            int dist = (int)MathF.Ceiling(v.Length());
            Rectangle rect = new Rectangle(0, 0, dist, width);
            spriteBatch.Draw(texture, p1, rect, color, MathF.Atan2(v.Y, v.X), new Vector2(0, width / 2), 1f, SpriteEffects.None, 0f);
        }
        /// <summary>
        /// Draws a line from point p in the direction of n with length and width in pixels, also a color
        /// </summary>
        /// <param name="p">Starting point as a Vector2</param>
        /// <param name="n">Direction vector as Vector2</param>
        /// <param name="length">Length of the line in pixels</param>
        /// <param name="width">Width of the line in pixels</param>
        /// <param name="color">Color of the line</param>
        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 p, Vector2 n, float length, int width, Color color)
        {
            Rectangle rect = new Rectangle(0, 0, (int)MathF.Ceiling(length), width);
            spriteBatch.Draw(texture, p, rect, color, MathF.Atan2(n.Y, n.X), new Vector2(0, width / 2), 1f, SpriteEffects.None, 0f);
        }
        public delegate float function(float x);
        public static void DrawFunction(this SpriteBatch spriteBatch, function f, float startX, float endX, Color color, float stepSize = 3f, float yOffset = 0, int width = 2)
        {
            for (float x = startX; x <= endX; x += stepSize)
            {
                Vector2 p1 = new Vector2(x, f(x));
                Vector2 p2 = new Vector2(x + stepSize, f(x + stepSize));
                p1.Y += yOffset;
                p2.Y += yOffset;
                spriteBatch.DrawLine(p1, p2, width, color);
            }
        }
    }
}
