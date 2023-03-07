using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.GameContent.UI;
using LostKingdom.Utils;

namespace LostKingdom.GameContent
{
    public class PlayerInput
    {
        public PlayerIndex index;
        public MouseState currMouseState;
        public MouseState lastMouseState;
        public GamePadState currGamePadState;
        public GamePadState lastGamePadState;
        public KeyboardState currKeyboardState;
        public KeyboardState lastKeyboardState;
        public int doubleClickDelay;
        private int[] _doubleClickDelay;
        public UIElement focusUI;

        public bool[] mouseState;
        public bool[] gamepadState;
        public bool[] keyboardState;
        public float opacity;

        public string mouseMsg;
        public string gamePadMsg;
        public string keyboardMsg;

        public enum InputState
        {
            /// <summary> 按下时 </summary>
            Down,
            /// <summary> 松开时 </summary>
            Up,
            /// <summary> 按住 </summary>
            Pressed,
            /// <summary> 放开 </summary>
            Released,
        }

        public PlayerInput(PlayerIndex who = PlayerIndex.One, int doubleClickDelay = 30)
        {
            this.index = who;
            this.doubleClickDelay = doubleClickDelay;
            this.currMouseState = this.lastMouseState = Mouse.GetState();
            this.currGamePadState = this.lastGamePadState = GamePad.GetState(who);
            this.currKeyboardState = this.lastKeyboardState = Keyboard.GetState(who);
            this._doubleClickDelay = new int[3];
            this.opacity = 0f;
            this.mouseMsg = "";
            this.gamePadMsg = "";
            this.keyboardMsg = "";
            this.focusUI = null;
        }

        public bool GetKeyboardCombination(KeyState state, params Keys[] keys)
        {
            if (state == KeyState.Down)
                foreach (Keys key in keys)
                    if (this.currKeyboardState.IsKeyDown(key)) return false;
            if (state == KeyState.Up)
                foreach (Keys key in keys)
                    if (this.currKeyboardState.IsKeyUp(key)) return false;
            return true;
        }

        public bool GetGamePadCombination(ButtonState state, params Buttons[] buttons)
        {
            if (state == ButtonState.Pressed)
                foreach (Buttons button in buttons)
                    if (this.currGamePadState.IsButtonDown(button)) return false;
            if (state == ButtonState.Released)
                foreach (Buttons button in buttons)
                    if (this.currGamePadState.IsButtonUp(button)) return false;
            return true;
        }

        public InputState GetInputState(Keys key)
        {
            bool curr = this.currKeyboardState.IsKeyDown(key);
            bool last = this.lastKeyboardState.IsKeyDown(key);
            if (curr) return last ? InputState.Pressed : InputState.Down;
            return !last ? InputState.Released : InputState.Up;
        }

        public InputState GetInputState(Buttons button)
        {
            bool curr = this.currGamePadState.IsButtonDown(button);
            bool last = this.lastGamePadState.IsButtonDown(button);
            if (curr) return last ? InputState.Pressed : InputState.Down;
            return !last ? InputState.Released : InputState.Up;
        }
        
