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
    /// <summary> 选项卡窗口 </summary>
    public class UITabbedPane : UIElement
    {
        public Texture2D panelBox;
        public int page;

        public UITabbedPane(Vector2 size)
        {
            this.panelBox = Asset.UI["UITabbedPanel1"];
            this.size = size;
            this.page = 0;
            this.IgnoreMe = true;
        }

        public void AddTab(UITab tab)
        {
            if ((tab.id = this.children.Count) == 0)
                tab.Visible = tab.ChildrenActive = tab.ChildrenVisible = true;
            this.AddFrom(tab);
        }

        protected override void DrawUI(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(this.panelBox, new Rectangle((int)location.X, (int)location.Y, (int)this.size.X, (int)this.size.Y), null, Color.White, 0f, Vector2.Zero, SpriteEffects.None, 0f);
        }
    }
}