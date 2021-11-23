using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace FlipEngine
{
    public class GameInput : IAlwaysUpdate
    {
        public static GameInput Instance;

        static GameInput()
        {
            Instance = new GameInput();
            FlipE.AlwaysUpdate.Add(Instance);
        }

        private readonly Dictionary<string, InputBinding> _controls;

        public bool IsClicking => CurrentMouseState.LeftButton == ButtonState.Pressed;
        public bool PreviousIsClicking => PreviousMouseState.LeftButton == ButtonState.Pressed;

        public bool IsRightClicking => CurrentMouseState.RightButton == ButtonState.Pressed;
        public bool PreviousRightIsClicking => PreviousMouseState.RightButton == ButtonState.Pressed;

        public bool JustClickingLeft => IsClicking && !PreviousIsClicking;
        public bool JustClickingRight => IsRightClicking && !PreviousRightIsClicking;

        public bool JustReleasedLeft => !IsClicking && PreviousIsClicking;
        public bool JustReleaseRight => !IsRightClicking && PreviousRightIsClicking;

        public KeyboardState PreviousKeyState { get; private set; }
        public KeyboardState CurrentKeyState { get; private set; }
        public MouseState PreviousMouseState { get; private set; }
        public MouseState CurrentMouseState { get; private set; }
        public GamePadState PreviousControllerState { get; private set; }
        public GamePadState CurrentControllerState { get; private set; }
        public Rectangle WorldDragArea { get; set; }
        public Rectangle ScreenDragArea { get; set; }

        public int DeltaScroll => CurrentMouseState.ScrollWheelValue - PreviousMouseState.ScrollWheelValue;
        public int DeltaScrollHorizontal => CurrentMouseState.HorizontalScrollWheelValue - PreviousMouseState.HorizontalScrollWheelValue;

        public Vector2 MousePosition => new Vector2(CurrentMouseState.X, CurrentMouseState.Y);
        public Vector2 LastPosition => new Vector2(PreviousMouseState.X, PreviousMouseState.Y);
        public Vector2 DeltaMousePosition => MousePosition - LastPosition;
        
        public bool UsingController => CurrentControllerState.IsConnected;

        public Point InitialScreenPoint;
        public Point InitialWorldPoint;

        public GameInput()
        {
            _controls = new Dictionary<string, InputBinding>();
        }

        private void UpdateDragArea()
        {
            Point world = FlipGame.MouseToDestination();
            Point screen = Mouse.GetState().Position;

            if (JustClickingLeft)
            {
                InitialWorldPoint = world;
                InitialScreenPoint = screen;
            }
            
            if(IsClicking)
            {
                //Inneficient, but its 3am :P
                //TODO: Get this the fuck out of here before its too late

                int L = InitialScreenPoint.X < screen.X ? InitialScreenPoint.X : screen.X;
                int R = InitialScreenPoint.X < screen.X ? screen.X : InitialScreenPoint.X;
                int U = InitialScreenPoint.Y < screen.Y ? InitialScreenPoint.Y : screen.Y;
                int D = InitialScreenPoint.Y < screen.Y ? screen.Y : InitialScreenPoint.Y;

                ScreenDragArea = new Rectangle(L, U, R - L, D - U);

                int LS = InitialWorldPoint.X < world.X ? InitialWorldPoint.X : world.X;
                int RS = InitialWorldPoint.X < world.X ? world.X : InitialWorldPoint.X;
                int US = InitialWorldPoint.Y < world.Y ? InitialWorldPoint.Y : world.Y;
                int DS = InitialWorldPoint.Y < world.Y ? world.Y : InitialWorldPoint.Y;

                WorldDragArea = new Rectangle(LS, US, RS - LS, DS - US);
            }
        }

        /// <summary>
        /// Make sure this is called as early as possible in the Game's main Update method.
        /// </summary>
        public void Update()
        {
            PreviousKeyState = CurrentKeyState;
            CurrentKeyState = Keyboard.GetState();

            PreviousMouseState = CurrentMouseState;
            CurrentMouseState = Mouse.GetState();

            // Current controller implementation only checks for the first controller.
            // Local multiplayer implementation required? Assuming not for now.
            PreviousControllerState = CurrentControllerState;
            CurrentControllerState = GamePad.GetState(PlayerIndex.One);

            UpdateDragArea();
        }

        public void RegisterControl(string name, Keys defaultKeyboard, Buttons defaultController)
        {
            _controls[name] = new InputBinding(defaultKeyboard, defaultController);
        }

        public void RegisterControl(string name, MouseInput defaultMouse, Buttons defaultController)
        {
            _controls[name] = new InputBinding(defaultMouse, defaultController);
        }

        public InputBinding this[string name] => _controls[name];
    }
}
