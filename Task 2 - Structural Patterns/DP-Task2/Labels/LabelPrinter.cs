using DP_Task2.Interfaces;

namespace DP_Task2.Labels
{
    public class LabelPrinter
    {
        public static void PrintLabel(ILabel label)
        {
            Console.WriteLine(label.Text);
        }

        public static void PrintLabelWithHelpText(IHelpLabel helpLabel)
        {
            PrintLabel(helpLabel);
            Console.WriteLine("Some help information about this label: " +
                helpLabel.HelpText);
        }
    }
}
