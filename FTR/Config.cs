using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace FTR
{
    /*Не секретный уровень, клянусь своим чем-нибудь*/
    class Config : Level
    {
        private Sprite ButtonBack;
        protected Image BText;
        private static List<Sprite> Stars = new List<Sprite>();
        private int[,] StarPoints = new int[,] { { 600, 300}, { 550, 500}, { 450, 670}, { 700, 400}, { 420, 350 }, { 670, 640}, { 1000, 440 }, { 850, 600 }, { 920, 330 }, { 1020, 710} };
        public override void LoadAssets()
        {
            MakeStars();
            global.form_menu.ChangeAmbient(Properties.Resources.Rain);
            BackgroundImg = FTR.Properties.Resources.room;
            BText = global.ScaleImage(FTR.Properties.Resources.BackToMenuText);
            ButtonBack = new Sprite(new Vector((global.form_menu.Map.Width / 10) - BText.Size.Width / 2, global.form_menu.Map.Height - BText.Size.Height), new Vector(1, 1), BText, "ButtonLevel"); AllSprites.Add(ButtonBack);
            Buttons.Add(ButtonBack);
        }
        public override void UpdateButtons(Form Window)
        {
            if (Buttons.Count() != 0)
            {
                ButtonBack.ChangePosition(new Vector((global.form_menu.Map.Width / 10) - BText.Size.Width / 2, global.form_menu.Map.Height - BText.Size.Height));
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
                global.form_menu.ChangeAmbient(Properties.Resources.Cloud_Rome_Ground);
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
