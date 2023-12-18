using DP_Task2.Interfaces;

namespace DP_Task2.Labels
{
    public class SimpleLabel : ILabel
    {
        string value;
        public SimpleLabel(string value)
        {
            this.value = value;
        }

        public string Text => value;

    }
}
