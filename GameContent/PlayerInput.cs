using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.GameContent.Localization;
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
        public int[] mousePressingTime;
        public int[] gamepadPressingTime;
        public int[] keyboardPressingTime;

        public bool[] mouseState;
        public bool[] gamepadState;
        public bool[] keyboardState;
        public float opacity;

        public string mouseMsg;
        public string gamePadMsg;
        public string keyboardMsg;

        /// <summary> 按键状态 </summary>
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

        /// <summary> 鼠标键位 </summary>
        public enum Mouses
        {
            /// <summary> X1键（前进键） </summary>
            XButton1 = 1,
            /// <summary> X2键（前进键） </summary>
            XButton2 = 2,
            /// <summary> 左键 </summary>
            LeftButton = 4,
            /// <summary> 中键 </summary>
            MiddleButton = 8,
            /// <summary> 右键 </summary>
            RightButton = 16,
        }
        
        /// <summary> 新建设备输入读取器 </summary>
        /// <param name="who"> 玩家编号 </param>
        /// <param name="doubleClickDelay"> 双击判定间隔 </param>
        public PlayerInput(PlayerIndex who = PlayerIndex.One, int doubleClickDelay = -1)
        {
            this.index = who;
            this.doubleClickDelay = doubleClickDelay < 0 ? Options.FramePerSecond / 2 : doubleClickDelay;
            this.currMouseState = this.lastMouseState = Mouse.GetState();
            this.currGamePadState = this.lastGamePadState = GamePad.GetState(who);
            this.currKeyboardState = this.lastKeyboardState = Keyboard.GetState(who);
            this._doubleClickDelay = new int[3];
            this.mousePressingTime = new int[Enum.GetValues(typeof(Mouses)).Length];
            this.gamepadPressingTime = new int[Enum.GetValues(typeof(Buttons)).Length];
            this.keyboardPressingTime = new int[Enum.GetValues(typeof(Keys)).Length];
            this.opacity = 0f;
            this.mouseMsg = "";
            this.gamePadMsg = "";
            this.keyboardMsg = "";
            this.focusUI = null;
        }

        /// <summary> 查看键盘是否形成特定组合输入 </summary>
        /// <param name="state"> 键状态 </param>
        /// <param name="keys"> 要组合的所有键 </param>
        public bool GetInputCombination(KeyState state, params Keys[] keys)
        {
            if (state == KeyState.Down)
                foreach (Keys key in keys)
                    if (this.currKeyboardState.IsKeyDown(key)) return false;
            if (state == KeyState.Up)
                foreach (Keys key in keys)
                    if (this.currKeyboardState.IsKeyUp(key)) return false;
            return true;
        }

        /// <summary> 查看手柄是否形成特定组合输入 </summary>
        /// <param name="state"> 按钮状态 </param>
        /// <param name="buttons"> 要组合的所有按钮 </param>
        public bool GetInputCombination(ButtonState state, params Buttons[] buttons)
        {
            if (state == ButtonState.Pressed)
                foreach (Buttons button in buttons)
                    if (this.currGamePadState.IsButtonDown(button)) return false;
            if (state == ButtonState.Released)
                foreach (Buttons button in buttons)
                    if (this.currGamePadState.IsButtonUp(button)) return false;
            return true;
        }

        /// <summary> 获取鼠标某个键的状态 </summary>
        /// <param name="button"> 键位 </param>
        public InputState GetInputState(Mouses button)
        {
            bool curr = this.IsButtonDown(this.currMouseState, button);
            bool last = this.IsButtonDown(this.lastMouseState, button);
            if (curr) return last ? InputState.Pressed : InputState.Down;
            return !last ? InputState.Released : InputState.Up;
        }

        /// <summary> 获取键盘某个键的状态 </summary>
        /// <param name="key"> 键位 </param>
        public InputState GetInputState(Keys key)
        {
            bool curr = this.currKeyboardState.IsKeyDown(key);
            bool last = this.lastKeyboardState.IsKeyDown(key);
            if (curr) return last ? InputState.Pressed : InputState.Down;
            return !last ? InputState.Released : InputState.Up;
        }

        /// <summary> 获取手柄某个按钮的状态 </summary>
        /// <param name="button"> 按钮 </param>
        public InputState GetInputState(Buttons button)
        {
            bool curr = this.currGamePadState.IsButtonDown(button);
            bool last = this.lastGamePadState.IsButtonDown(button);
            if (curr) return last ? InputState.Pressed : InputState.Down;
            return !last ? InputState.Released : InputState.Up;
        }

        private bool IsButtonDown(MouseState mouse, Mouses button)
        {
            switch (button)
            {
                case Mouses.XButton1: return mouse.XButton1 == ButtonState.Pressed;
                case Mouses.XButton2: return mouse.XButton2 == ButtonState.Pressed;
                case Mouses.LeftButton: return mouse.LeftButton == ButtonState.Pressed;
                case Mouses.MiddleButton: return mouse.MiddleButton == ButtonState.Pressed;
                case Mouses.RightButton: return mouse.RightButton == ButtonState.Pressed;
            }
            return false;
        }

        internal void Update(GameTime gameTime)
        {
            List<string> msg = new List<string>();
            MouseState mouseCurr = this.currMouseState;
            MouseState mouseLast = this.lastMouseState;
            Vector2 mouseOffset = new Vector2(mouseCurr.X - mouseLast.X, mouseCurr.Y - mouseLast.Y);
            int wheelOffset = mouseCurr.ScrollWheelValue - mouseLast.ScrollWheelValue;
            MouseEvent mouseEvt = new MouseEvent(mouseOffset, wheelOffset, false);

            bool down = false, up = false;
            bool[] buttonCurr = new bool[3]
            {
                mouseCurr.LeftButton == ButtonState.Pressed,
                mouseCurr.MiddleButton == ButtonState.Pressed,
                mouseCurr.RightButton == ButtonState.Pressed,
            };
            bool[] buttonLast = new bool[3]
            {
                mouseLast.LeftButton == ButtonState.Pressed,
                mouseLast.MiddleButton == ButtonState.Pressed,
                mouseLast.RightButton == ButtonState.Pressed,
            };

            string[][] _msg = new string[3][]
            {
                new string[2] { "PlayerInput.Mouse.LeftClick" , "PlayerInput.Mouse.LeftDoubleClick" },
                new string[2] { "PlayerInput.Mouse.MiddleClick" , "PlayerInput.Mouse.MiddleDoubleClick" },
                new string[2] { "PlayerInput.Mouse.RightClick" , "PlayerInput.Mouse.RightDoubleClick" },
            };

            for (int i = 0; i < 3; i++)
            {
                down |= buttonCurr[i] && !buttonLast[i];
                up |= buttonLast[i] && !buttonCurr[i];
                if (buttonLast[i] && !buttonCurr[i])
                {
                    MouseEvent _evt = new MouseEvent(mouseOffset, wheelOffset, true);
                    switch (i)
                    {
                        case 0:
                            mouseEvt.leftButton = true;
                            _evt.leftButton = true;
                            break;
                        case 1:
                            mouseEvt.middleButton = true;
                            _evt.middleButton = true;
                            break;
                        case 2:
                            mouseEvt.rightButton = true;
                            _evt.rightButton = true;
                            break;
                    }
                    this.MouseClick?.Invoke(this, _evt);
                    if (this._doubleClickDelay[i] > 0)
                    {
                        msg.Add(GameLocalization.GetText(_msg[i][1]));
                        this.MouseDoubleClick?.Invoke(this, _evt);
                        this._doubleClickDelay[i] = 0;
                    }
                    else
                    {
                        msg.Add(GameLocalization.GetText(_msg[i][0]));
                        this._doubleClickDelay[i] = this.doubleClickDelay;
                    }
                }
            }

            if (down) this.MouseDown?.Invoke(this, mouseEvt);
            if (up) this.MouseUp?.Invoke(this, mouseEvt);

            if (wheelOffset != 0)
            {
                msg.Add(GameLocalization.GetText(wheelOffset > 0 ? "PlayerInput.Mouse.WheelUp" : "PlayerInput.Mouse.WheelDown"));
                this.MouseWheel?.Invoke(this, mouseEvt);
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
                    msg.Add(GameLocalization.GetText("PlayerInput.GamePad.Connected"));
                    this.GamePadConnected?.Invoke(this, null);
                }
                if (gamePadLast.IsConnected && !gamePadCurr.IsConnected)
                {
                    msg.Add(GameLocalization.GetText("PlayerInput.GamePad.Disconnected"));
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
                        GameLocalization.GetText("PlayerInput.GamePad.DirectionPadUpLeft"),
                        GameLocalization.GetText("PlayerInput.GamePad.DirectionPadUpRight"),
                        GameLocalization.GetText("PlayerInput.GamePad.DirectionPadDownLeft"),
                        GameLocalization.GetText("PlayerInput.GamePad.DirectionPadDownRight"),
                        GameLocalization.GetText("PlayerInput.GamePad.DirectionPadUp"),
                        GameLocalization.GetText("PlayerInput.GamePad.DirectionPadDown"),
                        GameLocalization.GetText("PlayerInput.GamePad.DirectionPadLeft"),
                        GameLocalization.GetText("PlayerInput.GamePad.DirectionPadRight"),
                    },
                    {
                        GameLocalization.GetText("PlayerInput.GamePad.LeftStickUpLeft"),
                        GameLocalization.GetText("PlayerInput.GamePad.LeftStickUpRight"),
                        GameLocalization.GetText("PlayerInput.GamePad.LeftStickDownLeft"),
                        GameLocalization.GetText("PlayerInput.GamePad.LeftStickDownRight"),
                        GameLocalization.GetText("PlayerInput.GamePad.LeftStickUp"),
                        GameLocalization.GetText("PlayerInput.GamePad.LeftStickDown"),
                        GameLocalization.GetText("PlayerInput.GamePad.LeftStickLeft"),
                        GameLocalization.GetText("PlayerInput.GamePad.LeftStickRight"),
                    },
                    {
                        GameLocalization.GetText("PlayerInput.GamePad.RightStickUpLeft"),
                        GameLocalization.GetText("PlayerInput.GamePad.RightStickUpRight"),
                        GameLocalization.GetText("PlayerInput.GamePad.RightStickDownLeft"),
                        GameLocalization.GetText("PlayerInput.GamePad.RightStickDownRight"),
                        GameLocalization.GetText("PlayerInput.GamePad.RightStickUp"),
                        GameLocalization.GetText("PlayerInput.GamePad.RightStickDown"),
                        GameLocalization.GetText("PlayerInput.GamePad.RightStickLeft"),
                        GameLocalization.GetText("PlayerInput.GamePad.RightStickRight"),
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

                if (gamePadCurr.Buttons.BigButton == ButtonState.Pressed) msg.Add(GameLocalization.GetText("PlayerInput.GamePad.BigButton"));
                if (gamePadCurr.Buttons.A == ButtonState.Pressed) msg.Add(GameLocalization.GetText("PlayerInput.GamePad.KeyA"));
                if (gamePadCurr.Buttons.B == ButtonState.Pressed) msg.Add(GameLocalization.GetText("PlayerInput.GamePad.KeyB"));
                if (gamePadCurr.Buttons.X == ButtonState.Pressed) msg.Add(GameLocalization.GetText("PlayerInput.GamePad.KeyX"));
                if (gamePadCurr.Buttons.Y == ButtonState.Pressed) msg.Add(GameLocalization.GetText("PlayerInput.GamePad.KeyY"));
                if (gamePadCurr.Buttons.Back == ButtonState.Pressed) msg.Add(GameLocalization.GetText("PlayerInput.GamePad.Back"));
                if (gamePadCurr.Buttons.Start == ButtonState.Pressed) msg.Add(GameLocalization.GetText("PlayerInput.GamePad.Start"));
                if (gamePadCurr.Buttons.LeftShoulder == ButtonState.Pressed) msg.Add(GameLocalization.GetText("PlayerInput.GamePad.LeftBumper"));
                if (gamePadCurr.Buttons.RightShoulder == ButtonState.Pressed) msg.Add(GameLocalization.GetText("PlayerInput.GamePad.RightBumper"));
                if (gamePadCurr.Buttons.LeftStick == ButtonState.Pressed) msg.Add(GameLocalization.GetText("PlayerInput.GamePad.LeftStickPressed"));
                if (gamePadCurr.Buttons.RightStick == ButtonState.Pressed) msg.Add(GameLocalization.GetText("PlayerInput.GamePad.RightStickPressed"));
                if (gamePadCurr.Triggers.Left == 1f && gamePadLast.Triggers.Left != 1f) msg.Add(GameLocalization.GetText("PlayerInput.GamePad.LeftTrigger"));
                if (gamePadCurr.Triggers.Right == 1f && gamePadLast.Triggers.Right != 1f) msg.Add(GameLocalization.GetText("PlayerInput.GamePad.RightTrigger"));

                KeyboardState keyboardCurr = currKeyboardState;
                KeyboardState keyboardLast = lastKeyboardState;
                if (keyboardCurr != keyboardLast)
                {
                    KeyboardEvent keyboardEvt = new KeyboardEvent();
                    down = false;
                    up = false;
                    foreach (Keys key in Enum.GetValues(typeof(Keys)))
                    {
                        if (this.GetInputState(key) == InputState.Down) { keyboardEvt.down.Add(key); down = true; }
                        if (this.GetInputState(key) == InputState.Up) { keyboardEvt.up.Add(key); up = true; }
                    }
                    if (down) this.KeyboardDown?.Invoke(this, keyboardEvt);
                    if (up) this.KeyboardUp?.Invoke(this, keyboardEvt);
                }
            }

            if (msg.Count > 0)
            {
                this.gamePadMsg = string.Join(" + ", msg.ToArray());
                this.opacity = 3f;
            }

            if (this.opacity > 0.0f) this.opacity -= .02f;
            else
            {
                this.mouseMsg = "";
                this.gamePadMsg = "";
                this.keyboardMsg = "";
            }
            if (!Main.form.Focused) return;
            this.lastMouseState = this.currMouseState;
            this.currMouseState = Mouse.GetState();
            this.lastGamePadState = this.currGamePadState;
            this.currGamePadState = GamePad.GetState(this.index);
            this.lastKeyboardState = this.currKeyboardState;
            this.currKeyboardState = Keyboard.GetState(this.index);
        }

        internal void DrawPlayerInput(SpriteBatch spriteBatch, Vector2 pos)
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
                pos.Y -= 22f;
                string msg = GameLocalization.GetText("PlayerInput.Mouse.Message", mouseMsg);
                for (int i = 0; i < 4; i++)
                    Drawing.DrawColorfulString(spriteBatch, msg, pos + bpos[i], new Vector2(1.01f * Options.UIScale), 0, 0, this.opacity, c1);
                Drawing.DrawColorfulString(spriteBatch, msg, pos, new Vector2(Options.UIScale), Main.timer * 16, 16, this.opacity, Main.rainbow_raw);
            }
            if (gamePadMsg.Length > 0)
            {
                pos.Y -= 22f;
                string msg = GameLocalization.GetText("PlayerInput.GamePad.Message", gamePadMsg);
                for (int i = 0; i < 4; i++)
                    Drawing.DrawColorfulString(spriteBatch, msg, pos + bpos[i], new Vector2(1.01f * Options.UIScale), 0, 0, this.opacity, c1);
                Drawing.DrawColorfulString(spriteBatch, msg, pos, new Vector2(Options.UIScale), Main.timer * 16, 16, this.opacity, Main.rainbow_raw);
            }
        }
        
        public event Action<PlayerInput, MouseEvent> MouseUp;
        public event Action<PlayerInput, MouseEvent> MouseDown;
        public event Action<PlayerInput, MouseEvent> MouseClick;
        public event Action<PlayerInput, MouseEvent> MouseDoubleClick;
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
        protected virtual void OnMouseWheel(UIElement e, MouseEvent evt) { }
        protected virtual void OnKeyboardUp(UIElement e, KeyboardEvent evt) { }
        protected virtual void OnKeyboardPressing(UIElement e, KeyboardEvent evt) { }
        protected virtual void OnKeyboardDown(UIElement e, KeyboardEvent evt) { }
    }
}