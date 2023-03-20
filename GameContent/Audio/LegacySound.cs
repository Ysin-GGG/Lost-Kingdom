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
    /// <summary> 游戏音效 </summary>
    public class LegacySound
    {
        /// <summary> 音频名 </summary>
        public string Name { get => this.sound.Name; set => this.sound.Name = value; }
        /// <summary> 音量 </summary>
        public float Volume { get => this.instance.Volume; private set => this.instance.Volume = value; }
        /// <summary> 音调 </summary>
        public float Pitch { get => this.instance.Pitch; private set => this.instance.Pitch = value; }
        /// <summary> 声道占比 </summary>
        public float Pan { get => this.instance.Pan; private set => this.instance.Pan = value; }
        /// <summary> 音频时长 </summary>
        public long time;
        /// <summary> 自动循环播放 </summary>
        public bool loop;
        /// <summary> 是否属于环境音效 </summary>
        public bool ambient;
        /// <summary> 音效类型 </summary>
        public Audio.SoundStyle style;
        private float defVolume;
        private float defPitch;
        private float defPan;
        private AudioEmitter emitter;
        private AudioListener listener;
        private SoundEffect sound;
        private SoundEffectInstance instance;
        private static Timer timer = new Timer();
        private List<LegacySound> binds = new List<LegacySound>();

        /// <summary> 从资源库中建立一个音频 </summary>
        public LegacySound(string name, Audio.SoundStyle style = Audio.SoundStyle.None)
        {
            this.sound = Asset.Sound[name];
            this.instance = this.sound.CreateInstance();
            this.time = this.sound.Duration.Ticks;
            this.loop = false;
            this.style = style;
            this.Name = name;
            this.emitter = null;
            this.listener = null;
            this.Modify();
        }

        /// <summary> 重播音频 </summary>
        /// <param name="position"> 声源位置 </param>
        public void Replay(Vector3? position = null)
        {
            if (this.instance.State != SoundState.Stopped) this.instance.Stop();
            this.instance.Volume = this.defVolume * Options.SoundVolume * Options.MasterVolume;
            this.instance.Play();
        }

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

        /// <summary> 播放音频 </summary>
        /// <param name="position"> 声源位置 </param>
        public void Play(Vector3? position = null)
        {
            this.instance.Volume = this.defVolume * Options.SoundVolume * Options.MasterVolume;
            switch (this.instance.State)
            {
                case SoundState.Paused:
                    this.instance.Resume();
                    break;
                case SoundState.Stopped:
                    this.instance.Play();
                    break;
                case SoundState.Playing:
                    if (this.instance.IsLooped) this.Replay(position);
                    break;
            }
            foreach (LegacySound sound in this.binds)
                sound.Play(position);
        }

        /// <summary> 暂停播放音频 </summary>
        /// <param name="gradual"> 渐出渐入 </param>
        public void Pause(bool gradual = false)
        {
            if (!gradual) this.Volume = 0.0f;
            if (!timer.Enabled) timer.Start();
            timer.Tick += this.GradualVolume;
        }

        private void GradualVolume(object o, EventArgs args)
        {
            this.Volume = (this.Volume - .02f) * .98f;
            if (this.Volume <= 0.0f)
            {
                timer.Tick -= this.GradualVolume;
                this.Volume = 0.0f;
                this.instance.Pause();
            }
        }

        /// <summary> 绑定播放 </summary>
        /// <param name="sound"> 音频 </param>
        public void Bind(LegacySound sound)
        {
            if (!this.binds.Contains(sound))
                this.binds.Add(sound);
        }

        public void Apply3D()
        {
            this.emitter = new AudioEmitter();
            this.listener = new AudioListener();
        }
    }
}
