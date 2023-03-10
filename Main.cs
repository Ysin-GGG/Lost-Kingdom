using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using LostKingdom.Utils;
using LostKingdom.GameContent.Localization;
using LostKingdom.GameContent.UI;
using LostKingdom.GameContent;

namespace LostKingdom
{
    /// <summary> 游戏主类 </summary>
    public class Main : Game
    {
        /// <summary> 版本信息 </summary>
        public const string versionString = "beta v0.0.1.0";
        /// <summary> 版本号 </summary>
        public const string versionNumber = "0.0.1.0";

        public static GraphicsDeviceManager MyGraphics { get; private set; }
        public static SpriteBatch MySpriteBatch { get; private set; }
        public static SpriteFont MySpriteFont { get; private set; }
        public static RenderTarget2D MyRenderTarget { get; set; }
        public static RenderTarget2D MyRenderTargetSwap { get; set; }

        /// <summary> UI缩放对应的预设分辨率 </summary>
        public static Vector2 UIBestScreenSize => Main.BestScreenSize / Options.UIScale;
        /// <summary> 预设分辨率 </summary>
        public static Vector2 BestScreenSize => new Vector2(BestScreenWidth, BestScreenHeight);
        /// <summary> 预设分辨率宽度 </summary>
        public static int BestScreenWidth => 1920;
        /// <summary> 预设分辨率高度 </summary>
        public static int BestScreenHeight => 1080;
        /// <summary> 窗口框 </summary>
        public static Rectangle Bounds => new Rectangle(Main.instance.Window.ClientBounds.X, Main.instance.Window.ClientBounds.Y, Main.screenWidth, Main.screenHeight);
        /// <summary> UI缩放对应的当前分辨率 </summary>
        public static Vector2 UIScreenSize => Main.ScreenSize / Options.UIScale;
        /// <summary> 当前分辨率 </summary>
        public static Vector2 ScreenSize
        {
            get
            {
                Vector2 size = Vector2.Zero;
                size.X = Main.screenWidth = Main.instance.GraphicsDevice.PresentationParameters.BackBufferWidth;
                size.Y = Main.screenHeight = Main.instance.GraphicsDevice.PresentationParameters.BackBufferHeight;
                return size;
            }
            set
            {
                int width = (int)value.X;
                int height = (int)value.Y;
                bool full = false;
                if (width >= Main.form.MaximumSize.Width || height >= Main.form.MaximumSize.Height)
                {
                    width = Main.form.MaximumSize.Width;
                    height = Main.form.MaximumSize.Height;
                    full = true;
                }
                Main.instance.SetScreen(width, height, full);
            }
        }
        /// <summary> 鼠标在屏幕上的坐标 </summary>
        public static Vector2 MouseScreen
        {
            get => new Vector2(Main.CurrInput.currMouseState.X, Main.CurrInput.currMouseState.Y);
            protected set => Mouse.SetPosition((int)value.X, (int)value.Y);
        }

        public static Color[] rainbow_raw = null;
        public static Color rainbow => rainbow_raw[timer * 16 % rainbow_raw.Length];
        public static Color bgColor = Color.Black;

        /// <summary> 游戏日志 </summary>
        public static Logger logger = null;
        /// <summary> 视图矩阵 </summary>
        public static Matrix gameMatrix;
        /// <summary> UI矩阵 </summary>
        public static Matrix uiScaleMatrix;
        /// <summary> Main实例 </summary>
        public static Main instance = null;
        /// <summary> Main实例 </summary>
        public static Form form = null;
        /// <summary> 可选的分辨率 </summary>
        public static Vector2[] SupportedScreenResolution { get; private set; }
        /// <summary> 当前房间的敌人 </summary>
        public static readonly Enemy[] enemy = new Enemy[6];
        /// <summary> 当前房间的玩家 </summary>
        public static readonly Player[] player = new Player[4];
        /// <summary> 玩家设备输入信息 </summary>
        public static readonly PlayerInput[] playerInput = new PlayerInput[4];
        /// <summary> UI绘制图层 </summary>
        public static readonly List<UILayer> UIs = new List<UILayer>();
        /// <summary> 当前玩家设备输入信息 </summary>
        public static PlayerInput CurrInput => Main.playerInput[0];

        /// <summary> 画面全黑时 </summary>
        public static event Action ClientSizeChanged;
        /// <summary> 屏幕全黑时（UI画面切换） </summary>
        public static event Action ScreenOff;

        /// <summary> 公用计时器（屏幕全黑时清零） </summary>
        public static uint timer = 0;
        /// <summary> 当前窗口宽度（不包括边框） </summary>
        public static int screenWidth;
        /// <summary> 当前窗口高度（不包括边框） </summary>
        public static int screenHeight;
        /// <summary> 息屏时长（为0时全黑） </summary>
        public static short screenOffTime;
        /// <summary> 界面透明度（为0时全黑） </summary>
        public static float screenOpacity;

