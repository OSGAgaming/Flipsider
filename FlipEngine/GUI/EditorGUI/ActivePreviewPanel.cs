
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace FlipEngine
{
    struct FieldPreviewInfo
    {
        public NumberBoxScalableScroll UI;
        public string? Name;
    }
    internal class ActiveModeSelectPreview : ScrollPanel
    {
        public override Point v => new Point(20, 32);

        public override Point PreivewDimensions => new Point(160, FlipGame.Renderer.Destination.Height);

        public override Color Color => Color.Black;

        protected override void CustomDrawDirect(SpriteBatch sb)
        {
            if (EditorModeGUI.ModeScreens.ContainsKey(EditorModeGUI.mode)) EditorModeGUI.GetActiveScreen().DrawToSelector(sb);
        }

        protected override void OnUpdate()
        {
            if (EditorModeGUI.ModeScreens.ContainsKey(EditorModeGUI.mode))
            {
                PreviewAlpha = PreviewAlpha.ReciprocateTo(1);
                if (Active)
                {
                    PreviewHeight = EditorModeGUI.GetActiveScreen().PreviewHeight;
                    PreviewWidth = EditorModeGUI.GetActiveScreen().PreviewWidth;
                }
            }
            else
            {
                PreviewAlpha = PreviewAlpha.ReciprocateTo(0);
            }
        }
    }

    internal class BottomModeSelectPreview : ScrollPanel
    {
        public override Point v => new Point(FlipGame.Renderer.Destination.Left, FlipGame.Renderer.Destination.Bottom);

        public override Point PreivewDimensions => new Point(FlipGame.Renderer.Destination.Width, (int)FlipGame.ActualScreenSize.Y - FlipGame.Renderer.Destination.Bottom);

        public override Color Color => new Color(20, 20, 20);

        public static LayerScreen? Screen;

        protected override void CustomDrawDirect(SpriteBatch sb)
        {
            if (Screen != null)
            {
                PreviewHeight = Screen.PreviewHeight;
                PreviewWidth = Screen.PreviewWidth;
            }

            Screen?.DrawToBottomPanel(sb);
            if (EditorModeGUI.ModeScreens.ContainsKey(EditorModeGUI.mode)) EditorModeGUI.GetActiveScreen().DrawToBottomPanel(sb);
        }
    }

    internal class BottomLeftModeSelectPreview : ScrollPanel
    {
        public override Point v => new Point(20, FlipGame.Renderer.Destination.Bottom);

        public override Point PreivewDimensions => new Point(160, (int)FlipGame.ActualScreenSize.Y - FlipGame.Renderer.Destination.Bottom);

        public override Color Color => new Color(20, 20, 20);

        private Entity? EntityPreview;
        private ButtonScroll? Switch;

        public bool IsLeftMode;
        private List<FieldPreviewInfo> Numbers = new List<FieldPreviewInfo>();

        public BottomLeftModeSelectPreview()
        {
            Switch = new ButtonScroll();
            Switch.OnClick = () =>
            {
                IsLeftMode = !IsLeftMode;
            };
            Switch.Texture = FlipTextureCache.TileGUIPanels;
            Switch.RelativeDimensions = new Rectangle(0, 0, 15, 15);
            Switch.ScrollParent = this;
        }

        private void RefreshReflectionFields(Entity entity)
        {
            Numbers.Clear();

            List<MemberInfo> Info = new List<MemberInfo>();

            MemberInfo[]? fieldInfo = entity?.GetType()?.GetFields();
            MemberInfo[]? propertyInfo = entity?.GetType()?.GetProperties();

            if(fieldInfo != null) Info.AddRange(fieldInfo);
            if(propertyInfo != null) Info.AddRange(propertyInfo);

            int fieldCount = 0;
            if (Info != null)
            {
                foreach (MemberInfo field in Info)
                {
                    Type? UnderlyingType = field.GetUnderlyingType();

                    if (UnderlyingType == typeof(float) ||
                        UnderlyingType == typeof(int))
                    {
                        var Attributes = field.GetCustomAttributes(true);
                        int attributeCheck = 0;

                        foreach (Attribute attribute in Attributes)
                        {
                            if (attribute.GetType() == typeof(ExportAttribute)) attributeCheck++;
                        }

                        if (attributeCheck == 0) continue;

                        NumberBoxScalableScroll fieldBox = new NumberBoxScalableScroll();

                        MemberTypes memberTypes = field.MemberType;

                        fieldBox.OnEnterEvent = (num) =>
                        {
                            if(memberTypes == MemberTypes.Property) (field as PropertyInfo)?.SetValue(entity, (int)num);
                            if(memberTypes == MemberTypes.Field) (field as FieldInfo)?.SetValue(entity, (int)num);
                        };
                        fieldCount++;

                        string? fieldValue = "NaN";

                        if (memberTypes == MemberTypes.Property) fieldValue = (field as PropertyInfo)?.GetValue(entity)?.ToString();
                        if (memberTypes == MemberTypes.Field) fieldValue = (field as FieldInfo)?.GetValue(entity)?.ToString();

                        if (fieldValue != null)
                        {
                            fieldBox.inputText = fieldValue;
                        }
                        FieldPreviewInfo info;

                        info.UI = fieldBox;
                        info.Name = field.Name;

                        fieldBox.RelativeDimensions.Location = new Point((int)FlipE.font.MeasureString(info.Name).X + 5, 7 + fieldCount * 15);
                        fieldBox.ScrollParent = this;

                        Numbers.Add(info);
                    }
                }
            }
        }

        protected override void OnUpdate()
        {
            foreach (Chunk chunk in FlipGame.GetActiveChunks())
            {
                foreach (Entity entity in chunk.Entities)
                {
                    if (entity.CollisionFrame.Contains(FlipGame.MouseToDestination()) && GameInput.Instance.JustClickingLeft && Utils.MouseInBounds)
                    {
                        if (entity != EntityPreview)
                        {
                            RefreshReflectionFields(entity);
                        }
                        EntityPreview = entity;
                    }
                }
            }

            foreach (FieldPreviewInfo n in Numbers) n.UI.Update();
            
            Switch?.Update();
        }

        protected override void CustomDrawDirect(SpriteBatch sb)
        {
            if (IsLeftMode)
            {
                Utils.DrawTextToLeft($"Active Chunk: {TileManager.ToChunkCoords(FlipGame.Camera.Position.ToPoint())}", Color.White, new Vector2(5, 5), 0, 0.5f);
                Utils.DrawTextToLeft("Entities >", Color.White, new Vector2(5, 15), 0, 0.5f);

                int a = 0;
                foreach (Chunk chunk in FlipGame.GetActiveChunks())
                {
                    foreach (Entity entity in chunk.Entities)
                    {
                        Utils.DrawTextToLeft(entity.GetType().Name, Color.White, new Vector2(5, 25 + a * 10), 0, 0.5f);
                        a++;
                    }
                }

                PreviewHeight = a * 10 + 30;
            }
            else if (EntityPreview != null)
            {
                Utils.DrawTextToLeft($"{EntityPreview}", Color.White, new Vector2(5, 5), 0, 0.5f);

                int b = 0;

                foreach (FieldPreviewInfo n in Numbers)
                {
                    Utils.DrawTextToLeft(n.Name ?? "bitch", Color.White, new Vector2(2, 20 + b * 15));
                    n.UI.Draw(sb);
                    b++;
                }

                PreviewHeight = Numbers.Count * 15 + 20;
            }


            Switch?.Draw(sb);
        }
    }
}


