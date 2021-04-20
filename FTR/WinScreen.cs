using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace FTR
{
    class WinScreen : Level //Ничего интересного
    {
        private Sprite ButtonBack, TCredits, ButtonRetry, Moon;
        protected Image BIcon, CText, RIcon;
        Level RetryLevel;
        private Vector MoonCoords;
        private static List<Sprite> Stars = new List<Sprite>();
        private int[,] StarPoints = new int[,] { { 100, 200}, { 500, 400}, { 776, 300}, {1500, 50 }, { 170, 375}, {950, 220 },
         {300, 135 }, {570, 54 }, {1760, 320 }, {1900, 50 }, {875, 80 }, {1200, 146 }, { 1300, 386}, {1650, 230 }, { 20, 10},
        {1000, 450 }, {600, 500 }, {700, 50 }, {1500, 550 }, {1450, 350 }, {200, 50 }, {750, 450 }, {1050, 50 } , {565, 250 }, {1150, 300 },
        {1400, 200 } };
        public WinScreen(Image BackImg, int[] ShapesCount, int Height, int Width, Vector MoonCoords)
        {
            this.MoonCoords = MoonCoords;
            BackgroundImg = BackImg;
            RetryLevel = new Game(ShapesCount[0], ShapesCount[1], ShapesCount[2], ShapesCount[3],
                ShapesCount[4], ShapesCount[5], ShapesCount[6], Width, Height, BackgroundImg);
        }
        public override void LoadAssets()
        {
            MakeStars();
            BIcon = global.ScaleImage(FTR.Properties.Resources.Exit);
            CText = global.ScaleImage(FTR.Properties.Resources.WinScreen);
            RIcon = global.ScaleImage(FTR.Properties.Resources.Retry);
            Moon = new Sprite(new Vector(global.ScreenScale.X * (1.1f * MoonCoords.X + 800), global.ScreenScale.Y * (float)((Math.Pow(MoonCoords.X, 2)) / 6000) - 25f), new Vector(0.8f, 0.8f), global.ScaleImage(FTR.Properties.Resources.Moon), "Moon"); AllSprites.Add(Moon);
            ButtonBack = new Sprite(new Vector((global.form_menu.Map.Width / 2) - CText.Size.Width / 2 + 9 * BIcon.Width, global.form_menu.Map.Height / 2 - CText.Size.Height / 2 + 8 * BIcon.Height / 3), new Vector(1, 1), BIcon, "ButtonReturn"); AllSprites.Add(ButtonBack);
            ButtonRetry = new Sprite(new Vector((global.form_menu.Map.Width / 2) - CText.Size.Width / 2 + 4 * RIcon.Width, global.form_menu.Map.Height / 2 - CText.Size.Height / 2 + 8 * RIcon.Height / 3), new Vector(1, 1), RIcon, "ButtonReturn"); AllSprites.Add(ButtonRetry);
            TCredits = new Sprite(new Vector((global.form_menu.Map.Width / 2) - CText.Size.Width / 2, global.form_menu.Map.Height / 2 - CText.Size.Height / 2), new Vector(1, 1), CText, "LevelCompletedText"); AllSprites.Add(TCredits);
            Buttons.Add(ButtonBack); Buttons.Add(ButtonRetry);
        }
        public override void UpdateButtons(Form Window)
        {
            if (Buttons.Count() != 0)
            {
                ButtonBack.ChangePosition(new Vector((global.form_menu.Map.Width / 2) - CText.Size.Width / 2 + 9 * BIcon.Width, global.form_menu.Map.Height / 2 - CText.Size.Height / 2 + 8 * BIcon.Height / 3));
                ButtonRetry.ChangePosition(new Vector((global.form_menu.Map.Width / 2) - CText.Size.Width / 2 + 4 * RIcon.Width, global.form_menu.Map.Height / 2 - CText.Size.Height / 2 + 8 * RIcon.Height / 3));
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
                        Window.Selection.ChangePosition(new Vector(sprite.Position.X, sprite.Position.Y));
                        Window.Selection.ChangeImage(sprite.PreviewImage, new Vector(1,1));
                        Window.SpriteInFocus = sprite;
                        AllSprites.Remove(sprite);
                        AllSprites.Add(sprite);
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
                BackgroundImg = null;
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Selection());
            }
            else if (sprite == ButtonRetry)
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(RetryLevel);
            }
        }
        public override void Clear()
        {
            AllSprites.Clear();
            Stars.Clear();
            Buttons.Clear();
            GC.Collect();
        }
    }
}
