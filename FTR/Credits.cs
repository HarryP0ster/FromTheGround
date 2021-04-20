using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace FTR
{
    class Credits : Level
    {
        private Sprite ButtonBack, TCredits;
        protected Image BText, CText;
        Random rnd = new Random();
        private static List<Sprite> Stars = new List<Sprite>();
        private int[,] StarPoints = new int[,] { { 100, 200}, { 500, 400}, { 776, 300}, {1500, 50 }, { 170, 375}, {950, 220 },
         {300, 135 }, {570, 54 }, {1760, 320 }, {1900, 50 }, {875, 80 }, {1200, 146 }, { 1300, 386}, {1650, 230 }, { 20, 10},
        {1000, 450 }, {600, 500 }, {700, 50 }, {1500, 550 }, {1450, 350 }, {200, 50 }, {750, 450 }, {1050, 50 } , {565, 250 }, {1150, 300 },
        {1400, 200 }, {100, 750 }, {650, 968 }, {654, 650 }, {1337, 420 }, {310, 800 }, {180, 990 }, {110, 600 }, {900, 900 }, {1200, 850 }, 
        {300, 600 }, {1500, 800 }, {1487, 999 }, {1800, 650 }, {1900, 900 } };
        public override void LoadAssets()
        {
            MakeStars();
            BackgroundImg = FTR.Properties.Resources.JustSky;
            BText = global.ScaleImage(FTR.Properties.Resources.BackToMenuText);
            CText = global.ScaleImage(FTR.Properties.Resources.Credits);
            ButtonBack = new Sprite(new Vector((global.form_menu.Map.Width / 10) - BText.Size.Width / 2, global.form_menu.Map.Height - BText.Size.Height), new Vector(1, 1), BText, "ButtonLevel"); AllSprites.Add(ButtonBack);
            TCredits = new Sprite(new Vector((global.form_menu.Map.Width / 2) - CText.Size.Width / 2, global.form_menu.Map.Height / 2 - CText.Size.Height / 2), new Vector(1, 1), CText, "ButtonLevel"); AllSprites.Add(TCredits);
            Buttons.Add(ButtonBack);
        }
        public override void UpdateButtons(Form Window)
        {
            Form1 Form = Window as Form1;
            if (Buttons.Count() != 0)
            {
                ButtonBack.ChangePosition(new Vector((Form.Map.Width / 10) - BText.Size.Width / 2, Form.Map.Height - BText.Size.Height));
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
                        Window.Selection.ChangePosition(new Vector(sprite.Position.X - sprite.SpriteBtm.Width / 7, sprite.Position.Y - sprite.SpriteBtm.Height / 7));
                        Window.Selection.ChangeImage(FTR.Properties.Resources.Selection);
                        Window.SpriteInFocus = sprite;
                        AllSprites.Remove(sprite);
                        AllSprites.Add(sprite);
                        Buttons.Remove(sprite); Buttons.Insert(0, sprite);
                        Window.IsSelectionActive = true;
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
        private void MakeStars()
        {
            Image[] StarsCollection = new Image[] { FTR.Properties.Resources.Star01, FTR.Properties.Resources.Star02, FTR.Properties.Resources.Star03 };
            Random rnd = new Random();
            for (int i = 0; i < StarPoints.Length / 2; i++)
            {
                Stars.Add(new Sprite(new Vector(StarPoints[i, 0], StarPoints[i, 1]), new Vector(1, 1), StarsCollection[rnd.Next(0, 2)], "Background"));
            }
        }
        public override void StarsState()
        {
            Random rnd = new Random();
            foreach (Sprite back in Stars)
            {
                back.ChangeBrightness(rnd.Next(-50, 80) + back.GetBrightness);
            }
        }
        public override void ButtonsCheck(Form1 Window, Sprite sprite)
        {
            if (sprite == ButtonBack)
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Menu());
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
    }
}
