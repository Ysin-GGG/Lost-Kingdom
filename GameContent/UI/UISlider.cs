using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.Utils;

namespace LostKingdom.GameContent.UI
{
    /// <summary> UI组件：滑动条 </summary>
    public class UISlider : UIElement
    {
        public float curValue;
        public float minValue;
        public float maxValue;
        public float stepValue;
        public float percent;
        public Texture2D bar;
        public Texture2D tag;
        public bool sliding;
        public bool showValue;
        public int dights;
        public UIText name;
        public UIText text;
        public event Func<UISlider, float, bool> PercentChanged = new Func<UISlider, float, bool>((slider, percent) => true);
        private int delay = 60;
        private int speed = 60;

        public UISlider(float min = 0.0f, float max = 1.0f, float step = 0.0f, bool showValue = true)
        {
            this.dights = 1;
            this.curValue = min;
            this.minValue = min;
            this.maxValue = max;
            this.stepValue = Math.Abs(step) / max;
            this.showValue = showValue;
            this.bar = Asset.UI["UISlider1"];
            this.tag = Asset.UI["UISliderTag1"];
            this.size = new Vector2(MyMath.Max(this.bar.Width, this.tag.Width), MyMath.Max(this.bar.Height, this.tag.Height));
            this.name = new UIText(new Vector2(100, 30), "", Vector2.One, null, UIText.TextAlign.Right);
            this.name.AddTo(this);
            this.text = new UIText(new Vector2(100, 30), this.percent.ToString(), Vector2.One, null, UIText.TextAlign.Left);
            this.text.AddTo(this);
        }

        protected override void OnMouseDown(UIElement e, MouseEvent evt)
        {
            if (evt.leftButton) this.sliding = true;
        }

        protected override void OnMouseWheel(UIElement e, MouseEvent evt)
        {
            float step = this.stepValue == 0.0f ? .025f : this.stepValue;
            this.SetPercent(this.percent + (evt.wheelOffset > 0 ? step : -step));
        }

        protected override void OnKeyboardDown(UIElement e, KeyboardEvent evt)
        {
            if (this.IsMouseHovering)
            {
                if (evt.down.Contains(Keys.Down)) this.SetPercent(0.0f);
                if (evt.down.Contains(Keys.Up)) this.SetPercent(1.0f);
            }
        }

        public void SetPercent(float percent)
        {
            percent = MyMath.Clamp(this.stepValue > 0.0f ? ((int)Math.Round(percent / this.stepValue, 0) * this.stepValue) : percent, 0.0f, 1.0f);
            if (this.percent != percent && this.PercentChanged.Invoke(this, percent)) 
            {
                this.percent = percent;
                this.curValue = MyMath.Lerp(this.minValue, this.maxValue, this.percent);
            }
        }

        protected override void UpdateUI(GameTime gameTime)
        {
            if (this.sliding)
            {
                if (Main.CurrInput.GetInputState(PlayerInput.Mouses.LeftButton) == PlayerInput.InputState.Released)
                {
                    this.sliding = false;
                    Audio.Audio.RawSound[1].Play();
                }
                float percent = (Main.MouseScreen.X - this.location.X * Options.UIScale) / (this.size.X * Options.UIScale);
                this.SetPercent(percent);
            }
            else if (this.IsMouseHovering || this.IsSelected)
            {
                bool pressLeft = Main.CurrInput.GetInputState(Keys.Left) == PlayerInput.InputState.Pressed || Main.CurrInput.GetInputState(Buttons.DPadLeft) == PlayerInput.InputState.Pressed;
                bool pressRight = Main.CurrInput.GetInputState(Keys.Right) == PlayerInput.InputState.Pressed || Main.CurrInput.GetInputState(Buttons.DPadRight) == PlayerInput.InputState.Pressed;
                if (pressLeft != pressRight)
                {
                    if (delay-- % 5 == 0 && speed > 1) speed--;
                    bool flag = this.delay < 0 && delay % speed == 0;
                    if (pressLeft && flag)
                        this.SetPercent(this.percent - (this.stepValue == 0.0f ? .01f : this.stepValue));
                    if (pressRight && flag)
                        this.SetPercent(this.percent + (this.stepValue == 0.0f ? .01f : this.stepValue));
                }
                else
                {
                    this.speed = this.delay = 30;
                    if (Main.CurrInput.GetInputState(Keys.Left) == PlayerInput.InputState.Down || Main.CurrInput.GetInputState(Buttons.DPadLeft) == PlayerInput.InputState.Down)
                        this.SetPercent(this.percent - (this.stepValue == 0.0f ? .01f : this.stepValue));
                    if (Main.CurrInput.GetInputState(Keys.Right) == PlayerInput.InputState.Down || Main.CurrInput.GetInputState(Buttons.DPadRight) == PlayerInput.InputState.Down)
                        this.SetPercent(this.percent + (this.stepValue == 0.0f ? .01f : this.stepValue));
                }
            }
            else this.speed = this.delay = 30;
            this.name.LocateToParent = new Vector2(-this.name.size.X - 20f, (this.size.Y - this.text.size.Y) / 2);
            this.text.LocateToParent = new Vector2(this.size.X + 20f, (this.size.Y - this.text.size.Y) / 2);
            this.text.text = showValue ? string.Format("{0:f0}% ({1})", this.percent * 100, Math.Round(this.curValue, dights)) : string.Format("{0:f0}", this.percent * 100);
        }

        protected override void DrawUI(SpriteBatch spriteBatch)
        {
            Vector2 pos = this.location;
            pos.Y += this.size.Y * .5f;
            spriteBatch.Draw(this.bar, pos, null, Color.Lerp(Color.Black, Color.White, this.sliding ? MyMath.phi0 : 1f), 0f, this.bar.Left(), 1f, SpriteEffects.None, 0f);
            pos.X += this.size.X * this.percent;
            spriteBatch.Draw(this.tag, pos, null, Color.White, 0f,this.tag.Center(), 1f, SpriteEffects.None, 0f);
        }
    }
}