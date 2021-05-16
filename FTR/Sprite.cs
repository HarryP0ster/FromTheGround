using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace FTR
{
    public class Sprite
    {
        public Vector Position;
        public Vector Scale = new Vector(1, 1);
        public string Tag = "";
        public Bitmap SpriteBtm = null;
        public Image PreviewImage;
        public Vector SlotIndex;
        private float Brightness = 255;

        public Sprite(Vector Position, Vector Scale, Image SpriteImg, string Tag)
        {
            this.Position = Position;
            this.Scale = Scale;
            this.Tag = Tag;
            PreviewImage = (Image)(new Bitmap(SpriteImg, new Size((int)(Math.Floor(SpriteImg.Width * Scale.X)), (int)(Math.Floor(SpriteImg.Height * Scale.Y)))));
            SpriteBtm = new Bitmap(PreviewImage);
        }
        public Sprite(Vector Position, Vector Scale, Image SpriteImg, string Tag, Vector Index)
        {
            this.Position = new Vector(Position.X, Position.Y) ;
            this.Scale = Scale;
            this.Tag = Tag;
            SlotIndex = Index;
            PreviewImage = (Image) (new Bitmap(SpriteImg, new Size((int)(Math.Floor(SpriteImg.Width * Scale.X)), (int)(Math.Floor(SpriteImg.Height * Scale.Y)))));
            SpriteBtm = new Bitmap(PreviewImage);
        }
        public float GetBrightness
        {
            get => Brightness;
        }
        public void DestroySelf()
        {
        }
        public void ChangeState(Image SpriteImg, int scale_x, int scale_y)
        {
            this.SpriteBtm = new Bitmap(SpriteImg);
            this.Scale.X = scale_x;
            this.Scale.Y = scale_y;
        }
        public void ChangePosition(Vector NewPos)
        {
            this.Position = NewPos;
        }
        public void ChangeImage(Image NewImg)
        {
            PreviewImage = (Image)(new Bitmap(NewImg, new Size((int)(Math.Floor(NewImg.Width * Scale.X) * global.ScreenScale.X), (int)(Math.Floor(NewImg.Height * Scale.Y) * global.ScreenScale.Y))));
            this.SpriteBtm = new Bitmap(PreviewImage);
        }
        public void ChangeImage(Image NewImg, Vector CustomScale)
        {
            PreviewImage = (Image)(new Bitmap(NewImg, new Size((int)(Math.Floor(NewImg.Width * CustomScale.X)), (int)(Math.Floor(NewImg.Height * CustomScale.Y)))));
            this.SpriteBtm = new Bitmap(PreviewImage);
        }

        public void ChangeImage(int cos, int sin, Image Icon, Vector Scale)
        {
            PreviewImage = (Image)(new Bitmap(Icon, new Size((int)(Math.Floor(Icon.Width * Scale.X) * (Math.Abs(global.ScreenScale.X * cos) + Math.Abs(global.ScreenScale.Y * sin))), (int)(Math.Floor(Icon.Height * Scale.Y) * (Math.Abs(global.ScreenScale.Y * cos) + Math.Abs(global.ScreenScale.X * sin))))));
            this.SpriteBtm = new Bitmap(PreviewImage);
        }
        public void ChangeBrightness(float Value)
        {
            Graphics NewGraphics = Graphics.FromImage(SpriteBtm);
            float[][] FloatColorMatrix ={

                    new float[] {1, 0, 0, 1, 1},

                    new float[] {0, 1, 0, 1, 1},

                    new float[] { 0, 0, 1, 1, 1},

                    new float[] {0, 0, 0, 1, 1},

                    new float[] { Value / 255, Value / 255, Value / 255, 1, 1}
                };
            Brightness = Value / 255;
            ImageAttributes Attributes = new ImageAttributes();
            ColorMatrix NewColorMatrix = new ColorMatrix(FloatColorMatrix);
            Attributes.SetColorMatrix(NewColorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
            NewGraphics.DrawImage(SpriteBtm, new Rectangle(0, 0, SpriteBtm.Width, SpriteBtm.Height), 0, 0, SpriteBtm.Width, SpriteBtm.Height, GraphicsUnit.Pixel, Attributes);
        }
    }
}
