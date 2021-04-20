using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace FTR
{
    class Selection : Level //Уровень с выбором уровней
    {
        public static List<Sprite> Pads = new List<Sprite>();
        private Sprite ButtonBack, Moon;
        protected Image BText, Pad;
        protected Sprite[] LevelPads = new Sprite[10];
        private static List<Sprite> Stars = new List<Sprite>();
        private int[,] StarPoints = new int[,] { { 100, 200}, { 500, 400}, { 776, 300}, {1500, 50 }, { 170, 375}, {950, 220 },
         {300, 135 }, {570, 54 }, {1760, 320 }, {1900, 50 }, {875, 80 }, {1200, 146 }, { 1300, 386}, {1650, 230 }, { 20, 10} };
        public override void LoadAssets()
        {
            System.Resources.ResourceManager rm = FTR.Properties.Resources.ResourceManager;
            BackgroundImg = FTR.Properties.Resources.Menu_Sky;
            BText = global.ScaleImage(FTR.Properties.Resources.BackToMenuText);
            Pad = global.ScaleImage(FTR.Properties.Resources.NumPad01);
            ButtonBack = new Sprite(new Vector((global.form_menu.Map.Width / 10) - BText.Size.Width / 2, global.form_menu.Map.Height- BText.Size.Height), new Vector(1, 1), BText, "ButtonLevel"); AllSprites.Add(ButtonBack);
            MakeStars();
            LevelPads[0] = new Sprite(new Vector(Pad.Width, Pad.Height / 2), new Vector(1, 1), Pad, "LevelPad");
            Moon = new Sprite(new Vector(1735f * global.ScreenScale.X, 95.416f * global.ScreenScale.Y), new Vector(0.8f, 0.8f), global.ScaleImage(FTR.Properties.Resources.Moon), "Moon"); AllSprites.Add(Moon);
            AllSprites.Add(LevelPads[0]); Buttons.Add(LevelPads[0]);
            for (int i = 1; i < 10; i ++)
            {
                string name = "NumPad0"+(i+1).ToString();
                Pad = global.ScaleImage((Image)rm.GetObject(name));
                if (i < 5)
                    LevelPads[i] = new Sprite(new Vector(LevelPads[i-1].Position.X + 3 * Pad.Width / 2, Pad.Height / 2), new Vector(1, 1), Pad, "LevelPad"); 
                else
                    LevelPads[i] = new Sprite(new Vector(LevelPads[i - 5].Position.X, Pad.Height * 2), new Vector(1, 1), Pad, "LevelPad");
                AllSprites.Add(LevelPads[i]); Buttons.Add(LevelPads[i]);
            }
            Buttons.Add(ButtonBack);
        }
        public override void UpdateButtons(Form Window)
        {
            if (Buttons.Count() != 0)
            {
                Form1 Form = Window as Form1;
                ButtonBack.ChangePosition(new Vector((Form.Map.Width / 10) - BText.Size.Width / 2, Form.Map.Height - BText.Size.Height));
                LevelPads[0].ChangePosition(new Vector(Pad.Width, Pad.Height / 2));
                for (int i = 1; i < 5; i++)
                    LevelPads[i].ChangePosition(new Vector(LevelPads[i - 1].Position.X + 3 * Pad.Width / 2, Pad.Height / 2));
                for (int i = 5; i < 10; i++)
                    LevelPads[i].ChangePosition(new Vector(LevelPads[i - 5].Position.X, Pad.Height *2));
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
                        if (sprite.Tag != "LevelPad")
                        {
                            Window.Selection.ChangePosition(new Vector(sprite.Position.X - sprite.SpriteBtm.Width / 7, sprite.Position.Y - sprite.SpriteBtm.Height / 7));
                            Window.Selection.ChangeImage(FTR.Properties.Resources.Selection);
                        }
                        else
                        {
                            Window.Selection.ChangePosition(new Vector(sprite.Position.X, sprite.Position.Y));
                            Window.Selection.ChangeImage(FTR.Properties.Resources.NumPadSelect);
                        }
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
            else if (sprite == LevelPads[0])
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Game(2,0,0,0,2,0,0,4,4, FTR.Properties.Resources.DarkForest02));
            }
            else if (sprite == LevelPads[1])
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Game(1, 1, 0, 0, 1, 2, 1, 6, 4, FTR.Properties.Resources.DarkForest02));
            }
            else if (sprite == LevelPads[2])
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Game(0, 0, 1, 4, 4, 0, 0, 6, 6, FTR.Properties.Resources.DarkForest03));
            }
            else if (sprite == LevelPads[3])
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Game(2, 0, 1, 1, 1, 2, 0, 4, 7, FTR.Properties.Resources.DarkForest03));
            }
            else if (sprite == LevelPads[4])
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Game(1, 0, 0, 1, 2, 4, 2, 8, 5, FTR.Properties.Resources.DarkForest04));
            }
            else if (sprite == LevelPads[5])
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Game(2, 1, 1, 2, 1, 4, 1, 6, 8, FTR.Properties.Resources.DarkForest04));
            }
            else if (sprite == LevelPads[6])
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Game(3, 1, 1, 3, 3, 0, 3, 8, 7, FTR.Properties.Resources.DarkMountains));
            }
            else if (sprite == LevelPads[7])
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Game(4, 4, 0, 4, 2, 0, 2, 8, 8, FTR.Properties.Resources.DarkForest05));
            }
            else if (sprite == LevelPads[8])
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Game(4, 3, 3, 2, 5, 2, 1, 10, 8, FTR.Properties.Resources.DarkForest05));
            }
            else if (sprite == LevelPads[9])
            {
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Constructor());
            }
        }
        public override void Clear()
        {
            BackgroundImg = null;
            AllSprites.Clear();
            Stars.Clear();
            Buttons.Clear();
            Pads.Clear();
            GC.Collect();
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
    }
}
