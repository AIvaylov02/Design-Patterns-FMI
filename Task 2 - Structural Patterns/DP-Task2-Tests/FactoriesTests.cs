using DP_Task2.Interfaces;
using DP_Task2.LabelDecorators;
using DP_Task2.Labels;
using DP_Task2.Transformations;
using DP_Task2.Utilizers;

namespace DP_Task2_Tests
{
    [TestFixture]
    public class BaseLabelFactoryTests
    {
        const string SIMPLE_TYPE = "simple";
        const string RICH_TYPE = "rich";
        const string STANDARD_TEXT = "Abrakadabra";
        const string STANDARD_FONT = "Calibri";
        const string STANDARD_COLOUR = "dark blue";
        const double STANDARD_FONT_SIZE = 10.5;
        // custom type will be tested on the STDIN -> for throughout testing the factory needs to be a singleton and be initialized with the console in/out or stringreader

        [Test]
        public void Test_LabelCreation_InvalidType()
        {
            string type = "not_known";
            Assert.Throws<ArgumentException>(() => BaseLabelFactory.CreateLabel(type)); // will try to match it as a custom label if only one argument is given
        }

        [Test]
        public void Test_SimpleLabelCreation()
        {
            string? text = null;
            Assert.Throws<ArgumentNullException>(() => BaseLabelFactory.CreateLabel(SIMPLE_TYPE, text)); // text cannot be null in a simple label
            text = STANDARD_TEXT;
            ILabel baseLabel = BaseLabelFactory.CreateLabel(SIMPLE_TYPE, text);
            SimpleLabel? parsedLabel = baseLabel as SimpleLabel;
            Assert.IsNotNull(parsedLabel);
            Assert.That(parsedLabel.Text, Is.EqualTo(STANDARD_TEXT));
        }

        [Test]
        public void Test_RichLabelCreation_InvalidParameter()
        {
            string? text = null;
            string? textColour = STANDARD_COLOUR;
            string? font = STANDARD_FONT;
            double? fontSize = STANDARD_FONT_SIZE;
            Assert.Throws<ArgumentNullException>(() => BaseLabelFactory.CreateLabel(RICH_TYPE, text, textColour, font, fontSize)); // text cannot be null in rich label

            text = STANDARD_TEXT;
            textColour = null;
            Assert.Throws<ArgumentNullException>(() => BaseLabelFactory.CreateLabel(RICH_TYPE, text, textColour, font, fontSize)); // textColour cannot be null in a rich label

            textColour = STANDARD_COLOUR;
            font = null;
            Assert.Throws<ArgumentNullException>(() => BaseLabelFactory.CreateLabel(RICH_TYPE, text, textColour, font, fontSize)); // font cannot be null in a rich label

            font = STANDARD_FONT;
            fontSize = null;
            Assert.Throws<ArgumentNullException>(() => BaseLabelFactory.CreateLabel(RICH_TYPE, text, textColour, font, fontSize)); // fontSize cannot be null in a rich label

            fontSize = 0;
            Assert.Throws<ArgumentException>(() => BaseLabelFactory.CreateLabel(RICH_TYPE, text, textColour, font, fontSize)); // fontSize must be greater than 0

            fontSize = STANDARD_FONT_SIZE;
            ILabel baseLabel = BaseLabelFactory.CreateLabel(RICH_TYPE, text, textColour, font, fontSize);
            RichLabel? parsedLabel = baseLabel as RichLabel;
            Assert.IsNotNull(parsedLabel);
            Assert.That(parsedLabel.Text, Is.EqualTo(STANDARD_TEXT));
            Assert.That(parsedLabel.TextColor, Is.EqualTo(STANDARD_COLOUR));
            Assert.That(parsedLabel.Font, Is.EqualTo(STANDARD_FONT));
            Assert.That(parsedLabel.FontSize, Is.EqualTo(STANDARD_FONT_SIZE));
        }
    }

