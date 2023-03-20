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
    /// <summary> 游戏音乐 </summary>
    public class LegacyMusic
    {
        /// <summary> 音频名 </summary>
        public string Name { get => this.sound.Name; set => this.sound.Name = value; }
        /// <summary> 音量 </summary>
        public float Volume { get => this.instance.Volume; private set => this.instance.Volume = value; }
        /// <summary> 音调 </summary>
        public float Pitch { get => this.instance.Pitch; private set => this.instance.Pitch = value; }
        /// <summary> 声道占比 </summary>
        public float Pan { get => this.instance.Pan; private set => this.instance.Pan = value; }
        /// <summary> 音效类型 </summary>
        public Audio.SoundStyle style;
        private float defVolume;
        private float defPitch;
        private float defPan;
        private AudioEmitter emitter;
        private AudioListener listener;
        private SoundEffect sound;
        private SoundEffectInstance instance;

        /// <summary> 调整音频参数 </summary>
        /// <param name="volume"> 音量 </param>
        /// <param name="pitch"> 音调 </param>
        /// <param name="pan"> 声道占比（负数为左声道，正数为右声道） </param>
        public void Modify(float volume = 1f, float pitch = 0f, float pan = 0f)
        {
            this.defVolume = this.Volume = volume;
            this.defPitch = this.Pitch = pitch;
            this.defPan = this.Pan = pan;
        }
    }
}
