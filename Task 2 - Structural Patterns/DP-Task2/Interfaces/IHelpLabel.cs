namespace DP_Task2.Interfaces
{
    public interface IHelpLabel : ILabel
    {
        string HelpText { get; set; }

        public ILabel Label { get; }
    }
}
