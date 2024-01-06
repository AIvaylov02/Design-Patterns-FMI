using DP_Task2.Interfaces;
using DP_Task2.LabelDecorators;
using DP_Task2.Labels;
using DP_Task2.Transformations;

namespace DP_Task2_Tests
{

    [TestFixture]
    public class SimpleLabelTests
    {
        private const string EMPTY_STRING = "";
        private const string STRING_WITH_ONLY_SPACES = "          ";
        private const string NORMAL_ABC_STRING = "abc";

        [Test]
        public void Test_Label_Initilialization_EmptyString()
        {
            ILabel label = new SimpleLabel(EMPTY_STRING);
            Assert.That(label.Text == "");
        }

        [Test]
        public void Test_Label_Initilialization_StringWithOnlySpacebars()
        {

            ILabel label = new SimpleLabel(STRING_WITH_ONLY_SPACES);
            Assert.That(label.Text, Is.EqualTo(STRING_WITH_ONLY_SPACES));
        }

        [Test]
        public void Test_Label_Initialization_NormalAcceptableString()
        {

            ILabel label = new SimpleLabel(NORMAL_ABC_STRING);
            Assert.That(label.Text, Is.EqualTo(NORMAL_ABC_STRING));
            // Assert.Throws(label.Text = NORMAL_ABC_STRING); Reassigning is for now forbidden no need to test it. It leads to a compilation error
        }
    }

    [TestFixture]
    public class RichLabelTests
    {
        private const string EMPTY_STRING = "";
        private const string STRING_WITH_ONLY_SPACES = "          ";
        private const string NORMAL_ABC_STRING = "abc";

        private const string EXAMPLE_COLOR = "red";
        private const string EXAMPLE_FONT = "Arial";

        private const int DEFAULT_FONTSIZE = 1;
        private const double FONTSIZE_ZERO = 0.00001; // the allowed values are bigger than 0.0001 so the fourth digit needs to be different than 0
        private const double NEGATIVE_FONTSIZE = -0.1;

        [Test]
        public void Test_Label_Initilialization_EmptyStrings()
        {
            IRichLabel label = new RichLabel(EMPTY_STRING, EMPTY_STRING, EMPTY_STRING, DEFAULT_FONTSIZE);
            Assert.That(label.Text, Is.EqualTo(EMPTY_STRING));
            Assert.That(label.TextColor, Is.EqualTo(EMPTY_STRING));
            Assert.That(label.Font, Is.EqualTo(EMPTY_STRING));
            Assert.That(label.FontSize, Is.EqualTo(DEFAULT_FONTSIZE));
        }

        [Test]
        public void Test_Label_Initilialization_TextWithOnlySpacebars_OthersEmptyStrings()
        {
            IRichLabel label = new RichLabel(STRING_WITH_ONLY_SPACES, STRING_WITH_ONLY_SPACES, STRING_WITH_ONLY_SPACES, DEFAULT_FONTSIZE);
            Assert.That(label.Text, Is.EqualTo(STRING_WITH_ONLY_SPACES));
            Assert.That(label.TextColor, Is.EqualTo(STRING_WITH_ONLY_SPACES));
            Assert.That(label.Font, Is.EqualTo(STRING_WITH_ONLY_SPACES));
            Assert.That(label.FontSize, Is.EqualTo(DEFAULT_FONTSIZE));
        }

        [Test]
        public void Test_Label_Initialization_NormalAcceptableStrings()
        {
            IRichLabel label = new RichLabel(NORMAL_ABC_STRING, EXAMPLE_COLOR, EXAMPLE_FONT, DEFAULT_FONTSIZE);
            Assert.That(label.Text, Is.EqualTo(NORMAL_ABC_STRING));
            Assert.That(label.TextColor, Is.EqualTo(EXAMPLE_COLOR));
            Assert.That(label.Font, Is.EqualTo(EXAMPLE_FONT));
            Assert.That(label.FontSize, Is.EqualTo(DEFAULT_FONTSIZE));
            // Assert.Throws(label.Text = NORMAL_ABC_STRING); Reassigning is for now forbidden (for every attribute/property) 
            // so no need to test it as it leads to a compilation error
        }

