using System;
using System.Diagnostics;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.GameContent.Audio;
using LostKingdom.Utils;

namespace LostKingdom.GameContent.UI
{
    /// <summary> 下拉框 </summary>
    public class UIComboBox : UIElement
    {
        public Texture2D image;
        public Vector2 scale;
        public UIElement item;
        public UIText name;
        public UIText line;
        public float lineWidth { get => this.line.size.X; set => this.line.size.X = value; }
        
        public UIComboBox(Texture2D image)
        {
            this.image = image;
            this.scale = Vector2.One;
            this.size = image.Size();
            this.size.Y /= 2.0f;
            this.name = new UIText(new Vector2(300, 30), "", Vector2.One, null, UIText.TextAlign.Right);
            this.name.AddTo(this);
            this.line = new UIText(this.size, "", this.scale * .8f);
            this.line.AddTo(this);
            this.item = new UIElement(Vector2.Zero, this.line.size);
            this.item.IgnoreMe = !(this.item.ChildrenVisible = this.item.ChildrenActive = false);
            this.item.AddTo(this);
        }

        protected override void OnMouseEnter(UIElement e, MouseEvent evt)
        {
            if(e == this || this.item.ChildrenVisible) Audio.Audio.RawSound[0].Replay();
        }

        protected override void OnMouseClick(UIElement e, MouseEvent evt)
        {
            if (evt.leftButton)
            {
                Audio.Audio.RawSound[1].Play();
                if (e == this) this.item.ChildrenVisible = this.item.ChildrenActive = !this.item.ChildrenVisible;
                else
                {
                    this.item.IgnoreMe = !(this.item.ChildrenVisible = this.item.ChildrenActive = false);
                    this.line.text = ((UIText)e).text;
                }
            }
        }

        /// <summary> 添加选项 </summary>
        public UIText AddLine(string line, Action<UIElement, MouseEvent> click = null)
        {
            UIText text = new UIText(new Vector2(this.lineWidth, this.image.Height / 2f), line, this.line.scale);
            text.IgnoreMe = false;
            text.FocusMe = true;
            text.id = this.item.children.Count;
            text.AddTo(this.item);
            text.MouseClick += click;
            text.MouseClick += this.OnMouseClick;
            text.MouseEnter += this.OnMouseEnter;
            text.LocateToParent = new Vector2(0f, this.image.Height / 2 * this.item.children.Count - 2);
            return text;
        }

        /// <summary> 设为某个选项（仅改变文本） </summary>
        /// <param name="id"></param>
        public bool SetLine(int id)
        {
            try
            {
                UIText text = (UIText)this.item.children[id];
                this.line.text = text.text;
                return true;
            }
            catch
            {
                return false;
            }
        }

        protected override void UpdateUI(GameTime gameTime)
        {
            if (this.item.ChildrenVisible && !this.IsMouseHovering && Main.CurrInput.GetInputState(PlayerInput.Mouses.LeftButton) == PlayerInput.InputState.Up)
                this.item.IgnoreMe = !(this.item.ChildrenVisible = this.item.ChildrenActive = false);
            if (!this.item.ChildrenVisible) this.size.Y = this.image.Height / 2;
            else this.size.Y = this.image.Height / 2 * (this.item.children.Count + 1);
            this.item.size = this.size;
            this.item.MoveTo(this.location);
            this.name.LocateToParent = new Vector2(-this.name.size.X - 20f, (this.image.Height / 2 - this.name.size.Y) / 2);
            this.line.LocateToParent = Vector2.Zero;
        }

        protected sealed override void DrawUI(SpriteBatch spriteBatch)
        {
            Rectangle frame = this.image.Frame(0, this.item.ChildrenVisible ? 1 : 0, 1, 2);
            Color color = Color.Lerp(Color.Black, Color.White, this.item.ChildrenVisible || this.IsMouseHovering ? MyMath.phi0 : 1f);
            if (this.image != null) spriteBatch.Draw(this.image, this.location, frame, color, 0f, Vector2.Zero, this.scale, 0, 0f);
        }
    }
}
