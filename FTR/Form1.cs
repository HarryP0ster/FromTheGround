using System;
using System.Drawing;
using System.Windows.Forms;
using System.Threading;
using System.Media;
using System.Runtime.InteropServices;


namespace FTR
{
    public partial class Form1 : Form
    {
        private int CurrentVolume = 5;
        private Level Backup;

        [DllImport("winmm.dll")]
        public static extern int waveOutGetVolume(IntPtr hwo, out uint dwVolume); //Контроль громкости

        [DllImport("winmm.dll")]
        public static extern int waveOutSetVolume(IntPtr hwo, uint dwVolume); //Контроль громкости

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();

        private Sprite Mouse;
        public Sprite Selection;
        protected Image LSText, EText, CText;
        public bool IsSelectionActive = false;
        public Sprite SpriteInFocus;
        private Thread GameLoop = null;
        int frame = 0; Cursor CustomCursor;
        private bool AnyKeyDown = false;
        private Vector CursorOffset = new Vector();
        private SoundPlayer Ambience;
        Level CurrentLevel;

        public PictureBox Map
        {
            get => pictureBox1;
        }
        public Level CurLev
        {
            get => CurrentLevel;
        }
        public Vector GetMouseOffset
        {
            get => CursorOffset;
        }
        public Cursor MainCursor
        {
            set
            {
                CustomCursor = value;
            }
        }
        public int GetVolume
        {
            get => CurrentVolume;
        }
        public Level GetBackup
        {
            get => Backup;
        }
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //AllocConsole(); //Консоль для дебагинга
            ChangeVolume(0);
            Ambience = new SoundPlayer(FTR.Properties.Resources.Cloud_Rome_Ground);
            Ambience.PlayLooping();
            Icon Cur = Properties.Resources.MouseIcon;
            this.MainCursor = new Cursor(Cur.Handle);
            this.Cursor = CustomCursor;
            global.ScreenScale = new Vector((float)pictureBox1.Width / 1920, (float)pictureBox1.Height / 1080); //Размер экрана относительно 1080p
            Mouse = new Sprite(new Vector(-1, -1), new Vector(1, 1), FTR.Properties.Resources.Mouse, "Cursor"); //Изображение курсора
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            LoadLevel(new Menu()); //Первоначальная загрузка
            this.KeyDown += Window_KeyDown;
            this.KeyUp += Window_KeyUp;
            this.Resize += Form1_Resize;
            this.LocationChanged += Form1_Resize;
            this.SetDesktopLocation(0, 0);
            this.GotFocus += CallReset;
            GameLoop = new Thread(GameThread); //Начинаем поток для графики
            GameLoop.IsBackground = true;
            GameLoop.Priority = ThreadPriority.AboveNormal;
            GameLoop.Start();
            Controls.SetChildIndex(pictureBox1, 0);
        }
        public Sprite GetMouse
        {
            get => Mouse;
        }
        private void Form1_Resize(object sender, EventArgs e) //Перестраивает приложение
        {
            this.pictureBox1.Width = this.Width;
            this.pictureBox1.Height = (int)(this.pictureBox1.Width * 0.565f);
            pictureBox1.Location = new Point(0, this.Height / 2 - pictureBox1.Height / 2);
            if (CurrentLevel != null)
                CurrentLevel.UpdateButtons(this);
            global.ScreenScale = new Vector((float)pictureBox1.Width / 1920, (float)pictureBox1.Height / 1080);
            global.MapOffset = (int)((this.Height - this.pictureBox1.Height) / 2);
        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
            if (SpriteInFocus != null)
            {
                CurrentLevel.ButtonsCheck(this, SpriteInFocus);
            }
        }
        private void CallReset(object sender, EventArgs e) //Для фикса бага, но вызывает ещё баги ¯\_(ツ)_/¯
        {
            ResetCursor();
        }

        private void GameThread() //Поток
        {
            while (GameLoop.IsAlive)
            {
                try
                {
                    frame++;
                    if (CurrentLevel != null && frame % 2 == 0) CurrentLevel.StarsState();
                    global.form_menu.Invoke((MethodInvoker)delegate { pictureBox1.Refresh(); });
                    Thread.Sleep(5); //Можно изменить для повышения производительности, но с потерей плавности (если такова имеется на отдельно взятой машине)
                }
                catch
                {

                }
            }
        }

