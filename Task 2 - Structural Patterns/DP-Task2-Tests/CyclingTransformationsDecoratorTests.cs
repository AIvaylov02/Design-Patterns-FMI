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
        const string SUNNY_RESULT = "   Non-stop sunny   Will Be   May     ";

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

            Assert.That(alternatingDecorator.Text, Is.EqualTo(SUNNY_RESULT)); // Non-stop sunny replaces September
            Assert.That(alternatingDecorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT)); // September gets back by replacing Non-stop sunny
            Assert.That(alternatingDecorator.Text, Is.EqualTo(SUNNY_RESULT)); // Non-stop sunny replaces September
            Assert.That(alternatingDecorator.Text, Is.EqualTo(SAMPLE_RICH_TEXT)); // September gets back by replacing Non-stop sunny
        }

        [Test]
        public void Test_CyclicDecorator_RemoveStyle_Default_OnSameMatch()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
            };
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);
            Assert.That(dec.Text, Is.EqualTo(SUNNY_RESULT));

            dec.RemoveDecorator();
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }

        [Test]
        public void Test_CyclicDecorator_RemoveStyle_Default_NoStyles()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>();
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));

            dec.RemoveDecorator(); // nothing will happen
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }

        [Test]
        public void Test_CyclicDecorator_RemoveStyle_Default_MovedState()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                // bad word for first will be a replacement for second and vice versa
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER),
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
            };
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);
            Assert.That(dec.Text, Is.EqualTo(SUNNY_RESULT));
            dec.RemoveDecorator();
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT)); // apply the next transformation which does nothing
            Assert.That(dec.Text, Is.EqualTo(SUNNY_RESULT)); // the third transformation
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT)); // it is back to the second(now first transformation)
        }

        [Test]
        public void Test_CyclicDecorator_ResetStyles_Empty()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>();
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
            dec.ResetStyles(); // nothing will happen
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }

        [Test]
        public void Test_CyclicDecorator_ResetStyles_NonEmpty()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                // bad word for first will be a replacement for second and vice versa
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER),
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
            };
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);
            Assert.That(dec.Text, Is.EqualTo(SUNNY_RESULT));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
            dec.ResetStyles(); // all styles are gone, we are left with the OG label
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }

        [Test]
        public void Test_CyclicDecorator_RemoveStyle_Specific_EmptyStyles()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>();
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));

            ITextTransformation styleToRemove = new TrimLeftTransformation();
            dec.RemoveDecorator(styleToRemove);// no style is matched
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
        }

        [Test]
        public void Test_CyclicDecorator_RemoveStyle_Specific_FirstStyle()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new TrimLeftTransformation(),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER),
                new TrimRightTransformation(),
                new TrimLeftTransformation()
            };
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);

            ITextTransformation styleToRemove = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            dec.RemoveDecorator(styleToRemove);// first style is matched
            Assert.That(dec.Text, Is.EqualTo("September   Will Be   May     ")); // apply the next transformation
            transformations.Remove(styleToRemove);
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
        }

        [Test]
        public void Test_CyclicDecorator_RemoveStyle_Specific_MiddelStyle()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new TrimLeftTransformation(),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER),
                new TrimRightTransformation(),
                new TrimLeftTransformation()
            };
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);

            ITextTransformation styleToRemove = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            dec.RemoveDecorator(styleToRemove); // the 3rd style is matched
            Assert.That(dec.Text, Is.EqualTo(SUNNY_RESULT));
            transformations.Remove(styleToRemove);
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
        }

        [Test]
        public void Test_CyclicDecorator_RemoveStyle_Specific_TwoMatchingPickLast()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new TrimLeftTransformation(),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER),
                new TrimRightTransformation(),
                new TrimLeftTransformation()
            };
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);

            ITextTransformation styleToRemove = new TrimLeftTransformation();
            dec.RemoveDecorator(styleToRemove);// the 5th style is matched
            Assert.That(dec.Text, Is.EqualTo(SUNNY_RESULT));
            transformations.RemoveAt(4);
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
        }

        [Test]
        public void Test_CyclicDecorator_AddStyle_Null()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new TrimLeftTransformation(),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER),
                new TrimRightTransformation(),
            };
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);

            ITextTransformation? styleToAdd = null;
            dec.AddDecorator(styleToAdd);// the style won't be added
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
        }

        [Test]
        public void Test_CyclicDecorator_AddStyle_Single()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new TrimLeftTransformation(),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER),
                new TrimRightTransformation(),
            };
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);

            ITextTransformation? styleToAdd = new TrimLeftTransformation();
            dec.AddDecorator(styleToAdd);// a 5th style will be added
            transformations.Add(styleToAdd);
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
        }

        [Test]
        public void Test_CyclicDecorator_RemoveDecorator_TextTransformation()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new TrimLeftTransformation(),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER),
                new TrimRightTransformation()
            };
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);

            LabelDecoratorBase remover = new TextTransformationDecorator(label);
            dec.RemoveDecorator(remover); // no styles will be removed
            Assert.That(dec.Text, Is.EqualTo(SUNNY_RESULT));
            Assert.That(dec.Text, Is.EqualTo($"{SUNNY_DAYS}   Will Be   May     "));
            Assert.That(dec.Text, Is.EqualTo($"September   Will Be   May     "));
            Assert.That(dec.Text, Is.EqualTo("September   Will Be   May"));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));

            // remove multiple
            remover.AddDecorator(new TrimRightTransformation());
            remover.AddDecorator(new TrimLeftTransformation());
            dec.RemoveDecorator(remover); // the trims will be removed, but the styles are still in play as we save an intermediate state
            transformations.Remove(new TrimRightTransformation());
            transformations.Remove(new TrimLeftTransformation());
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
            Assert.That(dec.Text, Is.EqualTo($"{SUNNY_DAYS}   Will Be   May"));
            Assert.That(dec.Text, Is.EqualTo("September   Will Be   May"));

            // remove a single
            remover.RemoveDecorator(new TrimRightTransformation());
            remover.AddDecorator(new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS));
            dec.RemoveDecorator(remover); // the first decorator will be removed and the state will change after the next transformation
            transformations.Remove(new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
            Assert.That(dec.Text, Is.EqualTo($"September   Will Be   May"));
            // no more transformations will be applied this is the final state
            Assert.That(dec.Text, Is.EqualTo($"September   Will Be   May"));
        }

        [Test]
        public void Test_CyclicDecorator_RemoveDecorator_CyclicDecorator()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> trimmers = new List<ITextTransformation>()
            {
                new TrimLeftTransformation(),
                new TrimRightTransformation()
            };

            List<ITextTransformation> alternatingReplacers = new List<ITextTransformation>()
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER)
            };

            List<ITextTransformation> transformations = new List<ITextTransformation>(trimmers);
            transformations.AddRange(alternatingReplacers);

            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);

            LabelDecoratorBase remover = new CyclingTransformationsDecorator(label);
            dec.RemoveDecorator(remover); // no styles will be removed
            Assert.That(dec.Text, Is.EqualTo("September   Will Be   May     "));
            Assert.That(dec.Text, Is.EqualTo($"September   Will Be   May"));
            Assert.That(dec.Text, Is.EqualTo($"{SUNNY_DAYS}   Will Be   May"));
            Assert.That(dec.Text, Is.EqualTo("September   Will Be   May"));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));

            // remove multiple
            remover.ApplyStylesFromList(trimmers);
            dec.RemoveDecorator(remover); // the trims will be removed, but the styles are still in play as we save an intermediate state
            transformations.Remove(new TrimRightTransformation());
            transformations.Remove(new TrimLeftTransformation());
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
            Assert.That(dec.Text, Is.EqualTo($"{SUNNY_DAYS}   Will Be   May"));
            Assert.That(dec.Text, Is.EqualTo("September   Will Be   May"));

            // remove a single
            remover.RemoveDecorator(new TrimRightTransformation());
            remover.AddDecorator(new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS));
            dec.RemoveDecorator(remover); // the first decorator will be removed and the state will change after the next transformation
            transformations.Remove(new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
            Assert.That(dec.Text, Is.EqualTo($"September   Will Be   May"));
            // no more transformations will be applied this is the final state
            Assert.That(dec.Text, Is.EqualTo($"September   Will Be   May"));
        }

        [Test]
        public void Test_CyclicDecorator_RemoveDecorator_RandomDecorator()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new TrimLeftTransformation(),
                new TrimRightTransformation(),
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER)
            };

            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label, transformations);
            RandomTransformationDecorator randDec = new RandomTransformationDecorator(label);
            dec.RemoveDecorator(randDec); // nothing should be removed from the styles
            Assert.That(dec.Text, Is.EqualTo("September   Will Be   May     "));
            Assert.That(dec.Text, Is.EqualTo($"September   Will Be   May"));
            Assert.That(dec.Text, Is.EqualTo($"{SUNNY_DAYS}   Will Be   May"));
            Assert.That(dec.Text, Is.EqualTo("September   Will Be   May"));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));

            randDec.ApplyStylesFromList(transformations);
            // 2 styles are already applied 2 are still left for transformation
            string temp = randDec.Text;
            temp = randDec.Text;
            // all of them will be matched(removed from) the cyclic lable
            dec.RemoveDecorator(randDec);
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT)); // it will return its original label as there are no more styles applied
            Assert.That(dec.Transformations, Is.EqualTo(new List<ITextTransformation>()));
        }

        // add styles from decorators
        [Test]
        public void Test_CyclicDecorator_AddDecorator()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new TrimLeftTransformation(),
                new TrimRightTransformation(),
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER)
            };

            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label);
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
            Assert.That(dec.Transformations, Is.EqualTo(new List<ITextTransformation>()));

            CyclingTransformationsDecorator cyclic = new CyclingTransformationsDecorator(label, transformations);
            dec.AddDecorator(cyclic);
            Assert.That(dec.Text, Is.EqualTo("September   Will Be   May     "));
            const string TRIMMED_SUNNY_RESULT = $"{SUNNY_DAYS}   Will Be   May";
            const string TRIMMED_SEPTEMBER = "September   Will Be   May";
            Assert.That(dec.Text, Is.EqualTo(TRIMMED_SEPTEMBER));
            Assert.That(dec.Text, Is.EqualTo(TRIMMED_SUNNY_RESULT));
            Assert.That(dec.Text, Is.EqualTo(TRIMMED_SEPTEMBER));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));

            TextTransformationDecorator textDecorator = new TextTransformationDecorator(label);
            textDecorator.ApplyStylesFromList(transformations);
            dec.AddDecorator(textDecorator);// it will just cycle through non-stop sunny trimmed and trimmed september as we have already trimmed the text
            Assert.That(dec.Text, Is.EqualTo(TRIMMED_SEPTEMBER));
            Assert.That(dec.Text, Is.EqualTo(TRIMMED_SEPTEMBER));
            Assert.That(dec.Text, Is.EqualTo(TRIMMED_SUNNY_RESULT));
            Assert.That(dec.Text, Is.EqualTo(TRIMMED_SEPTEMBER));

            List<ITextTransformation> buffedTransformations = new List<ITextTransformation>(transformations);
            buffedTransformations.AddRange(transformations);
            Assert.That(dec.Transformations, Is.EqualTo(buffedTransformations));

            RandomTransformationDecorator randDec = new RandomTransformationDecorator(label, transformations);
            dec.AddDecorator(randDec);
            Assert.That(dec.Text, Is.EqualTo(TRIMMED_SEPTEMBER));
            Assert.That(dec.Text, Is.EqualTo(TRIMMED_SEPTEMBER));
            Assert.That(dec.Text, Is.EqualTo(TRIMMED_SUNNY_RESULT));
            Assert.That(dec.Text, Is.EqualTo(TRIMMED_SEPTEMBER));
            buffedTransformations.AddRange(transformations);
            Assert.That(dec.Transformations, Is.EqualTo(buffedTransformations));
        }

        [Test]
        public void Test_CyclicDecorator_RemovingDecorators_StaticFunction_Styles()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label);
            dec.AddDecorator(replacer);
            replacer = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            dec.AddDecorator(replacer);
            dec.AddDecorator(new TrimLeftTransformation());
            dec.AddDecorator(new TrimRightTransformation());
            dec.AddDecorator(new SpaceNormalizationTransformation());
            _ = dec.Text;
            _ = dec.Text;
            _ = dec.Text;
            _ = dec.Text;
            _ = dec.Text;
            const string SEPTEMBER_TRIMMED = "September Will Be May";
            const string SUNNY_TRIMMED = $"{SUNNY_DAYS} Will Be May";
            Assert.That(dec.Text, Is.EqualTo(SUNNY_TRIMMED));
            List<ITextTransformation> transformations = new List<ITextTransformation>(dec.Transformations);

            // null shouldn't change anything
            ITextTransformation? styleToRemove = null;
            LabelDecoratorBase.RemoveDecoratorFrom(dec, styleToRemove);
            Assert.That(dec.Text, Is.EqualTo(SEPTEMBER_TRIMMED));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));

            // only the trim left transformation should be removed
            styleToRemove = new TrimLeftTransformation();
            LabelDecoratorBase.RemoveDecoratorFrom(dec, styleToRemove);
            transformations.Remove(styleToRemove);
            Assert.That(dec.Text, Is.EqualTo(SEPTEMBER_TRIMMED));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));

            // remove the trimright
            styleToRemove = new TrimRightTransformation();
            LabelDecoratorBase.RemoveDecoratorFrom(dec, styleToRemove);
            transformations.Remove(styleToRemove);
            Assert.That(dec.Text, Is.EqualTo(SEPTEMBER_TRIMMED));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));

            // no more replacing of sunny with september
            styleToRemove = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            LabelDecoratorBase.RemoveDecoratorFrom(dec, styleToRemove);
            transformations.Remove(styleToRemove);
            Assert.That(dec.Text, Is.EqualTo(SEPTEMBER_TRIMMED));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));

            // add a new style which may match one of the few decorators, the result will end up the same
            dec.AddDecorator(new TrimRightTransformation());
            Assert.That(dec.Text, Is.EqualTo(SUNNY_TRIMMED));
            LabelDecoratorBase.RemoveDecoratorFrom(dec, new TrimRightTransformation());
            Assert.That(dec.Text, Is.EqualTo(SUNNY_TRIMMED));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
        }

        [Test]
        public void Test_CyclicDecorator_RemovingDecorators_StaticFunction_Decorators()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            CyclingTransformationsDecorator dec = new CyclingTransformationsDecorator(label);
            dec.AddDecorator(replacer);
            // "   September   Will Be   May     "
            replacer = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            dec.AddDecorator(replacer);
            dec.AddDecorator(new TrimLeftTransformation());
            dec.AddDecorator(new TrimRightTransformation());
            dec.AddDecorator(new SpaceNormalizationTransformation());
            _ = dec.Text;
            _ = dec.Text;
            _ = dec.Text;
            _ = dec.Text;
            _ = dec.Text;
            const string SEPTEMBER_TRIMMED = "September Will Be May";
            const string SUNNY_TRIMMED = $"{SUNNY_DAYS} Will Be May";
            Assert.That(dec.Text, Is.EqualTo(SUNNY_TRIMMED));
            List<ITextTransformation> transformations = new List<ITextTransformation>(dec.Transformations);

            // null shouldn't change anything
            TextTransformationDecorator secondDecorator = new TextTransformationDecorator(label, null);
            LabelDecoratorBase.RemoveDecoratorFrom(dec, secondDecorator);
            Assert.That(dec.Transformations, Is.EqualTo(transformations));

            // only the trim left transformation should be removed
            secondDecorator = new TextTransformationDecorator(label, new TrimLeftTransformation());
            LabelDecoratorBase.RemoveDecoratorFrom(dec, secondDecorator); // we remove a decorator but the state remains unchanged(until all of them are null styles)
            Assert.That(dec.Text, Is.EqualTo(SEPTEMBER_TRIMMED));
            transformations.Remove(secondDecorator.ExtractTransformationsToOuterWorld().ToList()[0]);
            Assert.That(dec.Transformations, Is.EqualTo(transformations));

            // multiple decorator removal - only space normalization and sunny days converter will remain after it
            secondDecorator = new TextTransformationDecorator(label, new TrimRightTransformation());
            secondDecorator = new TextTransformationDecorator(secondDecorator, new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER));
            LabelDecoratorBase.RemoveDecoratorFrom(dec, secondDecorator);
            List<ITextTransformation> appliedTransformations = secondDecorator.ExtractTransformationsToOuterWorld().ToList();
            transformations.Remove(appliedTransformations[0]);
            transformations.Remove(appliedTransformations[1]);
            Assert.That(dec.Text, Is.EqualTo(SUNNY_TRIMMED));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));

            // add a new style which may match one of the few decorators, the result will end up the same
            dec.AddDecorator(new TrimRightTransformation());
            Assert.That(dec.Text, Is.EqualTo(SUNNY_TRIMMED));
            LabelDecoratorBase.RemoveDecoratorFrom(dec, secondDecorator);
            Assert.That(dec.Text, Is.EqualTo(SUNNY_TRIMMED));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
        }
    }
}
