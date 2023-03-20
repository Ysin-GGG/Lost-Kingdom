using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.Utils;

namespace LostKingdom.GameContent.UI
{
    /// <summary> UI组件 </summary>
    public class UIElement
    {
        /// <summary> UI相对窗口坐标 </summary>
        public Vector2 location;
        /// <summary> UI相对亲UI坐标 </summary>
        public Vector2 LocateToParent
        {
            get => this.location - (this.parent == null ? Vector2.Zero : this.parent.location);
            set => this.location = (this.parent == null ? Vector2.Zero : this.parent.location) + value;
        }
        /// <summary> UI尺寸 </summary>
        public Vector2 size;
        /// <summary> 边框宽度（“xyzw”分别对应“左上右下”） </summary>
        public Vector4 borderSize;
        /// <summary> 亲UI </summary>
        public UIElement parent;
        /// <summary> 子UI </summary>
        public List<UIElement> children;
        /// <summary> UI编号（不强求设定） </summary>
        public int id;
        /// <summary> 是否持续刷新 </summary>
        public bool Active { get; set; }
        /// <summary> 子UI是否持续刷新 </summary>
        public bool ChildrenActive { get; set; }
        /// <summary> 是否可见 </summary>
        public bool Visible { get; set; }
        /// <summary> 子UI是否可见 </summary>
        public bool ChildrenVisible { get; set; }
        /// <summary> 是否在拖动中 </summary>
        public bool Dragging { get; private set; }
        /// <summary> 不会成为SelectedUI的目标 </summary>
        public bool IgnoreMe { get; set; }
        /// <summary> 成为SelectedUI的目标后，阻断其他UI的事件 </summary>
        public bool FocusMe { get; set; }
        /// <summary> 当前UI是否被选中 </summary>
        public bool IsSelected { get; set; }
        /// <summary> 亲UI是否被选中 </summary>
        public bool IsParentSelected { get; private set; }
        /// <summary> 子UI是否被选中 </summary>
        public bool IsChildrenSelected { get; private set; }
        /// <summary> 鼠标是否在该UI上 </summary>
        public bool IsMouseHovering { get; private set; }
        /// <summary> 检测事件时是否忽略上方UI </summary>
        public bool IgnoreLayer { get; set; }
        /// <summary> 是否可越出窗口边界 </summary>
        public bool IgnoreBorder { get; set; }
        /// <summary> UI相对亲UI坐标 </summary>
        public Rectangle Bounds
        {
            get => new Rectangle((int)this.location.X, (int)this.location.Y, (int)this.size.X, (int)this.size.Y);
            set
            {
                this.location.X = value.X;
                this.location.Y = value.Y;
                this.size.X = value.Width;
                this.size.Y = value.Height;
            }
        }

        public Vector2 TopLeft => Utils.Utils.GetPoint(this.Bounds, 0, 0);
        public Vector2 Top => Utils.Utils.GetPoint(this.Bounds, 1, 0);
        public Vector2 TopRight => Utils.Utils.GetPoint(this.Bounds, 2, 0);
        public Vector2 Left => Utils.Utils.GetPoint(this.Bounds, 0, 1);
        public Vector2 Center => Utils.Utils.GetPoint(this.Bounds, 1, 1);
        public Vector2 Right => Utils.Utils.GetPoint(this.Bounds, 2, 1);
        public Vector2 BottomLeft => Utils.Utils.GetPoint(this.Bounds, 0, 2);
        public Vector2 Bottom => Utils.Utils.GetPoint(this.Bounds, 1, 2);
        public Vector2 BottomRight => Utils.Utils.GetPoint(this.Bounds, 2, 2);
        
        public event Action<UIElement, MouseEvent> MouseUp;
        public event Action<UIElement, MouseEvent> MouseDown;
        public event Action<UIElement, MouseEvent> MouseEnter;
        public event Action<UIElement, MouseEvent> MouseLeave;
        public event Action<UIElement, MouseEvent> MouseClick;
        public event Action<UIElement, MouseEvent> MouseDoubleClick;
        public event Action<UIElement, MouseEvent> MouseWheel;
        public event Action<UIElement, MouseEvent> MouseDragBegin;
        public event Action<UIElement, MouseEvent> MouseDragging;
        public event Action<UIElement, MouseEvent> MouseDragEnd;
        public event Action<UIElement, KeyboardEvent> KeyboardUp;
        public event Action<UIElement, KeyboardEvent> KeyboardPressing;
        public event Action<UIElement, KeyboardEvent> KeyboardDown;
        public event Action<UIElement, GamePadEvent> GamePadDown;
        public event Action<UIElement, GamePadEvent> GamePadStick;
        public event Action<UIElement, GamePadEvent> GamePadUp;

