using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace LostKingdom.GameContent.Audio
{
    public static class Audio
    {
        public static void PlaySound(SoundEffect sound, float volume = 1f, float pitch = 0f, float pan = 0f)
        {
            sound.Play(volume, pitch, pan);
        }
    }

    public class LegacySound
    {
        private SoundEffect sound;
        public LegacySound(SoundEffect sound)
        {
        }
    }
}
