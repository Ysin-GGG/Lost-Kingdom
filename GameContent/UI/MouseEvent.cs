using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace LostKingdom.GameContent.UI
{
    public class MouseEvent
    {
        public Vector2 mouseOffset;
        public int wheelOffset;
        public bool click;
        public bool leftButton;
        public bool middleButton;
        public bool rightButton;
        public bool XButton1;
        public bool XButton2;

        public MouseEvent(Vector2 mouseOffset, int wheelOffset, bool click)
        {
            this.mouseOffset = mouseOffset;
            this.wheelOffset = wheelOffset;
            this.click = click;
            this.leftButton = false;
            this.middleButton = false;
            this.rightButton = false;
            this.XButton1 = false;
            this.XButton2 = false;
        }
    }
}