    [TestFixture]
    public class CensorerTransformationFactoryTests
    {
        const string? NULL_BAD_WORD = null;
        const string ABCD_BAD_WORD = "abcd";
        const string BAD_WORD_SEPTEMBER = "September";
        const string BARE_MINIMUM_LONG_CENSORER = "five!";

        [Test]
        public void Test_Null_Bad_Word()
        {
            Assert.Throws<ArgumentNullException>(() => CensorerTransformationSingletonFactory.Instance.CreateCensorer(NULL_BAD_WORD));
        }

        [Test]
        public void Test_Short_Censorers()
        {
            CensorerTransformation censorer = CensorerTransformationSingletonFactory.Instance.CreateCensorer(ABCD_BAD_WORD);
            Dictionary<string, CensorerTransformation>? currentState = CensorerTransformationSingletonFactory.Instance.Censorships as Dictionary<string, CensorerTransformation>;
            Assert.IsNotNull(currentState);
            Assert.True(currentState.ContainsKey(ABCD_BAD_WORD));
            Assert.That(currentState[ABCD_BAD_WORD], Is.EqualTo(censorer));
            // censorer.BadWord = "different"; as it retrives the state from tha factory
            // it could however become another censorer
            censorer = new CensorerTransformation(BAD_WORD_SEPTEMBER); // it is still possible to do it from the outside
            currentState = CensorerTransformationSingletonFactory.Instance.Censorships as Dictionary<string, CensorerTransformation>;
            Assert.IsNotNull(currentState);
            Assert.True(currentState.ContainsKey(ABCD_BAD_WORD));
            CensorerTransformation anotherCensorer = CensorerTransformationSingletonFactory.Instance.CreateCensorer(ABCD_BAD_WORD);
            Assert.That(currentState[ABCD_BAD_WORD], Is.EqualTo(anotherCensorer));
            Assert.False(currentState.ContainsKey(BAD_WORD_SEPTEMBER));
            // Assert.That(currentState[BAD_WORD_SEPTEMBER], Is.EqualTo(censorer));

            censorer = new CensorerTransformation(ABCD_BAD_WORD);
            Assert.IsFalse(Object.ReferenceEquals(censorer, anotherCensorer)); // It is not created from the factory, so the instance is not shared
            censorer = CensorerTransformationSingletonFactory.Instance.CreateCensorer(ABCD_BAD_WORD);
            Assert.IsTrue(Object.ReferenceEquals(censorer, anotherCensorer)); // both of them are null or both of them share the same instance
        }

        [Test]
        public void Test_Long_Censorers()
        {
            CensorerTransformation censorer = CensorerTransformationSingletonFactory.Instance.CreateCensorer(BARE_MINIMUM_LONG_CENSORER);
            Dictionary<string, CensorerTransformation>? currentState = CensorerTransformationSingletonFactory.Instance.Censorships as Dictionary<string, CensorerTransformation>;
            Assert.IsNotNull(currentState);
            Assert.That(censorer.BadWord, Is.EqualTo(BARE_MINIMUM_LONG_CENSORER));
            Assert.False(currentState.ContainsKey(BARE_MINIMUM_LONG_CENSORER)); // the word is too long
            /* 
              Assert.That(currentState[BARE_MINIMUM_LONG_CENSORER], Is.EqualTo(censorer)); doesn't hold up
              censorer.BadWord = "different"; as it retrives the state from tha factory */
            CensorerTransformation anotherCensorer = new CensorerTransformation(BARE_MINIMUM_LONG_CENSORER);
            Assert.That(anotherCensorer.BadWord, Is.EqualTo(BARE_MINIMUM_LONG_CENSORER));
            Assert.IsFalse(Object.ReferenceEquals(censorer, anotherCensorer)); // It is not created from the factory, so the instance is not shared
            anotherCensorer = CensorerTransformationSingletonFactory.Instance.CreateCensorer(BARE_MINIMUM_LONG_CENSORER);
            Assert.That(anotherCensorer.BadWord, Is.EqualTo(BARE_MINIMUM_LONG_CENSORER));
            Assert.IsFalse(Object.ReferenceEquals(censorer, anotherCensorer)); // The word is too long, so the factory doesn't hold it, it creates a new instance of transformation
            currentState = CensorerTransformationSingletonFactory.Instance.Censorships as Dictionary<string, CensorerTransformation>;
            Assert.False(currentState.ContainsKey(BARE_MINIMUM_LONG_CENSORER));
        }
    }

