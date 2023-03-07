using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace LostKingdom.GameContent.UI
{
    public class GamePadEvent
    {
        public Vector2 mouseOffset;
        public int wheelOffset;
        public bool click;

        public GamePadEvent(Vector2 mouseOffset, int wheelOffset, bool click)
        {
            this.mouseOffset = mouseOffset;
            this.wheelOffset = wheelOffset;
            this.click = click;
        }
    }
}