        public void Update(GameTime gameTime)
        {
            List<string> msg = new List<string>();
            MouseState mouseCurr = this.currMouseState;
            MouseState mouseLast = this.lastMouseState;
            Vector2 mouseOffset = new Vector2(mouseCurr.X - mouseLast.X, mouseCurr.Y - mouseLast.Y);
            int wheelOffset = mouseCurr.ScrollWheelValue - mouseLast.ScrollWheelValue;
            MouseEvent evt = new MouseEvent(mouseOffset, wheelOffset, true);

            bool buttonCurr, buttonLast, down = false, up = false;
            buttonCurr = mouseCurr.LeftButton == ButtonState.Pressed;
            buttonLast = mouseLast.LeftButton == ButtonState.Pressed;
            down |= buttonCurr && !buttonLast;
            up |= buttonLast && !buttonCurr;
            if (buttonLast && !buttonCurr)
            {
                this.MouseClick?.Invoke(this, evt);
                if (this._doubleClickDelay[0] > 0)
                {
                    msg.Add(Localization.Localization.GetText("PlayerInput.Mouse.LeftDoubleClick"));
                    this.MouseClick2?.Invoke(this, evt);
                    this._doubleClickDelay[0] = 0;
                }
                else
                {
                    msg.Add(Localization.Localization.GetText("PlayerInput.Mouse.LeftClick"));
                    this._doubleClickDelay[0] = this.doubleClickDelay;
                }
            }

            buttonCurr = mouseCurr.MiddleButton == ButtonState.Pressed;
            buttonLast = mouseLast.MiddleButton == ButtonState.Pressed;
            down |= buttonCurr && !buttonLast;
            up |= buttonLast && !buttonCurr;
            if (buttonLast && !buttonCurr)
            {
                this.MouseMiddleClick?.Invoke(this, evt);
                if (this._doubleClickDelay[1] > 0)
                {
                    msg.Add(Localization.Localization.GetText("PlayerInput.Mouse.MiddleDoubleClick"));
                    this.MouseMiddleClick2?.Invoke(this, evt);
                    this._doubleClickDelay[1] = 0;
                }
                else
                {
                    msg.Add(Localization.Localization.GetText("PlayerInput.Mouse.MiddleClick"));
                    this._doubleClickDelay[1] = this.doubleClickDelay;
                }
            }

            buttonCurr = mouseCurr.RightButton == ButtonState.Pressed;
            buttonLast = mouseLast.RightButton == ButtonState.Pressed;
            down |= buttonCurr && !buttonLast;
            up |= buttonLast && !buttonCurr;
            if (buttonLast && !buttonCurr)
            {
                this.MouseRightClick?.Invoke(this, evt);
                if (this._doubleClickDelay[2] > 0)
                {
                    msg.Add(Localization.Localization.GetText("PlayerInput.Mouse.RightDoubleClick"));
                    this.MouseRightClick2?.Invoke(this, evt);
                    this._doubleClickDelay[2] = 0;
                }
                else
                {
                    msg.Add(Localization.Localization.GetText("PlayerInput.Mouse.RightClick"));
                    this._doubleClickDelay[2] = this.doubleClickDelay;
                }
            }

            if (down) this.MouseDown?.Invoke(this, evt);
            if (up) this.MouseUp?.Invoke(this, evt);

            if (wheelOffset != 0)
            {
                msg.Add(Localization.Localization.GetText(wheelOffset > 0 ? "PlayerInput.Mouse.WheelUp" : "PlayerInput.Mouse.WheelDown"));
                this.MouseWheel?.Invoke(this, evt);
            }

            for (int i = 0; i < 3; i++)
                if (this._doubleClickDelay[i] > 0)
                    this._doubleClickDelay[i]--;
            
            if (msg.Count > 0)
            {
                this.mouseMsg = string.Join(" + ", msg.ToArray());
                this.opacity = 3f;
            }

            msg.Clear();
            GamePadState gamePadCurr = this.currGamePadState;
            GamePadState gamePadLast = this.lastGamePadState;
            if (gamePadCurr != gamePadLast)
            {
                if (gamePadCurr.IsConnected && !gamePadLast.IsConnected)
                {
                    msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.Connected"));
                    this.GamePadConnected?.Invoke(this, null);
                }
                if (gamePadLast.IsConnected && !gamePadCurr.IsConnected)
                {
                    msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.Disconnected"));
                    this.GamePadDisconnected?.Invoke(this, null);
                }

                bool[,] dpad = new bool[3, 4]
                {
                    { gamePadCurr.DPad.Up == ButtonState.Pressed, gamePadCurr.DPad.Down == ButtonState.Pressed, gamePadCurr.DPad.Left == ButtonState.Pressed, gamePadCurr.DPad.Right == ButtonState.Pressed },
                    { gamePadCurr.ThumbSticks.Left.Y > 0, gamePadCurr.ThumbSticks.Left.Y < 0, gamePadCurr.ThumbSticks.Left.X < 0, gamePadCurr.ThumbSticks.Left.X > 0 },
                    { gamePadCurr.ThumbSticks.Right.Y > 0, gamePadCurr.ThumbSticks.Right.Y < 0, gamePadCurr.ThumbSticks.Right.X < 0, gamePadCurr.ThumbSticks.Right.X > 0 },
                };

                bool[,,] dpad2 = new bool[3, 2, 2]
                {
                    { { dpad[0, 0] && dpad[0, 2], dpad[0, 0] && dpad[0, 3] },  { dpad[0, 1] && dpad[0, 2], dpad[0, 1] && dpad[0, 3] } },
                    { { dpad[1, 0] && dpad[1, 2], dpad[1, 0] && dpad[1, 3] },  { dpad[1, 1] && dpad[1, 2], dpad[1, 1] && dpad[1, 3] } },
                    { { dpad[2, 0] && dpad[2, 2], dpad[2, 0] && dpad[2, 3] },  { dpad[2, 1] && dpad[2, 2], dpad[2, 1] && dpad[2, 3] } },
                };

                string[,] txt = new string[3, 8]
                {
                    {
                        Localization.Localization.GetText("PlayerInput.GamePad.DirectionPadUpLeft"),
                        Localization.Localization.GetText("PlayerInput.GamePad.DirectionPadUpRight"),
                        Localization.Localization.GetText("PlayerInput.GamePad.DirectionPadDownLeft"),
                        Localization.Localization.GetText("PlayerInput.GamePad.DirectionPadDownRight"),
                        Localization.Localization.GetText("PlayerInput.GamePad.DirectionPadUp"),
                        Localization.Localization.GetText("PlayerInput.GamePad.DirectionPadDown"),
                        Localization.Localization.GetText("PlayerInput.GamePad.DirectionPadLeft"),
                        Localization.Localization.GetText("PlayerInput.GamePad.DirectionPadRight"),
                    },
                    {
                        Localization.Localization.GetText("PlayerInput.GamePad.LeftStickUpLeft"),
                        Localization.Localization.GetText("PlayerInput.GamePad.LeftStickUpRight"),
                        Localization.Localization.GetText("PlayerInput.GamePad.LeftStickDownLeft"),
                        Localization.Localization.GetText("PlayerInput.GamePad.LeftStickDownRight"),
                        Localization.Localization.GetText("PlayerInput.GamePad.LeftStickUp"),
                        Localization.Localization.GetText("PlayerInput.GamePad.LeftStickDown"),
                        Localization.Localization.GetText("PlayerInput.GamePad.LeftStickLeft"),
                        Localization.Localization.GetText("PlayerInput.GamePad.LeftStickRight"),
                    },
                    {
                        Localization.Localization.GetText("PlayerInput.GamePad.RightStickUpLeft"),
                        Localization.Localization.GetText("PlayerInput.GamePad.RightStickUpRight"),
                        Localization.Localization.GetText("PlayerInput.GamePad.RightStickDownLeft"),
                        Localization.Localization.GetText("PlayerInput.GamePad.RightStickDownRight"),
                        Localization.Localization.GetText("PlayerInput.GamePad.RightStickUp"),
                        Localization.Localization.GetText("PlayerInput.GamePad.RightStickDown"),
                        Localization.Localization.GetText("PlayerInput.GamePad.RightStickLeft"),
                        Localization.Localization.GetText("PlayerInput.GamePad.RightStickRight"),
                    },
                };

                for (int i = 0; i < 3; i++)
                {
                    if (dpad2[i, 0, 0])
                    {
                        dpad[i, 0] = dpad[i, 2] = false;
                        msg.Add(txt[i, 0]);
                    }
                    if (dpad2[i, 0, 1])
                    {
                        dpad[i, 0] = dpad[i, 3] = false;
                        msg.Add(txt[i, 1]);
                    }
                    if (dpad2[i, 1, 0])
                    {
                        dpad[i, 1] = dpad[i, 2] = false;
                        msg.Add(txt[i, 2]);
                    }
                    if (dpad2[i, 1, 1])
                    {
                        dpad[i, 1] = dpad[i, 3] = false;
                        msg.Add(txt[i, 3]);
                    }
                    if (dpad[i, 0]) msg.Add(txt[i, 4]);
                    if (dpad[i, 1]) msg.Add(txt[i, 5]);
                    if (dpad[i, 2]) msg.Add(txt[i, 6]);
                    if (dpad[i, 3]) msg.Add(txt[i, 7]);
                }

                if (gamePadCurr.Buttons.BigButton == ButtonState.Pressed) msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.BigButton"));
                if (gamePadCurr.Buttons.A == ButtonState.Pressed) msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.KeyA"));
                if (gamePadCurr.Buttons.B == ButtonState.Pressed) msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.KeyB"));
                if (gamePadCurr.Buttons.X == ButtonState.Pressed) msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.KeyX"));
                if (gamePadCurr.Buttons.Y == ButtonState.Pressed) msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.KeyY"));
                if (gamePadCurr.Buttons.Back == ButtonState.Pressed) msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.Back"));
                if (gamePadCurr.Buttons.Start == ButtonState.Pressed) msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.Start"));
                if (gamePadCurr.Buttons.LeftShoulder == ButtonState.Pressed) msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.LeftBumper"));
                if (gamePadCurr.Buttons.RightShoulder == ButtonState.Pressed) msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.RightBumper"));
                if (gamePadCurr.Buttons.LeftStick == ButtonState.Pressed) msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.LeftStickPressed"));
                if (gamePadCurr.Buttons.RightStick == ButtonState.Pressed) msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.RightStickPressed"));
                if (gamePadCurr.Triggers.Left == 1f && gamePadLast.Triggers.Left != 1f) msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.LeftTrigger"));
                if (gamePadCurr.Triggers.Right == 1f && gamePadLast.Triggers.Right != 1f) msg.Add(Localization.Localization.GetText("PlayerInput.GamePad.RightTrigger"));
            }

            if (msg.Count > 0)
            {
                this.gamePadMsg = string.Join(" + ", msg.ToArray());
                this.opacity = 3f;
            }

            if (this.opacity > 0.0f) this.opacity -= .05f;
            else
            {
                this.mouseMsg = "";
                this.gamePadMsg = "";
                this.keyboardMsg = "";
            }
            this.lastMouseState = this.currMouseState;
            this.currMouseState = Mouse.GetState();
            this.lastGamePadState = this.currGamePadState;
            this.currGamePadState = GamePad.GetState(this.index);
            this.lastKeyboardState = this.currKeyboardState;
            this.currKeyboardState = Keyboard.GetState(this.index);
        }

