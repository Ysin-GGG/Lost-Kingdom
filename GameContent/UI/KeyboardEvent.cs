using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace LostKingdom.GameContent.UI
{
    public class KeyboardEvent
    {
        public List<Keys> down;
        public List<Keys> up;

        public KeyboardEvent()
        {
            this.down = new List<Keys>();
            this.up = new List<Keys>();
        }
    }
}
