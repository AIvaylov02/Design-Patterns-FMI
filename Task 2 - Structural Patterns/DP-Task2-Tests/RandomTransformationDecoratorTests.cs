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
        const string SEPTEMBER = "September";
        const string SUNNY_DAYS = "Non-stop sunny";
        const string SUNNY_RESULT = "   Non-stop sunny   Will Be   May     ";

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

        // from here onwards change them!!!!!
        //
        //
        [Test]
        public void Test_RandomDecorator_RemoveStyles_Default()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new TrimLeftTransformation(),
                new TrimRightTransformation()
            };
            RandomTransformationDecorator dec = new RandomTransformationDecorator(label, transformations);
            _ = dec.Text; // transfers a style to already applied

            List<ITextTransformation> alreadyApplied = dec.AlreadyApplied.ToList();
            List<ITextTransformation> awaiting = dec.Transformations.ToList();
            dec.RemoveDecorator();
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            alreadyApplied.RemoveAt(0);
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));


            dec.RemoveDecorator(); // remove from the list of styles awaiting still
            awaiting.RemoveAt(0);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            dec.RemoveDecorator();
            awaiting.RemoveAt(0);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            dec.RemoveDecorator(); // nothing happens as all styles are empty now
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));// return the default text
        }

        [Test]
        public void Test_RandomDecorator_ResetStyles_Empty()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>();
            RandomTransformationDecorator dec = new RandomTransformationDecorator(label, transformations);
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(transformations));
            dec.ResetStyles(); // nothing will happen
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(transformations));
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
        }

        [Test]
        public void Test_RandomDecorator_ResetStyles_NonEmpty()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                // bad word for first will be a replacement for second and vice versa
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER),
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
            };
            RandomTransformationDecorator dec = new RandomTransformationDecorator(label, transformations);

            _ = dec.Text;
            List<ITextTransformation> alreadyApplied = dec.AlreadyApplied.ToList();
            List<ITextTransformation> awaiting = dec.Transformations.ToList();
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));

            dec.ResetStyles(); // all styles are gone, we are left with the OG label
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
            alreadyApplied = new List<ITextTransformation>();
            Assert.That(dec.Transformations, Is.EqualTo(alreadyApplied));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));
        }

        [Test]
        public void Test_RandomDecorator_RemoveStyle_Specific_EmptyStyles()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>();
            RandomTransformationDecorator dec = new RandomTransformationDecorator(label, transformations);
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));

            ITextTransformation styleToRemove = new TrimLeftTransformation();
            dec.RemoveDecorator(styleToRemove);// no style is matched
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(transformations));
        }

        [Test]
        public void Test_RandomDecorator_RemoveStyle_Specific_PriorityGuaranteed()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new TrimRightTransformation(),
                new TrimLeftTransformation()
            };
            RandomTransformationDecorator dec = new RandomTransformationDecorator(label, transformations);
            _ = dec.Text;
            Assert.That(dec.Text, Is.EqualTo("September   Will Be   May"));
            // all of these transformations are alreadyApplied, add a duplicate of the first in the awaiting category

            ITextTransformation styleToRemove = new TrimRightTransformation();
            dec.AddDecorator(styleToRemove);// first style is matched
            List<ITextTransformation> alreadyApplied = dec.AlreadyApplied.ToList();
            List<ITextTransformation> awaiting = dec.Transformations.ToList();

            dec.RemoveDecorator(styleToRemove); // the already trimmed right will be matched
            alreadyApplied.Remove(styleToRemove);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));

            dec.RemoveDecorator(styleToRemove); // no trim right in the alreadyApplied, the new style in awaiting will be removed
            awaiting.Remove(styleToRemove);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied)); // only trim left remains in the alreadyApplied list
        }

        
        [Test]
        public void Test_RandomDecorator_AddStyle()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new TrimLeftTransformation(),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER),
                new TrimRightTransformation(),
            };
            RandomTransformationDecorator dec = new RandomTransformationDecorator(label, transformations);

            ITextTransformation? styleToAdd = null;
            dec.AddDecorator(styleToAdd);// the style won't be added
            Assert.That(dec.Transformations, Is.EqualTo(transformations));

            styleToAdd = new TrimLeftTransformation();
            dec.AddDecorator(styleToAdd);// a 5th style will be added
            transformations.Add(styleToAdd);
            Assert.That(dec.Transformations, Is.EqualTo(transformations));
        }

        [Test]
        public void Test_RandomDecorator_RemoveDecorator_TextTransformation()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new TrimLeftTransformation(),
                new SpaceNormalizationTransformation(),
                new TrimRightTransformation()
            };
            RandomTransformationDecorator dec = new RandomTransformationDecorator(label, transformations);
            const int TRANSFORMATIONS_TO_APPLY = 4;
            for (int i = 0; i < TRANSFORMATIONS_TO_APPLY; i++)
            {
                _ = dec.Text;
            }
            dec.AddDecorator(new TrimLeftTransformation());
            dec.AddDecorator(new SpaceNormalizationTransformation());
            // 4 in already applied, 2 in awaiting transformations
            List<ITextTransformation> alreadyApplied = dec.AlreadyApplied.ToList();
            List<ITextTransformation> awaiting = dec.Transformations.ToList();

            // null removal, Currently AlreadyApplied(Replacer, Left, Space, Right), Awaiting(Left, Space)
            LabelDecoratorBase remover = new TextTransformationDecorator(label);
            dec.RemoveDecorator(remover); // no styles will be removed
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));


            // remove one trimLeft, it will be taken from the already applied. Styles will be AlreadyApplied(Replacer, Space, Right), Awaiting(Left, Space)
            transformations.Clear();
            transformations.Add(new TrimLeftTransformation());
            remover.ApplyStylesFromList(transformations);
            dec.RemoveDecorator(remover);
            alreadyApplied.Remove(transformations[0]);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));


            // one space(from alreadyApplied), trimLeft(from awaiting), replacer(from alreadyApplied), space (from awaiting)
            // Styles will be AlreadyApplied(Right), Awaiting()
            transformations.Insert(0, new SpaceNormalizationTransformation());
            transformations.Add(new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS));
            transformations.Add(new SpaceNormalizationTransformation());
            transformations.Add(new TrimLeftTransformation()); // add a non-matching transformation, it will be skipped (we have removed all left trimmers)
            remover = new TextTransformationDecorator(label); // reset its styles

            remover.ApplyStylesFromList(transformations);
            dec.RemoveDecorator(remover);
            alreadyApplied.Remove(transformations[0]);
            alreadyApplied.Remove(transformations[2]);
            awaiting.RemoveAt(0);
            awaiting.RemoveAt(0);
            // only trimRight willRemain
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));
        }

        [Test]
        public void Test_RandomDecorator_RemoveDecorator_CyclicDecorator()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new TrimLeftTransformation(),
                new SpaceNormalizationTransformation(),
                new TrimRightTransformation()
            };
            RandomTransformationDecorator dec = new RandomTransformationDecorator(label, transformations);
            _ = dec.Text;
            _ = dec.Text;
            _ = dec.Text;
            _ = dec.Text;
            dec.AddDecorator(new TrimLeftTransformation());
            dec.AddDecorator(new SpaceNormalizationTransformation());
            // 4 in already applied, 2 in awaiting transformations
            List<ITextTransformation> alreadyApplied = dec.AlreadyApplied.ToList();
            List<ITextTransformation> awaiting = dec.Transformations.ToList();

            // null removal
            LabelDecoratorBase remover = new CyclingTransformationsDecorator(label);
            dec.RemoveDecorator(remover); // no styles will be removed
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));


            // remove one trimLeft, it will be taken from the already applied
            transformations.Clear();
            transformations.Add(new TrimLeftTransformation());
            remover.ApplyStylesFromList(transformations);
            dec.RemoveDecorator(remover);
            alreadyApplied.Remove(transformations[0]);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));


            // one space(from alreadyApplied), trimLeft(from awaiting), replacer(from alreadyApplied), space (from awaiting)
            transformations.Insert(0, new SpaceNormalizationTransformation());
            transformations.Add(new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS));
            transformations.Add(new SpaceNormalizationTransformation());
            transformations.Add(new TrimLeftTransformation()); // add a non-matching transformation, it will be skipped (we have removed all left trimmers)
            remover = new CyclingTransformationsDecorator(label); // reset its styles

            remover.ApplyStylesFromList(transformations);
            dec.RemoveDecorator(remover);
            alreadyApplied.Remove(transformations[0]);
            alreadyApplied.Remove(transformations[2]);
            awaiting.RemoveAt(0);
            awaiting.RemoveAt(0);
            // only trimRight willRemain
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));
        }

        [Test]
        public void Test_RandomDecorator_RemoveDecorator_RandomDecorator()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new TrimLeftTransformation(),
                new SpaceNormalizationTransformation(),
                new TrimRightTransformation()
            };
            RandomTransformationDecorator dec = new RandomTransformationDecorator(label, transformations);
            _ = dec.Text;
            _ = dec.Text;
            _ = dec.Text;
            _ = dec.Text;
            dec.AddDecorator(new TrimLeftTransformation());
            dec.AddDecorator(new SpaceNormalizationTransformation());
            // 4 in already applied, 2 in awaiting transformations
            List<ITextTransformation> alreadyApplied = dec.AlreadyApplied.ToList();
            List<ITextTransformation> awaiting = dec.Transformations.ToList();

            // null removal
            LabelDecoratorBase remover = new RandomTransformationDecorator(label);
            dec.RemoveDecorator(remover); // no styles will be removed
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));


            // remove one trimLeft, it will be taken from the already applied
            transformations.Clear();
            transformations.Add(new TrimLeftTransformation());
            remover.ApplyStylesFromList(transformations);
            dec.RemoveDecorator(remover);
            alreadyApplied.Remove(transformations[0]);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));


            // one space(from alreadyApplied), trimLeft(from awaiting), replacer(from alreadyApplied), space (from awaiting)
            transformations.Insert(0, new SpaceNormalizationTransformation());
            transformations.Add(new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS));
            transformations.Add(new SpaceNormalizationTransformation());
            transformations.Add(new TrimLeftTransformation()); // add a non-matching transformation, it will be skipped (we have removed all left trimmers)
            remover = new RandomTransformationDecorator(label); // reset its styles

            remover.ApplyStylesFromList(transformations);
            dec.RemoveDecorator(remover);
            alreadyApplied.Remove(transformations[0]);
            alreadyApplied.Remove(transformations[2]);
            awaiting.RemoveAt(0);
            awaiting.RemoveAt(0);
            // only trimRight willRemain
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));
        }

        // add styles from decorators
        [Test]
        public void Test_RandomDecorator_AddDecorator()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new TrimLeftTransformation(),
                new TrimRightTransformation(),
                new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS),
                new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER)
            };

            RandomTransformationDecorator dec = new RandomTransformationDecorator(label);
            Assert.That(dec.Text, Is.EqualTo(SAMPLE_RICH_TEXT));
            Assert.That(dec.Transformations, Is.EqualTo(new List<ITextTransformation>()));


            CyclingTransformationsDecorator cyclic = new CyclingTransformationsDecorator(label, transformations);
            dec.AddDecorator(cyclic);
            Assert.That(dec.Transformations, Is.EqualTo(transformations));

            TextTransformationDecorator textDecorator = new TextTransformationDecorator(label);
            textDecorator.ApplyStylesFromList(transformations);
            dec.AddDecorator(textDecorator);
            List<ITextTransformation> buffedTransformations = new List<ITextTransformation>(transformations);
            buffedTransformations.AddRange(transformations);
            Assert.That(dec.Transformations, Is.EqualTo(buffedTransformations));

            RandomTransformationDecorator randDec = new RandomTransformationDecorator(label, transformations);
            dec.AddDecorator(randDec);
            buffedTransformations.AddRange(transformations);
            Assert.That(dec.Transformations, Is.EqualTo(buffedTransformations));
        }

        [Test]
        public void Test_RandomDecorator_RemovingDecorators_StaticFunction_Styles()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            RandomTransformationDecorator dec = new RandomTransformationDecorator(label);
            dec.AddDecorator(replacer);
            dec.AddDecorator(new TrimRightTransformation());
            dec.AddDecorator(new TrimLeftTransformation());
            const int TRANSFORMATIONS_TO_APPLY = 3;
            for (int i = 0; i < TRANSFORMATIONS_TO_APPLY; i++)
            {
                _ = dec.Text;
            } // 1 and one trimRight and one trimLeft have been applied
            
            replacer = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            dec.AddDecorator(replacer);
            dec.AddDecorator(new SpaceNormalizationTransformation());
            
            // null shouldn't change anything
            ITextTransformation? styleToRemove = null;
            List<ITextTransformation> awaiting = new List<ITextTransformation>(dec.Transformations);
            List<ITextTransformation> alreadyApplied = dec.AlreadyApplied.ToList();
            LabelDecoratorBase.RemoveDecoratorFrom(dec, styleToRemove);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));

            // only the trim left transformation should be removed(from the applied)
            styleToRemove = new TrimLeftTransformation();
            LabelDecoratorBase.RemoveDecoratorFrom(dec, styleToRemove);
            alreadyApplied.Remove(styleToRemove);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));

            // remove the trimright(from the already applied)
            styleToRemove = new TrimRightTransformation();
            LabelDecoratorBase.RemoveDecoratorFrom(dec, styleToRemove);
            alreadyApplied.Remove(styleToRemove);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));

            // no more replacing of sunny with september(from the awaiting)
            styleToRemove = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            LabelDecoratorBase.RemoveDecoratorFrom(dec, styleToRemove);
            awaiting.Remove(styleToRemove);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));

            // add a new style which may match one of the few decorators, the result will end up the same
            dec.AddDecorator(new TrimRightTransformation());
            LabelDecoratorBase.RemoveDecoratorFrom(dec, new TrimRightTransformation());
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));
        }

        [Test]
        public void Test_RandomDecorator_RemovingDecorators_StaticFunction_Decorators()
        {
            ILabel label = new SimpleLabel(SAMPLE_RICH_TEXT);
            ITextTransformation replacer = new ReplacerTransformation(SEPTEMBER, SUNNY_DAYS);
            RandomTransformationDecorator dec = new RandomTransformationDecorator(label);
            dec.AddDecorator(replacer);
            dec.AddDecorator(new TrimRightTransformation());
            dec.AddDecorator(new TrimLeftTransformation());
            const int TRANSFORMATIONS_TO_APPLY = 3;
            for (int i = 0; i < TRANSFORMATIONS_TO_APPLY; i++)
            {
                _ = dec.Text;
            } // 1 and one trimRight and one trimLeft have been applied

            replacer = new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER);
            dec.AddDecorator(replacer);
            dec.AddDecorator(new SpaceNormalizationTransformation());

            // null shouldn't change anything
            TextTransformationDecorator secondDecorator = new TextTransformationDecorator(label, null);
            List<ITextTransformation> awaiting = new List<ITextTransformation>(dec.Transformations);
            List<ITextTransformation> alreadyApplied = dec.AlreadyApplied.ToList();
            LabelDecoratorBase.RemoveDecoratorFrom(dec, secondDecorator);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));

            // only the trim left transformation should be removed(from the applied)
            secondDecorator = new TextTransformationDecorator(label, new TrimLeftTransformation());
            LabelDecoratorBase.RemoveDecoratorFrom(dec, secondDecorator); // we remove a decorator but the state remains unchanged(until all of them are null styles)
            alreadyApplied.Remove(secondDecorator.ExtractTransformationsToOuterWorld().ToList()[0]);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));

            // multiple decorator removal - trim right from already applied and sunny to september replacer from awaiting
            secondDecorator = new TextTransformationDecorator(label, new TrimRightTransformation());
            secondDecorator = new TextTransformationDecorator(secondDecorator, new ReplacerTransformation(SUNNY_DAYS, SEPTEMBER));
            LabelDecoratorBase.RemoveDecoratorFrom(dec, secondDecorator);
            List<ITextTransformation> appliedTransformations = secondDecorator.ExtractTransformationsToOuterWorld().ToList();
            alreadyApplied.Remove(appliedTransformations[0]);
            awaiting.Remove(appliedTransformations[1]);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));

            // add a new style which may match one of the few decorators, the result will end up the same
            dec.AddDecorator(new TrimRightTransformation());
            LabelDecoratorBase.RemoveDecoratorFrom(dec, secondDecorator);
            Assert.That(dec.Transformations, Is.EqualTo(awaiting));
            Assert.That(dec.AlreadyApplied, Is.EqualTo(alreadyApplied));
        }

    }
}
