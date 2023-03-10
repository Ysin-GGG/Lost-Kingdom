using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.Utils;

namespace LostKingdom.GameContent.UI
{
    /// <summary> UI图层 </summary>
    public class UILayer
    {
        /// <summary> 图层名 </summary>
        public string name;
        /// <summary> 图层深度（绘制顺序：深→浅） </summary>
        public float depth;
        /// <summary> 是否保持UI组件刷新 </summary>
        public bool active;
        /// <summary> 是否绘制该图层 </summary>
        public bool visible;
        /// <summary> UI组件搭载器 </summary>
        public readonly UIElement origin;

        public enum LayerName
        {
            Background = 1,
            MainElements = 2,
            MainElements2 = 3,
            MainElements3 = 5,
            Settings = 4,
            Splash = 29,
            PlayerInput = 30,
            DebugMsg = 31,
            Cursor = 32,
        }

        /// <summary> 插入UI图层 </summary>
        /// <param name="name"></param>
        /// <param name="depth"></param>
        public UILayer(string name, float depth)
        {
            this.name = name;
            this.depth = depth;
            this.active = true;
            this.visible = true;
            this.origin = new UIElement();
            Main.UIs.Add(this);
            Main.UIs.Sort(new Comparison<UILayer>((a, b) => Math.Sign(a.depth - b.depth)));
            this.Initialize();
        }

        public virtual void Initialize()
        {
            Vector2 location = Vector2.Zero;
            switch ((UILayer.LayerName)this.depth)
            {
                case UILayer.LayerName.Background:
                    break;
                case UILayer.LayerName.MainElements:
                    this.origin.location = new Vector2(150.0f, Main.BestScreenHeight - 400.0f);
                    string[] content =
                    {
                        Localization.Localization.GetText("Game.ButtonText.Play"),
                        Localization.Localization.GetText("Game.ButtonText.Library"),
                        Localization.Localization.GetText("Game.ButtonText.About"),
                        Localization.Localization.GetText("Game.ButtonText.Exit"),
                    };
                    UIImageButton button1;
                    foreach (string s in content)
                    {
                        button1 = new UIImageButton(Asset.UI["Button1"]);
                        button1.AddTo(this.origin);
                        button1.LocateToParent = location;
                        if(s == content[3]) button1.MouseClick += new Action<UIElement, MouseEvent>((e, evt) => Main.instance.Exit());
                        UIText text = new UIText(button1.size, s, Vector2.One * .9f, null, UIText.TextAlign.Center);
                        text.AddTo(button1);
                        text.LocateToParent = Vector2.Zero;
                        location.Y += button1.size.Y + 10.0f;
                    }
                    button1 = new UIImageButton(Asset.UI["Button3"]);
                    button1.AddTo(this.origin);
                    button1.LocateToParent = location;
                    button1.MouseClick += new Action<UIElement, MouseEvent>((e, evt) => Main.UIs[(int)UILayer.LayerName.Settings - 1].visible = !Main.UIs[(int)UILayer.LayerName.Settings - 1].visible);
                    break;
                case UILayer.LayerName.MainElements2:
                    this.visible = true;
                    this.origin.location = new Vector2(Main.BestScreenWidth / 2 - 300.0f, 250.0f);
                    UIImageButton button2;
                    break;
                case UILayer.LayerName.MainElements3:
                    break;
                case UILayer.LayerName.Settings:
                    this.visible = false;
                    Vector2 size = Main.BestScreenSize * .7f;
                    UITabbedPane panel = new UITabbedPane(size);
                    panel.AddTo(this.origin);
                    UITab tab = new UITab(Localization.Localization.GetText("Game.ButtonText.Settings"));
                    panel.AddTab(tab);
                    UIElement ele = new UIElement();
                    ele.LocateToParent = new Vector2(Main.BestScreenWidth / 2 - 300.0f, 250.0f);
                    ele.AddTo(tab);
                    for (int i = 0; i < 3; i++)
                    {
                        button2 = new UIImageButton(Asset.UI["Button1"]);
                        button2.id = 60 << i;
                        button2.AddTo(ele);
                        button2.LocateToParent = location;
                        button2.MouseClick += new Action<UIElement, MouseEvent>((e, evt) => Options.FramePerSecond = e.id);
                        UIText text = new UIText(button2.size, $"{button2.id} FPS", Vector2.One * .75f, null, UIText.TextAlign.Center);
                        text.AddTo(button2);
                        text.LocateToParent = Vector2.Zero;
                        location.Y += button2.size.Y + 10.0f;
                    }

                    tab = new UITab(Localization.Localization.GetText("Game.ButtonText.Controls"));
                    panel.AddTab(tab);
                    ele = new UIElement();
                    ele.LocateToParent = new Vector2(Main.BestScreenWidth / 2 - 300.0f, 250.0f);
                    ele.AddTo(tab);
                    location = Vector2.Zero;
                    foreach (Vector2 resolution in Main.SupportedScreenResolution)
                    {
                        button2 = new UIImageButton(Asset.UI["Button1"]);
                        button2.id = ele.children.Count;
                        button2.AddTo(ele);
                        button2.LocateToParent = location;
                        button2.MouseClick += new Action<UIElement, MouseEvent>((e, evt) => Options.ScreenResolutionNum = (byte)e.id);
                        UIText text = new UIText(button2.size, $"{resolution.X}x{resolution.Y}", Vector2.One * .75f, null, UIText.TextAlign.Center);
                        text.AddTo(button2);
                        text.LocateToParent = Vector2.Zero;
                        location.Y += button2.size.Y + 10.0f;
                    }
                    break;
                case UILayer.LayerName.Splash:
                    break;
                case UILayer.LayerName.PlayerInput:
                    break;
                case UILayer.LayerName.DebugMsg:
                    break;
                case UILayer.LayerName.Cursor:
                    break;
            }
        }

