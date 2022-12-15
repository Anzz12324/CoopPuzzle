using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace CoopPuzzle
{
    public static class Assets
    {
        public static Texture2D white;
        
        public static void LoadTextures(ContentManager gd)
        {
            white = gd.Load<Texture2D>("white");
           
        }
    }
}
