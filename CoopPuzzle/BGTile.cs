

namespace CoopPuzzle
{
    public class BGTile
    {
        private Texture2D tex;
        private Vector2 pos;
        private Rectangle[] srcRec;
        private int rndSrc, texNum;

        public Vector2 Pos { get { return pos; } }

        public BGTile(Vector2 pos, int texNum, int rndSrc)
        {
            this.pos = pos;
            this.rndSrc = rndSrc;
            this.texNum = texNum;

            LoadTex();
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Draw(tex, pos, srcRec[rndSrc], Color.White);
        }

        private void LoadTex()
        {
            if (texNum == 0)
            {
                tex = Assets.StoneTileSet;
                srcRec = new Rectangle[10];
                srcRec[0] = new Rectangle(8,6,40,40);
                srcRec[1] = new Rectangle(47,4,40,40);
                srcRec[2] = new Rectangle(46,47,40,40);
                srcRec[3] = new Rectangle(12,44,40,40);
                srcRec[4] = new Rectangle(170,43,40,40);
                srcRec[5] = new Rectangle(8, 6, 40, 40);
                srcRec[6] = new Rectangle(47, 4, 40, 40);
                srcRec[7] = new Rectangle(46, 47, 40, 40);
                srcRec[8] = new Rectangle(12, 44, 40, 40);
                srcRec[9] = new Rectangle(170, 43, 40, 40);
            }
            if (texNum == 1)
            {
                tex = Assets.GrassTileSet;
                srcRec= new Rectangle[10];
                srcRec[0] = new Rectangle(157,93,40,40);
                srcRec[1] = new Rectangle(125,30,40,40);
                srcRec[2] = new Rectangle(76,13,40,40);
                srcRec[3] = new Rectangle(63,71,40,40);
                srcRec[4] = new Rectangle(15,54,40,40);
                srcRec[5] = new Rectangle(76, 13, 40, 40);
                srcRec[6] = new Rectangle(63, 71, 40, 40);
                srcRec[7] = new Rectangle(15, 54, 40, 40);
                srcRec[8] = new Rectangle(76, 13, 40, 40);
                srcRec[9] = new Rectangle(63, 71, 40, 40);
            }
        }
    }
}
