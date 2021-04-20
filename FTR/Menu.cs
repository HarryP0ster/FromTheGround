using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace FTR
{
    class Menu : Level //Меню
    {
        private Sprite ButtonLevel, ButtonExit, ButtonCredits, Moon, Settings;
        private static List<Sprite> Stars = new List<Sprite>();
        private int[,] StarPoints = new int[,] { { 100, 200}, { 500, 400}, { 776, 300}, {1500, 50 }, { 170, 375}, {950, 220 },
         {300, 135 }, {570, 54 }, {1760, 320 }, {1900, 50 }, {875, 80 }, {1200, 146 }, { 1300, 386}, {1650, 230 }, { 20, 10} };
        protected Image LSText, EText, CText;
        public override void LoadAssets()
        {
            BackgroundImg = FTR.Properties.Resources.Menu_Sky;
            LSText = global.ScaleImage(FTR.Properties.Resources.LevelSelectText);
            CText = global.ScaleImage(FTR.Properties.Resources.CreditsText);
            EText = global.ScaleImage(FTR.Properties.Resources.ExitText);
            MakeStars();
            ButtonLevel = new Sprite(new Vector(1,1), new Vector(1, 1), LSText, "ButtonLevel"); AllSprites.Add(ButtonLevel);
            ButtonCredits = new Sprite(new Vector(1,1), new Vector(1, 1), CText, "ButtonCredits"); AllSprites.Add(ButtonCredits);
            ButtonExit = new Sprite(new Vector(1,1), new Vector(1, 1), EText, "ButtonExit"); AllSprites.Add(ButtonExit);
            Moon = new Sprite(new Vector(1735f * global.ScreenScale.X, 95.416f * global.ScreenScale.Y), new Vector(0.8f, 0.8f), global.ScaleImage(FTR.Properties.Resources.Moon), "Moon"); AllSprites.Add(Moon);
            Settings = new Sprite(new Vector(1, 1), new Vector(1, 1), global.ScaleImage(FTR.Properties.Resources.Settings), "ButtonSettings"); AllSprites.Add(Settings);
            Buttons.Add(ButtonLevel); Buttons.Add(ButtonCredits); Buttons.Add(ButtonExit); Buttons.Add(Settings); Buttons.Add(Moon);
        }
        public override void UpdateButtons(Form Window)
        {
            if (Buttons.Count() != 0)
            {
                Form1 Form = Window as Form1;
                ButtonLevel.ChangePosition(new Vector((Form.Map.Width / 2) - ButtonLevel.SpriteBtm.Width / 2, Form.Map.Height / 2));
                ButtonCredits.ChangePosition(new Vector((Form.Map.Width / 2) - ButtonCredits.SpriteBtm.Width / 2, Form.Map.Height / 2 + ButtonLevel.SpriteBtm.Height * 2));
                ButtonExit.ChangePosition(new Vector((Form.Map.Width / 2) - ButtonExit.SpriteBtm.Width / 2, Form.Map.Height / 2 + ButtonLevel.SpriteBtm.Height * 4));
                Settings.ChangePosition(new Vector((Form.Map.Width) - Settings.SpriteBtm.Width * 1.2f, Form.Map.Height - Settings.SpriteBtm.Height * 1.2f));
                Moon.ChangePosition(new Vector(1735f * global.ScreenScale.X, 95.416f * global.ScreenScale.Y));
            }
        }
        public override void CheckMouse(Form1 Window, Sprite Mouse)
        {
            foreach (Sprite sprite in Buttons)
            {
                if ((Cursor.Position.X <= sprite.Position.X + sprite.SpriteBtm.Width && Cursor.Position.X >= sprite.Position.X) && (Cursor.Position.Y - global.MapOffset <= sprite.Position.Y + sprite.SpriteBtm.Height && Cursor.Position.Y - global.MapOffset >= sprite.Position.Y))
                {
                    if (Window.SpriteInFocus != sprite)
                    {
                        if (sprite.Tag != "ButtonSettings")
                            Window.Selection.ChangePosition(new Vector(sprite.Position.X - sprite.SpriteBtm.Width / 5, sprite.Position.Y - sprite.SpriteBtm.Height / 5));
                        else
                            Window.Selection.ChangePosition(new Vector(sprite.Position.X - sprite.SpriteBtm.Width / 2, sprite.Position.Y - sprite.SpriteBtm.Height / 4));
                        if (sprite != Moon)
                            Window.Selection.ChangeImage(FTR.Properties.Resources.Selection);
                        Window.SpriteInFocus = sprite;
                        AllSprites.Remove(sprite);
                        AllSprites.Add(sprite);
                        Buttons.Remove(sprite); Buttons.Insert(0, sprite);
                        Window.IsSelectionActive = true;
                        //Window.PlaySound("Select");
                    }
                    return;
                }
            }
            if (Window.IsSelectionActive)
            {
                Window.IsSelectionActive = false;
                Window.SpriteInFocus = null;
                Window.Selection.ChangeImage(FTR.Properties.Resources.Empty);
            }
        }
        public override void MakeBackground(Graphics g)
        {
            while (true) //Иогда можно попасть на фрейм перерисовки и приложение выдаёт ошибку, этот этап должен пофиксить
            {
                try
                {
                    foreach (Sprite back in Stars)
                    {
                        g.DrawImage(back.SpriteBtm, back.Position.X * global.ScreenScale.X, back.Position.Y * global.ScreenScale.Y);
                    }
                    return;
                }
                catch
                {

                }
            }
        }
        public override void StarsState()
        {
            Random rnd = new Random();
            foreach (Sprite back in Stars)
            {
                back.ChangeBrightness(rnd.Next(-50,80) + back.GetBrightness);
            }
        }
        public override void ButtonsCheck(Form1 Window, Sprite sprite)
        {
            if (sprite == ButtonExit)
                Window.Close();
            else if (sprite == ButtonLevel)
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Selection());
            }
            else if (sprite == ButtonCredits)
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Credits());
            }
            else if (sprite == Settings)
            {
                Window.MakeLevelBackup();
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Settings());
            }
            else if (sprite == Moon)
            {
                Window.MakeLevelBackup();
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Config());
            }
        }
        public override void Clear()
        {
            BackgroundImg = null;
            AllSprites.Clear();
            Stars.Clear();
            Buttons.Clear();
            GC.Collect();
        }

        private void MakeStars()
        {
            Image[] StarsCollection = new Image[] { FTR.Properties.Resources.Star01 , FTR.Properties.Resources.Star02, FTR.Properties.Resources.Star03};
            Random rnd = new Random();
            for (int i = 0; i < StarPoints.Length / 2; i++)
            {
                Stars.Add(new Sprite(new Vector(StarPoints[i,0], StarPoints[i, 1]), new Vector(1, 1), StarsCollection[rnd.Next(0, 2)], "Background"));
            }
        }
    }
}