        /// <summary> 帧率 </summary>
        public static byte FPS = 0;
        /// <summary> 占用的运存 </summary>
        public static long Memory = 0;
        /// <summary> 逻辑刷新（锁60帧） </summary>
        public static bool GameUpdate = false;
        /// <summary> 当前是否全屏 </summary>
        public bool isFullScreen = false;
        /// <summary> 当前是否窗口最大化 </summary>
        public bool isScreenMaximized = false;
        
        private static float screenOpacitySpeed;
        private static long oldTicks = 0;
        private Stopwatch fpsTimer = new Stopwatch();
        private int fpsCounter = 0;


        public Main()
        {
            Localization.Initialize();
            Main.instance = this;
            Main.MyGraphics = new GraphicsDeviceManager(this);
            Main.logger = new Logger();
            Main.logger.Write(Localization.GetText("Logger.Message.Loading"), Localization.GetText("Logger.Sender.GameName"));
            this.Content.RootDirectory = "Content";
            this.Window.AllowUserResizing = true;
            this.Window.Title = Localization.GetText("Game.GameName");
            Main.form = (Form)Control.FromHandle(this.Window.Handle);
            Main.form.FormBorderStyle = FormBorderStyle.FixedSingle;
            Main.form.MaximizeBox = false;
        }

        protected override void Initialize()
        {
            base.Initialize();
            Main.playerInput[0] = new PlayerInput(PlayerIndex.One, 20);
            Main.MySpriteBatch = new SpriteBatch(this.GraphicsDevice);
            Main.screenOffTime = -30;
            Main.screenOpacity = 0.0f;
            Main.screenOpacitySpeed = 0.01f;
            Main.InitRainbow();
            this.InitScreen();
            this.InitTargets();
            this.InitUserInterface();
            this.fpsTimer.Start();
            Options.FramePerSecond = 120;
            Main.gameMatrix = Matrix.Identity;
            Main.uiScaleMatrix = Matrix.CreateScale(Options.UIScale, Options.UIScale, 0.0f);
            this.Window.ClientSizeChanged += new EventHandler<EventArgs>(this.OnSizeChanged);
        }

        private void OnSizeChanged(object sender, EventArgs args)
        {
            Main.screenWidth = Main.instance.GraphicsDevice.PresentationParameters.BackBufferWidth;
            Main.screenHeight = Main.instance.GraphicsDevice.PresentationParameters.BackBufferHeight;
            Main.ClientSizeChanged?.Invoke();
            this.InitTargets();
        }
        
        private void InitScreen()
        {
            List<Vector2> resolution = new List<Vector2>();
            foreach (DisplayMode mode in GraphicsAdapter.DefaultAdapter.SupportedDisplayModes)
            {
                Vector2 size = new Vector2(mode.Width, mode.Height);
                if (size.X / size.Y >= 1.5f) resolution.Add(size);
            }
            Main.SupportedScreenResolution = resolution.ToArray();
            Point maxSize = Main.SupportedScreenResolution.Last().ToPoint();
            Main.form.MaximumSize = new System.Drawing.Size(maxSize.X, maxSize.Y);
            Main.ScreenSize = Main.SupportedScreenResolution[Main.SupportedScreenResolution.Length / 2];
        }

        protected void ClearTargets()
        {
            if (Main.MyRenderTarget != null)
            {
                Main.MyRenderTarget.Dispose();
                Main.MyRenderTarget = null;
            }
            if (Main.MyRenderTargetSwap != null)
            {
                Main.MyRenderTargetSwap.Dispose();
                Main.MyRenderTargetSwap = null;
            }
        }

        protected void InitTargets()
        {
            bool mipMap = false;
            int width = MyMath.Min(this.GraphicsDevice.PresentationParameters.BackBufferWidth, 2048);
            int height = MyMath.Min(this.GraphicsDevice.PresentationParameters.BackBufferHeight, 2048);
            SurfaceFormat surface = this.GraphicsDevice.PresentationParameters.BackBufferFormat;
            DepthFormat depth = DepthFormat.None;

            this.ClearTargets();
            if (Main.MyRenderTarget == null) Main.MyRenderTarget = new RenderTarget2D(this.GraphicsDevice, width, height, mipMap, surface, depth, 0, RenderTargetUsage.PreserveContents);
            if (Main.MyRenderTargetSwap == null) Main.MyRenderTargetSwap = new RenderTarget2D(this.GraphicsDevice, width, height, mipMap, surface, depth, 0, RenderTargetUsage.PreserveContents);
        }

        protected void InitUserInterface()
        {
            foreach (UILayer.LayerName layer in Enum.GetValues(typeof(UILayer.LayerName)))
                new UILayer(layer.ToString(), (int)layer);
        }
        