        [Test]
        public void Test_Label_Initialization_NegativeFontSize()
        {
            Assert.Throws<ArgumentException>(() => new RichLabel(NORMAL_ABC_STRING, EXAMPLE_COLOR, EXAMPLE_FONT, NEGATIVE_FONTSIZE));
        }

        [Test]
        public void Test_Label_Initialization_FontSizeZero()
        {
            Assert.Throws<ArgumentException>(() => new RichLabel(NORMAL_ABC_STRING, EXAMPLE_COLOR, EXAMPLE_FONT, FONTSIZE_ZERO));
        }
    }

    [TestFixture]
    public class CustomLabelTests
    {
        private const string EMPTY_STRING = "";

        // test initialization
        [Test]
        public void Test_Label_Initilialization_EmptyString_WithoutLabelChanging()
        {
            // label content
            StringReader reader = new StringReader(EMPTY_STRING);
            StringWriter writer = new StringWriter();

            ILabel label = new TestingCustomLabel(reader, writer);
            string input = label.Text;
            string output = writer.ToString();
            Assert.That(input, Is.EqualTo(EMPTY_STRING));
            string EXPECTED_OUTPUT = "Please add an timeout option after which many calls you will be bothered again!\r\n" +
                "Enter '0' if you don't wish a timeout, otherwise enter an positive integer\r\n" +
                "Your entered number: Please enter the labels name, which you want to use for this label\r\n" +
                "Your label name: ";
            Assert.That(output, Is.EqualTo(EXPECTED_OUTPUT));
        }

        [Test]
        public void Test_Label_Initilialization_NonEmptyString_WithoutLabelChanging()
        {
            // label content - the TextReader won't be bothered after the initialization of the text
            StringReader reader = new StringReader("0\n" + "abc all the way\n");
            StringWriter writer = new StringWriter();

            ILabel label = new TestingCustomLabel(reader, writer);
            string input = label.Text;
            string output = writer.ToString();
            Assert.That(input, Is.EqualTo("abc all the way"));
            string EXPECTED_OUTPUT = "Please add an timeout option after which many calls you will be bothered again!\r\n" +
                "Enter '0' if you don't wish a timeout, otherwise enter an positive integer\r\n" +
                "Your entered number: Please enter the labels name, which you want to use for this label\r\n" +
                "Your label name: ";
            Assert.That(output, Is.EqualTo(EXPECTED_OUTPUT));

            const int TRIES_TO_IMPLY_CORRECTNESS = 10;
            for (int i = 0; i < TRIES_TO_IMPLY_CORRECTNESS; i++)
            {
                input = label.Text;
                output = writer.ToString();
                Assert.That(input, Is.EqualTo("abc all the way"));
                Assert.That(output, Is.EqualTo(EXPECTED_OUTPUT));
            }
        }

        [Test]
        public void Test_Label_Initilialization_LabelChanging()
        {
            // label content
            // first we are how frequently should we disturb the real label, than a real label, than label change negative choice and then affirmative choice, then label 
            StringReader reader = new StringReader("2\n" + "abc all the way\n" + "n\n" + "y\n" + "do not disturb");
            StringWriter writer = new StringWriter();
            // once we enter a text, it will be repeated one more time, before we are asked again( on every other turn the IN reader will be asked)
            ILabel label = new TestingCustomLabel(reader, writer);
            string labelUnderneath = label.Text;
            Assert.That(labelUnderneath, Is.EqualTo("abc all the way"));
            string output = writer.ToString();
            string EXPECTED_OUTPUT = "Please add an timeout option after which many calls you will be bothered again!\r\n" +
                "Enter '0' if you don't wish a timeout, otherwise enter an positive integer\r\n" +
                "Your entered number: Please enter the labels name, which you want to use for this label\r\n" +
                "Your label name: ";
            Assert.That(output, Is.EqualTo(EXPECTED_OUTPUT));
            labelUnderneath = label.Text;
            Assert.That(labelUnderneath, Is.EqualTo("abc all the way")); // the transformation still stays
            output = writer.ToString();
            Assert.That(output, Is.EqualTo(EXPECTED_OUTPUT));


            labelUnderneath = label.Text; // we will be asked to enter a label again, so new possible input and guaranteed outputStream change
            Assert.That(labelUnderneath, Is.EqualTo("abc all the way")); // the transformation still stays as we have chosen it this way
            output = writer.ToString();
            EXPECTED_OUTPUT += "Do you want to change the label for this item? If so type one of the following chars { 'y', 'Y', 'yes', 'Yes', 'YES' }\r\nYour choice: ";
            Assert.That(output, Is.EqualTo(EXPECTED_OUTPUT));

            labelUnderneath = label.Text; // it stays the same
            Assert.That(labelUnderneath, Is.EqualTo("abc all the way"));
            output = writer.ToString();
            Assert.That(output, Is.EqualTo(EXPECTED_OUTPUT));

            labelUnderneath = label.Text; // we will be asked to enter a label again, so new possible input and guaranteed outputStream change. We change it
            Assert.That(labelUnderneath, Is.EqualTo("do not disturb")); // the transformation still stays as we have chosen it this way
            output = writer.ToString();
            EXPECTED_OUTPUT += "Do you want to change the label for this item? If so type one of the following chars { 'y', 'Y', 'yes', 'Yes', 'YES' }\r\nYour choice: Please enter the labels name, which you want to use for this label\r\nYour label name: ";
            Assert.That(output, Is.EqualTo(EXPECTED_OUTPUT));

            // next try will be without disturb
            labelUnderneath = label.Text;
            Assert.That(labelUnderneath, Is.EqualTo("do not disturb"));
            output = writer.ToString();
            Assert.That(output, Is.EqualTo(EXPECTED_OUTPUT));
        }
    }

