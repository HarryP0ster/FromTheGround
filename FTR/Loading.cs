using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;

namespace FTR
{
    class Loading : Level
    {
        public override void LoadAssets()
        {
            BackgroundImg = FTR.Properties.Resources.DarkForestGeneric;
            Loaded = true;
        }
        public override void UpdateButtons(Form Window) { }
        public override void CheckMouse(Form1 Window, Sprite Mouse) { }
        public override void MakeBackground(Graphics g) { }
        public override void StarsState() { }
        public override void Clear() { }
        public override void ButtonsCheck(Form1 Window, Sprite sprite) { }
    }
}