    [TestFixture]
    public class TextTransformationFactoryTests
    {
        const string SHORT_BAD_WORD = "abcd";
        const string LONG_BAD_WORD = "ABCD!";
        const string EXAMPLE_REPLACEMENT = "Non-abc welcome hood exists here!";

        ITextTransformation firstTransformation;
        ITextTransformation secondTransformation;

        // in each test we will make sure that the references defer(*censorer is special case), while the objects are equal(same transformation is applied)

        [Test]
        public void Test_Creation_InvalidType()
        {
            const string INVALID_TYPE = "husky";
            Assert.Throws<ArgumentException>(() => TextTransformationFactory.CreateTransformation(INVALID_TYPE));
        }

        [Test]
        public void Test_Creation_Capitalizer()
        {
            const string CAPITALIZE_TYPE = "capitalize";
            firstTransformation = TextTransformationFactory.CreateTransformation(CAPITALIZE_TYPE);
            secondTransformation = TextTransformationFactory.CreateTransformation(CAPITALIZE_TYPE);
            Assert.IsNotNull(firstTransformation);
            Assert.IsNotNull(secondTransformation);
            Assert.That(firstTransformation, Is.EqualTo(secondTransformation));
            Assert.False(Object.ReferenceEquals(firstTransformation, secondTransformation));

            // parse to the concrete type
            CapitalizeTransformation? firstParsed = firstTransformation as CapitalizeTransformation;
            Assert.IsNotNull(firstParsed);
            CapitalizeTransformation? secondParsed = secondTransformation as CapitalizeTransformation;
            Assert.IsNotNull(secondParsed);
            Assert.That(firstParsed, Is.EqualTo(secondParsed));
            Assert.False(Object.ReferenceEquals(firstParsed, secondParsed));
        }

        [Test]
        public void Test_Creation_DecorationTransformation()
        {
            const string DECORATION_TYPE = "decorate";
            firstTransformation = TextTransformationFactory.CreateTransformation(DECORATION_TYPE);
            secondTransformation = TextTransformationFactory.CreateTransformation(DECORATION_TYPE);
            Assert.IsNotNull(firstTransformation);
            Assert.IsNotNull(secondTransformation);
            Assert.That(firstTransformation, Is.EqualTo(secondTransformation));
            Assert.False(Object.ReferenceEquals(firstTransformation, secondTransformation));

            // parse to the concrete type
            DecorationTransformation? firstParsed = firstTransformation as DecorationTransformation;
            Assert.IsNotNull(firstParsed);
            DecorationTransformation? secondParsed = secondTransformation as DecorationTransformation;
            Assert.IsNotNull(secondParsed);
            Assert.That(firstParsed, Is.EqualTo(secondParsed));
            Assert.False(Object.ReferenceEquals(firstParsed, secondParsed));
        }

        [Test]
        public void Test_Creation_SpaceNormalizationTransformation()
        {
            const string SPACE_TRANSFORMATION_TYPE = "spacer";
            firstTransformation = TextTransformationFactory.CreateTransformation(SPACE_TRANSFORMATION_TYPE);
            secondTransformation = TextTransformationFactory.CreateTransformation(SPACE_TRANSFORMATION_TYPE);
            Assert.IsNotNull(firstTransformation);
            Assert.IsNotNull(secondTransformation);
            Assert.That(firstTransformation, Is.EqualTo(secondTransformation));
            Assert.False(Object.ReferenceEquals(firstTransformation, secondTransformation));

            // parse to the concrete type
            SpaceNormalizationTransformation? firstParsed = firstTransformation as SpaceNormalizationTransformation;
            Assert.IsNotNull(firstParsed);
            SpaceNormalizationTransformation? secondParsed = secondTransformation as SpaceNormalizationTransformation;
            Assert.IsNotNull(secondParsed);
            Assert.That(firstParsed, Is.EqualTo(secondParsed));
            Assert.False(Object.ReferenceEquals(firstParsed, secondParsed));
        }

