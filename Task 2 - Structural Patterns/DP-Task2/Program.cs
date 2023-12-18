using DP_Task2.Labels;

namespace DP_Task2
{
    public class Program
    {
        static void Main(string[] args)
        {
            SimpleLabel simp = new SimpleLabel("abc");
            string myLabel = simp.Text;
            myLabel = myLabel.ToUpper();
            Console.WriteLine(myLabel);
            Console.WriteLine(simp.Text);
            Console.WriteLine();
            LabelPrinter.PrintLabel(simp);
        }

    }
}
