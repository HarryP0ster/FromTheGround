using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace FTR
{
    class Game : Level //Лень ¯\_( ͡° ͜ʖ ͡°)_/¯
    {
        /*Tetrominos*/
        public static HashSet<Tetromino>[] TetrominoCollection = new HashSet<Tetromino>[7];
        private Tetromino[,] TetrominoSlots;
        private Tetromino ItemInUse;
        /*Tetrominos*/

        private Sprite Exit, Panel, Moon, Reset, Tutor;
        private int FamilyIndex = 0;
        int[,] Slots;
        Sprite[,] SlotsSprites;
        private Image SlotImg, ExitImg, PanelImg, RImage, TImage;
        private int SHeight, SWidth;
        private int[,] SlotsToTake = new int[,] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };
        private Vector SelectedIndex = new Vector(-1, -1);
        private static Sprite[] ScreenCounts = new Sprite[10];
        private int[] ShapesCount = new int[7];
        private float[] Counters = new float[] { 205f, 442f, 722f, 985f, 1242f, 1548f, 1830f };
        private int FreeSlots; private Vector MoonCoords = new Vector(850f, 70.56f);
        private Vector SlotsScale;
        private static List<Sprite> Stars = new List<Sprite>();
        private int[,] StarPoints = new int[,] { { 100, 200}, { 500, 400}, { 776, 300}, {1500, 50 }, { 170, 375}, {950, 220 },
         {300, 135 }, {570, 54 }, {1760, 320 }, {1900, 50 }, {875, 80 }, {1200, 146 }, { 1300, 386}, {1650, 230 }, { 20, 10},
        {1000, 450 }, {600, 500 }, {700, 50 }, {1500, 550 }, {1450, 350 }, {200, 50 }, {750, 450 }, {1050, 50 } , {565, 250 }, {1150, 300 },
        {1400, 200 } };
        public Tetromino GetItem
        {
            get => ItemInUse;
        }
        public Game(int L, int I, int O, int J, int Z, int T, int S, int Width, int Height, Image Background)
        {
            BackgroundImg = Background;
            Slots = new int[Height,Width];
            SlotsSprites = new Sprite[Height, Width];
            TetrominoSlots = new Tetromino[Height, Width];
            SHeight = Height;
            SWidth = Width;
            FreeSlots = Width * Height;
            for (int i = 0; i < TetrominoCollection.Length; i++)
                TetrominoCollection[i] = new HashSet<Tetromino>();
            ShapesCount[0] = L; ShapesCount[1] = I; ShapesCount[2] = O;
            ShapesCount[3] = J; ShapesCount[4] = Z; ShapesCount[5] = T;
            ShapesCount[6] = S;
        }
        public override void LoadAssets()
        {
            float Scale = 1; float Position = (float)8/SHeight;
            if (((float)SWidth / (float)SHeight) >= 2) Scale = 0.75f;
            else Scale = (float)(1 - (0.025 * Math.Pow(SHeight, 2) / Math.Pow(Math.Log(1.8 * SHeight), 1.95)));
            SlotsScale = new Vector(Scale, Scale);
            Moon = new Sprite(new Vector(global.ScreenScale.X * (1.1f * MoonCoords.X + 800), global.ScreenScale.Y * (float)((Math.Pow(MoonCoords.X, 2)) / 6000) - 25f), new Vector(0.8f, 0.8f), global.ScaleImage(FTR.Properties.Resources.Moon), "Moon"); AllSprites.Add(Moon);
            ExitImg = global.ScaleImage(FTR.Properties.Resources.Exit);
            PanelImg = global.ScaleImage(FTR.Properties.Resources.TetrominoPanel);
            SlotImg = (Image) (new Bitmap(FTR.Properties.Resources.Slot, new Size ((int)Math.Floor(FTR.Properties.Resources.Slot.Width*SlotsScale.X * global.ScreenScale.X), (int)Math.Floor(FTR.Properties.Resources.Slot.Height*SlotsScale.Y * global.ScreenScale.Y))));
            for (int i = 0; i < SHeight; i++)
            {
                for (int j = 0; j < SWidth; j++)
                {
                    Slots[i, j] = 0;
                    TetrominoSlots[i, j] = null;
                    SlotsSprites[i,j] = new Sprite(new Vector(SlotImg.Size.Width+(SlotImg.Size.Width)*j, Scale * Position * SlotImg.Size.Height+(SlotImg.Size.Height*i)), new Vector(1,1), SlotImg, "Slot", new Vector(i, j)); Buttons.Add(SlotsSprites[i, j]);
                    AllSprites.Add(SlotsSprites[i, j]);
                }
            }
            Exit = new Sprite(new Vector(global.form_menu.Map.Width - (ExitImg.Size.Width / 3) * 5, ExitImg.Size.Height / 3), new Vector(1, 1), ExitImg, "Button"); AllSprites.Add(Exit);
            Panel = new Sprite(new Vector(global.form_menu.Map.Width - PanelImg.Size.Width, global.form_menu.Map.Height - PanelImg.Size.Height), new Vector(1, 1), PanelImg, "Sprite"); AllSprites.Add(Panel);
            TImage = global.ScaleImage(FTR.Properties.Resources.Tutor);
            Tutor = new Sprite(new Vector(global.form_menu.Map.Width - TImage.Size.Width * global.ScreenScale.X, global.form_menu.Map.Height - PanelImg.Size.Height - TImage.Size.Height * global.ScreenScale.Y), new Vector(1, 1), FTR.Properties.Resources.Empty, "Sprite"); AllSprites.Add(Tutor);
            RImage = global.ScaleImage(FTR.Properties.Resources.Retry);
            Reset = new Sprite(new Vector(Exit.Position.X - RImage.Width * 2, Exit.Position.Y), new Vector(1, 1), RImage, "ButtonReturn"); AllSprites.Add(Reset);
            Image Quanity = (Image)(new Bitmap(FTR.Properties.Resources.Quanity0, new Size((int)Math.Floor(FTR.Properties.Resources.Quanity0.Width * global.ScreenScale.X), (int)Math.Floor(FTR.Properties.Resources.Quanity0.Height * global.ScreenScale.Y))));
            for (int i = 0; i < 7; i++)
            {
                ScreenCounts[i] = new Sprite(new Vector(Counters[i] * global.ScreenScale.X, global.form_menu.Map.Height - (8 * PanelImg.Size.Height / 10)), new Vector(1, 1), Quanity, "Button");
                AllSprites.Add(ScreenCounts[i]);
            }
            GiveTetrominos(ShapesCount[0], ShapesCount[1], ShapesCount[2], ShapesCount[3], ShapesCount[4], ShapesCount[5], ShapesCount[6]);
            Buttons.Add(Exit); Buttons.Add(Reset); MakeStars();
        }
        public override void UpdateButtons(Form Window)
        {
            if (Buttons.Count() != 0)
            {
                Exit.ChangePosition(new Vector(global.form_menu.Map.Width - (ExitImg.Size.Width / 3) * 5, ExitImg.Size.Height / 3));   
            }
        }
        public override void CheckMouse(Form1 Window, Sprite Mouse)
        {
            foreach (Sprite sprite in Buttons)
            {
                if ((Cursor.Position.X <= sprite.Position.X + sprite.SpriteBtm.Width && Cursor.Position.X >= sprite.Position.X) && (Cursor.Position.Y - global.MapOffset <= sprite.Position.Y + sprite.SpriteBtm.Height && Cursor.Position.Y - global.MapOffset >= sprite.Position.Y))
                {
                    if (Window.SpriteInFocus != sprite || (SelectedIndex.X < 0 && Window.SpriteInFocus.Tag == "Slot"))
                    {
                        Window.Selection.ChangePosition(new Vector(sprite.Position.X, sprite.Position.Y));
                        if (sprite.Tag != "Slot")
                            Window.Selection.ChangeImage(sprite.PreviewImage, new Vector(1,1));
                        Window.SpriteInFocus = sprite;
                        Window.IsSelectionActive = true;
                        if (sprite.Tag == "Slot" && ItemInUse != null && Slots[(int)sprite.SlotIndex.X, (int)sprite.SlotIndex.Y] == 0)
                        {
                            SelectedIndex = sprite.SlotIndex;
                        }
                        Buttons.Remove(sprite); Buttons.Insert(0, sprite);
                    }
                    return;
                }
            }

            if (Window.IsSelectionActive)
            {
                SelectedIndex = new Vector(-1, -1);
                Window.IsSelectionActive = false;
                Window.SpriteInFocus = null;
                Window.Selection.ChangeImage(FTR.Properties.Resources.Empty);
            }
            return;
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
            if (Moon != null)
            {
                MoonCoords = new Vector(MoonCoords.X - 0.15f, 0);
                Moon.ChangePosition(new Vector(global.ScreenScale.X * (1.1f * MoonCoords.X + 800), global.ScreenScale.Y * (float)((Math.Pow(MoonCoords.X, 2)) / 6000) - 25f));
            }
        }
        public override void ButtonsCheck(Form1 Window, Sprite sprite)
        {
            if (sprite == Exit)
            {
                global.form_menu.ResetCursor();
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Menu());
            }
            else if (sprite.Tag == "Slot" && ItemInUse != null)
            {
                PutTetromino();
            }
            else if (sprite.Tag == "Slot" && ItemInUse == null && Slots[(int)sprite.SlotIndex.X, (int)sprite.SlotIndex.Y] > 0)
            {
                PickTetromino((int)sprite.SlotIndex.X, (int)sprite.SlotIndex.Y);
                SelectedIndex = new Vector(-1, -1);
            }
            else if (sprite == Reset)
            {
                Image Bck = Level.BackgroundImg;
                Clear();
                GC.SuppressFinalize(this);
                Window.LoadLevel(new Game(ShapesCount[0], ShapesCount[1], ShapesCount[2], ShapesCount[3],
                ShapesCount[4], ShapesCount[5], ShapesCount[6], SWidth, SHeight, Bck));
            }
        }
        public override void Clear()
        {
            BackgroundImg = null;
            AllSprites.Clear();
            Buttons.Clear();
            Stars.Clear();
            GC.Collect();
        }

        private bool TryToOccupie(int h, int w, Tetromino sigil)
        {
            SlotsToTake = new int[,] { { 0, 0 }, { 0, 0 }, { 0, 0 }, { 0, 0 } };
            for (int i = 0; i < 4; i++)
            {
                try
                {
                    if (Slots[h - (sigil.Cosinus * sigil.Coords[i, 1] + sigil.Sinus * sigil.Coords[i, 0]), w + (sigil.Cosinus * sigil.Coords[i, 0] - sigil.Sinus * sigil.Coords[i, 1])] == 0)
                    {
                        SlotsToTake[i, 0] = h - (sigil.Cosinus * sigil.Coords[i, 1] + sigil.Sinus * sigil.Coords[i, 0]);
                        SlotsToTake[i, 1] = w + (sigil.Cosinus * sigil.Coords[i, 0] - sigil.Sinus * sigil.Coords[i, 1]);
                    }
                    else
                    {
                        return false;
                    }
                }
                catch
                {
                    return false;
                }
            }
            return true;
        }
        public void Rotate()
        {
            if (ItemInUse != null && ItemInUse.Shape > 0)
            {
                ItemInUse.Rotate(SlotsScale);
                global.form_menu.IsSelectionActive = false;
                global.form_menu.MouseCursorOffset = new Vector(ItemInUse.GetOffset.X * SlotsScale.X * global.ScreenScale.X, ItemInUse.GetOffset.Y * SlotsScale.Y * global.ScreenScale.Y);
                SelectedIndex = new Vector(-1, -1);
            }
        }

        public void TakeTetromino(int Shape)
        {
            if (TetrominoCollection[Shape].Count == 0) return;
            ResetItem();
            Tutor.ChangeImage(TImage);
            ItemInUse = TetrominoCollection[Shape].First();
            TetrominoCollection[Shape].Remove(ItemInUse);
            UpdateCounts(Shape);
            Image NewCursor = (Image) (new Bitmap(ItemInUse.GetIcon, new Size((int)Math.Floor(ItemInUse.GetIcon.Width * SlotsScale.X), (int)Math.Floor(ItemInUse.GetIcon.Height * SlotsScale.Y))));
            global.form_menu.ChangeCursor(NewCursor, new Vector(ItemInUse.GetOffset.X * SlotsScale.X * global.ScreenScale.X, ItemInUse.GetOffset.Y * SlotsScale.Y * global.ScreenScale.Y));
        }

        private void PickTetromino(int IndexH, int IndexW)
        {
            ItemInUse = new Tetromino(TetrominoSlots[IndexH, IndexW].Shape);
            Tutor.ChangeImage(TImage);
            int IndexF = Slots[IndexH, IndexW];
            int Shape = ItemInUse.Shape;
            Image SlotImg = (Image)(new Bitmap(FTR.Properties.Resources.Slot, new Size((int)Math.Floor(FTR.Properties.Resources.Slot.Width * SlotsScale.X), (int)Math.Floor(FTR.Properties.Resources.Slot.Height * SlotsScale.Y))));
            Image NewCursor = (Image)(new Bitmap(ItemInUse.GetIcon, new Size((int)Math.Floor(ItemInUse.GetIcon.Width * SlotsScale.X), (int)Math.Floor(ItemInUse.GetIcon.Height * SlotsScale.Y))));
            global.form_menu.ChangeCursor(NewCursor, new Vector(ItemInUse.GetOffset.X * SlotsScale.X * global.ScreenScale.X, ItemInUse.GetOffset.Y * SlotsScale.Y * global.ScreenScale.Y));
            for (int i = 0; i < TetrominoSlots[IndexH, IndexW].Rotation; i++)
                Rotate();
            for (int i = 0; i < SHeight; i++)
            {
                for (int j = 0; j < SWidth; j++)
                {
                    if (Slots[i,j] == IndexF)
                    {
                        TetrominoSlots[i, j] = null;
                        Slots[i, j] = 0;
                        FreeSlots++;
                        SlotsSprites[i, j].ChangeImage(SlotImg);
                    }
                }
            }
        }
        public void ResetItem()
        {
            if (ItemInUse != null)
            {
                Tutor.ChangeImage(FTR.Properties.Resources.Empty);
                TetrominoCollection[ItemInUse.Shape].Add(new Tetromino(ItemInUse.Shape));
                UpdateCounts(ItemInUse.Shape);
            }
            global.form_menu.IsSelectionActive = false;
            ItemInUse = null;
            SelectedIndex = new Vector(-1, -1);
            global.form_menu.ResetCursor();
        }

        private void GiveTetrominos(int L, int I, int O, int J, int Z, int T, int S)
        {
            for (int i = 0; i < L; i++)
            {
                TetrominoCollection[2].Add(new Tetromino(2));
                UpdateCounts(2);
            }

            for (int i = 0; i < Z; i++)
            {
                TetrominoCollection[4].Add(new Tetromino(4));
                UpdateCounts(4);
            }

            for (int i = 0; i < T; i++)
            {
                TetrominoCollection[6].Add(new Tetromino(6));
                UpdateCounts(6);
            }

            for (int i = 0; i < I; i++)
            {
                TetrominoCollection[5].Add(new Tetromino(5));
                UpdateCounts(5);
            }

            for (int i = 0; i < S; i++)
            {
                TetrominoCollection[3].Add(new Tetromino(3));
                UpdateCounts(3);
            }

            for (int i = 0; i < J; i++)
            {
                TetrominoCollection[1].Add(new Tetromino(1));
                UpdateCounts(1);
            }

            for (int i = 0; i < O; i++)
            {
                TetrominoCollection[0].Add(new Tetromino(0));
                UpdateCounts(0);
            }
        }
        private void UpdateCounts(int Shape)
        {
            switch (Shape)
            {
                case 0:
                    ScreenCounts[1].ChangeImage(GetCountPreview(TetrominoCollection[Shape].Count));
                    return;
                case 1:
                    ScreenCounts[3].ChangeImage(GetCountPreview(TetrominoCollection[Shape].Count));
                    return;
                case 2:
                    ScreenCounts[0].ChangeImage(GetCountPreview(TetrominoCollection[Shape].Count));
                    return;
                case 3:
                    ScreenCounts[2].ChangeImage(GetCountPreview(TetrominoCollection[Shape].Count));
                    return;
                case 4:
                    ScreenCounts[4].ChangeImage(GetCountPreview(TetrominoCollection[Shape].Count));
                    return;
                case 5:
                    ScreenCounts[5].ChangeImage(GetCountPreview(TetrominoCollection[Shape].Count));
                    return;
                case 6:
                    ScreenCounts[6].ChangeImage(GetCountPreview(TetrominoCollection[Shape].Count));
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

        private void PutTetromino()
        {
            if (TryToOccupie((int)SelectedIndex.X, (int)SelectedIndex.Y, ItemInUse))
            {
                Tutor.ChangeImage(FTR.Properties.Resources.Empty);
                Cursor.Show();
                FamilyIndex++;
                for (int i = 0; i < 4; i++)
                {
                    TetrominoSlots[SlotsToTake[i, 0], SlotsToTake[i, 1]] = ItemInUse;
                    Slots[SlotsToTake[i, 0], SlotsToTake[i, 1]] = FamilyIndex;
                    Image SlotImg = (Image)(new Bitmap(TetrominoSlots[SlotsToTake[i, 0], SlotsToTake[i, 1]].GetPreviews[i], new Size((int)Math.Floor(FTR.Properties.Resources.Slot.Width * SlotsScale.X), (int)Math.Floor(FTR.Properties.Resources.Slot.Height * SlotsScale.Y))));
                    SlotsSprites[SlotsToTake[i, 0], SlotsToTake[i, 1]].ChangeImage(SlotImg);
                    SlotsSprites[SlotsToTake[i, 0], SlotsToTake[i, 1]].ChangeBrightness(-20);
                }
                FreeSlots -= 4;
                SelectedIndex = new Vector(-1, -1);
                global.form_menu.ResetCursor();
                ItemInUse = null;
                if (FreeSlots == 0)
                    GameOver();
            }
        }
        private void GameOver()
        {
            Image Bck = Level.BackgroundImg;
            global.form_menu.ResetCursor();
            Clear();
            GC.SuppressFinalize(this);
            global.form_menu.LoadLevel(new WinScreen(Bck, ShapesCount, SHeight, SWidth, MoonCoords));
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
