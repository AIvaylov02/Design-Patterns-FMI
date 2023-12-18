using DP_Task2.Interfaces;

namespace DP_Task2.Labels
{
    public class LabelPrinter
    {
        public static void PrintLabel(ILabel label)
        {
            Console.WriteLine(label.Text);
        }
    }
}