    [TestFixture]
    public class HelpLabelTests // test it as a part of every class - SimpleLabel, RichLabel, CustomLabel and try to decorate it
    {
        // firstly we create a base Label type - then we put it as part of a HelpLabel and then it gets decorated -> symbolises composite
        // Note that by doing it this way, Labels without help text can exist as just ILabel implementations
        const string MISSING_HELP_MESSAGE = HelpLabel.MISSING_HELP_MESSAGE;
        const string EXAMPLE_TEXT = "abc";
        const string EXAMPLE_HELP_MESSAGE = "Super useful help message!";

        [Test]
        public void Test_HelpLabel_Initilialization_EmptyHelpText()
        {
            ILabel label = new SimpleLabel(EXAMPLE_TEXT);
            IHelpLabel helpLabel = new HelpLabel(label, string.Empty);
            Assert.That(helpLabel.Text, Is.EqualTo(EXAMPLE_TEXT));
            Assert.That(helpLabel.HelpText, Is.EqualTo(HelpLabel.MISSING_HELP_MESSAGE));
        }

        [Test]
        public void Test_HelpLabel_NullHelpText()
        {
            ILabel label = new SimpleLabel(EXAMPLE_TEXT);
            Assert.Throws<ArgumentNullException>(() => new HelpLabel(label, null));
            IHelpLabel helpLabel = new HelpLabel(label, EXAMPLE_HELP_MESSAGE);
            Assert.That(helpLabel.Text, Is.EqualTo(EXAMPLE_TEXT));
            Assert.That(helpLabel.HelpText, Is.EqualTo(EXAMPLE_HELP_MESSAGE));
            Assert.Throws<ArgumentNullException>(() => helpLabel.HelpText = null);
            Assert.That(helpLabel.HelpText, Is.EqualTo(EXAMPLE_HELP_MESSAGE));
        }

        [Test]
        public void Test_HelpLabel_RichLabelWithHelp()
        {
            ILabel label = new RichLabel(EXAMPLE_TEXT, "red", "Arial", 10.5);
            Assert.Throws<ArgumentNullException>(() => new HelpLabel(label, null));
            IHelpLabel helpLabel = new HelpLabel(label, EXAMPLE_HELP_MESSAGE);
            Assert.That(helpLabel.Text, Is.EqualTo(EXAMPLE_TEXT));
            Assert.That(helpLabel.HelpText, Is.EqualTo(EXAMPLE_HELP_MESSAGE));
            helpLabel.HelpText = string.Empty;
            Assert.That(helpLabel.HelpText, Is.EqualTo(MISSING_HELP_MESSAGE));
        }

