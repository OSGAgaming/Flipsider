using System;

namespace TextureCacheGenerator.Generator.Components.Brackets
{
    public enum BracketType
    {
        // probably never gonna be used
        // ( )
        RoundParenthesis,

        // probably never gonna be used
        // [ ]
        SquareBox,

        // { }
        CurlyBrace,

        // probably never gonna be used
        // < >
        AngleChevron
    }

    public enum BracketDirection
    {
        Opening,
        Closing
    }

    /// <summary>
    ///     Component for adding a bracket.
    /// </summary>
    public class BracketComponent : ICodeComponent
    {
        public BracketType Bracket { get; }

        public BracketDirection Direction { get; }

        public BracketComponent(BracketType bracket, BracketDirection direction)
        {
            Bracket = bracket;
            Direction = direction;
        }

        public virtual char GetBracket()
        {
            return Bracket switch
            {
                BracketType.RoundParenthesis => Direction switch
                {
                    BracketDirection.Opening => '(',
                    BracketDirection.Closing => ')',
                    _ => throw new ArgumentOutOfRangeException()
                },
                BracketType.SquareBox => Direction switch
                {
                    BracketDirection.Opening => '[',
                    BracketDirection.Closing => ']',
                    _ => throw new ArgumentOutOfRangeException()
                },
                BracketType.CurlyBrace => Direction switch
                {
                    BracketDirection.Opening => '{',
                    BracketDirection.Closing => '}',
                    _ => throw new ArgumentOutOfRangeException()
                },
                BracketType.AngleChevron => Direction switch
                {
                    BracketDirection.Opening => '<',
                    BracketDirection.Closing => '>',
                    _ => throw new ArgumentOutOfRangeException()
                },
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        public string SerializeComponent() => GetBracket().ToString();
    }
}