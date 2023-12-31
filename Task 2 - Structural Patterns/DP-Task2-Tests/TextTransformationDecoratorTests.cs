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

        const string SAMPLE_RICH_TEXT = "   September   Will Be   May     ";
        const string EMPTY_TEXT = "";
        const string SEPTEMBER = "September";
        const string SUNNY_DAYS = "Non-stop sunny";
        const string SUNNY_RESULT = $"   {SUNNY_DAYS}   Will Be   May     ";

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
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
        }

        // TODO in part 4 the transformations may change - add a transformation for each kind after it has been set with null in the beginning

        // add a style on an empty decorator, on a already having one
        [Test]
        public void Test_TextTransformationDecorator_Later_Initialization()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
            decorator.AddDecorator(replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
        }

        [Test]
        public void Test_TextTransformationDecorator_Later_Initialization_On_Full_Decorator_ChainingChecked()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
            ITextTransformation alternator = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            decorator.AddDecorator(alternator);
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
            decorator = new TextTransformationDecorator(decorator, replacer); // both ways are valid chaining
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
        }


        // remove a style default, remove a style by signifying it and actually receiving the correct, remove a non-existent style

        [Test]
        public void Test_TextTransformationDecorator_RemoveStyle_Default_OnSameMatch()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
            decorator.RemoveDecorator();
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }

        [Test]
        public void Test_TextTransformationDecorator_RemoveStyle_Default_OnNextMatch()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
            decorator = new TextTransformationDecorator(decorator);
            decorator.RemoveDecorator(); // the first invalid transformation will be skipped and it will iterate through the next valid one
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }

        [Test]
        public void Test_TextTransformationDecorator_RemoveStyle_Default_Recursively()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));

            decorator = new TextTransformationDecorator(decorator);
            ITextTransformation antiReplacer = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            decorator = new TextTransformationDecorator(decorator, antiReplacer);
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));

            decorator.RemoveDecorator(); // the first transformation is valid, remove it
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
            decorator.RemoveDecorator(); // the next valid transformation the firstly added one, remove it (skipping the second)
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }

        [Test]
        public void Test_TextTransformationDecorator_RemoveStyle_SpecificMatch_First()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
            ITextTransformation replacerCopy = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            decorator.RemoveDecorator(replacerCopy); // this should guarantee that we have overloaded Equals operator in a meaningful manner (it searches not only addresses but meaning of content)
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }

        [Test]
        public void Test_TextTransformationDecorator_RemoveStyle_SpecificMatch_NextStyle()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
            ITextTransformation replacerAlternative = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            decorator = new TextTransformationDecorator(decorator, replacerAlternative);
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
            ITextTransformation replacerCopy = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);

            decorator.RemoveDecorator(replacerCopy); // this should guarantee that we have overloaded Equals operator in a meaningful manner (it searches not only addresses but meaning of content)
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT)); // The first transformation doesn't replace anything as there is no match in the label string
        }

        [Test]
        public void Test_TextTransformationDecorator_RemoveStyle_SpecificMatch_Middle()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
            ITextTransformation replacerAlternative = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            decorator = new TextTransformationDecorator(decorator, replacerAlternative);
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
            ITextTransformation leftTrimmer = new TrimLeftTransformation();
            decorator = new TextTransformationDecorator(decorator, leftTrimmer);
            Assert.That(decorator.Text, Is.EqualTo("September   Will Be   May     "));
            ITextTransformation rightTrimmer = new TrimRightTransformation();
            decorator = new TextTransformationDecorator(decorator, rightTrimmer);
            Assert.That(decorator.Text, Is.EqualTo("September   Will Be   May"));

            decorator.RemoveDecorator(replacerAlternative); // this should guarantee that we have overloaded Equals operator in a meaningful manner (it searches not only addresses but meaning of content)
            Assert.That(decorator.Text, Is.EqualTo($"{SUNNY_DAYS}   Will Be   May")); // The first transformation doesn't replace anything as there is no match in the label string
        }

        [Test]
        public void Test_TextTransformationDecorator_RemoveStyle_SpecificMatch_OnlyLabelRemains()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
            decorator.RemoveDecorator(replacer); // this should guarantee that we have overloaded Equals operator in a meaningful manner (it searches not only addresses but meaning of content)
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT)); // The first transformation doesn't replace anything as there is no match in the label string
        }

        [Test]
        public void Test_TextTransformationDecorator_RemoveStyle_SpecificNoMatch_TypeMissmatch_OnFirst()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
            ITextTransformation spaceNormalizer = new SpaceNormalizationTransformation();
            decorator.RemoveDecorator(spaceNormalizer); // this should guarantee that we have overloaded Equals operator in a meaningful manner (it searches not only addresses but meaning of content)
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
        }

        [Test]
        public void Test_TextTransformationDecorator_RemoveStyle_SpecificNoMatch_ContentMissmatch_OnFirst()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
            ITextTransformation replacerNotMatchingCopy = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            decorator.RemoveDecorator(replacerNotMatchingCopy); // this should guarantee that we have overloaded Equals operator in a meaningful manner (it searches not only addresses but meaning of content)
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
        }

        [Test]
        public void Test_TextTransformationDecorator_RemoveStyle_SpecificNoMatch_ContentMissmatch_OnAll()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));

            ITextTransformation alternative = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            decorator = new TextTransformationDecorator(decorator, alternative);
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));

            ITextTransformation spaceNormalizer = new SpaceNormalizationTransformation();
            decorator = new TextTransformationDecorator(decorator, spaceNormalizer);
            const string EXPECTED_RESULT = " September Will Be May ";
            Assert.That(decorator.Text, Is.EqualTo(EXPECTED_RESULT));

            ITextTransformation searchedRemoval = new ReplacerTransformation(SUNNY_DAYS, "wakanda");
            decorator.RemoveDecorator(searchedRemoval);
            Assert.That(decorator.Text, Is.EqualTo(EXPECTED_RESULT));
            searchedRemoval = new ReplacerTransformation("Pernik", SUNNY_DAYS);
            decorator.RemoveDecorator(searchedRemoval);
            Assert.That(decorator.Text, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_TextTransformationDecorator_ChainingDecorators_SecondNull()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
            replacer = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            decorator = new TextTransformationDecorator(decorator, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));

            TextTransformationDecorator secondDecorator = new TextTransformationDecorator(label, null);
            Assert.That(secondDecorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));

            decorator.AddDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }


        // BaseLabelDecorator with 2 styles
        // ADD A DECORATOR WITH 2 styles, only one style, null styles
        // Remove a decorator with 2 styles, only one style, null styles(one zero or 2 matching)
        // The removal needs to be from the first, the second place or the final

        // Repeat the tests for transformations but they are only one

        [Test]
        public void Test_TextTransformationDecorator_ChainingDecorators_MultipleTransformations()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SUNNY_RESULT));
            replacer = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            decorator = new TextTransformationDecorator(decorator, replacer);
            Assert.That(decorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT));

            TextTransformationDecorator secondDecorator = new TextTransformationDecorator(label, new TrimLeftTransformation());
            Assert.That(secondDecorator.Text, Is.EqualTo("September   Will Be   May     "));
            secondDecorator.AddDecorator(new TrimRightTransformation());
            Assert.That(secondDecorator.Text, Is.EqualTo("September   Will Be   May"));

            decorator.AddDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo("September   Will Be   May"));

            secondDecorator = new TextTransformationDecorator(label, new SpaceNormalizationTransformation());
            decorator.AddDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo("September Will Be May"));
        }

        [Test]
        public void Test_TextTransformationDecorator_RemovingChained_TextTransformationDecorators_Recursively()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);

            replacer = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            decorator.AddDecorator(replacer);
            decorator.AddDecorator(new TrimLeftTransformation());
            decorator.AddDecorator(new TrimRightTransformation());
            decorator.AddDecorator(new SpaceNormalizationTransformation());
            Assert.That(decorator.Text, Is.EqualTo("September Will Be May"));

            // null shouldn't change anything
            TextTransformationDecorator secondDecorator = new TextTransformationDecorator(label, null);
            decorator.RemoveDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo("September Will Be May"));

            // only the trim left transformation should be removed
            secondDecorator = new TextTransformationDecorator(label, new TrimLeftTransformation());
            decorator.RemoveDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo(" September Will Be May"));

            // multiple decorator removal - only space normalization and sunny days converter will remain after it
            secondDecorator = new TextTransformationDecorator(label, new TrimRightTransformation());
            secondDecorator = new TextTransformationDecorator(secondDecorator, new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER));
            decorator.RemoveDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May "));

            // add a new style which may match one of the few decorators, the result will end up the same
            decorator.AddDecorator(new TrimRightTransformation());
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May"));
            decorator.RemoveDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May "));
        }

        [Test]
        public void Test_TextTransformationDecorator_RemovingChained_CyclicTransformationDecorator_Recursively()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);

            replacer = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            decorator.AddDecorator(replacer);
            decorator.AddDecorator(new TrimLeftTransformation());
            decorator.AddDecorator(new TrimRightTransformation());
            decorator.AddDecorator(new SpaceNormalizationTransformation());
            Assert.That(decorator.Text, Is.EqualTo("September Will Be May"));

            // null shouldn't change anything
            CyclingTransformationsDecorator secondDecorator = new CyclingTransformationsDecorator(label);
            decorator.RemoveDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo("September Will Be May"));

            // only the trim left transformation should be removed
            secondDecorator.AddDecorator(new TrimLeftTransformation());
            decorator.RemoveDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo(" September Will Be May"));

            // multiple decorator removal - only space normalization and sunny days converter will remain after it
            secondDecorator.ResetStyles();
            secondDecorator.AddDecorator(new TrimRightTransformation());
            secondDecorator.AddDecorator(new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER));
            decorator.RemoveDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May "));

            // add a new style which may match one of the few decorators, the result will end up the same
            decorator.AddDecorator(new TrimRightTransformation());
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May"));
            decorator.RemoveDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May "));
        }

        [Test]
        public void Test_TextTransformationDecorator_RemovingChained_RandomTransformationDecorator_Recursively()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);

            replacer = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            decorator.AddDecorator(replacer);
            decorator.AddDecorator(new TrimLeftTransformation());
            decorator.AddDecorator(new TrimRightTransformation());
            decorator.AddDecorator(new SpaceNormalizationTransformation());
            Assert.That(decorator.Text, Is.EqualTo("September Will Be May"));

            // null shouldn't change anything
            RandomTransformationDecorator secondDecorator = new RandomTransformationDecorator(label);
            decorator.RemoveDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo("September Will Be May"));

            // only the trim left transformation should be removed
            secondDecorator.AddDecorator(new TrimLeftTransformation());
            decorator.RemoveDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo(" September Will Be May"));

            // multiple decorator removal - only space normalization and sunny days converter will remain after it
            secondDecorator.ResetStyles();
            secondDecorator.AddDecorator(new TrimRightTransformation());
            secondDecorator.AddDecorator(new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER));
            decorator.RemoveDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May "));

            // add a new style which may match one of the few decorators, the result will end up the same
            decorator.AddDecorator(new TrimRightTransformation());
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May"));
            decorator.RemoveDecorator(secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May "));
        }

        [Test]
        public void Test_TextTransformationDecorator_RemovingChainedDecorators_StaticFunction_Styles()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);

            replacer = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            decorator.AddDecorator(replacer);
            decorator.AddDecorator(new TrimLeftTransformation());
            decorator.AddDecorator(new TrimRightTransformation());
            decorator.AddDecorator(new SpaceNormalizationTransformation());
            Assert.That(decorator.Text, Is.EqualTo("September Will Be May"));

            // null shouldn't change anything
            ITextTransformation? styleToRemove = null;
            LabelDecoratorBase.RemoveDecoratorFrom(decorator, styleToRemove);
            Assert.That(decorator.Text, Is.EqualTo("September Will Be May"));

            // only the trim left transformation should be removed
            styleToRemove = new TrimLeftTransformation();
            LabelDecoratorBase.RemoveDecoratorFrom(decorator, styleToRemove);
            Assert.That(decorator.Text, Is.EqualTo(" September Will Be May"));

            // remove the trimright
            styleToRemove = new TrimRightTransformation();
            LabelDecoratorBase.RemoveDecoratorFrom(decorator, styleToRemove);
            Assert.That(decorator.Text, Is.EqualTo($" September Will Be May "));

            // no more replacing of sunny with september
            styleToRemove = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            LabelDecoratorBase.RemoveDecoratorFrom(decorator, styleToRemove);
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May "));

            // add a new style which may match one of the few decorators, the result will end up the same
            decorator.AddDecorator(new TrimRightTransformation());
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May"));
            LabelDecoratorBase.RemoveDecoratorFrom(decorator, new TrimRightTransformation());
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May "));
        }

        [Test]
        public void Test_TextTransformationDecorator_RemovingChainedDecorators_StaticFunction_Decorators()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            TextTransformationDecorator decorator = new TextTransformationDecorator(label, replacer);

            replacer = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            decorator.AddDecorator(replacer);
            decorator.AddDecorator(new TrimLeftTransformation());
            decorator.AddDecorator(new TrimRightTransformation());
            decorator.AddDecorator(new SpaceNormalizationTransformation());
            Assert.That(decorator.Text, Is.EqualTo("September Will Be May"));

            // null shouldn't change anything
            TextTransformationDecorator secondDecorator = new TextTransformationDecorator(label, null);
            LabelDecoratorBase.RemoveDecoratorFrom(decorator, secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo("September Will Be May"));

            // only the trim left transformation should be removed
            secondDecorator = new TextTransformationDecorator(label, new TrimLeftTransformation());
            LabelDecoratorBase.RemoveDecoratorFrom(decorator, secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo(" September Will Be May"));

            // multiple decorator removal - only space normalization and sunny days converter will remain after it
            secondDecorator = new TextTransformationDecorator(label, new TrimRightTransformation());
            secondDecorator = new TextTransformationDecorator(secondDecorator, new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER));
            LabelDecoratorBase.RemoveDecoratorFrom(decorator, secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May "));

            // add a new style which may match one of the few decorators, the result will end up the same
            decorator.AddDecorator(new TrimRightTransformation());
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May"));
            LabelDecoratorBase.RemoveDecoratorFrom(decorator, secondDecorator);
            Assert.That(decorator.Text, Is.EqualTo($" {SUNNY_DAYS} Will Be May "));
        }

    }
}
