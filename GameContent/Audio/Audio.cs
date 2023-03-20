using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace LostKingdom.GameContent.Audio
{
    public static class Audio
    {
        public static LegacySound[] RawSound;
        public static LegacySound[] RawMusic;
        public static LegacyMusic CurMusic;

        public static void PlaySound(string name, float volume = 1f, float pitch = 0f, float pan = 0f)
        {
            SoundEffect sound = Asset.Sound[name];
            sound.Play(volume, pitch, pan);
        }

        public static void Initialize()
        {
            List<LegacySound> sounds = new List<LegacySound>();
            sounds.Add(new LegacySound("Mouse_Hover"));
            sounds.Add(new LegacySound("Mouse_Click"));
            Audio.RawSound = sounds.ToArray();
            //Main.PostUpdate += new Action<GameTime>((gameTime) => Audio.CurMusic.Modify(Options.MusicVolume * Options.MasterVolume));
        }

        public enum SoundStyle
        {
            None = 0,
            Surround = 1,
            Stereo = 2,
        }
    }
}
