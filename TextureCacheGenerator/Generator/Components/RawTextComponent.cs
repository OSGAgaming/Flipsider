namespace TextureCacheGenerator.Generator.Components
{
    public class RawTextComponent : ICodeComponent
    {
        public string Text { get; }

        public RawTextComponent(string text)
        {
            Text = text;
        }

        public string SerializeComponent() => Text;
    }
}