        public void DrawPlayerInput(SpriteBatch spriteBatch, Vector2 pos)
        {
            float offset = 1.2f;
            Vector2[] bpos = new Vector2[4]
            {
                new Vector2(offset, offset),
                new Vector2(-offset, offset),
                new Vector2(-offset, -offset),
                new Vector2(offset, -offset),
            };
            Color c1 = Color.Black * .25f * MyMath.Min(this.opacity, 1f);
            if (mouseMsg.Length > 0)
            {
                pos.Y -= 22f * Options.UIScale;
                string msg = Localization.Localization.GetText("PlayerInput.Mouse.Message", mouseMsg);
                for (int i = 0; i < 4; i++)
                    Drawing.DrawColorfulString(spriteBatch, msg, pos + bpos[i], new Vector2(1.01f), 0, 0, this.opacity, c1);
                Drawing.DrawColorfulString(spriteBatch, msg, pos, Vector2.One, Main.timer * 16, 16, this.opacity, Main.rainbow_raw);
            }
            if (gamePadMsg.Length > 0)
            {
                pos.Y -= 22f * Options.UIScale;
                string msg = Localization.Localization.GetText("PlayerInput.GamePad.Message", gamePadMsg);
                for (int i = 0; i < 4; i++)
                    Drawing.DrawColorfulString(spriteBatch, msg, pos + bpos[i], new Vector2(1.01f), 0, 0, this.opacity, c1);
                Drawing.DrawColorfulString(spriteBatch, msg, pos, Vector2.One, Main.timer * 16, 16, this.opacity, Main.rainbow_raw);
            }
        }
        
