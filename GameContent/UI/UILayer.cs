using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.GameContent.Localization;
using LostKingdom.GameContent.ID;
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

        /// <summary> 图层名 </summary>
        public enum LayerName
        {
            /// <summary> 主界面 </summary>
            Splash = 1,
            /// <summary> 主界面互动部分 </summary>
            SplashElement = 2,
            /// <summary> 设置界面 </summary>
            Option = 3,
            /// <summary> 游戏场景 </summary>
            Field = 5,
            /// <summary> 场景环境结构 </summary>
            Wild = 6,
            /// <summary> 弹幕（绘制在实体后方） </summary>
            BackProjectile = 7,
            /// <summary> 实体 </summary>
            Entity = 8,
            /// <summary> 弹幕（绘制在实体前方） </summary>
            FrontProjectile = 9,
            /// <summary> 卡牌 </summary>
            Card = 10,
            /// <summary> 玩家设备输入 </summary>
            PlayerInput = 30,
            /// <summary> 调试信息 </summary>
            DebugMsg = 31,
            /// <summary> 光标和淡出淡入 </summary>
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

        /// <summary> 初始化UI </summary>
        public virtual void Initialize()
        {
            this.origin.children.Clear();
            Vector2 location = Vector2.Zero;
            switch ((UILayer.LayerName)this.depth)
            {
                case UILayer.LayerName.Splash:
                    break;
                case UILayer.LayerName.SplashElement:
                    this.origin.location = new Vector2(150.0f, Main.BestScreenHeight - 400.0f);
                    string[] content =
                    {
                        GameLocalization.GetText("UserInterface.Main.Play"),
                        GameLocalization.GetText("UserInterface.Main.Library"),
                        GameLocalization.GetText("UserInterface.Main.About"),
                        GameLocalization.GetText("UserInterface.Main.Exit"),
                    };
                    UIImageButton button1;
                    foreach (string s in content)
                    {
                        button1 = new UIImageButton(Asset.UI["UIButton1"]);
                        button1.AddTo(this.origin);
                        button1.LocateToParent = location;
                        if (s == content[3]) button1.MouseClick += new Action<UIElement, MouseEvent>((e, evt) => Main.instance.Exit());
                        UIText text = new UIText(button1.size, s, Vector2.One * .9f, null, UIText.TextAlign.Center);
                        text.AddTo(button1);
                        text.LocateToParent = Vector2.Zero;
                        location.Y += button1.size.Y + 10.0f;
                    }
                    button1 = new UIImageButton(Asset.UI["UIButton3"]);
                    button1.AddTo(this.origin);
                    button1.LocateToParent = location;
                    button1.MouseClick += new Action<UIElement, MouseEvent>((e, evt) => this.visible = !(Main.UIs[(int)UILayer.LayerName.Option - 1].visible = !Main.UIs[(int)UILayer.LayerName.Option - 1].visible));
                    break;
                case UILayer.LayerName.Option:
                    this.visible = false;
                    Vector2 size = Main.BestScreenSize * .7f;
                    UITabbedPane panel = new UITabbedPane(size);
                    panel.AddTo(this.origin);
                    UITab tab = new UITab(GameLocalization.GetText("UserInterface.Options.Tab.Settings"));
                    panel.AddTab(tab);
                    UIElement ele = new UIElement();
                    ele.IgnoreMe = true;
                    ele.AddTo(tab);
                    ele.LocateToParent = new Vector2(Main.BestScreenWidth / 2 - 300.0f, 250.0f);
                    UISlider slider;
                    float[] volume =
                    {
                        Options.MasterVolume,
                        Options.MusicVolume,
                        Options.SoundVolume,
                        Options.AmbientVolume,
                    };
                    Func<UISlider, float, bool>[] func = {
                        (x, y) => (Options.MasterVolume = y) >= 0,
                        (x, y) => (Options.MusicVolume = y) >= 0,
                        (x, y) => (Options.SoundVolume = y) >= 0,
                        (x, y) => (Options.AmbientVolume = y) >= 0,
                    };
                    string[] name =
                    {
                        GameLocalization.GetText("UserInterface.Options.Volume.Master"),
                        GameLocalization.GetText("UserInterface.Options.Volume.Music"),
                        GameLocalization.GetText("UserInterface.Options.Volume.Sound"),
                        GameLocalization.GetText("UserInterface.Options.Volume.Ambient"),
                    };
                    for (int i = 0; i < 4; i++)
                    {
                        slider = new UISlider(0, 1, 0, false);
                        slider.SetPercent(volume[i]);
                        slider.AddTo(ele);
                        slider.PercentChanged += func[i];
                        slider.LocateToParent = location;
                        slider.name.text = name[i];
                        location.Y += slider.size.Y + 30.0f;
                    }

                    ele = new UIElement();
                    ele.IgnoreMe = true;
                    ele.LocateToParent = new Vector2(Main.BestScreenWidth / 2 - 300.0f, 250.0f);
                    ele.AddTo(tab);
                    for(int i = 0; i < 3; i++) new UIComboBox(Asset.UI["UIComboBox2"]).AddTo(ele);
                    UIComboBox comboBox = (UIComboBox)ele.children[2];
                    comboBox.name.text = GameLocalization.GetText("UserInterface.Options.ScreenResolution.Name");
                    comboBox.lineWidth -= 16f;
                    comboBox.LocateToParent = location;
                    foreach (Vector2 resolution in Main.SupportedScreenResolution)
                        comboBox.AddLine($"{resolution.X}x{resolution.Y}").MouseClick += new Action<UIElement, MouseEvent>((e, evt) => Options.ScreenResolutionNum = (byte)e.id);
                    comboBox.SetLine(Options.ScreenResolutionNum);

                    location.Y += 60.0f;
                    comboBox = (UIComboBox)ele.children[1];
                    comboBox.name.text = GameLocalization.GetText("UserInterface.Options.FPS.Name");
                    comboBox.lineWidth -= 16f;
                    comboBox.LocateToParent = location;
                    comboBox.AddLine("60").MouseClick += new Action<UIElement, MouseEvent>((e, evt) => Options.FramePerSecond = 60);
                    comboBox.AddLine("120").MouseClick += new Action<UIElement, MouseEvent>((e, evt) => Options.FramePerSecond = 120);
                    comboBox.AddLine("240").MouseClick += new Action<UIElement, MouseEvent>((e, evt) => Options.FramePerSecond = 240);
                    comboBox.SetLine(Options.FramePerSecond == 240 ? 2 : (Options.FramePerSecond == 120 ? 1 : 0));


                    location.Y += 60.0f;
                    comboBox = (UIComboBox)ele.children[0];
                    comboBox.name.text = GameLocalization.GetText("UserInterface.Options.Language.Name");
                    comboBox.lineWidth -= 16f;
                    comboBox.LocateToParent = location;
                    foreach (GameCulture culture in GameCulture.GameCultureInfo.Values)
                        if (culture.id > 0)
                            comboBox.AddLine(GameLocalization.GetText(culture.key)).MouseClick += new Action<UIElement, MouseEvent>((e, evt) => Options.LanguageID = (byte)(e.id + 1));
                    comboBox.SetLine(Options.LanguageID - 1);

                    tab = new UITab(GameLocalization.GetText("UserInterface.Options.Tab.Controls"));
                    panel.AddTab(tab);
                    ele = new UIElement();
                    ele.IgnoreMe = true;
                    ele.LocateToParent = new Vector2(Main.BestScreenWidth / 2 - 300.0f, 250.0f);
                    ele.AddTo(tab);
                    location = Vector2.Zero;

                    break;
                case UILayer.LayerName.PlayerInput:
                    break;
                case UILayer.LayerName.DebugMsg:
                    break;
                case UILayer.LayerName.Cursor:
                    break;
            }
        }

        /// <summary> 刷新UI </summary>
        /// <param name="gameTime"></param>
        public virtual void UpdateUI(GameTime gameTime)
        {
            switch ((UILayer.LayerName)this.depth)
            {
                case UILayer.LayerName.Splash:
                    break;
                case UILayer.LayerName.SplashElement:
                    this.active = this.visible;
                    break;
                case UILayer.LayerName.Option:
                    if (Main.CurrInput.GetInputState(Microsoft.Xna.Framework.Input.Keys.Escape) == PlayerInput.InputState.Up)
                    {
                        Main.UIs[(int)UILayer.LayerName.SplashElement - 1].visible = true;
                        this.visible = false;
                    }
                    this.active = this.visible;
                    break;
                case UILayer.LayerName.Card:
                    if (Player.isFighting)
                    {
                        foreach (Card card in Player.Me.hand)
                            card.Update();
                        foreach (Card card in Player.Me.deck)
                            card.Update();
                        foreach (Card card in Player.Me.drawPile)
                            card.Update();
                        foreach (Card card in Player.Me.discardPile)
                            card.Update();
                        foreach (Card card in Player.Me.exhaustPile)
                            card.Update();
                    }
                    break;
                case UILayer.LayerName.PlayerInput:
                    break;
                case UILayer.LayerName.DebugMsg:
                    break;
                case UILayer.LayerName.Cursor:
                    break;
            }
        }

        /// <summary> 绘制UI </summary>
        /// <param name="spriteBatch"></param>
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Vector2 location = Vector2.Zero;
            switch ((UILayer.LayerName)this.depth)
            {
                case UILayer.LayerName.Splash:
                    Texture2D tex = Asset.Splash["Splash"];
                    location = Main.ScreenSize * new Vector2(.5f, .3f);
                    spriteBatch.Draw(Asset.Splash["SplashBG"], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
                    spriteBatch.Draw(tex, location, null, Color.White, 0.0f, tex.Center(), Options.UIScale, SpriteEffects.None, 0.0f);
                    break;
                case UILayer.LayerName.SplashElement:
                    break;
                case UILayer.LayerName.Option:
                    UITabbedPane panel = (UITabbedPane)origin.children[0];
                    panel.location = (Main.UIScreenSize - panel.size) / 2f;
                    float width = 8f;
                    Drawing.DrawBoxByCorner(spriteBatch, Asset.UI["UICorner1"], (panel.location - new Vector2(width / 2f, 48f)) * Options.UIScale, (panel.size + new Vector2(width, 48f + width)) * Options.UIScale);
                    break;
                case UILayer.LayerName.BackProjectile:
                    foreach (Projectile projectile in Main.projectile)
                        if (projectile != null && projectile.active && projectile.behind)
                            projectile.DrawProjectile(spriteBatch);
                    break;
                case UILayer.LayerName.Entity:
                    foreach (Enemy enemy in Main.enemy)
                        if (enemy != null && enemy.active)
                            enemy.DrawEntity(spriteBatch);
                    foreach (Player player in Main.player)
                        if (player != null && player.active)
                            player.DrawEntity(spriteBatch);
                    break;
                case UILayer.LayerName.FrontProjectile:
                    foreach (Projectile projectile in Main.projectile)
                        if (projectile != null && projectile.active && !projectile.behind)
                            projectile.DrawProjectile(spriteBatch);
                    break;
                case UILayer.LayerName.Card:
                    spriteBatch.End();
                    spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.AnisotropicClamp, DepthStencilState.Default, RasterizerState.CullNone);
                    if (Player.drawCardsInHand)
                    {
                        int count = Player.Me.hand.Count;
                        if (count > 0)
                        {
                            float range = MyMath.Min(MyMath.deg01 * 20 * count, MyMath.deg105);
                            float unit = range / (count + 1) * Main.screenOpacity2;
                            float angle = -range / 2f;
                            float scale = Options.UIScale * 1f * MyMath.Lerp(.75f, 1f, Main.screenOpacity2);
                            Vector2 offset1 = new Vector2(0, 320f);
                            Vector2 offset2 = new Vector2(0, -320f * scale);
                            Color color = Color.White * Main.screenOpacity2;
                            for (int i = 0; i < count; i++)
                            {
                                
                                angle += unit;
                                Card card = Player.Me.hand[i];
                                Vector2 pos = Main.MouseScreen + offset1 + offset2.ToRotation(angle) * new Vector2(1f, MyMath.phi0);
                                tex = Asset.Card[card.InnerName];
                                spriteBatch.Draw(tex, pos, null, color, angle, tex.Center(), scale, 0, 0f);
                                /*
                                Card card = Player.Me.hand[i];
                                Vector2 pos = Main.MouseScreen + Vector2.Lerp(new Vector2(-360, 0),new Vector2(360, 0), (float)i / count * MyMath.Lerp(.75f, 1f, Main.screenOpacity2) * MyMath.Lerp(.75f, 1f, Main.screenOpacity2));
                                tex = Asset.Card[card.InnerName];
                                spriteBatch.Draw(tex, pos, null, color, 0f, tex.Left(), scale, 0, 0f);
                                */
                            }
                        }
                    }
                    if (Player.drawCardsInDeck)
                        foreach (Card card in Player.Me.deck)
                            card.Draw(spriteBatch);
                    if (Player.drawCardsInDrawPile)
                        foreach (Card card in Player.Me.drawPile)
                            card.Draw(spriteBatch);
                    if (Player.drawCardsInDiscardPile)
                        foreach (Card card in Player.Me.discardPile)
                            card.Draw(spriteBatch);
                    if (Player.drawCardsInExhaustPile)
                        foreach (Card card in Player.Me.exhaustPile)
                            card.Draw(spriteBatch);
                    break;
                case UILayer.LayerName.PlayerInput:
                    if (Options.ShowPlayerInput) Main.playerInput[0].DrawPlayerInput(spriteBatch, Main.ScreenSize * new Vector2(0f, .99f));
                    break;
                case UILayer.LayerName.DebugMsg:
                    if (Options.ShowDebugMessage)
                    {
                        float offset = 2f * Options.UIScale;
                        string msg = string.Join("\n",
                            GameLocalization.GetText("UserInterface.Debug.FPS", Main.FPS),
                            GameLocalization.GetText("UserInterface.Debug.Memory", Utils.Utils.ConvertMemory(Main.Memory)),
                            GameLocalization.GetText("UserInterface.Debug.ScreenSize", string.Format("{0}x{1}", Main.screenWidth, Main.screenHeight))
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