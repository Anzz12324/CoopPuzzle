using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoopPuzzle
{
    internal class SoundManager
    {
        public SoundManager()
        {
            MediaPlayer.Play(Assets.song);
            MediaPlayer.IsRepeating = true;
            MediaPlayer.Volume = 0.25f;
        }
    }
}
