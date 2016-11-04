namespace YeongHun.EmueraFramework.Data
{
    public class ConsoleInput
    {
        public ConsoleInputType Type { get; }
        public long IntValue { get; }
        public string StrValue { get; }

        public ConsoleInput(ConsoleInputType type)
        {
            Type = type;
        }

        public ConsoleInput(long value):this(ConsoleInputType.INTEGER)
        {
            IntValue = value;
        }

        public ConsoleInput(string value) : this(ConsoleInputType.STRING)
        {
            StrValue = value;
        }
    }

    public enum ConsoleInputType
    {
        ANYKEY,
        ENTERKEY,
        INTEGER,
        STRING
    }
}