        [Test]
        public void Test_Creation_LeftTrimmer()
        {
            const string LEFT_TRIMMER_TYPE = "trimLeft";
            firstTransformation = TextTransformationFactory.CreateTransformation(LEFT_TRIMMER_TYPE);
            secondTransformation = TextTransformationFactory.CreateTransformation(LEFT_TRIMMER_TYPE);
            Assert.IsNotNull(firstTransformation);
            Assert.IsNotNull(secondTransformation);
            Assert.That(firstTransformation, Is.EqualTo(secondTransformation));
            Assert.False(Object.ReferenceEquals(firstTransformation, secondTransformation));

            // parse to the concrete type
            TrimLeftTransformation? firstParsed = firstTransformation as TrimLeftTransformation;
            Assert.IsNotNull(firstParsed);
            TrimLeftTransformation? secondParsed = secondTransformation as TrimLeftTransformation;
            Assert.IsNotNull(secondParsed);
            Assert.That(firstParsed, Is.EqualTo(secondParsed));
            Assert.False(Object.ReferenceEquals(firstParsed, secondParsed));
        }

        [Test]
        public void Test_Creation_RightTrimmer()
        {
            const string RIGHT_TRIMMER_TYPE = "trimRight";
            firstTransformation = TextTransformationFactory.CreateTransformation(RIGHT_TRIMMER_TYPE);
            secondTransformation = TextTransformationFactory.CreateTransformation(RIGHT_TRIMMER_TYPE);
            Assert.IsNotNull(firstTransformation);
            Assert.IsNotNull(secondTransformation);
            Assert.That(firstTransformation, Is.EqualTo(secondTransformation));
            Assert.False(Object.ReferenceEquals(firstTransformation, secondTransformation));

            // parse to the concrete type
            TrimRightTransformation? firstParsed = firstTransformation as TrimRightTransformation;
            Assert.IsNotNull(firstParsed);
            TrimRightTransformation? secondParsed = secondTransformation as TrimRightTransformation;
            Assert.IsNotNull(secondParsed);
            Assert.That(firstParsed, Is.EqualTo(secondParsed));
            Assert.False(Object.ReferenceEquals(firstParsed, secondParsed));
        }

        [Test]
        public void Test_Creation_Replacement()
        {
            const string REPLACER_TYPE = "replacement";
            firstTransformation = TextTransformationFactory.CreateTransformation(REPLACER_TYPE, SHORT_BAD_WORD, EXAMPLE_REPLACEMENT);
            secondTransformation = TextTransformationFactory.CreateTransformation(REPLACER_TYPE, SHORT_BAD_WORD, EXAMPLE_REPLACEMENT);
            Assert.IsNotNull(firstTransformation);
            Assert.IsNotNull(secondTransformation);
            Assert.That(firstTransformation, Is.EqualTo(secondTransformation));
            Assert.False(Object.ReferenceEquals(firstTransformation, secondTransformation));

            // parse to the concrete type
            ReplacerTransformation? firstParsed = firstTransformation as ReplacerTransformation;
            Assert.IsNotNull(firstParsed);
            ReplacerTransformation? secondParsed = secondTransformation as ReplacerTransformation;
            Assert.IsNotNull(secondParsed);
            Assert.That(firstParsed, Is.EqualTo(secondParsed));
            Assert.False(Object.ReferenceEquals(firstParsed, secondParsed));
        }

