using DP_Task2.Interfaces;
using DP_Task2.LabelDecorators;
using DP_Task2.Labels;
using DP_Task2.Transformations;

namespace DP_Task2
{
    public class Program
    {
        static void Main(string[] args)
        {
            const string MISSING_HELP_MESSAGE = HelpLabel.MISSING_HELP_MESSAGE;
            const string EXAMPLE_TEXT = "abc";
            const string EXAMPLE_HELP_MESSAGE = "Super useful help message!";

            ILabel label = new SimpleLabel(EXAMPLE_TEXT);
            IHelpLabel helpLabel = new HelpLabel(label, EXAMPLE_HELP_MESSAGE);
            LabelPrinter.PrintLabelWithHelpText(helpLabel);
            Console.WriteLine();

            const string EXPECTED_DECORATED_RESULT = "-={ abc }=-";
            ILabel decoratedContentWithoutHelpLabel = new TextTransformationDecorator(label, new DecorationTransformation());
            LabelPrinter.PrintLabel(decoratedContentWithoutHelpLabel);
            //LabelPrinter.PrintLabelWithHelpText(decoratedContentWithoutHelpLabel);
            Console.WriteLine();

            ILabel decoratedContentWithoutAccessToHelpLabel = new TextTransformationDecorator(helpLabel, new DecorationTransformation());
            LabelPrinter.PrintLabel(decoratedContentWithoutAccessToHelpLabel);
            //LabelPrinter.PrintLabelWithHelpText(decoratedContentWithoutAccessToHelpLabel);
            Console.WriteLine();

            IHelpLabel deroratedContentWithHelpLabel = new TextTransformationDecorator(helpLabel, new DecorationTransformation());
            LabelPrinter.PrintLabelWithHelpText(deroratedContentWithHelpLabel);
            Console.WriteLine();

            IHelpLabel decoratedContentWithoutHelpMarkedAsHelpLabel = new TextTransformationDecorator(label, new DecorationTransformation());
            LabelPrinter.PrintLabelWithHelpText(decoratedContentWithoutHelpMarkedAsHelpLabel);
            Console.WriteLine();
            // add a help message in the future
            decoratedContentWithoutHelpMarkedAsHelpLabel.HelpText = EXAMPLE_HELP_MESSAGE;
            LabelPrinter.PrintLabelWithHelpText(decoratedContentWithoutHelpMarkedAsHelpLabel);
        }

    }
}
