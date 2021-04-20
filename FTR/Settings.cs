using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace FTR
{
    class Settings : Level
    {
        private Sprite ButtonBack, ButtonAdd, VolumeBar, 
            ButtonDistract, VolumeText;
        protected Image BText, Bar;
        Random rnd = new Random();
        private static List<Sprite> Stars = new List<Sprite>();
        private int[,] StarPoints = new int[,] { {1500, 50 }, {570, 54 }, {1900, 50 }, {875, 80 }, {1200, 146 }, { 20, 10} };
        public override void LoadAssets()
        {
            MakeStars();
            BackgroundImg = FTR.Properties.Resources.ForestSunset;
            BText = global.ScaleImage(FTR.Properties.Resources.BackToMenuText);
            Bar = global.ScaleImage(FTR.Properties.Resources.VolumeBar5);
            Image Add = global.ScaleImage(FTR.Properties.Resources.VolumeAdd);
            Image Sub = global.ScaleImage(FTR.Properties.Resources.VolumeSubtract);
            ButtonBack = new Sprite(new Vector((global.form_menu.Map.Width / 10) - BText.Size.Width / 2, global.form_menu.Map.Height - BText.Size.Height), new Vector(1, 1), BText, "ButtonLevel"); AllSprites.Add(ButtonBack);
            ButtonAdd = new Sprite(new Vector(((global.form_menu.Map.Width / 2) + Bar.Width/2 + Add.Width/2), (global.form_menu.Map.Height/2)), new Vector(1, 1), Add, "VolumeControl"); AllSprites.Add(ButtonAdd);
            ButtonDistract = new Sprite(new Vector(((global.form_menu.Map.Width / 2) - Bar.Width / 2 - Sub.Width * 1.5f), (global.form_menu.Map.Height / 2)), new Vector(1, 1), Sub, "VolumeControl"); AllSprites.Add(ButtonDistract);
            VolumeBar = new Sprite(new Vector((960*global.ScreenScale.X - Bar.Width/2), global.form_menu.Map.Height / 2), new Vector(1, 1), Bar, "VolumeBar"); AllSprites.Add(VolumeBar);
            TrackBar(global.form_menu.GetVolume);
            VolumeText = new Sprite(new Vector(((global.form_menu.Map.Width / 2) - global.ScaleImage(FTR.Properties.Resources.VolumeText).Width / 2), global.form_menu.Map.Height / 2 - Bar.Height), new Vector(1, 1), global.ScaleImage(FTR.Properties.Resources.VolumeText), "VolumeText"); AllSprites.Add(VolumeText);
            Buttons.Add(ButtonBack); Buttons.Add(ButtonAdd); Buttons.Add(ButtonDistract);
        }
        public override void UpdateButtons(Form Window)
        {
            if (Buttons.Count() != 0)
            {
                Image Add = global.ScaleImage(FTR.Properties.Resources.VolumeAdd);
                Image Sub = global.ScaleImage(FTR.Properties.Resources.VolumeSubtract);
                ButtonBack.ChangePosition(new Vector((global.form_menu.Map.Width / 10) - BText.Size.Width / 2, global.form_menu.Map.Height - BText.Size.Height));
                ButtonAdd.ChangePosition(new Vector(((global.form_menu.Map.Width / 2) + Bar.Width / 2 + Add.Width / 2), (global.form_menu.Map.Height / 2)));
                ButtonDistract.ChangePosition(new Vector(((global.form_menu.Map.Width / 2) - Bar.Width / 2 - Sub.Width * 1.5f), (global.form_menu.Map.Height/ 2)));
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
                        if (sprite.Tag != "VolumeControl")
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
                Window.LoadLevel(Window.GetBackup);
            }
            else if (sprite == ButtonAdd)
            {
                global.form_menu.ChangeVolume(1);
                TrackBar(global.form_menu.GetVolume);
            }
            else if (sprite == ButtonDistract)
            {
                global.form_menu.ChangeVolume(-1);
                TrackBar(global.form_menu.GetVolume);
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
        private void TrackBar(int Volume)
        {
            System.Resources.ResourceManager rm = FTR.Properties.Resources.ResourceManager;
            Image BarImg = (Image)rm.GetObject("VolumeBar" + Volume.ToString());
            VolumeBar.ChangeImage(BarImg);
        }
    }
}
