using System.Collections.Generic;
using System.Drawing;

namespace FTR
{
    class Tetromino //Невероятно, но этот класс отвечает за тетромино
    {
        private int _shape; //Форма фигуры
        public int[,] Coords = new int[4, 2]; //Координаты ячеек
        private int sin, cos;
        private Vector Offset;
        private Image Icon;
        private Vector[] Offsets;
        private int _rot = 0;
        Image[] Previews = new Image[4];
        private int PossibleRotations = 4;
        private List<int[,]> AllCoords = new List<int[,]>();
        private int Row; private int Col;

        public int Shape
        {
            get => _shape;
            set
            {
                _shape = value;
            }
        }
        public int GetMaxRot
        {
            get => PossibleRotations;
        }
        public Image[] GetPreviews
        {
            get => Previews;
        }
        public int Sinus
        {
            get => sin;
        }
        public int Cosinus
        {
            get => cos;
        }
        public Vector GetOffset
        {
            get => Offset;
        }
        public Image GetIcon
        {
            get => Icon;
        }
        public int Rotation
        {
            get => _rot;
            set { this._rot = value % 4; BuildRot(); }
        }
        public List<int[,]> CoordsList
        {
            get => AllCoords;
        }
        public int RowLength
        {
            get => Row;
        }
        public int ColLength
        {
            get => Col;
        }
        public Tetromino(int Shape)
        {
            _shape = Shape;
            sin = 0;
            cos = 1;
            GetCoords(Shape);
            Offsets = BuildOffsets(Shape);
            Offset = Offsets[3];
            Icon = GetImage();
            GetSlotsPreviews(Shape);
        }
        public Tetromino(int Shape, bool simple)
        {
            _shape = Shape;
            sin = 0;
            cos = 1;
            GetCoords(Shape, true);
        }
        private void GetCoords(int Shape) //Функция строит координаты ячеек фигуры
        {
            switch (Shape)
            {
                case 0:
                    Coords = new int[4, 2] { { 0, 0 }, { 1, 0 }, { 0, -1 }, { 1, -1 } }; //O
                    break;
                case 1:
                    Coords = new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, -1 }, { -1, -1 } }; //J
                    break;
                case 2:
                    Coords = new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, -1 }, { 1, -1 } }; //L
                    break;
                case 3:
                    Coords = new int[4, 2] { { 0, 0 }, { 1, 0 }, { 0, -1 }, { -1, -1 } }; //S
                    break;
                case 4:
                    Coords = new int[4, 2] { { 0, 0 }, { -1, 0 }, { 0, -1 }, { 1, -1 } }; //Z
                    break;
                case 5:
                    Coords = new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, -1 }, { 0, -2 } }; //I
                    break;
                case 6:
                    Coords = new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { -1, 0 } }; //T
                    break;
            }
            return;
        }
        public void Rotate(Vector Scale) //Функция поворачивает фигуры
        {
            if (cos == 1 && sin == 0)
            {
                Offset = Offsets[0];
                cos = 0; sin = 1;
                global.form_menu.GetMouse.ChangeImage(cos, sin, Icon, Scale);
                global.form_menu.GetMouse.SpriteBtm.RotateFlip(RotateFlipType.Rotate270FlipNone);
                for (int i = 0; i < 4; i++)
                    Previews[i].RotateFlip(RotateFlipType.Rotate270FlipNone);
                _rot++;
            }
            else if (cos == 0 && sin == 1)
            {
                Offset = Offsets[1];
                cos = -1; sin = 0;
                global.form_menu.GetMouse.ChangeImage(cos, sin, Icon, Scale);
                global.form_menu.GetMouse.SpriteBtm.RotateFlip(RotateFlipType.Rotate180FlipNone);
                for (int i = 0; i < 4; i++)
                    Previews[i].RotateFlip(RotateFlipType.Rotate270FlipNone);
                _rot++;
            }
            else if (cos == -1 && sin == 0)
            {
                Offset = Offsets[2];
                cos = 0; sin = -1;
                global.form_menu.GetMouse.ChangeImage(cos, sin, Icon, Scale);
                global.form_menu.GetMouse.SpriteBtm.RotateFlip(RotateFlipType.Rotate90FlipNone);
                for (int i = 0; i < 4; i++)
                    Previews[i].RotateFlip(RotateFlipType.Rotate270FlipNone);
                _rot++;
            }
            else
            {
                Offset = Offsets[3];
                cos = 1; sin = 0;
                global.form_menu.GetMouse.ChangeImage(cos, sin, Icon, Scale);
                for (int i = 0; i < 4; i++)
                    Previews[i].RotateFlip(RotateFlipType.Rotate270FlipNone);
                _rot = 0;
            }
        }
        private void BuildRot() //Конвертирует поворот в синус и косинус
        {
            if (_rot == 0)
            {
                cos = 0; sin = 1;
            }
            else if (_rot == 1)
            {
                cos = -1; sin = 0;
            }
            else if (_rot == 2)
            {
                cos = 0; sin = -1;
            }
            else
            {
                cos = 1; sin = 0;
            }
        }
        private Image GetImage() //Возвращает изображение тетромино
        {
            switch (Shape)
            {
                case 0:
                    return FTR.Properties.Resources.O;
                case 1:
                    return FTR.Properties.Resources.J;
                case 2:
                    return FTR.Properties.Resources.L;
                case 3:
                    return FTR.Properties.Resources.S;
                case 4:
                    return FTR.Properties.Resources.Z;
                case 5:
                    return FTR.Properties.Resources.I;
                case 6:
                    return FTR.Properties.Resources.T;
            }
            return FTR.Properties.Resources.Empty;
        }
        private Vector[] BuildOffsets(int Shape) //Эти координаты отвечают за то, нассколько удален рисунок от курсора при использовании тетромино
        {
            switch (Shape)
            {
                case 0:
                    return new Vector[] { new Vector(-65, -200), new Vector(-65, -200), new Vector(-200, -200), new Vector(-65, -65) };
                case 1:
                    return new Vector[] { new Vector(-200, -65), new Vector(-65, -200), new Vector(-200, -200), new Vector(-200, -200) };
                case 2:
                    return new Vector[] { new Vector(-200, -200), new Vector(-200, -200), new Vector(-200, -65), new Vector(-65, -200) };
                case 3:
                    return new Vector[] { new Vector(-65, -200), new Vector(-200, -200), new Vector(-200, -200), new Vector(-200, -65) };
                case 4:
                    return new Vector[] { new Vector(-65, -200), new Vector(-200, -200), new Vector(-200, -200), new Vector(-200, -65) };
                case 5:
                    return new Vector[] { new Vector(-200, -65), new Vector(-65, -345), new Vector(-335, -65), new Vector(-65, -200) };
                case 6:
                    return new Vector[] { new Vector(-200, -200), new Vector(-200, -65), new Vector(-65, -200), new Vector(-200, -200) };
            }
            return new Vector[0];
        }

        public void GetSlotsPreviews(int Shape)
        {
            switch (Shape)
            {
                case 0:
                    Previews[0] = FTR.Properties.Resources.SlotCornerLeftU;
                    Previews[1] = FTR.Properties.Resources.SlotCornerRightU;
                    Previews[2] = FTR.Properties.Resources.SlotCornerLeftD;
                    Previews[3] = FTR.Properties.Resources.SlotCornerRightD;
                    break;
                case 1:
                    Previews[0] = FTR.Properties.Resources.SlotTube;
                    Previews[1] = FTR.Properties.Resources.SlotClosedUp;
                    Previews[2] = FTR.Properties.Resources.SlotCornerRightDC;
                    Previews[3] = FTR.Properties.Resources.SlotClosedLeft;
                    break;
                case 2:
                    Previews[0] = FTR.Properties.Resources.SlotTube;
                    Previews[1] = FTR.Properties.Resources.SlotClosedUp;
                    Previews[2] = FTR.Properties.Resources.SlotCornerLeftDC;
                    Previews[3] = FTR.Properties.Resources.SlotClosedRight;
                    break;
                case 3:
                    Previews[0] = FTR.Properties.Resources.SlotCornerLeftUC;
                    Previews[1] = FTR.Properties.Resources.SlotClosedRight;
                    Previews[2] = FTR.Properties.Resources.SlotCornerRightDC;
                    Previews[3] = FTR.Properties.Resources.SlotClosedLeft;
                    break;
                case 4:
                    Previews[0] = FTR.Properties.Resources.SlotCornerRightUC;
                    Previews[1] = FTR.Properties.Resources.SlotClosedLeft;
                    Previews[2] = FTR.Properties.Resources.SlotCornerLeftDC;
                    Previews[3] = FTR.Properties.Resources.SlotClosedRight;
                    break;
                case 5:
                    Previews[0] = FTR.Properties.Resources.SlotTube;
                    Previews[1] = FTR.Properties.Resources.SlotClosedUp;
                    Previews[2] = FTR.Properties.Resources.SlotTube;
                    Previews[3] = FTR.Properties.Resources.SlotClosedDown;
                    break;
                case 6:
                    Previews[0] = FTR.Properties.Resources.SlotDown;
                    Previews[1] = FTR.Properties.Resources.SlotClosedUp;
                    Previews[2] = FTR.Properties.Resources.SlotClosedRight;
                    Previews[3] = FTR.Properties.Resources.SlotClosedLeft;
                    break;
            }
        }
        private void SwitchOccupied() //Координаты в упрощенном варианте для генератора
        {
            switch (Shape)
            {
                case 0: //O
                    Row = 1;
                    Col = 1;
                    Coords = new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 } }; //O
                    return;
                case 1: //J
                    switch (_rot)
                    {
                        case 0:
                            Row = 2;
                            Col = 1;
                            break;
                        case 1:
                            Row = 1;
                            Col = 2;
                            break;
                        case 2:
                            Row = 2;
                            Col = 1;
                            break;
                        case 3:
                            Row = 1;
                            Col = 2;
                            break;
                    }
                    return;
                case 2: //L
                    switch (_rot)
                    {
                        case 0:
                            Row = 1;
                            Col = 2;
                            break;
                        case 1:
                            Row = 2;
                            Col = 1;
                            break;
                        case 2:
                            Row = 1;
                            Col = 2;
                            break;
                        case 3:
                            Row = 2;
                            Col = 1;
                            break;
                    }
                    return;
                case 3: //S
                    switch (_rot)
                    {
                        case 0:
                            Row = 1;
                            Col = 2;
                            break;
                        case 1:
                            Row = 2;
                            Col = 1;
                            break;
                    }
                    return;
                case 4: //Z
                    switch (_rot)
                    {
                        case 0:
                            Row = 2;
                            Col = 1;
                            break;
                        case 1:
                            Row = 1;
                            Col = 2;
                            break;
                    }
                    return;
                case 5: //I
                    switch (_rot)
                    {
                        case 0:
                            Row = 3;
                            Col = 0;
                            break;
                        case 1:
                            Row = 0;
                            Col = 3;
                            break;
                    }
                    return;
                case 6: //T
                    switch (_rot)
                    {
                        case 0:
                            Row = 1;
                            Col = 1;
                            break;
                        case 1:
                            Row = 1;
                            Col = 1;
                            break;
                        case 2:
                            Row = 1;
                            Col = 1;
                            break;
                        case 3:
                            Row = 1;
                            Col = 1;
                            break;
                    }
                    return;
            }
        }
        private void GetCoords(int Shape, bool Simple) //Функция строит координаты ячеек фигуры
        {
            switch (Shape)
            {
                case 0:
                    PossibleRotations = 1;
                    Coords = new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 } }; //O
                    AllCoords.Add(new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 1, 1 } });
                    break;
                case 1:
                    PossibleRotations = 4;
                    Coords = new int[4, 2] { { 0, 0 }, { 1, 0 }, { 1, 1 }, { 1, 2 } }; //J
                    AllCoords.Add(new int[4, 2] { { 0, 0 }, { 1, 0 }, { 1, 1 }, { 1, 2 } });
                    AllCoords.Add(new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 0 }, { 2, 0 } });
                    AllCoords.Add(new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 2 }, { 1, 2 } });
                    AllCoords.Add(new int[4, 2] { { 0, 1 }, { 1, 1 }, { 2, 1 }, { 2, 0 } });
                    break;
                case 2:
                    PossibleRotations = 4;
                    Coords = new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 2 }, { 1, 0 } }; //L
                    AllCoords.Add(new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 2 }, { 1, 0 } });
                    AllCoords.Add(new int[4, 2] { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 2, 1 } });
                    AllCoords.Add(new int[4, 2] { { 0, 2 }, { 1, 0 }, { 1, 1 }, { 1, 2 } });
                    AllCoords.Add(new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 1 }, { 2, 1 } });
                    break;
                case 3:
                    PossibleRotations = 2;
                    Coords = new int[4, 2] { { 0, 0 }, { 1, 0 }, { 1, 1 }, { 2, 1 } }; //S
                    AllCoords.Add(new int[4, 2] { { 0, 0 }, { 1, 0 }, { 1, 1 }, { 2, 1 } });
                    AllCoords.Add(new int[4, 2] { { 1, 0 }, { 1, 1 }, { 0, 1 }, { 0, 2 } });
                    break;
                case 4:
                    PossibleRotations = 2;
                    Coords = new int[4, 2] { { 0, 1 }, { 1, 1 }, { 1, 0 }, { 2, 0 } }; //Z
                    AllCoords.Add(new int[4, 2] { { 0, 1 }, { 1, 1 }, { 1, 0 }, { 2, 0 } });
                    AllCoords.Add(new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 1 }, { 1, 2 } });
                    break;
                case 5:
                    PossibleRotations = 2;
                    Coords = new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 2 }, { 0, 3 } }; //I
                    AllCoords.Add(new int[4, 2] { { 0, 0 }, { 0, 1 }, { 0, 2 }, { 0, 3 } });
                    AllCoords.Add(new int[4, 2] { { 0, 0 }, { 1, 0 }, { 2, 0 }, { 3, 0 } });
                    break;
                case 6:
                    PossibleRotations = 4;
                    Coords = new int[4, 2] { { 0, 0 }, { 1, 0 }, { 1, 1 }, { 2, 0 } }; //T
                    AllCoords.Add(new int[4, 2] { { 0, 0 }, { 1, 0 }, { 1, 1 }, { 2, 0 } });
                    AllCoords.Add(new int[4, 2] { { 0, 1 }, { 1, 0 }, { 1, 1 }, { 1, 2 } });
                    AllCoords.Add(new int[4, 2] { { 0, 1 }, { 1, 0 }, { 1, 1 }, { 2, 1 } });
                    AllCoords.Add(new int[4, 2] { { 0, 0 }, { 0, 1 }, { 1, 1 }, { 0, 2 } });
                    break;
            }
            SwitchOccupied();
            return;
        }
    }
}