        [Test]
        public void Test_Creation_BasicComposite()
        {
            const string COMPOSE_TYPE = "compose";
            // NOTE THE COMPOSE IS VERY LIMITED HERE
            firstTransformation = TextTransformationFactory.CreateTransformation(COMPOSE_TYPE);
            secondTransformation = TextTransformationFactory.CreateTransformation(COMPOSE_TYPE);
            Assert.IsNotNull(firstTransformation);
            Assert.IsNotNull(secondTransformation);
            Assert.That(firstTransformation, Is.EqualTo(secondTransformation));
            Assert.False(Object.ReferenceEquals(firstTransformation, secondTransformation));

            // parse to the concrete type
            CompositeTransformation? firstParsed = firstTransformation as CompositeTransformation;
            Assert.IsNotNull(firstParsed);
            CompositeTransformation? secondParsed = secondTransformation as CompositeTransformation;
            Assert.IsNotNull(secondParsed);
            Assert.That(firstParsed, Is.EqualTo(secondParsed));
            Assert.False(Object.ReferenceEquals(firstParsed, secondParsed));
        }

        [Test]
        public void Test_Creation_ShortCensorer()
        {
            const string CENSORER_TYPE = "censor";
            firstTransformation = TextTransformationFactory.CreateTransformation(CENSORER_TYPE, SHORT_BAD_WORD);
            secondTransformation = TextTransformationFactory.CreateTransformation(CENSORER_TYPE, SHORT_BAD_WORD);
            Assert.IsNotNull(firstTransformation);
            Assert.IsNotNull(secondTransformation);
            Assert.That(firstTransformation, Is.EqualTo(secondTransformation));
            Assert.True(Object.ReferenceEquals(firstTransformation, secondTransformation)); // the reference is shared as the word is short

            // parse to the concrete type
            CensorerTransformation? firstParsed = firstTransformation as CensorerTransformation;
            Assert.IsNotNull(firstParsed);
            CensorerTransformation? secondParsed = secondTransformation as CensorerTransformation;
            Assert.IsNotNull(secondParsed);
            Assert.That(firstParsed, Is.EqualTo(secondParsed));
            Assert.True(Object.ReferenceEquals(firstParsed, secondParsed)); // the reference is shared as the word is short
        }

        [Test]
        public void Test_Creation_LongCensorer()
        {
            const string CENSORER_TYPE = "censor";
            firstTransformation = TextTransformationFactory.CreateTransformation(CENSORER_TYPE, LONG_BAD_WORD);
            secondTransformation = TextTransformationFactory.CreateTransformation(CENSORER_TYPE, LONG_BAD_WORD);
            Assert.IsNotNull(firstTransformation);
            Assert.IsNotNull(secondTransformation);
            Assert.That(firstTransformation, Is.EqualTo(secondTransformation));
            Assert.False(Object.ReferenceEquals(firstTransformation, secondTransformation)); // the reference is not shared as the word is too long(>4 chars)

            // parse to the concrete type
            CensorerTransformation? firstParsed = firstTransformation as CensorerTransformation;
            Assert.IsNotNull(firstParsed);
            CensorerTransformation? secondParsed = secondTransformation as CensorerTransformation;
            Assert.IsNotNull(secondParsed);
            Assert.That(firstParsed, Is.EqualTo(secondParsed));
            Assert.False(Object.ReferenceEquals(firstParsed, secondParsed)); // the reference is not shared as the word is too long(>4 chars)
        }
    }

    [TestFixture]
    public class DecoratorFactoryTests
    {
        const string TEXT_TO_DECORATE = "today was a sunny day  ";
        const string CENSORER_TRANSFORMATION_TYPE = "censor";
        const string EXAMPLE_BAD_WORD = "sunny";
        const string RIGHT_TRIMMER_TRANSFORMATION_TYPE = "trimRight";
        const string CAPITALIZATION_TRANSFORMATION_TYPE = "capitalize";

