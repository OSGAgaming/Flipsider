
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;


namespace FlipEngine
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

        public void DrawDirectOnScreen(SpriteBatch sb)
        {
            foreach (UIScreen UIS in Components)
            {
                UIS.DrawToScreenDirect(sb);
            }
        }

        static UIScreenManager()
        {
            Instance = new UIScreenManager();
        }
    }
}
