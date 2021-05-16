using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;

namespace FTR
{
    public abstract class Level //Класс отвечающий за уровни
    {
        internal static List<Sprite> AllSprites = new List<Sprite>(); //Список всех объектов уровня
        internal static List<Sprite> Buttons = new List<Sprite>(); //Список активных кнопок на уровне
        internal static List<Sprite> ScalableSlots = new List<Sprite>(); //Игровое поле
        public bool Loaded = false;

        public static Image BackgroundImg; //Задник
        public abstract void LoadAssets(); //Отвечает за загрузку всего нужного в уровень
        public abstract void UpdateButtons(Form Window); //Меняет положение кнопок при смене размера окна, хотя оно и нужно и только в меню, раньше планировалось изменение размеров экрана откуда угодно
        public abstract void CheckMouse(Form1 Window, Sprite Mouse); //Где сейчас курсор
        public abstract void MakeBackground(Graphics g); //Добавляет звезды
        public abstract void StarsState(); //Обновляет звезды, создает мерцание
        public abstract void Clear(); //Удаляет уровень
        public abstract void ButtonsCheck(Form1 Window, Sprite sprite); //Проверка нажатия на кнопки
    }
}
