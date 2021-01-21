using Flipsider.Engine.Input;
using Microsoft.Xna.Framework.Input;


namespace Flipsider
{
    // TODO dude.
#nullable disable
    public class RegisterControls
    {
        public static void Invoke()
        {
            GameInput.Instance.RegisterControl("MoveLeft", Keys.A, Buttons.LeftThumbstickLeft);
            GameInput.Instance.RegisterControl("MoveRight", Keys.D, Buttons.LeftThumbstickRight);
            GameInput.Instance.RegisterControl("MoveUp", Keys.W, Buttons.LeftThumbstickUp);
            GameInput.Instance.RegisterControl("MoveDown", Keys.S, Buttons.LeftThumbstickDown);
            GameInput.Instance.RegisterControl("Jump", Keys.Space, Buttons.A);
            GameInput.Instance.RegisterControl("NPCEditor", Keys.N, Buttons.DPadUp);
            GameInput.Instance.RegisterControl("EditorPlaceTile", MouseInput.Left, Buttons.RightTrigger);
            GameInput.Instance.RegisterControl("EdtiorRemoveTile", MouseInput.Right, Buttons.RightShoulder);
            GameInput.Instance.RegisterControl("EditorSwitchModes", Keys.Z, Buttons.RightStick);
            GameInput.Instance.RegisterControl("EditorTileEditor", Keys.T, Buttons.LeftStick);
            GameInput.Instance.RegisterControl("EditorZoomIn", MouseInput.ScrollUp, Buttons.DPadUp);
            GameInput.Instance.RegisterControl("EditorZoomOut", MouseInput.ScrollDown, Buttons.DPadDown);
            GameInput.Instance.RegisterControl("WorldSaverMode", Keys.OemSemicolon, Buttons.DPadRight);
            GameInput.Instance.RegisterControl("PropEditorMode", Keys.OemPeriod, Buttons.LeftTrigger);
            GameInput.Instance.RegisterControl("LightEditorMode", Keys.L, Buttons.LeftShoulder);
            GameInput.Instance.RegisterControl("InvEditorMode", Keys.I, Buttons.BigButton);
            GameInput.Instance.RegisterControl("WaterEditorMode", Keys.Tab, Buttons.LeftShoulder);
        }
    }
}
