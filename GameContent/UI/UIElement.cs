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
        public event Action<UIElement, MouseEvent> MouseClick2;
        public event Action<UIElement, MouseEvent> MouseRightClick;
        public event Action<UIElement, MouseEvent> MouseRightClick2;
        public event Action<UIElement, MouseEvent> MouseMiddleClick;
        public event Action<UIElement, MouseEvent> MouseMiddleClick2;
        public event Action<UIElement, MouseEvent> MouseWheel;
        public event Action<UIElement, MouseEvent> MouseDragBegin;
        public event Action<UIElement, MouseEvent> MouseDragging;
        public event Action<UIElement, MouseEvent> MouseDragEnd;
        public event Action<UIElement, KeyboardEvent> KeyboardUp;
        public event Action<UIElement, KeyboardEvent> KeyboardPressing;
        public event Action<UIElement, KeyboardEvent> KeyboardDown;

        protected virtual void OnMouseUp(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseDown(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseEnter(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseLeave(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseClick(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseClick2(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseRightClick(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseRightClick2(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseMiddleClick(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseMiddleClick2(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseWheel(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseDragBegin(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseDragging(UIElement e, MouseEvent evt) { }
        protected virtual void OnMouseDragEnd(UIElement e, MouseEvent evt) { }
        protected virtual void OnKeyboardUp(UIElement e, KeyboardEvent evt) { }
        protected virtual void OnKeyboardPressing(UIElement e, KeyboardEvent evt) { }
        protected virtual void OnKeyboardDown(UIElement e, KeyboardEvent evt) { }

        /// <summary> 刷新入口 </summary>
        /// <param name="gameTime"> 计时器 </param>
        internal virtual void Update(GameTime gameTime)
        {
            if (Main.CurrInput.currMouseState != Main.CurrInput.lastMouseState)
            {
                Vector2 currMousePos = new Vector2(Main.CurrInput.currMouseState.X, Main.CurrInput.currMouseState.Y);
                Vector2 lastMousePos = new Vector2(Main.CurrInput.lastMouseState.X, Main.CurrInput.lastMouseState.Y);
                int wheelOffset = Main.CurrInput.currMouseState.ScrollWheelValue - Main.CurrInput.currMouseState.ScrollWheelValue;

                bool[] currButtons = new bool[3]
                {
                    Main.CurrInput.currMouseState.LeftButton == ButtonState.Pressed,
                    Main.CurrInput.currMouseState.MiddleButton == ButtonState.Pressed,
                    Main.CurrInput.currMouseState.RightButton == ButtonState.Pressed,
                };
                bool[] lastButtons = new bool[3]
                {
                    Main.CurrInput.lastMouseState.LeftButton == ButtonState.Pressed,
                    Main.CurrInput.lastMouseState.MiddleButton == ButtonState.Pressed,
                    Main.CurrInput.lastMouseState.RightButton == ButtonState.Pressed,
                };
                bool currButton = currButtons.Contains(true);
                bool lastButton = lastButtons.Contains(true);
                MouseEvent diff = new MouseEvent(currMousePos - lastMousePos, wheelOffset, currButton);

                Rectangle rect = this.Bounds;
                rect.X = (int)(rect.X * Options.UIScale);
                rect.Y = (int)(rect.Y * Options.UIScale);
                rect.Width = (int)(rect.Width * Options.UIScale);
                rect.Height = (int)(rect.Height * Options.UIScale);
                bool containCurr = rect.Contains(currMousePos.ToPoint());
                bool containLast = rect.Contains(lastMousePos.ToPoint());
                if (containCurr && !containLast || !this.IsMouseHovering && containCurr)
                {
                    this.IsMouseHovering = true;
                    this.MouseEnter?.Invoke(this, diff);
                }
                if (containLast && !containCurr || this.IsMouseHovering && !containCurr)
                {
                    this.IsMouseHovering = false;
                    this.MouseLeave?.Invoke(this, diff);
                }
                if (this.IsMouseHovering)
                {
                    if (currButton && !lastButton) this.MouseDown?.Invoke(this, diff);
                    if (lastButton && !currButton) this.MouseUp?.Invoke(this, diff);
                    if (lastButtons[0] && !currButtons[0]) this.MouseClick?.Invoke(this, diff);
                    if (lastButtons[1] && !currButtons[1]) this.MouseMiddleClick?.Invoke(this, diff);
                    if (lastButtons[2] && !currButtons[2]) this.MouseRightClick?.Invoke(this, diff);
                    if (wheelOffset != 0) this.MouseWheel(this, diff);
                }
            }
            if (this.Active) this.UpdateUI(gameTime);
            if (this.ChildrenActive) foreach (UIElement child in this.children) child.Update(gameTime);
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

        public UIElement()
        {
            this.location = Vector2.Zero;
            this.size = Vector2.One;
            this.Active = true;
            this.Visible = true;
            this.ChildrenActive = true;
            this.ChildrenVisible = true;
            this.IgnoreLayer = false;
            this.parent = null;
            this.children = new List<UIElement>();
            this.Initialize();
        }

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
            this.MouseClick2 += this.OnMouseClick2;
            this.MouseRightClick += this.OnMouseRightClick;
            this.MouseRightClick2 += this.OnMouseRightClick2;
            this.MouseMiddleClick += this.OnMouseMiddleClick;
            this.MouseMiddleClick2 += this.OnMouseMiddleClick2;
            this.MouseWheel += this.OnMouseWheel;
            this.MouseDragBegin += this.OnMouseDragBegin;
            this.MouseDragging += this.OnMouseDragging;
            this.MouseDragEnd += this.OnMouseDragEnd;
            this.KeyboardUp += this.OnKeyboardUp;
            this.KeyboardPressing += this.OnKeyboardPressing;
            this.KeyboardDown += this.OnKeyboardDown;
        }

        /// <summary> 将当前UI添加到其他UI的子列表中 </summary>
        /// <param name="e"> 亲UI </param>
        public void AddTo(UIElement e)
        {
            if (e == null)
            {
                string msg = "传入的UIElement为空对象";
                Main.logger.WrittenByErrorSender(msg);
                throw new NullReferenceException(msg);
            }
            this.parent?.children.Remove(this);
            this.parent = e;
            e.children.Add(this);
        }

        /// <summary> 为当前UI的子列表添加UI </summary>
        /// <param name="e"> 子UI </param>
        public void AddFrom(UIElement e)
        {
            if (e == null)
            {
                string msg = "传入的UIElement为空对象";
                Main.logger.WrittenByErrorSender(msg);
                throw new NullReferenceException(msg);
            }
            e.parent?.children.Remove(e);
            e.parent = this;
            this.children.Add(e);
        }

        public void Move(Vector2 offset)
        {
            if (offset == Vector2.Zero) return;
            this.location += offset;
            foreach (UIElement child in this.children)
                child.Move(offset);
        }

        public void MoveTo(Vector2 location) => this.Move(location - this.location);
    }
}
