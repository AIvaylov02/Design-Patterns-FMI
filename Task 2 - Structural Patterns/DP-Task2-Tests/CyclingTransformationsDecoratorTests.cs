using DP_Task2.Interfaces;
using DP_Task2.LabelDecorators;
using DP_Task2.Labels;
using DP_Task2.Transformations;

namespace DP_Task2_Tests
{
    [TestFixture]
    public class CyclingTransformationsDecoratorTests
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
        public void Test_CyclicDecorator_Basic_Initialization_Empty_Text()
        {
            ILabel label = new SimpleLabel(EMPTY_TEXT);
            CyclingTransformationsDecorator emptyDecorator = new CyclingTransformationsDecorator(label);
            Assert.That(emptyDecorator.Text, Is.EqualTo(EMPTY_TEXT));
        }

        [Test]
        public void Test_CyclicDecorator_Basic_Initialization_Rich_Text()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            CyclingTransformationsDecorator emptyDecorator = new CyclingTransformationsDecorator(label);
            Assert.That(emptyDecorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }

        [Test]
        public void Test_CyclicDecorator_Doubling_Initialization_Empty_Text()
        {
            ILabel label = new SimpleLabel(EMPTY_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>
            {
                new TrimLeftTransformation(),
                new TrimRightTransformation(),
                new TrimLeftTransformation()
            };
            CyclingTransformationsDecorator doublingDecorator = new CyclingTransformationsDecorator(label, transformations);
            Assert.That(doublingDecorator.Text, Is.EqualTo(EMPTY_TEXT));
            Assert.That(doublingDecorator.Text, Is.EqualTo(EMPTY_TEXT));
            Assert.That(doublingDecorator.Text, Is.EqualTo(EMPTY_TEXT));
        }

        [Test]
        public void Test_CyclicDecorator_Doubling_Initialization_Rich_Text()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>
            {
                new TrimLeftTransformation(),
                new TrimRightTransformation(),
                new TrimLeftTransformation()
            };
            CyclingTransformationsDecorator doublingDecorator = new CyclingTransformationsDecorator(label, transformations);
            Assert.That(doublingDecorator.Text, Is.EqualTo("September   Will Be   May     ")); // leftTrimmer has been applied
            Assert.That(doublingDecorator.Text, Is.EqualTo("September   Will Be   May")); // rightTrimmer has been applied
            Assert.That(doublingDecorator.Text, Is.EqualTo("September   Will Be   May")); // another leftTrimmer has been applied, nothing more will happen
            Assert.That(doublingDecorator.Text, Is.EqualTo("September   Will Be   May")); // the first leftTrimmer has been applied, nothing more will occur
        }

        [Test]
        public void Test_CyclicDecorator_OrderMatters_Initialization_Rich_Text()
        {
            const string UNCAPITALIZER_SAMPLE_RICH_TEXT = "september   will be   may";
            ILabel label = new SimpleLabel(UNCAPITALIZER_SAMPLE_RICH_TEXT);
            List<ITextTransformation> capitalizeThenReplace = new List<ITextTransformation>
            {
                new CapitalizeTransformation(),
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS)
            };
            CyclingTransformationsDecorator orderMattersDecorator = new CyclingTransformationsDecorator(label, capitalizeThenReplace);
            Assert.That(orderMattersDecorator.Text, Is.EqualTo("September   will be   may")); // capitalize has been applied, september has been capitalized
            Assert.That(orderMattersDecorator.Text, Is.EqualTo("Non-stop sunny   will be   may")); // replace has been applied, so September has been replaced

            /* NOTE! In C# objects are always passed by referance. All labels and transformations in constructors are directly connected and a subject to change.
                In my opinion, no label should be used by 2 different decorators simultaniously, thats why there is no deep copying of the objects, moreso it is object passing!
            */
            List<ITextTransformation> replaceThenCapitalize = new List<ITextTransformation>
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new CapitalizeTransformation()
            };
            orderMattersDecorator = new CyclingTransformationsDecorator(label, replaceThenCapitalize);
            
            Assert.That(orderMattersDecorator.Text, Is.EqualTo("september   will be   may")); // replace has been applied, so nothing will occur
            Assert.That(orderMattersDecorator.Text, Is.EqualTo("September   will be   may")); // capitalize has been applied, september has been capitalized
            Assert.That(orderMattersDecorator.Text, Is.EqualTo("Non-stop sunny   will be   may")); // list is rerolled so the replace will now work as expected as it finds a match
        }

        [Test]
        public void Test_CyclicDecorator_TransformationAlternating_Initialization_Rich_Text()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);

            List<ITextTransformation> transformations = new List<ITextTransformation>
            {
                // bad word for first will be a replacement for second and vice versa
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER)
            };
            CyclingTransformationsDecorator alternatingDecorator = new CyclingTransformationsDecorator(label, transformations);
            const string TRANSFORMED_SUNNY_SENTANCE = "   Non-stop sunny   Will Be   May     ";

            Assert.That(alternatingDecorator.Text, Is.EqualTo(TRANSFORMED_SUNNY_SENTANCE)); // Non-stop sunny replaces September
            Assert.That(alternatingDecorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT)); // September gets back by replacing Non-stop sunny
            Assert.That(alternatingDecorator.Text, Is.EqualTo(TRANSFORMED_SUNNY_SENTANCE)); // Non-stop sunny replaces September
            Assert.That(alternatingDecorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT)); // September gets back by replacing Non-stop sunny
        }

        // TODO in part 4 the transformations may change

    }
}