        [Test]
        public void Test_HelpLabel_CustomLabelWithHelp()
        {
            StringReader reader = new StringReader("0\n" + "abc\n");
            StringWriter writer = new StringWriter();
            ILabel label = new TestingCustomLabel(reader, writer);
            Assert.Throws<ArgumentNullException>(() => new HelpLabel(label, null));
            IHelpLabel helpLabel = new HelpLabel(label, EXAMPLE_HELP_MESSAGE);
            Assert.That(helpLabel.Text, Is.EqualTo(EXAMPLE_TEXT));
            string output = writer.ToString();
            string EXPECTED_OUTPUT = "Please add an timeout option after which many calls you will be bothered again!\r\n" +
                "Enter '0' if you don't wish a timeout, otherwise enter an positive integer\r\n" +
                "Your entered number: Please enter the labels name, which you want to use for this label\r\n" +
                "Your label name: ";
            Assert.That(output, Is.EqualTo(EXPECTED_OUTPUT));
            Assert.That(helpLabel.HelpText, Is.EqualTo(EXAMPLE_HELP_MESSAGE));
            helpLabel.HelpText = string.Empty;
            Assert.That(helpLabel.HelpText, Is.EqualTo(MISSING_HELP_MESSAGE));
        }

        [Test]
        public void Test_HelpLabel_LabelDecorating()
        {
            ILabel label = new SimpleLabel(EXAMPLE_TEXT); // we have covered decorating all kinds of labels, together with getting help with all kinds of labels
            // by combining the properties of the 2 assertions, we guarantee coverage on the whole equality set by just expressing one example(SimpleLabel)
            IHelpLabel helpLabel = new HelpLabel(label, EXAMPLE_HELP_MESSAGE);
            Assert.That(helpLabel.Text, Is.EqualTo(EXAMPLE_TEXT));
            Assert.That(helpLabel.HelpText, Is.EqualTo(EXAMPLE_HELP_MESSAGE));

            const string EXPECTED_DECORATED_RESULT = "-={ abc }=-";
            ILabel decoratedContentWithoutHelpLabel = new TextTransformationDecorator(label, new DecorationTransformation());
            Assert.That(decoratedContentWithoutHelpLabel.Text, Is.EqualTo(EXPECTED_DECORATED_RESULT));
            // Assert.That(decoratedContentWithoutHelpLabel.HelpText, Is.EqualTo(EXAMPLE_HELP_MESSAGE)); -> it is not marked to contain helpLabel

            ILabel decoratedContentWithoutAccessToHelpLabel = new TextTransformationDecorator(helpLabel, new DecorationTransformation());
            Assert.That(decoratedContentWithoutHelpLabel.Text, Is.EqualTo(EXPECTED_DECORATED_RESULT));
            // Assert.That(decoratedContentWithoutHelpLabel.HelpText, Is.EqualTo(EXAMPLE_HELP_MESSAGE)); -> it is not marked to contain helpLabel

            IHelpLabel deroratedContentWithHelpLabel = new TextTransformationDecorator(helpLabel, new DecorationTransformation());
            Assert.That(decoratedContentWithoutHelpLabel.Text, Is.EqualTo(EXPECTED_DECORATED_RESULT));
            Assert.That(deroratedContentWithHelpLabel.HelpText, Is.EqualTo(EXAMPLE_HELP_MESSAGE));

            IHelpLabel decoratedContentWithoutHelpMarkedAsHelpLabel = new TextTransformationDecorator(label, new DecorationTransformation());
            Assert.That(decoratedContentWithoutHelpMarkedAsHelpLabel.Text, Is.EqualTo(EXPECTED_DECORATED_RESULT));
            Assert.That(decoratedContentWithoutHelpMarkedAsHelpLabel.HelpText, Is.EqualTo(MISSING_HELP_MESSAGE));
            // add a help message in the future
            decoratedContentWithoutHelpMarkedAsHelpLabel.HelpText = EXAMPLE_HELP_MESSAGE;
            Assert.That(decoratedContentWithoutHelpMarkedAsHelpLabel.Text, Is.EqualTo(EXPECTED_DECORATED_RESULT));
            Assert.That(decoratedContentWithoutHelpMarkedAsHelpLabel.HelpText, Is.EqualTo(EXAMPLE_HELP_MESSAGE));
        }


        // the label printer is unchangeble and uses static functions, so I have tested the helpLabel with it in the main method.
        // Swapping writers/readers is forbidden - Here is the tested code below - it works as intended
        /*
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
         */
    }
}