        protected override void LoadContent()
        {
            List<Texture2D> list = new List<Texture2D>();
            Asset.LoadFonts();
            Asset.LoadImages();
            Asset.LoadUIs();
            Asset.LoadCursor();
            Asset.LoadSounds();
            Asset.LoadSplashs();
            MySpriteFont = Asset.Font["Text"];
        }
        
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        private void SetScreen(int width, int height, bool fullScreen)
        {
            Form form = Control.FromHandle(this.Window.Handle) as Form;
            bool changeFullScreen = this.isFullScreen != fullScreen;
            if (Main.screenWidth != width || Main.screenHeight != height || changeFullScreen)
            {
                Main.instance.InitTargets();
                Main.MyGraphics.PreferredBackBufferWidth = Main.screenWidth = width;
                Main.MyGraphics.PreferredBackBufferHeight = Main.screenHeight = height;
                Main.MyGraphics.ApplyChanges();
            }
            if (changeFullScreen)
            {
                if (this.isFullScreen)
                {
                    Main.MyGraphics.ToggleFullScreen();
                    this.isFullScreen = false;
                    form.FormBorderStyle = FormBorderStyle.FixedSingle;
                    form.WindowState = isScreenMaximized ? FormWindowState.Maximized : FormWindowState.Normal;
                }
                else
                {
                    this.isFullScreen = true;
                    form.FormBorderStyle = FormBorderStyle.None;
                    if (form.WindowState == FormWindowState.Maximized)
                    {
                        form.WindowState = FormWindowState.Normal;
                        isScreenMaximized = true;
                    }
                    Main.MyGraphics.ToggleFullScreen();
                }
            }
        }

        protected override void Update(GameTime gameTime)
        {
            foreach (PlayerInput input in Main.playerInput) input?.Update(gameTime);
            
            Main.GameUpdate = false;
            if (gameTime.TotalGameTime.Ticks - oldTicks >= 166667)
            {
                if (Main.screenOpacitySpeed != 0.0f)
                {
                    if (Main.screenOffTime < 0) Main.screenOffTime++;
                    else Main.screenOpacity += Main.screenOpacitySpeed;
                    if (Main.screenOffTime >= 0 && (Main.screenOpacity <= 0.0f || Main.screenOpacity >= 1.0f))
                    {
                        Main.screenOpacity = MyMath.Clamp(Main.screenOpacity, 0.0f, 1.0f);
                        if (Main.screenOpacity != 0.0f) Main.screenOpacitySpeed = 0.0f;
                        else
                        {
                            Main.screenOpacitySpeed *= -1.0f;
                            Main.screenOffTime *= -1;
                            Main.ScreenOff?.Invoke();
                        }
                    }
                }
                foreach (Player p in Main.player) if (p != null && p.active) p.UpdateEffect();

                Main.timer++;
                oldTicks += 166667;
                Main.GameUpdate = true;
            }

            foreach (UILayer ui in Main.UIs)
            {
                ui.UpdateUI(gameTime);
                if (ui.active) ui.origin?.Update(gameTime);
            }
        }

        protected override void Draw(GameTime gameTime)
        {
            if (fpsTimer.ElapsedMilliseconds >= 1000)
            {
                Main.Memory = Process.GetCurrentProcess().WorkingSet64;
                Main.FPS = (byte)fpsCounter;
                this.fpsCounter = 0;
                fpsTimer.Restart();
            }
            else this.fpsCounter++;

            Main.uiScaleMatrix = Matrix.CreateScale(Options.UIScale, Options.UIScale, 0.0f);
            foreach (UILayer ui in Main.UIs)
            {
                if (!ui.visible) continue;
                Main.MySpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Matrix.Identity);
                ui.Draw(Main.MySpriteBatch);
                Main.MySpriteBatch.End();
                Main.MySpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default, RasterizerState.CullNone, null, Main.uiScaleMatrix);
                ui.origin?.Draw(Main.MySpriteBatch);
                Main.MySpriteBatch.End();
            }
        }

        public static void InitRainbow()
        {
            List<Color> color = new List<Color>();
            for (int i = 0; i < 256; i++) color.Add(new Color(0xff, i, 0x00));
            for (int i = 0; i < 256; i++) color.Add(new Color(0xff - i, 0xff, 0x00));
            for (int i = 0; i < 256; i++) color.Add(new Color(0x00, 0xff, i));
            for (int i = 0; i < 256; i++) color.Add(new Color(0x00, 0xff - i, 0xff));
            for (int i = 0; i < 256; i++) color.Add(new Color(i, 0x00, 0xff));
            for (int i = 0; i < 256; i++) color.Add(new Color(0xff, 0x00, 0xff - i));
            rainbow_raw = color.ToArray();
        }
    }
}
