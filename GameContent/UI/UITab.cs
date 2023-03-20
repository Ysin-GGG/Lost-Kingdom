using System;
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
    public class UITab : UIElement
    {
        /// <summary> 标签名（将会绘制在窗口上） </summary>
        public string name;
        public Texture2D box;
        private UIText text;

        /// <summary> 外部绘制委托（Vector2为UI相对于窗口的坐标） </summary>
        public new Action<SpriteBatch, Vector2> Draw;

        public UITab(string name)
        {
            this.name = name;
            this.box = Asset.UI["UITab1"];
            this.size = this.box.Size();
            this.ChildrenActive = false;
            this.ChildrenVisible = false;
            this.text = new UIText(this.size, name, Vector2.One * .8f, null, UIText.TextAlign.Bottom);
        }

        protected override void OnMouseClick(UIElement e, MouseEvent evt)
        {
            if (evt.leftButton == true)
            {
                UITabbedPane panel = this.parent as UITabbedPane;
                panel.children[panel.page].ChildrenActive = false;
                panel.children[panel.page].ChildrenVisible = false;
                panel.page = this.id;
                this.ChildrenActive = true;
                this.ChildrenVisible = true;
            }
        }

        protected override void UpdateUI(GameTime gameTime)
        {
            this.LocateToParent = new Vector2(this.box.Width * 1.2f * this.id + 24, -this.box.Height);
            if (this.ChildrenVisible) this.location.Y += 10f;
            this.text.location = this.location;
        }

        /// <summary> 绘制在窗口上的内容 </summary>
        protected override void DrawUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.box, this.location, null, Color.White, 0f, Vector2.Zero, 1f, SpriteEffects.None, 0f);
            this.text.Draw(spriteBatch);
            this.Draw?.Invoke(spriteBatch, parent.location);
        }
    }
}