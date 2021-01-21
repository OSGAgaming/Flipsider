using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace Flipsider.GUI
{
    public class UIScreenManager : Manager<UIScreen>
    {
        public static UIScreenManager? Instance;
        public UIScreenManager() : base(false)
        {
        }
        public void DrawOnScreen()
        {
            foreach (UIScreen UIS in Components)
            {
                UIS.DrawToScreen();
            }
        }
        static UIScreenManager()
        {
            Instance = new UIScreenManager();
        }
    }
}