        public virtual void UpdateUI(GameTime gameTime)
        {

            switch ((UILayer.LayerName)this.depth)
            {
                case UILayer.LayerName.Background:
                    break;
                case UILayer.LayerName.MainElements:
                    break;
                case UILayer.LayerName.MainElements2:
                    this.active = this.visible;
                    break;
                case UILayer.LayerName.Settings:
                    this.active = this.visible;
                    break;
                case UILayer.LayerName.Splash:
                    this.visible = !Main.UIs[(int)UILayer.LayerName.Settings - 1].visible;
                    break;
                case UILayer.LayerName.PlayerInput:
                    break;
                case UILayer.LayerName.DebugMsg:
                    break;
                case UILayer.LayerName.Cursor:
                    break;
            }
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 location = Vector2.Zero;
            switch ((UILayer.LayerName)this.depth)
            {
                case UILayer.LayerName.Background:
                    spriteBatch.Draw(Asset.Splash["SplashBG"], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
                    break;
                case UILayer.LayerName.MainElements:
                    break;
                case UILayer.LayerName.MainElements2:
                    break;
                case UILayer.LayerName.Splash:
                    Texture2D tex = Asset.Splash["Splash"];
                    location = Main.ScreenSize * new Vector2(.5f, .3f);
                    spriteBatch.Draw(tex, location, null, Color.White, 0.0f, tex.Center(), 1f, SpriteEffects.None, 0.0f);
                    break;
                case UILayer.LayerName.Settings:
                    UITabbedPane panel = (UITabbedPane)origin.children[0];
                    panel.location = (Main.UIScreenSize - panel.size) / 2f;
                    float width = 4f;
                    Drawing.DrawBoxByCorner(spriteBatch, Asset.UI["UICorner1"], (panel.location - new Vector2(width, 62)) * Options.UIScale, (panel.size + new Vector2(width * 2, 64)) * Options.UIScale);
                    break;
                case UILayer.LayerName.PlayerInput:
                    if (Options.ShowPlayerInput) Main.playerInput[0].DrawPlayerInput(spriteBatch, Main.ScreenSize * new Vector2(0f, .99f));
                    break;
                case UILayer.LayerName.DebugMsg:
                    if (Options.ShowDebugMessage)
                    {
                        float offset = 2f * Options.UIScale;
                        string msg = string.Join("\n",
                            Localization.Localization.GetText("Game.Debug.FPS", Main.FPS),
                            Localization.Localization.GetText("Game.Debug.Memory", Utils.Utils.ConvertMemory(Main.Memory)),
                            Localization.Localization.GetText("Game.Debug.ScreenSize", string.Format("{0}x{1}", Main.screenWidth, Main.screenHeight))
                            );
                        Vector2[] bpos = new Vector2[4]
                        {
                            new Vector2(offset, offset),
                            new Vector2(-offset, offset),
                            new Vector2(-offset, -offset),
                            new Vector2(offset, -offset),
                        };
                        for (int i = 0; i < 4; i++)
                            Drawing.DrawColorfulString(spriteBatch, msg, location + bpos[i], new Vector2(Options.UIScale), 0, 0, .4f, Color.Black);
                        Drawing.DrawColorfulString(spriteBatch, msg, location, new Vector2(Options.UIScale), Main.timer * 16, 16, 1f, Main.rainbow_raw);
                    }
                    break;
                case UILayer.LayerName.Cursor:
                    spriteBatch.Draw(Asset.Image["Pixel"], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * (1f - Main.screenOpacity));
                    Texture2D cursor = Asset.Cursor["Cursor2"];
                    spriteBatch.Draw(cursor, Main.MouseScreen, null, Color.White, 0f, Vector2.Zero, Options.UIScale, SpriteEffects.None, 0f);
                    break;
            }
        }
    }
}