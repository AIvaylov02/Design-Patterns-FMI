using DP_Task2.Interfaces;
using DP_Task2.LabelDecorators;
using DP_Task2.Labels;
using DP_Task2.Transformations;


namespace DP_Task2_Tests
{
    [TestFixture]
    public class TextTransformationDecoratorTests
    {
        // input text is immutable as well as the label underneath

        // TODO in part 4 the transformations may change
        const string SAMPLE_RICH_TEXT = "   September   Will Be   May     ";
        const string EMPTY_TEXT = "";
        const string SEPTEMBER = "September";
        const string SUNNY_DAYS = "Non-stop sunny";

        /*  For code coverage we need tests with each kind of transformation.
            Label has already been covered as well as the transformation independently so
            we should test only their proper linkage */

        // Basic = NULL Transformation, so only plain text will be shown
        [Test]
        public void Test_TextTransformationDecorator_Basic_Initialization_Empty_Text()
        {
            ILabel label = new SimpleLabel(EMPTY_TEXT);
            TextTransformationDecorator emptyDecorator = new TextTransformationDecorator(label);
            Assert.That(emptyDecorator.Text, Is.EqualTo(EMPTY_TEXT));
        }

        [Test]
        public void Test_TextTransformationDecorator_Basic_Initialization_Rich_Text()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            TextTransformationDecorator emptyDecorator = new TextTransformationDecorator(label);
            Assert.That(emptyDecorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }

        [Test]
        public void Test_TextTransformationDecorator_Capitalizer_Initialization_Empty_Text()
        {
            ILabel label = new SimpleLabel(EMPTY_TEXT);
            ITextTransformation capitalizer = new CapitalizeTransformation();
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, capitalizer);
            Assert.That(decorator.Text, Is.EqualTo(EMPTY_TEXT));
        }

        [Test]
        public void Test_TextTransformationDecorator_Capitalizer_Initialization_Rich_Text()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation capitalizer = new CapitalizeTransformation();
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, capitalizer);
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }

        [Test]
        public void Test_TextTransformationDecorator_LeftTrimmer_Initialization_Empty_Text()
        {
            ILabel label = new SimpleLabel(EMPTY_TEXT);
            ITextTransformation leftTrimmer = new TrimLeftTransformation();
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, leftTrimmer);
            Assert.That(decorator.Text, Is.EqualTo(EMPTY_TEXT));
        }

        [Test]
        public void Test_TextTransformationDecorator_LeftTrimmer_Initialization_Rich_Text()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation leftTrimmer = new TrimLeftTransformation();
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, leftTrimmer);
            const string EXPECTED_RESULT = "September   Will Be   May     ";
            Assert.That(decorator.Text, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_TextTransformationDecorator_RightTrimmer_Initialization_Empty_Text()
        {
            ILabel label = new SimpleLabel(EMPTY_TEXT);
            ITextTransformation rightTrimmer = new TrimRightTransformation();
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, rightTrimmer);
            Assert.That(decorator.Text, Is.EqualTo(EMPTY_TEXT));
        }

        [Test]
        public void Test_TextTransformationDecorator_RightTrimmer_Initialization_Rich_Text()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation rightTrimmer = new TrimRightTransformation();
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, rightTrimmer);
            const string EXPECTED_RESULT = "   September   Will Be   May";
            Assert.That(decorator.Text, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_TextTransformationDecorator_Normalize_Initialization_Empty_Text()
        {
            ILabel label = new SimpleLabel(EMPTY_TEXT);
            ITextTransformation normalizer = new SpaceNormalizationTransformation();
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, normalizer);
            Assert.That(decorator.Text, Is.EqualTo(EMPTY_TEXT));
        }

        [Test]
        public void Test_TextTransformationDecorator_Normalize_Initialization_Rich_Text()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation normalizer = new SpaceNormalizationTransformation();
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, normalizer);
            const string EXPECTED_RESULT = " September Will Be May ";
            Assert.That(decorator.Text, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_TextTransformationDecorator_Decoration_Initialization_Empty_Text()
        {
            ILabel label = new SimpleLabel(EMPTY_TEXT);
            ITextTransformation decoration = new DecorationTransformation();
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, decoration);
            const string EXPECTED_RESULT = "-={  }=-";
            Assert.That(decorator.Text, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_TextTransformationDecorator_Decoration_Initialization_Rich_Text()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation decoration = new DecorationTransformation();
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, decoration);
            const string EXPECTED_RESULT = "-={    September   Will Be   May      }=-";
            Assert.That(decorator.Text, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_TextTransformationDecorator_Censorer_Initialization_Empty_Text()
        {
            ILabel label = new SimpleLabel(EMPTY_TEXT);
            ITextTransformation censorer = new CensorerTransformation(SEPTEMBER);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, censorer);
            Assert.That(decorator.Text, Is.EqualTo(EMPTY_TEXT));
        }

        [Test]
        public void Test_TextTransformationDecorator_Censorer_Initialization_Rich_Text()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation censorer = new CensorerTransformation(SEPTEMBER);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, censorer);
            const string EXPECTED_RESULT = "   *********   Will Be   May     ";
            Assert.That(decorator.Text, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_TextTransformationDecorator_Replacer_Initialization_Empty_Text()
        {
            ILabel label = new SimpleLabel(EMPTY_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(EMPTY_TEXT));
        }

        [Test]
        public void Test_TextTransformationDecorator_Replacer_Initialization_Rich_Text()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            const string EXPECTED_RESULT = $"   {SUNNY_DAYS}   Will Be   May     ";
            Assert.That(decorator.Text, Is.EqualTo(EXPECTED_RESULT));
        }

        // TODO in part 4 the transformations may change - add a transformation for each kind after it has been set with null in the beginning
        // try adding a new transformation on top of an already set one
    }
}