        ILabel labelToDecorate;
        List<ITextTransformation> transformations;
        [SetUp]
        public void Setup()
        {
            labelToDecorate = new SimpleLabel(TEXT_TO_DECORATE);
            transformations = new List<ITextTransformation>()
            {
                TextTransformationFactory.CreateTransformation(CENSORER_TRANSFORMATION_TYPE, EXAMPLE_BAD_WORD),
                TextTransformationFactory.CreateTransformation(RIGHT_TRIMMER_TRANSFORMATION_TYPE),
                TextTransformationFactory.CreateTransformation(CAPITALIZATION_TRANSFORMATION_TYPE)
            };
        }

        [Test]
        public void Test_DecoratorCreation_InvalidType()
        {
            Assert.Throws<ArgumentException>(() => DecoratorFactory.CreateDecorator("husky", transformations, labelToDecorate));
        }

        [Test]
        public void Test_DecoratorCreation_TextDecorator()
        {
            const string TEXT_DECORATOR_TYPE = "text";
            ILabel decoratorAsILabel = DecoratorFactory.CreateDecorator(TEXT_DECORATOR_TYPE, transformations, labelToDecorate);
            const string EXPECTED_RESULT = "Today was a ***** day";
            Assert.That(decoratorAsILabel.Text, Is.EqualTo(EXPECTED_RESULT));

            LabelDecoratorBase? decoratorBase = decoratorAsILabel as LabelDecoratorBase;
            Assert.IsNotNull(decoratorBase);
            Assert.That(decoratorBase.Text, Is.EqualTo(EXPECTED_RESULT));

            TextTransformationDecorator? textDecorator = decoratorBase as TextTransformationDecorator;
            Assert.IsNotNull(textDecorator);
            Assert.That(textDecorator.Text, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_DecoratorCreation_CyclicDecorator()
        {
            const string CYCLIC_DECORATOR_TYPE = "cyclic";
            ILabel decoratorAsILabel = DecoratorFactory.CreateDecorator(CYCLIC_DECORATOR_TYPE, transformations, labelToDecorate);
            string EXPECTED_RESULT = "today was a ***** day  ";
            Assert.That(decoratorAsILabel.Text, Is.EqualTo(EXPECTED_RESULT)); // first transformation is applied (censor)

            LabelDecoratorBase? decoratorBase = decoratorAsILabel as LabelDecoratorBase;
            Assert.IsNotNull(decoratorBase);
            EXPECTED_RESULT = "today was a ***** day";
            Assert.That(decoratorBase.Text, Is.EqualTo(EXPECTED_RESULT)); // second transformation is applied (trim right)

            CyclingTransformationsDecorator? cyclicDecorator = decoratorBase as CyclingTransformationsDecorator;
            Assert.IsNotNull(cyclicDecorator);
            EXPECTED_RESULT = "Today was a ***** day";
            Assert.That(cyclicDecorator.Text, Is.EqualTo(EXPECTED_RESULT)); // third transformation is applied (capitalize)
        }

        [Test]
        public void Test_DecoratorCreation_RandomDecorator()
        {
            const string RANDOM_DECORATOR_TYPE = "random";
            ILabel decoratorAsILabel = DecoratorFactory.CreateDecorator(RANDOM_DECORATOR_TYPE, transformations, labelToDecorate);
            string currentText = decoratorAsILabel.Text;

            LabelDecoratorBase? decoratorBase = decoratorAsILabel as LabelDecoratorBase;
            Assert.IsNotNull(decoratorBase);
            currentText = decoratorBase.Text;

            RandomTransformationDecorator? randomDecorator = decoratorBase as RandomTransformationDecorator;
            Assert.IsNotNull(randomDecorator);
            currentText = randomDecorator.Text;

            string EXPECTED_RESULT = "Today was a ***** day";
            Assert.That(randomDecorator.Text, Is.EqualTo(EXPECTED_RESULT));            
        }
    }


}
