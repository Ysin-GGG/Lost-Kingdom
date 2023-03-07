using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LostKingdom.Utils;

namespace LostKingdom.GameContent.UI
{
    public class UILayer
    {
        public string name;
        public float layer;
        public UIElement state;

        public enum LayerName
        {
            Background = 1,
            MainElements = 2,
            Splash = 29,
            PlayerInput = 30,
            DebugMsg = 31,
            Cursor = 32,
        }

        public UILayer(string name, float layer)
        {
            this.name = name;
            this.layer = layer;
            this.state = null;
            Main.UIs.Add(this);
            Main.UIs.Sort(new Comparison<UILayer>((a, b) => Math.Sign(a.layer - b.layer)));
        }

        public virtual void UpdateUI(GameTime gameTime)
        {
            this.state?.Update(gameTime);
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            UIElement ui = null;
            bool init = this.state == null;
            switch ((UILayer.LayerName)this.layer)
            {
                case UILayer.LayerName.Background:
                    ui = DrawUserInterface.Draw_UI_1_Background(spriteBatch, init);
                    break;
                case UILayer.LayerName.MainElements:
                    ui = DrawUserInterface.Draw_UI_2_MainElements(spriteBatch, init);
                    break;
                case UILayer.LayerName.Splash:
                    ui = DrawUserInterface.Draw_UI_29_Splash(spriteBatch, init);
                    break;
                case UILayer.LayerName.PlayerInput:
                    ui = DrawUserInterface.Draw_UI_30_PlayerInput(spriteBatch, init);
                    break;
                case UILayer.LayerName.DebugMsg:
                    ui = DrawUserInterface.Draw_UI_31_DebugMsg(spriteBatch, init);
                    break;
                case UILayer.LayerName.Cursor:
                    ui = DrawUserInterface.Draw_UI_32_Cursor(spriteBatch, init);
                    break;
            }
            if (this.state == null) this.state = ui;
            this.state?.Draw(spriteBatch);
        }

        /// <summary> UI图层绘制 </summary>
        public static class DrawUserInterface
        {
            /// <summary> 图层：背景图 </summary>
            public static UIElement Draw_UI_1_Background(SpriteBatch spriteBatch, bool init)
            {
                spriteBatch.Draw(Asset.Splash["SplashBG"], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.White);
                return null;
            }

            /// <summary> 图层：主界面UI </summary>
            public static UIElement Draw_UI_2_MainElements(SpriteBatch spriteBatch, bool init)
            {
                if (init)
                {
                    UIElement state = new UIElement(new Vector2(150.0f, Main.BestScreenHeight - 400.0f), Vector2.One);
                    Vector2 location = Vector2.Zero;
                    string[] content =
                    {
                        "开始游戏",
                        "档案库",
                        "开发者信息",
                        "退出",
                    };
                    foreach (string s in content)
                    {
                        UIImageButton button = new UIImageButton(Asset.UI["Button1"]);
                        button.AddTo(state);
                        button.LocateToParent = location;
                        UIText text = new UIText(button.size, s, Vector2.One * .75f, null, UIText.TextAlign.Center);
                        text.AddTo(button);
                        text.LocateToParent = Vector2.Zero;
                        location.Y += button.size.Y + 10.0f;
                    }
                    return state;
                }
                return null;
            }

            /// <summary> 图层：主界面标题 </summary>
            public static UIElement Draw_UI_29_Splash(SpriteBatch spriteBatch, bool init)
            {
                Texture2D tex = Asset.Splash["Splash"];
                Vector2 pos = (Main.BestScreenSize - tex.Size()) / 2f;
                pos.Y -= 200f;
                spriteBatch.Draw(tex, pos * Options.UIScale, null, Color.White, 0.0f, Vector2.Zero, Options.UIScale, SpriteEffects.None, 0.0f);
                spriteBatch.Draw(Asset.Image["Pixel"], new Rectangle(0, 0, Main.screenWidth, Main.screenHeight), Color.Black * (1f - Main.screenOpacity));
                return null;
            }

            /// <summary> 图层：玩家输入设备信息 </summary>
            public static UIElement Draw_UI_30_PlayerInput(SpriteBatch spriteBatch, bool init)
            {
                if (Options.ShowPlayerInput) Main.playerInput[0].DrawPlayerInput(spriteBatch, new Vector2(0f, Main.screenHeight - 18f));
                return null;
            }

            /// <summary> 图层：Debug信息 </summary>
            public static UIElement Draw_UI_31_DebugMsg(SpriteBatch spriteBatch, bool init)
            {
                if (Options.ShowDebugMessage)
                {
                    float offset = 2f * Options.UIScale;
                    string msg = string.Join("\n",
                        Localization.Localization.GetText("Game.Debug.FPS", Main.FPS),
                        Localization.Localization.GetText("Game.Debug.Memory", Utils.Utils.ConvertMemory(Main.Memory)),
                        Localization.Localization.GetText("Game.Debug.ScreenSize", Main.ScreenSize)
                        );
                    Vector2 pos = new Vector2();
                    Vector2[] bpos = new Vector2[4]
                    {
                        new Vector2(offset, offset),
                        new Vector2(-offset, offset),
                        new Vector2(-offset, -offset),
                        new Vector2(offset, -offset),
                    };
                    for (int i = 0; i < 4; i++)
                        Drawing.DrawColorfulString(spriteBatch, msg, pos + bpos[i], Vector2.One, 0, 0, .4f, Color.Black);
                    Drawing.DrawColorfulString(spriteBatch, msg, pos, Vector2.One, Main.timer * 16, 16, 1f, Main.rainbow_raw);
                }
                return null;
            }

            /// <summary> 图层：光标 </summary>
            public static UIElement Draw_UI_32_Cursor(SpriteBatch spriteBatch, bool init)
            {
                //if (Main.Bounds.Contains((Main.MouseScreen + Main.Bounds.TopLeft()).ToPoint()))
                {
                    Texture2D cursor = Asset.Cursor["Cursor2"];
                    //spriteBatch.Draw(cursor, Main.MouseScreen, new Rectangle(0, cursor.Height / 2, cursor.Width, cursor.Height / 2), Color.White, 0, Vector2.Zero, .33f, SpriteEffects.None, 0f);
                    spriteBatch.Draw(cursor, Main.MouseScreen, null, Color.White, 0f, Vector2.Zero, Options.UIScale, SpriteEffects.None, 0f);
                }
                return null;
            }
        }
    }

}