        protected virtual void OnMouseUp(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseDown(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseEnter(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseLeave(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseClick(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseDoubleClick(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseWheel(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseDragBegin(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseDragging(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseDragEnd(UIElement e, MouseEvent evt) { }
        protected virtual void OnKeyboardUp(UIElement e, KeyboardEvent evt) { }
        protected virtual void OnKeyboardPressing(UIElement e, KeyboardEvent evt) { }
        protected virtual void OnKeyboardDown(UIElement e, KeyboardEvent evt) { }

        /// <summary> 除了SelectedUI以及IgnoreMe的UI之外，部分事件的检测受到限制 </summary>
        public static UIElement SelectedUI;
        private static long tick = 0;

        /// <summary> 刷新入口 </summary>
        /// <param name="gameTime"> 计时器 </param>
        internal virtual void Update(GameTime gameTime)
        {
            Rectangle rect = this.Bounds;
            rect.X = (int)(rect.X * Options.UIScale);
            rect.Y = (int)(rect.Y * Options.UIScale);
            rect.Width = (int)(rect.Width * Options.UIScale);
            rect.Height = (int)(rect.Height * Options.UIScale);

            if (Main.timer != UIElement.tick) UIElement.SelectedUI = null;
            UIElement.tick = Main.timer;
            if (this.ChildrenActive)
                for(int i = this.children.Count; i > 0;) //倒序刷新（因为UI图层高低与顺序表相反，即越后越高）
                    this.children[--i].Update(gameTime);

            if (UIElement.SelectedUI == null && this.Active && !this.IgnoreMe && rect.Contains(Main.MouseScreen.ToPoint())) UIElement.SelectedUI = this;
            bool doEvent = this.IgnoreLayer || UIElement.SelectedUI == this;

            if (UIElement.SelectedUI == null || !UIElement.SelectedUI.FocusMe || UIElement.SelectedUI == this)
            {
                MouseState mouseCurr = Main.CurrInput.currMouseState;
                MouseState mouseLast = Main.CurrInput.lastMouseState;
                if (mouseCurr != mouseLast)
                {
                    Vector2 currMousePos = new Vector2(mouseCurr.X, mouseCurr.Y);
                    Vector2 lastMousePos = new Vector2(mouseLast.X, mouseLast.Y);
                    int wheelOffset = mouseCurr.ScrollWheelValue - mouseLast.ScrollWheelValue;

                    bool[] down = new bool[3]
                    {
                    mouseCurr.LeftButton == ButtonState.Pressed && mouseLast.LeftButton == ButtonState.Released,
                    mouseCurr.MiddleButton == ButtonState.Pressed && mouseLast.LeftButton == ButtonState.Released,
                    mouseCurr.RightButton == ButtonState.Pressed && mouseLast.LeftButton == ButtonState.Released,
                    };
                    bool[] up = new bool[3]
                    {
                    Main.CurrInput.lastMouseState.LeftButton == ButtonState.Pressed && mouseCurr.LeftButton == ButtonState.Released,
                    Main.CurrInput.lastMouseState.MiddleButton == ButtonState.Pressed && mouseCurr.LeftButton == ButtonState.Released,
                    Main.CurrInput.lastMouseState.RightButton == ButtonState.Pressed && mouseCurr.LeftButton == ButtonState.Released,
                    };
                    Vector2 mouseOffset = currMousePos - lastMousePos;
                    MouseEvent evt = new MouseEvent(mouseOffset, wheelOffset, false);

                    bool containCurr = rect.Contains(currMousePos.ToPoint());
                    bool containLast = rect.Contains(lastMousePos.ToPoint());
                    if (containCurr && !containLast || !this.IsMouseHovering && containCurr)
                    {
                        this.IsMouseHovering = true;
                        this.MouseEnter?.Invoke(this, evt);
                    }
                    if (containLast && !containCurr || this.IsMouseHovering && !containCurr)
                    {
                        this.IsMouseHovering = false;
                        this.MouseLeave?.Invoke(this, evt);
                    }
                    bool _down = down.Contains(true);
                    bool _up = up.Contains(true);
                    if (this.IsMouseHovering)
                    {
                        if (doEvent)
                        {
                            if (_down) this.MouseDown?.Invoke(this, new MouseEvent(mouseOffset, wheelOffset, true) { leftButton = down[0], middleButton = down[1], rightButton = down[2] });
                            if (_up) this.MouseUp?.Invoke(this, new MouseEvent(mouseOffset, wheelOffset, true) { leftButton = up[0], middleButton = up[1], rightButton = up[2] });
                            if (_up && !_down) this.MouseClick?.Invoke(this, new MouseEvent(mouseOffset, wheelOffset, true) { leftButton = up[0] && !down[0], middleButton = up[1] && !down[1], rightButton = up[2] && !down[2] });
                            if (wheelOffset != 0) this.MouseWheel(this, evt);
                        }
                    }
                }

                KeyboardState keyboardCurr = Main.CurrInput.currKeyboardState;
                KeyboardState keyboardLast = Main.CurrInput.lastKeyboardState;
                if (keyboardCurr != keyboardLast)
                {
                    KeyboardEvent keyboardEvt = new KeyboardEvent();
                    bool _down = false;
                    bool _up = false;
                    foreach (Keys key in Enum.GetValues(typeof(Keys)))
                    {
                        if (doEvent)
                        {
                            if (Main.CurrInput.GetInputState(key) == PlayerInput.InputState.Down) { keyboardEvt.down.Add(key); _down = true; }
                            if (Main.CurrInput.GetInputState(key) == PlayerInput.InputState.Up) { keyboardEvt.up.Add(key); _up = true; }
                        }
                    }

                    if (doEvent)
                    {
                        if (_down) this.KeyboardDown?.Invoke(this, keyboardEvt);
                        if (_up) this.KeyboardUp?.Invoke(this, keyboardEvt);
                    }
                }
            }
            else this.IsMouseHovering = false;
            if (this.Active) this.UpdateUI(gameTime);
        }

        /// <summary> 绘制入口 </summary>
        /// <param name="spriteBatch"> 绘制精灵 </param>
        internal void Draw(SpriteBatch spriteBatch)
        {
            if (this.Visible) this.DrawUI(spriteBatch);
            if (this.ChildrenVisible) foreach (UIElement child in this.children) child.Draw(spriteBatch);
        }

        /// <summary> 刷新UI </summary>
        /// <param name="gameTime"></param>
        protected virtual void UpdateUI(GameTime gameTime) { }

        /// <summary> 绘制UI </summary>
        /// <param name="spriteBatch"> 绘制精灵 </param>
        protected virtual void DrawUI(SpriteBatch spriteBatch) { }

        /// <summary> UI基本组件 </summary>
        public UIElement()
        {
            this.location = Vector2.Zero;
            this.size = Vector2.One;
            this.Active = true;
            this.Visible = true;
            this.ChildrenActive = true;
            this.ChildrenVisible = true;
            this.IgnoreLayer = false;
            this.IgnoreMe = false;
            this.parent = null;
            this.children = new List<UIElement>();
            this.Initialize();
        }

        /// <summary> UI基本组件 </summary>
        /// <param name="location"> 坐标 </param>
        /// <param name="size"> 尺寸 </param>
        public UIElement(Vector2 location, Vector2 size)
        {
            this.location = location;
            this.size = size;
            this.Active = true;
            this.Visible = true;
            this.ChildrenActive = true;
            this.ChildrenVisible = true;
            this.IgnoreLayer = false;
            this.parent = null;
            this.children = new List<UIElement>();
            this.Initialize();
        }

        private void Initialize()
        {
            this.MouseUp += this.OnMouseUp;
            this.MouseDown += this.OnMouseDown;
            this.MouseEnter += this.OnMouseEnter;
            this.MouseLeave += this.OnMouseLeave;
            this.MouseClick += this.OnMouseClick;
            this.MouseDoubleClick += this.OnMouseDoubleClick;
            this.MouseWheel += this.OnMouseWheel;
            this.MouseDragBegin += this.OnMouseDragBegin;
            this.MouseDragging += this.OnMouseDragging;
            this.MouseDragEnd += this.OnMouseDragEnd;
            this.KeyboardUp += this.OnKeyboardUp;
            this.KeyboardPressing += this.OnKeyboardPressing;
            this.KeyboardDown += this.OnKeyboardDown;
        }

        /// <summary> 将当前UI添加到其他UI的子列表中 </summary>
        /// <param name="parent"> 亲UI </param>
        /// <param name="index"> 插入索引 </param>
        public void AddTo(UIElement parent, int index = -1)
        {
            if (parent == null)
            {
                string msg = "传入的UIElement为空对象";
                Main.logger.WrittenByErrorSender(msg);
                if (Main.IgnoreError) return;
                else throw new NullReferenceException(msg);
            }
            this.parent?.children.Remove(this);
            this.parent = parent;
            if (index < 0 || parent.children.Count >= index) parent.children.Add(this);
            else parent.children.Insert(index, this);
        }

        /// <summary> 为当前UI的子列表添加UI </summary>
        /// <param name="child"> 子UI </param>
        /// <param name="index"> 插入索引 </param>
        public void AddFrom(UIElement child, int index = -1)
        {
            if (child == null)
            {
                string msg = "传入的UIElement为空对象";
                Main.logger.WrittenByErrorSender(msg);
                if (Main.IgnoreError) return;
                else throw new NullReferenceException(msg);
            }
            child.parent?.children.Remove(child);
            child.parent = this;
            if (index < 0 || this.children.Count >= index) this.children.Add(child);
            else this.children.Insert(index, child);
        }

        /// <summary> 整体移动 </summary>
        /// <param name="offset"> 移动长度 </param>
        public void Move(Vector2 offset)
        {
            if (offset == Vector2.Zero) return;
            this.location += offset;
            foreach (UIElement child in this.children)
                child.Move(offset);
        }

        /// <summary> 整体移动至 </summary>
        /// <param name="location"> 新坐标 </param>
        public void MoveTo(Vector2 location) => this.Move(location - this.location);
    }
}
