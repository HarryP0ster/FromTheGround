using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace FTR
{
    class Generator // (╯ ͠° ͟ʖ ͡°)╯┻━┻
    {
        private Tetromino[] Tetrominos = new Tetromino[0];
        private static int[,] Slots = new int[0, 0];
        private int[] Quanities = new int[0];
        private static HashSet<string> InfoCollection = new HashSet<string>();
        Random rnd = new Random();
        int _height; int _width;
        int[] Occupies = new int[] { 0, 0, 0, 0 };
        int[] OcInfo = new int[4];
        int TetrInUse = 0;
        int TetrInCheck = 0;
        bool Solved = false;
        string TestString = "";
        Image[] Backgrounds = new Image[] { FTR.Properties.Resources.DarkForest02, FTR.Properties.Resources.DarkForest03,
            FTR.Properties.Resources.DarkForest04, FTR.Properties.Resources.DarkForest05, FTR.Properties.Resources.DarkMountains};

        public Generator(int Width, int Height)
        {
            _height = Height;
            _width = Width;
            int n = (int)((float)Width * (float)Height / 4);
            OcInfo = new int[n];
            Occupies = new int[_height];
            for (int i = 0; i < _height; i++)
            {
                Occupies[i] = 0;
                TestString = TestString + _width.ToString();
            }
            Tetrominos = new Tetromino[n];
            Slots = new int[Height, Width];
            Quanities = new int[7];
        }

        public bool Generate() //Генерирует массив тетромино
        {
            int index = 0;
            for (int i = 0; i < Tetrominos.Length; i++)
            {
                index = rnd.Next(0, 7);
                Tetrominos[i] = new Tetromino(index);
                Quanities[index]++;
            }
            IsSolvable(_width, _height);
            if (Solved)
            {
                global.form_menu.LoadLevel(new Game(Quanities[2], Quanities[5], Quanities[0], Quanities[1], Quanities[4], Quanities[6], Quanities[3], _width, _height, Backgrounds[rnd.Next(0,5)]));
                return true;
            }
            else
            {
                Console.WriteLine("Failed with L = " + Quanities[2].ToString() + "| I = " + Quanities[5].ToString() + "| O = " + Quanities[0].ToString() + "| J =  " + Quanities[1].ToString() + "| Z = " + Quanities[4].ToString() + "| T = " + Quanities[6].ToString() + "| S = " + Quanities[3].ToString());
            }
            return false;
        }
        public bool Generate(int[] Quanity) //Задает массив тетромино
        {
            Quanity.CopyTo(Quanities,0);
            int index = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < Quanities[i]; j++)
                {
                    Tetrominos[index] = new Tetromino(i);
                    index++;
                }
            }
            IsSolvable(_width, _height);
            if (Solved)
            {
                global.form_menu.LoadLevel(new Game(Quanities[2], Quanities[5], Quanities[0], Quanities[1], Quanities[4], Quanities[6], Quanities[3], _width, _height, Properties.Resources.DarkForest03));
                return true;
            }
            else
            {
                Console.WriteLine("Failed with L = " + Quanities[2].ToString() + "| I = " + Quanities[5].ToString() + "| O = " + Quanities[0].ToString() + "| J =  " + Quanities[1].ToString() + "| Z = " + Quanities[4].ToString() + "| T = " + Quanities[6].ToString() + "| S = " + Quanities[3].ToString());
            }
            return false;
        }
        private bool IsSolvable(int Width, int Height) //Начинает проверку
        {
            InfoCollection.Clear();
            BruteForce(0, Occupies, OcInfo);
            return true;
        }

        private void BruteForce(int _index, int[] Occupied, int[] TetrInfo) //Очень жадный алгоритм, проверяет все возможные решения
        {
            int[] TempInfo = TetrInfo;
            for (int k = 0; k < 4; k++) //Все повороты
            {
                if (Solved) break;
                for (int i = 0; i < Occupied.Length; i++) //Все позиции
                {
                    bool Check = true;
                    if (Solved) break;
                    int[] TempOccupied = new int[Occupied.Length];
                    Occupied.CopyTo(TempOccupied, 0);
                    try
                    {
                        if (i + Tetrominos[_index].OcSlots.Length <= _height)
                        {
                            for (int j = 0; j < Tetrominos[_index].OcSlots.Length; j++)
                            {
                                TempOccupied[i + j] += Tetrominos[_index].OcSlots[j];
                                if (TempOccupied[i + j] > _width)
                                    Check = false;
                            }
                            TempInfo[_index] = Tetrominos[_index].Rotation;
                        }
                    }
                    catch { }
                    TetrInUse = _index;
                    if (TetrInUse < Tetrominos.Length - 1 && Check)
                    {
                        TetrInUse++;
                        BruteForce(TetrInUse, TempOccupied, TempInfo);
                    }
                    else
                    {
                        if (String.Join("", TempOccupied) == TestString && !Solved)
                            FitIn(0, Slots);
                    }
                }
                Tetrominos[_index].Rotate();
            }
        }

        private void ResetTetromino(int Width, int Height, int _index, int[,] CSlots)
        {
            for (int k = 0; k < Height; k++)
            {
                for (int l = 0; l < Width; l++)
                {
                    if (CSlots[k, l] == _index + 1)
                    {
                        CSlots[k, l] = 0;
                    }
                }
            }
        }

        private void FitIn(int _index, int [,] OCSlots) //Вызывается если есть достойный кандидат, раньше мы работали с колличеством ячеек, теперь мы решаем пазл на плоскости
        {
            int[,] TempSlots = new int[_height, _width];
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    TempSlots[i, j] = OCSlots[i, j];
                }
            }

            for (int i = 0; i < _height; i++)
            {
                if (Solved) break;
                for (int j = 0; j < _width; j++)
                {
                    if (Solved) break;
                    TetrInCheck = _index;
                    try
                    {
                        int[] Point1 = new int[] { (Tetrominos[_index].Cosinus * Tetrominos[_index].Coords[0, 1] + Tetrominos[_index].Sinus * Tetrominos[_index].Coords[0, 0]) + i, (Tetrominos[_index].Cosinus * Tetrominos[_index].Coords[0, 0] - Tetrominos[_index].Sinus * Tetrominos[_index].Coords[0, 1]) + j }; //Ничего страшного
                        int[] Point2 = new int[] { (Tetrominos[_index].Cosinus * Tetrominos[_index].Coords[1, 1] + Tetrominos[_index].Sinus * Tetrominos[_index].Coords[1, 0]) + i, (Tetrominos[_index].Cosinus * Tetrominos[_index].Coords[1, 0] - Tetrominos[_index].Sinus * Tetrominos[_index].Coords[1, 1]) + j }; //Просто координаты
                        int[] Point3 = new int[] { (Tetrominos[_index].Cosinus * Tetrominos[_index].Coords[2, 1] + Tetrominos[_index].Sinus * Tetrominos[_index].Coords[2, 0]) + i, (Tetrominos[_index].Cosinus * Tetrominos[_index].Coords[2, 0] - Tetrominos[_index].Sinus * Tetrominos[_index].Coords[2, 1]) + j }; //Которые учитывают
                        int[] Point4 = new int[] { (Tetrominos[_index].Cosinus * Tetrominos[_index].Coords[3, 1] + Tetrominos[_index].Sinus * Tetrominos[_index].Coords[3, 0]) + i, (Tetrominos[_index].Cosinus * Tetrominos[_index].Coords[3, 0] - Tetrominos[_index].Sinus * Tetrominos[_index].Coords[3, 1]) + j }; //Поворот тетромино
                        if (TempSlots[Point1[0], Point1[1]] == 0
                            && TempSlots[Point2[0], Point2[1]] == 0
                            && TempSlots[Point3[0], Point3[1]] == 0
                            && TempSlots[Point4[0], Point4[1]] == 0)
                        {
                            TempSlots[Point1[0], Point1[1]] = _index + 1;
                            TempSlots[Point2[0], Point2[1]] = _index + 1;
                            TempSlots[Point3[0], Point3[1]] = _index + 1;
                            TempSlots[Point4[0], Point4[1]] = _index + 1;
                            if (TetrInCheck < Tetrominos.Length - 1)
                            {
                                TetrInCheck++;
                                FitIn(TetrInCheck, TempSlots);
                            }
                            else
                            {
                                CheckSolution(TempSlots);
                            }
                        }
                    }
                    catch
                    { }
                    ResetTetromino(_width,_height,_index,TempSlots);
                }
            }
        }

        private bool CheckSolution(int[,] CSlots) //Финальная проверка
        {
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    if (CSlots[i, j] == 0)
                        return false;
                }
            }
            Solved = true;
            Console.Write("\n"); //Печатает решение
            for (int i = 0; i < _height; i++)
            {
                for (int j = 0; j < _width; j++)
                {
                    Console.Write(CSlots[i,Math.Abs(j - _width + 1)].ToString());
                }
                Console.Write("\n");
            }
            Console.WriteLine("Solved");
            return true;
        }
    }
}
