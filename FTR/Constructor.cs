using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace FTR
{
    class Constructor : Level // На самом деле ничего интересного ಠ_ಠ
    {
        private Sprite ButtonBack, ButtonProcceed;
        private Sprite Hud;
        private Sprite[] SlotField = new Sprite[14];
        private Sprite[] HUDNums = new Sprite[14];
        protected Image BText, PText, Field;
        private static List<Sprite> Stars = new List<Sprite>();
        private int[,] StarPoints = new int[,] { { 100, 200}, { 500, 400}, { 776, 300}, {1500, 50 }, { 170, 375}, {950, 220 },
         {300, 135 }, {570, 54 }, {1760, 320 }, {1900, 50 }, {875, 80 }, {1200, 146 }, { 1300, 386}, {1650, 230 }, { 20, 10} };
        private int Width = 4;
        private int Height = 4;
        private int[] Quanities = new int[7];
        private static Sprite[] ScreenCounts = new Sprite[10];
        private float[,] Counters = new float[,] { { 1290f, 240f }, { 1290f, 370f }, { 1290f, 490f }, { 1655f, 240f }, { 1655f, 370f },  { 1655f, 490f }, { 1450f, 650f } };
        private float[,] Controls = new float[,] { { 1020f, 230f }, { 1020f, 280f }, { 1020, 362f }, { 1020, 412f }, { 1020, 480f }, { 1020, 530f }, { 1260f, 650f },
        { 1260f, 690f }, { 1410f, 230f }, { 1410f, 280f }, { 1410, 362f }, { 1410, 412f }, { 1410, 480f }, { 1410, 530f }};
        int TetrominosInUse = 0;
        private Sprite[] AddTetromino = new Sprite[14];

        public override void LoadAssets()
        {
            System.Resources.ResourceManager rm = FTR.Properties.Resources.ResourceManager;
            MakeStars();
            BackgroundImg = FTR.Properties.Resources.Menu_Sky;
            BText = global.ScaleImage(FTR.Properties.Resources.BackToMenuText);
            PText = global.ScaleImage(FTR.Properties.Resources.Generate);
            Field = global.ScaleImage(FTR.Properties.Resources.FieldHUD);
            ButtonProcceed = new Sprite(new Vector(1, 1), new Vector(1, 1), PText, "Button"); AllSprites.Add(ButtonProcceed);
            Hud = new Sprite(new Vector(0, 0), new Vector(1, 1), global.ScaleImage(FTR.Properties.Resources.Generator_HUD), "HUD"); AllSprites.Add(Hud);
            for (int i = 0; i < 7; i++)
            {
                SlotField[i] = new Sprite(new Vector(200f * global.ScreenScale.X, 100f * global.ScreenScale.Y + Field.Width * i), new Vector(1, 1), Field, "Field"); AllSprites.Add(SlotField[i]);
                string Name = "HUD_" + (i + 4).ToString();
                Image Num = global.ScaleImage((Image)rm.GetObject(Name));
                HUDNums[i] = new Sprite(new Vector(SlotField[i].Position.X + Field.Width/2.8f, SlotField[i].Position.Y + Field.Height/4), new Vector(1, 1), Num, "ButtonSlots"); AllSprites.Add(HUDNums[i]);
                SlotField[i+7] = new Sprite(new Vector(550f * global.ScreenScale.X, 100f * global.ScreenScale.Y + Field.Width * i), new Vector(1, 1), Field, "Field"); AllSprites.Add(SlotField[i + 7]);
                HUDNums[i+7] = new Sprite(new Vector(SlotField[i+7].Position.X + Field.Width / 2.8f, SlotField[i+7].Position.Y + Field.Height / 4), new Vector(1, 1), Num, "ButtonSlots"); AllSprites.Add(HUDNums[i+7]);
                Buttons.Add(SlotField[i]); Buttons.Add(SlotField[i+7]);
            }
            Image Quanity = (Image)(new Bitmap(FTR.Properties.Resources.Quanity0, new Size((int)Math.Floor(FTR.Properties.Resources.Quanity0.Width * global.ScreenScale.X), (int)Math.Floor(FTR.Properties.Resources.Quanity0.Height * global.ScreenScale.Y))));
            for (int i = 0; i < 7; i++)
            {
                ScreenCounts[i] = new Sprite(new Vector(Counters[i, 0] * global.ScreenScale.X, Counters[i, 1] * global.ScreenScale.Y), new Vector(1.3f, 1.3f), Quanity, "Count");
                AllSprites.Add(ScreenCounts[i]);
            }
            SlotField[0].ChangeImage(FTR.Properties.Resources.FieldHUD_Selected);
            SlotField[7].ChangeImage(FTR.Properties.Resources.FieldHUD_Selected);
            ButtonBack = new Sprite(new Vector((global.form_menu.Map.Width / 10) - BText.Size.Width / 2, global.form_menu.Map.Height - BText.Size.Height), new Vector(1, 1), BText, "ButtonBack"); AllSprites.Add(ButtonBack);
            Buttons.Add(ButtonBack); Buttons.Add(ButtonProcceed);
            for (int i = 0; i < 14; i++)
            {
                Image preview;
                string Tag;
                if (i % 2 == 0)
                {
                    Tag = "TetAdd";
                    preview = global.ScaleImage(FTR.Properties.Resources.TetrAdd);
                }
                else
                {
                    Tag = "TetRemove";
                    preview = global.ScaleImage(FTR.Properties.Resources.TetrRemove);
                }
                AddTetromino[i] = new Sprite(new Vector(Controls[i, 0] * global.ScreenScale.X, Controls[i, 1] * global.ScreenScale.Y), new Vector(0.5f, 0.5f), preview, Tag); AllSprites.Add(AddTetromino[i]);
                Buttons.Add(AddTetromino[i]);
            }
        }
        public override void UpdateButtons(Form Window)
        {
            if (Buttons.Count() != 0)
            {
                ButtonBack.ChangePosition(new Vector((global.form_menu.Map.Width / 10) - BText.Size.Width / 2, global.form_menu.Map.Height - BText.Size.Height));
                ButtonProcceed.ChangePosition(new Vector((global.form_menu.Map.Width) - PText.Size.Width * 2.1f, global.form_menu.Map.Height - PText.Size.Height * 2.6f));
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
                        if (sprite.Tag == "ButtonBack")
                        {
                            Window.Selection.ChangePosition(new Vector(sprite.Position.X - sprite.SpriteBtm.Width / 7, sprite.Position.Y - sprite.SpriteBtm.Height / 7));
                            Window.Selection.ChangeImage(FTR.Properties.Resources.Selection);
                            AllSprites.Remove(sprite);
                            AllSprites.Add(sprite);
                        }
                        else
                        {
                            Window.Selection.ChangePosition(new Vector(sprite.Position.X, sprite.Position.Y));
                            Window.Selection.ChangeImage(sprite.PreviewImage, new Vector(1,1));
                        }
                        Window.SpriteInFocus = sprite;
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
            else if (sprite == ButtonProcceed)
            {
                if (TetrominosInUse == 0 && (Width * Height) % 4 == 0)
                {
                    Clear();
                    GC.SuppressFinalize(this);
                    Window.LoadLevel(new Loading());
                    Window.Refresh();
                    Window.SuspendRender();
                    Generator NewGenerator;
                    bool Task;
                    do //бесконечный цикл, ну почти
                    {
                        NewGenerator = new Generator(Width, Height);
                        GC.Collect();
                        Task = NewGenerator.Generate(); //Проверяет случайное решение на решаемость
                    } while (Task != true);
                    Window.ResumeRender();
                }
                else if ((Width * Height) % 4 == 0)
                {
                    Clear();
                    GC.SuppressFinalize(this);
                    Window.LoadLevel(new Game(Quanities[2], Quanities[5], Quanities[0], Quanities[1], Quanities[4], Quanities[6], Quanities[3], Width, Height, Properties.Resources.DarkForest03));
                }
                else
                {
                    ButtonProcceed.ChangeImage(FTR.Properties.Resources.GenerateError);
                }
            }
            else if (sprite.Tag == "Field")
            {
                ButtonProcceed.ChangeImage(FTR.Properties.Resources.Generate);
                for (int i = 0; i < 14; i++)
                {
                    if (sprite == SlotField[i])
                    {
                        if (i < 7)
                        {
                            SlotField[Width - 4].ChangeImage(FTR.Properties.Resources.FieldHUD);
                            Width = i + 4;
                        }
                        else
                        {
                            SlotField[Height + 3].ChangeImage(FTR.Properties.Resources.FieldHUD);
                            Height = i - 3;
                        }
                        SlotField[i].ChangeImage(FTR.Properties.Resources.FieldHUD_Selected);
                    }
                }
            }
            else if (sprite.Tag == "TetAdd")
            {
                if (sprite == AddTetromino[0] && Quanities[2] < 9) //L 2
                {
                    Quanities[2]++;
                    TetrominosInUse++;
                    UpdateCounts(2);
                }
                else if (sprite == AddTetromino[2] && Quanities[5] < 9) //I 5
                {
                    Quanities[5]++;
                    TetrominosInUse++;
                    UpdateCounts(5);
                }
                else if (sprite == AddTetromino[4] && Quanities[3] < 9) //S 3
                {
                    Quanities[3]++;
                    TetrominosInUse++;
                    UpdateCounts(3);
                }
                else if (sprite == AddTetromino[6] && Quanities[0] < 9) //O 0
                {
                    Quanities[0]++;
                    TetrominosInUse++;
                    UpdateCounts(0);
                }
                else if (sprite == AddTetromino[8] && Quanities[1] < 9) //J 1
                {
                    Quanities[1]++;
                    TetrominosInUse++;
                    UpdateCounts(1);
                }
                else if (sprite == AddTetromino[10] && Quanities[6] < 9) //T 6
                {
                    Quanities[6]++;
                    TetrominosInUse++;
                    UpdateCounts(6);
                }
                else if (sprite == AddTetromino[12] && Quanities[4] < 9) //Z 4
                {
                    Quanities[4]++;
                    TetrominosInUse++;
                    UpdateCounts(4);
                }
            }
            else if (sprite.Tag == "TetRemove")
            {
                if (sprite == AddTetromino[1] && Quanities[2] > 0) //L 2
                {
                    Quanities[2]--;
                    TetrominosInUse--;
                    UpdateCounts(2);
                }
                else if (sprite == AddTetromino[3] && Quanities[5] > 0) //I 5
                {
                    Quanities[5]--;
                    TetrominosInUse--;
                    UpdateCounts(5);
                }
                else if (sprite == AddTetromino[5] && Quanities[3] > 0) //S 3
                {
                    Quanities[3]--;
                    TetrominosInUse--;
                    UpdateCounts(3);
                }
                else if (sprite == AddTetromino[7] && Quanities[0] > 0) //O 0
                {
                    Quanities[0]--;
                    TetrominosInUse--;
                    UpdateCounts(0);
                }
                else if (sprite == AddTetromino[9] && Quanities[1] > 0) //J 1
                {
                    Quanities[1]--;
                    TetrominosInUse--;
                    UpdateCounts(1);
                }
                else if (sprite == AddTetromino[11] && Quanities[6] > 0) //T 6
                {
                    Quanities[6]--;
                    TetrominosInUse--;
                    UpdateCounts(6);
                }
                else if (sprite == AddTetromino[13] && Quanities[4] > 0) //Z 4
                {
                    Quanities[4]--;
                    TetrominosInUse--;
                    UpdateCounts(4);
                }
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
        private void UpdateCounts(int Shape)
        {
            ButtonProcceed.ChangeImage(FTR.Properties.Resources.Generate);
            switch (Shape)
            {
                case 0:
                    ScreenCounts[6].ChangeImage(GetCountPreview(Quanities[Shape]));
                    return;
                case 1:
                    ScreenCounts[3].ChangeImage(GetCountPreview(Quanities[Shape]));
                    return;
                case 2:
                    ScreenCounts[0].ChangeImage(GetCountPreview(Quanities[Shape]));
                    return;
                case 3:
                    ScreenCounts[2].ChangeImage(GetCountPreview(Quanities[Shape]));
                    return;
                case 4:
                    ScreenCounts[5].ChangeImage(GetCountPreview(Quanities[Shape]));
                    return;
                case 5:
                    ScreenCounts[1].ChangeImage(GetCountPreview(Quanities[Shape]));
                    return;
                case 6:
                    ScreenCounts[4].ChangeImage(GetCountPreview(Quanities[Shape]));
                    return;
            }
        }

        private Image GetCountPreview(int Num)
        {
            System.Resources.ResourceManager rm = FTR.Properties.Resources.ResourceManager;

            if (Num < 10)
                return (Image)rm.GetObject("Quanity" + Num.ToString());
            else
                return FTR.Properties.Resources.Quanity10;
        }
    }
}