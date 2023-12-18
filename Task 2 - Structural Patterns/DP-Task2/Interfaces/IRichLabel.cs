namespace DP_Task2.Interfaces
{
    public interface IRichLabel : ILabel
    {
        public string TextColor { get; }
        public string Font { get; }
        public double FontSize { get; }
    }
}
