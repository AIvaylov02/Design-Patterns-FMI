using DP_Task2.Interfaces;
using DP_Task2.Labels;

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
}