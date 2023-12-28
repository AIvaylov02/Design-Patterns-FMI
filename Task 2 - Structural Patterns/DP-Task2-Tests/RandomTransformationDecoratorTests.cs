using DP_Task2.Interfaces;
using DP_Task2.LabelDecorators;
using DP_Task2.Labels;
using DP_Task2.Transformations;

namespace DP_Task2_Tests
{
    [TestFixture]
    public class RandomTransformationDecoratorTests
    {
        // empty text
        // no styles in ctor
        // all styles applied in the end
        const string SAMPLE_RICH_TEXT = "   September   Will Be   May     ";
        const string EMPTY_TEXT = "";

        [Test]
        public void Test_RandomDecorator_Basic_Initialization_Empty_Text()
        {
            ILabel label = new SimpleLabel(EMPTY_TEXT);
            RandomTransformationDecorator emptyDecorator = new RandomTransformationDecorator(label);
            Assert.That(emptyDecorator.Text, Is.EqualTo(EMPTY_TEXT));
        }

        [Test]
        public void Test_RandomDecorator_Basic_Initialization_Rich_Text()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            RandomTransformationDecorator emptyDecorator = new RandomTransformationDecorator(label);
            Assert.That(emptyDecorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }

        [Test]
        public void Test_RandomDecorator_Rich_Initialization_Rich_Text()
        {
            ILabel label = new SimpleLabel(EMPTY_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>
            {
                new TrimLeftTransformation(),
                new TrimRightTransformation(),
                new SpaceNormalizationTransformation()
            };
            RandomTransformationDecorator fullEnrollmentDecorator = new RandomTransformationDecorator(label, transformations);
            string currentText = fullEnrollmentDecorator.Text;
            currentText = fullEnrollmentDecorator.Text;
            currentText = fullEnrollmentDecorator.Text;
            Assert.That(currentText, Is.EqualTo(EMPTY_TEXT));
        }

        [Test]
        public void Test_RandomDecorator_Rich_Initialization_Empty_Text()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>
            {
                new TrimLeftTransformation(),
                new TrimRightTransformation(),
                new SpaceNormalizationTransformation()
            };
            RandomTransformationDecorator fullEnrollmentDecorator = new RandomTransformationDecorator(label, transformations);
            string currentText = fullEnrollmentDecorator.Text;
            currentText = fullEnrollmentDecorator.Text;
            currentText = fullEnrollmentDecorator.Text;
            const string EXPECTED_RESULT = "September Will Be May";
            Assert.That(currentText, Is.EqualTo(EXPECTED_RESULT));
            /* a full cycle has been applied, the transformations will be rerolled and are ready for round 2 of application
             Although no more changes will be met as it is already an end product
             */
            currentText = fullEnrollmentDecorator.Text;
            currentText = fullEnrollmentDecorator.Text;
            currentText = fullEnrollmentDecorator.Text;
            Assert.That(currentText, Is.EqualTo(EXPECTED_RESULT));
        }
        
    }
}