        public void SuspendRender()
        {
            GameLoop.Abort();
        }
        public void ResumeRender()
        {
            GameLoop = null;
            GC.Collect();
            GameLoop = new Thread(GameThread);
            GameLoop.Start();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e) //Графика
        {
            CurrentLevel.MakeBackground(e.Graphics);
            CurrentLevel.CheckMouse(this, Mouse);
            foreach (Sprite sprite in Level.AllSprites)
            {
                e.Graphics.DrawImage(sprite.SpriteBtm, sprite.Position.X, sprite.Position.Y);
            }
            if (CurrentLevel is Game && ((Game)CurrentLevel).GetItem != null)
            {
                Mouse.Position = new Vector(Cursor.Position.X + CursorOffset.X, Cursor.Position.Y - global.MapOffset + CursorOffset.Y);
                e.Graphics.DrawImage(Mouse.SpriteBtm, Mouse.Position.X, Mouse.Position.Y);
            }
        }
        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            GameLoop.Abort();
        }

        private void Window_KeyDown(object sender, KeyEventArgs key)
        {
            if (key.KeyCode == Keys.R && CurrentLevel is Game && !AnyKeyDown)
                ((Game)CurrentLevel).Rotate();
            if (key.KeyCode == Keys.G && CurrentLevel is Game && !AnyKeyDown)
                ((Game)CurrentLevel).ResetItem();
            else if (key.KeyCode == Keys.D1 && CurrentLevel is Game && !AnyKeyDown)
                ((Game)CurrentLevel).TakeTetromino(2);
            else if (key.KeyCode == Keys.D2 && CurrentLevel is Game && !AnyKeyDown)
                ((Game)CurrentLevel).TakeTetromino(0);
            else if (key.KeyCode == Keys.D3 && CurrentLevel is Game && !AnyKeyDown)
                ((Game)CurrentLevel).TakeTetromino(3);
            else if (key.KeyCode == Keys.D4 && CurrentLevel is Game && !AnyKeyDown)
                ((Game)CurrentLevel).TakeTetromino(1);
            else if (key.KeyCode == Keys.D5 && CurrentLevel is Game && !AnyKeyDown)
                ((Game)CurrentLevel).TakeTetromino(4);
            else if (key.KeyCode == Keys.D6 && CurrentLevel is Game && !AnyKeyDown)
                ((Game)CurrentLevel).TakeTetromino(5);
            else if (key.KeyCode == Keys.D7 && CurrentLevel is Game && !AnyKeyDown)
                ((Game)CurrentLevel).TakeTetromino(6);

            AnyKeyDown = true;
        }

        private void Window_KeyUp(object sender, KeyEventArgs key)
        {
            AnyKeyDown = false;
        }

        public void LoadLevel(Level LevelToLoad) //Вызывает загрузку заданного уровня (дочернего класса Level)
        {
            CurrentLevel = null;
            ResetCursor();
            CurrentLevel = LevelToLoad;
            Selection = new Sprite(new Vector(-1, -1), new Vector(1, 1), FTR.Properties.Resources.Empty, "Dynamics"); Level.AllSprites.Add(Selection);
            CurrentLevel.LoadAssets();
            pictureBox1.Image = Level.BackgroundImg;
            CurrentLevel.UpdateButtons(this);
            GC.Collect();
        }
        public void ChangeCursor(Image NewPreview, Vector Offset)
        {
            this.MainCursor = new Cursor(Properties.Resources.MouseDrag.Handle);
            this.Cursor = CustomCursor;
            Mouse.ChangeImage(NewPreview);
            CursorOffset = Offset;
        }
        
        public override void ResetCursor()
        {
            this.MainCursor = new Cursor(Properties.Resources.MouseIcon.Handle);
            this.Cursor = CustomCursor;
            Mouse.ChangeImage(FTR.Properties.Resources.Mouse);
            CursorOffset = new Vector();
        }
        public Vector MouseCursorOffset
        {
            set
            {
                CursorOffset = value;
            }
        }
        public void ChangeVolume(int Ammount) //Звук и его громкость
        {
            CurrentVolume += Ammount;
            if (CurrentVolume > 10) CurrentVolume = 10;
            else if (CurrentVolume < 0) CurrentVolume = 0;
            uint CurrVol = 0;
            int NewVolume;
            waveOutGetVolume(IntPtr.Zero, out CurrVol);
            NewVolume = (CurrentVolume * ushort.MaxValue) / 10;
            uint NewVolumeAllChannels = (((uint)NewVolume & 0x0000ffff) | ((uint)NewVolume << 16));
            waveOutSetVolume(IntPtr.Zero, NewVolumeAllChannels);
        }
        public void MakeLevelBackup() //Оно что-то делает \_(ツ)_/¯
        {
            Backup = CurrentLevel;
        }
        public void ChangeAmbient(System.IO.UnmanagedMemoryStream File, bool Loop) //Смена звука, но зачем?
        {
            Ambience.Dispose();
            Ambience = new SoundPlayer(File);
            if (Loop)
                Ambience.PlayLooping();
            else
                Ambience.Play();
            GC.Collect();
        }
    }
}
