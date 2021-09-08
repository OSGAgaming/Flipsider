
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using static FlipEngine.NPC;

namespace FlipEngine
{
    internal class NPCScreen : ModeScreen
    {
        private NPCPreviewPanel[]? npcPanel;

        public static string? CurrentProp;
        public static bool CanPlace;
        public override Mode Mode => Mode.NPC;

        public override int PreviewHeight
        {
            get
            {
                if (npcPanel != null)
                    return (npcPanel.Length / 3) * 70;
                else return 0;
            }
        }
        public override void CustomDrawToScreen()
        {
            Texture2D? icon = SelectedNPCType?.GetField("icon")?.GetValue(null) as Texture2D;

            if (SelectedNPCType != null && icon != null)
            {
                Main.spriteBatch.Draw(icon, Main.MouseToDestination().ToVector2(), icon?.Bounds, Color.White * Math.Abs(Time.SineTime(3f)), 0f, (icon ?? TextureCache.BackBicep).TextureCenter(), 1f, SpriteEffects.None, 0f);
            }
        }
        public override void DrawToSelector(SpriteBatch sb)
        {
            if (npcPanel != null)
            {
                for (int i = 0; i < npcPanel.Length; i++)
                {
                    npcPanel[i].Draw(sb);
                    npcPanel[i].Update();
                }
            }
        }

        public override void CustomUpdate()
        {
            if (GameInput.Instance.JustClickingLeft && SelectedNPCType != null)
            {
                SpawnNPC(Main.MouseToDestination().ToVector2(), SelectedNPCType);
            }

            CanPlace = true;
        }
        protected override void OnLoad()
        {
            npcPanel = new NPCPreviewPanel[NPCTypes.Length];
            if (npcPanel.Length != 0)
            {
                for (int i = 0; i < npcPanel.Length; i++)
                {
                    npcPanel[i] = new NPCPreviewPanel(NPCTypes[i], EditorModeGUI.ModePreview);
                    npcPanel[i].Index = i;
                }
            }
        }

        internal override void OnDrawToScreenDirect() { }
    }

    internal class NPCPreviewPanel : PreviewElement
    {
        public NPCInfo Info;
        public int Index;
        private Texture2D? tex;
        public NPCPreviewPanel(NPCInfo Info, ScrollPanel p) : base(p)
        {
            this.Info = Info;
            tex ??= Info.type.GetField("icon")?.GetValue(null) as Texture2D;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (PreviewPanel != null && tex != null)
            {
                RelativeDimensions = new Rectangle(10 + (Index % 3) * 55, 20 + (Index / 3) * 70, 32, 32);

                if(SelectedNPCType == Info.type)
                {
                    Utils.DrawRectangle(RelativeDimensions, Color.Yellow * Time.SineTime(6f));
                }
                spriteBatch.Draw(tex, RelativeDimensions, Color.White);
            }
        }

        protected override void OnLeftClick()
        {
            SelectedNPCType = Info.type;
            Logger.NewText(Info.type.Name);
        }

        protected override void OnHover()
        {
            Logger.NewText(Info.type.Name);

        }
        protected override void CustomUpdate()
        {

        }
    }
}


