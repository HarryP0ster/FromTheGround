using System;
using System.Linq;
using System.Drawing;

namespace FTR
{
    class Generator // (╯ ͠° ͟ʖ ͡°)╯┻━┻
    {
        private Tetromino[] Tetrominos = new Tetromino[0];
        private int[] Quanities = new int[0];
        Random rnd = new Random();
        int _height; int _width;
        int TetrInCheck = 0;
        Image[] Backgrounds = new Image[] { FTR.Properties.Resources.DarkForest02, FTR.Properties.Resources.DarkForest03,
            FTR.Properties.Resources.DarkForest04, FTR.Properties.Resources.DarkForest05, FTR.Properties.Resources.DarkMountains};
        int[,] Slots; int attempt = 0;

        public Generator(int Width, int Height)
        {
            _height = Height;
            _width = Width;
            Tetrominos = new Tetromino[(int)((float)_width * (float)_height / 4)];
            Quanities = new int[7];
            Slots = new int[_height, _width];
        }

        public bool Generate() //Генерирует массив тетромино
        {
            int index;
            for (int i = 0; i < Tetrominos.Length; i++)
            {
                index = rnd.Next(0, 7);
                Tetrominos[i] = new Tetromino(index, true);
                Quanities[index]++;
            }
            bool Solved = FitIn(0);
            if (Solved)
            {
                Console.Write("\n"); //Печатает решение
                for (int p = 0; p < _height; p++)
                {
                    for (int l = 0; l < _width; l++)
                    {
                        Console.Write(Slots[p, _width - l - 1]);
                    }
                    Console.Write("\n");
                }
                global.form_menu.LoadLevel(new Game(Quanities[2], Quanities[5], Quanities[0], Quanities[1], Quanities[4], Quanities[6], Quanities[3], _width, _height, Backgrounds[rnd.Next(0, 5)]));
            }
            else
            {
                Console.WriteLine("Failed with L = " + Quanities[2].ToString() + "| I = " + Quanities[5].ToString() + "| O = " + Quanities[0].ToString() + "| J =  " + Quanities[1].ToString() + "| Z = " + Quanities[4].ToString() + "| T = " + Quanities[6].ToString() + "| S = " + Quanities[3].ToString());
            }
            return Solved;
        }

        public bool Generate(int[] Quanity) //Задает массив тетромино
        {
            Quanity.CopyTo(Quanities, 0);
            int index = 0;
            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < Quanities[i]; j++)
                {
                    Tetrominos[index] = new Tetromino(i, true);
                    index++;
                }
            }
            bool Solved = FitIn(0);
            if (Solved)
            {
                Console.Write("\n"); //Печатает решение
                for (int p = 0; p < _height; p++)
                {
                    for (int l = 0; l < _width; l++)
                    {
                        Console.Write(Slots[p, _width - l - 1]);
                    }
                    Console.Write("\n");
                }
            }
            return Solved;
        }

        private bool FitIn(int _index) //Жадный алгоритм
        {
            for (int k = 0; k < Tetrominos[_index].GetMaxRot; k++) //Все повороты
            {
                for (int i = 0; i < _height - Tetrominos[_index].ColLength; i++)
                {
                    for (int j = 0; j < _width - Tetrominos[_index].RowLength; j++) //Все позиции - i,j
                    {
                        try
                        {
                            attempt++;
                            if (Slots[i, j] != 0) continue;
                            int[] Point1 = new int[] { (Tetrominos[_index].CoordsList.ElementAt(k))[0, 0] + i, (Tetrominos[_index].CoordsList.ElementAt(k))[0, 1] + j }; //Ничего страшного
                            int[] Point2 = new int[] { (Tetrominos[_index].CoordsList.ElementAt(k))[1, 0] + i, (Tetrominos[_index].CoordsList.ElementAt(k))[1, 1] + j }; //Просто координаты
                            int[] Point3 = new int[] { (Tetrominos[_index].CoordsList.ElementAt(k))[2, 0] + i, (Tetrominos[_index].CoordsList.ElementAt(k))[2, 1] + j }; //Которые учитывают
                            int[] Point4 = new int[] { (Tetrominos[_index].CoordsList.ElementAt(k))[3, 0] + i, (Tetrominos[_index].CoordsList.ElementAt(k))[3, 1] + j }; //Поворот тетромино
                            if (Slots[Point1[0], Point1[1]] == 0
                                && Slots[Point2[0], Point2[1]] == 0
                                && Slots[Point3[0], Point3[1]] == 0
                                && Slots[Point4[0], Point4[1]] == 0)
                            {
                                TetrInCheck = _index;
                                Slots[Point1[0], Point1[1]] = _index + 1;
                                Slots[Point2[0], Point2[1]] = _index + 1;
                                Slots[Point3[0], Point3[1]] = _index + 1;
                                Slots[Point4[0], Point4[1]] = _index + 1;
                                if (TetrInCheck < Tetrominos.Length - 1)
                                {
                                    TetrInCheck++;
                                    if (FitIn(TetrInCheck)) return true;
                                }
                                else
                                {
                                    return true;
                                }
                                Slots[Point1[0], Point1[1]] = 0;
                                Slots[Point2[0], Point2[1]] = 0;
                                Slots[Point3[0], Point3[1]] = 0;
                                Slots[Point4[0], Point4[1]] = 0;
                            }
                        }
                        catch
                        {
                        }
                    }
                }
            }
            return false;
        }
    }
}