        public event Action<PlayerInput, MouseEvent> MouseUp;
        public event Action<PlayerInput, MouseEvent> MouseDown;
        public event Action<PlayerInput, MouseEvent> MouseClick;
        public event Action<PlayerInput, MouseEvent> MouseClick2;
        public event Action<PlayerInput, MouseEvent> MouseRightClick;
        public event Action<PlayerInput, MouseEvent> MouseRightClick2;
        public event Action<PlayerInput, MouseEvent> MouseMiddleClick;
        public event Action<PlayerInput, MouseEvent> MouseMiddleClick2;
        public event Action<PlayerInput, MouseEvent> MouseWheel;
        public event Action<PlayerInput, KeyboardEvent> KeyboardUp;
        public event Action<PlayerInput, KeyboardEvent> KeyboardPressing;
        public event Action<PlayerInput, KeyboardEvent> KeyboardDown;
        public event Action<PlayerInput, GamePadEvent> GamePadConnected;
        public event Action<PlayerInput, GamePadEvent> GamePadDisconnected;

        protected virtual void OnMouseUp(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseDown(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseClick(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseClick2(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseRightClick(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseRightClick2(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseMiddleClick(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseMiddleClick2(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseWheel(UIElement e, MouseEvent evt) { }
        protected virtual void OnKeyboardUp(UIElement e, KeyboardEvent evt) { }
        protected virtual void OnKeyboardPressing(UIElement e, KeyboardEvent evt) { }
        protected virtual void OnKeyboardDown(UIElement e, KeyboardEvent evt) { }
    }
}