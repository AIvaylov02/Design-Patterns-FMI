using DP_Task2.Interfaces;
using DP_Task2.LabelDecorators;
using DP_Task2.Labels;
using DP_Task2.LabelUtilizers;
using DP_Task2.Transformations;
using DP_Task2.Utilizers;


namespace DP_Task2_Tests
{
    [TestFixture]
    public class LabelBuilderTests
    {
        const string DEFAULT_TEXT_VALUE = "abc";

        [Test]
        public void Test_LabelBuilder_Initialization()
        {
            LabelBuilder builder = new LabelBuilder(DEFAULT_TEXT_VALUE);
            Assert.That(builder.LabelType, Is.Null);
            Assert.That(builder.HelpText, Is.Null);
            Assert.That(builder.Transformations, Is.Empty);
            Assert.That(builder.TextColor, Is.Null);
            Assert.That(builder.Font, Is.Null);
            Assert.That(builder.FontSize, Is.Null);
        }

        [Test]
        public void Test_LabelBuilder_ParametersChanging()
        {
            LabelBuilder builder = new LabelBuilder(DEFAULT_TEXT_VALUE);
            Assert.That(builder.LabelType, Is.Null);
            const string BASIC_LABEL_TYPE = "simple";
            builder.LabelType = BASIC_LABEL_TYPE;
            Assert.That(builder.LabelType, Is.EqualTo(BASIC_LABEL_TYPE)); // label type cannot be marked null, in order to create an object, the person needs to type it before finalization

            Assert.That(builder.HelpText, Is.Null);
            const string BASIC_HELP_TEXT = "press X";
            builder.HelpText = BASIC_HELP_TEXT;
            Assert.That(builder.HelpText, Is.EqualTo(BASIC_HELP_TEXT));
            builder.HelpText = null;
            Assert.That(builder.HelpText, Is.Null);

            Assert.That(builder.Transformations, Is.Empty);
            const string CENSORER_TRANSFORMATION_TYPE = "censor";
            const string DEFAULT_BAD_WORD = "abc";
            builder.AddTransformation(CENSORER_TRANSFORMATION_TYPE, DEFAULT_BAD_WORD);
            CensorerTransformation transformation = (CensorerTransformation)TextTransformationFactory.CreateTransformation(CENSORER_TRANSFORMATION_TYPE, DEFAULT_BAD_WORD);
            Assert.True(Object.ReferenceEquals(builder.Transformations.ToList()[0], transformation));
            builder.RemoveTransformation();
            Assert.That(builder.Transformations, Is.Empty);

            Assert.That(builder.TextColor, Is.Null);
            const string EXAMPLE_COLOR = "yellow";
            builder.AddTextColor(EXAMPLE_COLOR);
            Assert.That(builder.TextColor, Is.EqualTo(EXAMPLE_COLOR));
            builder.RemoveTextColor();
            Assert.That(builder.TextColor, Is.Null);

            Assert.That(builder.Font, Is.Null);
            const string EXAMPLE_FONT = "Calibri";
            builder.AddFont(EXAMPLE_FONT);
            Assert.That(builder.Font, Is.EqualTo(EXAMPLE_FONT));
            builder.RemoveFont();
            Assert.That(builder.Font, Is.Null);

            Assert.That(builder.FontSize, Is.Null);
            const double EXAMPLE_FONT_SIZE = 10.5;
            builder.AddFontSize(EXAMPLE_FONT_SIZE);
            Assert.That(builder.FontSize, Is.EqualTo(EXAMPLE_FONT_SIZE));
            builder.RemoveFontSize();
            Assert.That(builder.FontSize, Is.Null);
        }

        [Test]
        public void Test_LabelBuilder_AddTransformations()
        {
            // test that both ways are valid in terms of transformation addition
            LabelBuilder builder = new LabelBuilder(DEFAULT_TEXT_VALUE);
            Assert.That(builder.Transformations, Is.Empty);
            const string DEFAULT_BAD_WORD = "abc";
            builder.AddTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD);

            // since we shallowly include transformations, just the censorer with long word should be a different referance
            const string DEFAULT_LONG_BAD_WORD = "ABCD!";
            ITextTransformation transformation = TextTransformationFactory.CreateTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_LONG_BAD_WORD);
            builder.AddTransformation(transformation);

            // this should reuse the transformation, it should not create a new one underneath
            transformation = TextTransformationFactory.CreateTransformation(TextTransformationFactory.RIGHT_TRIMMER_TYPE);
            builder.AddTransformation(transformation);

            List<ITextTransformation> transformationsInBuilder = builder.Transformations.ToList();
            Assert.True(Object.ReferenceEquals(transformationsInBuilder[2], transformation));
            transformation = TextTransformationFactory.CreateTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_LONG_BAD_WORD);
            Assert.False(Object.ReferenceEquals(transformationsInBuilder[1], transformation));
            transformation = TextTransformationFactory.CreateTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD);
            Assert.True(Object.ReferenceEquals(transformationsInBuilder[0], transformation));

        }

        [Test]
        public void Test_LabelBuilder_RemoveTransformations()
        {
            // test that all ways of removal are valid - defaultly the last style, a specific style you push through or a generated style from parameters
            LabelBuilder builder = new LabelBuilder(DEFAULT_TEXT_VALUE);
            const string DEFAULT_BAD_WORD = "abc";
            builder.AddTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD);

            // since we shallowly include transformations, just the censorer with long word should be a different referance
            const string DEFAULT_LONG_BAD_WORD = "ABCD!";
            ITextTransformation transformation = TextTransformationFactory.CreateTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_LONG_BAD_WORD);
            builder.AddTransformation(transformation);

            // this should reuse the transformation, it should not create a new one underneath
            transformation = TextTransformationFactory.CreateTransformation(TextTransformationFactory.RIGHT_TRIMMER_TYPE);
            builder.AddTransformation(transformation);

            List<ITextTransformation> transformationsInBuilder = builder.Transformations.ToList();
            Assert.That(builder.Transformations.Count, Is.EqualTo(transformationsInBuilder.Count));
            Assert.That(builder.Transformations, Is.EquivalentTo(transformationsInBuilder));

            transformationsInBuilder.RemoveAt(2);
            builder.RemoveTransformation(); // default removal - the lastly added one
            Assert.That(builder.Transformations.Count, Is.EqualTo(transformationsInBuilder.Count));
            Assert.That(builder.Transformations, Is.EquivalentTo(transformationsInBuilder));

            transformationsInBuilder.RemoveAt(1);
            transformation = TextTransformationFactory.CreateTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_LONG_BAD_WORD); // the long transformation needs to be matched with ==
            builder.RemoveTransformation(transformation); // specific removal by passing a concrete transformation to remove
            Assert.That(builder.Transformations.Count, Is.EqualTo(transformationsInBuilder.Count));
            Assert.That(builder.Transformations, Is.EquivalentTo(transformationsInBuilder));

            transformationsInBuilder.RemoveAt(0);
            transformation = TextTransformationFactory.CreateTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_LONG_BAD_WORD); // the long transformation needs to be matched with ==
            builder.RemoveTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD); // specific removal by passing parameters of a concrete transformation
            Assert.That(builder.Transformations.Count, Is.EqualTo(transformationsInBuilder.Count));
            Assert.That(builder.Transformations, Is.EquivalentTo(transformationsInBuilder));

            // the builder is already empty the following lines should do nothing
            builder.RemoveTransformation();
            Assert.That(builder.Transformations, Is.EquivalentTo(transformationsInBuilder));
            builder.RemoveTransformation(transformation);
            Assert.That(builder.Transformations, Is.EquivalentTo(transformationsInBuilder));
            builder.RemoveTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD);
            Assert.That(builder.Transformations, Is.EquivalentTo(transformationsInBuilder));
        }

        [Test]
        public void Test_LabelBuilder_ClearTransformations()
        {
            // test that all ways of removal are valid - defaultly the last style, a specific style you push through or a generated style from parameters
            LabelBuilder builder = new LabelBuilder(DEFAULT_TEXT_VALUE);
            const string DEFAULT_BAD_WORD = "abc";
            builder.AddTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD);

            // since we shallowly include transformations, just the censorer with long word should be a different referance
            const string DEFAULT_LONG_BAD_WORD = "ABCD!";
            ITextTransformation transformation = TextTransformationFactory.CreateTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_LONG_BAD_WORD);
            builder.AddTransformation(transformation);

            // this should reuse the transformation, it should not create a new one underneath
            transformation = TextTransformationFactory.CreateTransformation(TextTransformationFactory.RIGHT_TRIMMER_TYPE);
            builder.AddTransformation(transformation);

            List<ITextTransformation> transformationsInBuilder = builder.Transformations.ToList();
            Assert.That(builder.Transformations.Count, Is.EqualTo(transformationsInBuilder.Count));
            Assert.That(builder.Transformations, Is.EquivalentTo(transformationsInBuilder));

            transformationsInBuilder = new List<ITextTransformation>();
            builder.ClearTransformations();
            Assert.That(builder.Transformations.Count, Is.EqualTo(transformationsInBuilder.Count));
            Assert.That(builder.Transformations, Is.EquivalentTo(transformationsInBuilder));

            builder.ClearTransformations(); // as the transformations are already clear, this won't suffice to any change
            Assert.That(builder.Transformations.Count, Is.EqualTo(transformationsInBuilder.Count));
            Assert.That(builder.Transformations, Is.EquivalentTo(transformationsInBuilder));
        }

        [Test]
        public void Test_LabelBuilder_ResetStyles()
        {
            LabelBuilder builder = new LabelBuilder(DEFAULT_TEXT_VALUE);
            const string BASIC_LABEL_TYPE = "simple";
            builder.LabelType = BASIC_LABEL_TYPE;
            Assert.That(builder.LabelType, Is.EqualTo(BASIC_LABEL_TYPE)); // label type cannot be marked null, in order to create an object, the person needs to type it before finalization

            const string BASIC_HELP_TEXT = "press X";
            builder.HelpText = BASIC_HELP_TEXT;
            Assert.That(builder.HelpText, Is.EqualTo(BASIC_HELP_TEXT));
            const string DEFAULT_BAD_WORD = "abc";
            builder.AddTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD);
            CensorerTransformation transformation = (CensorerTransformation)TextTransformationFactory.CreateTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD);
            Assert.True(Object.ReferenceEquals(builder.Transformations.ToList()[0], transformation));
            builder.ApplyTransformationsAsADecorator(DecoratorFactory.TEXT_DECORATOR_TYPE);
            builder.AddTransformation(TextTransformationFactory.CAPITALIZER_TYPE);

            Assert.That(builder.Transformations, Is.EquivalentTo(new List<ITextTransformation>() { new CapitalizeTransformation() }));
            // one decorator and one transformation

            const string EXAMPLE_COLOR = "yellow";
            builder.AddTextColor(EXAMPLE_COLOR);
            Assert.That(builder.TextColor, Is.EqualTo(EXAMPLE_COLOR));

            const string EXAMPLE_FONT = "Calibri";
            builder.AddFont(EXAMPLE_FONT);
            Assert.That(builder.Font, Is.EqualTo(EXAMPLE_FONT));

            const double EXAMPLE_FONT_SIZE = 10.5;
            builder.AddFontSize(EXAMPLE_FONT_SIZE);
            Assert.That(builder.FontSize, Is.EqualTo(EXAMPLE_FONT_SIZE));
            ILabel? firstProduct = builder.BuildLabel(); // will return a decorator (it has 2 decorators set in it)

            HelpLabel? parsedToHelpLabel = firstProduct as HelpLabel;
            Assert.That(parsedToHelpLabel, Is.Null); // decorator is not a help label, it just contains one
            LabelDecoratorBase? parsedToLabelDecorator = firstProduct as LabelDecoratorBase;
            Assert.That(parsedToLabelDecorator, Is.Not.Null);
            Assert.That(parsedToLabelDecorator.HelpText, Is.EqualTo(BASIC_HELP_TEXT));

            builder.ResetStyles(); // only the transformations and decorators(product) need to be reset
            Assert.That(builder.LabelType, Is.EqualTo(BASIC_LABEL_TYPE));
            Assert.That(builder.HelpText, Is.EqualTo(BASIC_HELP_TEXT));
            Assert.That(builder.TextColor, Is.EqualTo(EXAMPLE_COLOR));
            Assert.That(builder.Font, Is.EqualTo(EXAMPLE_FONT));
            Assert.That(builder.FontSize, Is.EqualTo(EXAMPLE_FONT_SIZE));

            Assert.That(builder.Transformations, Is.EquivalentTo(new List<ITextTransformation>()));
            ILabel? secondProduct = builder.BuildLabel(); // it contains only the helpText, text, labelType, color, font and fontSize
            parsedToHelpLabel = secondProduct as HelpLabel;
            Assert.That(parsedToHelpLabel, Is.Not.Null); // it is a helpLabel now, after it got upgraded from simpleLabel(it just contains simpleLabel inside)
        }

        [Test]
        public void Test_LabelBuilder_ResetLabel()
        {
            LabelBuilder builder = new LabelBuilder(DEFAULT_TEXT_VALUE);
            const string BASIC_LABEL_TYPE = "simple";
            builder.LabelType = BASIC_LABEL_TYPE;
            Assert.That(builder.LabelType, Is.EqualTo(BASIC_LABEL_TYPE)); // label type cannot be marked null, in order to create an object, the person needs to type it before finalization

            const string BASIC_HELP_TEXT = "press X";
            builder.HelpText = BASIC_HELP_TEXT;
            Assert.That(builder.HelpText, Is.EqualTo(BASIC_HELP_TEXT));
            const string DEFAULT_BAD_WORD = "abc";
            builder.AddTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD);
            CensorerTransformation transformation = (CensorerTransformation)TextTransformationFactory.CreateTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD);
            Assert.True(Object.ReferenceEquals(builder.Transformations.ToList()[0], transformation));
            builder.ApplyTransformationsAsADecorator(DecoratorFactory.TEXT_DECORATOR_TYPE);
            builder.AddTransformation(TextTransformationFactory.CAPITALIZER_TYPE);

            Assert.That(builder.Transformations, Is.EquivalentTo(new List<ITextTransformation>() { new CapitalizeTransformation() }));
            // one decorator and one transformation

            const string EXAMPLE_COLOR = "yellow";
            builder.AddTextColor(EXAMPLE_COLOR);
            Assert.That(builder.TextColor, Is.EqualTo(EXAMPLE_COLOR));

            const string EXAMPLE_FONT = "Calibri";
            builder.AddFont(EXAMPLE_FONT);
            Assert.That(builder.Font, Is.EqualTo(EXAMPLE_FONT));

            const double EXAMPLE_FONT_SIZE = 10.5;
            builder.AddFontSize(EXAMPLE_FONT_SIZE);
            Assert.That(builder.FontSize, Is.EqualTo(EXAMPLE_FONT_SIZE));
            ILabel? generatedProduct = builder.BuildLabel(); // will return a decorator (it has 2 decorators set in it)

            HelpLabel? parsedToHelpLabel = generatedProduct as HelpLabel;
            Assert.That(parsedToHelpLabel, Is.Null); // decorator is not a help label, it just contains one
            LabelDecoratorBase? parsedToLabelDecorator = generatedProduct as LabelDecoratorBase;
            Assert.That(parsedToLabelDecorator, Is.Not.Null);
            Assert.That(parsedToLabelDecorator.HelpText, Is.EqualTo(BASIC_HELP_TEXT));

            builder.ResetLabel(); // all properties will be reset
            Assert.That(builder.LabelType, Is.Null); // this is the most important property reset, as it makes the building of labels impossible until it is properly set
            Assert.That(builder.HelpText, Is.Null);
            Assert.That(builder.TextColor, Is.Null);
            Assert.That(builder.Font, Is.Null);
            Assert.That(builder.FontSize, Is.Null);

            Assert.That(builder.Transformations, Is.EquivalentTo(new List<ITextTransformation>()));
            generatedProduct = builder.BuildLabel();
            Assert.That(generatedProduct, Is.Null);
        }

        [Test]
        public void Test_LabelBuilder_DefaultDecoratorRemoval()
        {
            LabelBuilder builder = new LabelBuilder(DEFAULT_TEXT_VALUE);
            const string BASIC_LABEL_TYPE = "simple";
            builder.LabelType = BASIC_LABEL_TYPE;
            Assert.That(builder.LabelType, Is.EqualTo(BASIC_LABEL_TYPE)); // label type cannot be marked null, in order to create an object, the person needs to type it before finalization

            builder.RemoveDecorator(); // product is null nothing happens

            ILabel? label = builder.BuildLabel(); // the product is of type simpleLabel, not a decorator
            Assert.That(label, Is.AssignableTo<SimpleLabel>());
            // Assert.That(label, Is.AssignableTo<HelpLabel>()); // it is not helpLabel as it doesn't contain a helpMessage
            builder.RemoveDecorator(); // nothing will happen as it is not a decorator

            const string DEFAULT_BAD_WORD = "abc";
            builder.AddTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD);
            label = builder.BuildLabel(); // now it constructs a decorator
            Assert.That(label, Is.AssignableTo<LabelDecoratorBase>());
            Assert.That(((LabelDecoratorBase)label).HelpText, Is.EqualTo(HelpLabel.MISSING_HELP_MESSAGE));

            builder.AddTransformation(TextTransformationFactory.LEFT_TRIMMER_TYPE);
            builder.AddTransformation(TextTransformationFactory.RIGHT_TRIMMER_TYPE);
            label = builder.BuildLabel(); // it appends the second decorator onto the first

            builder.RemoveDecorator(); // the third decorator will be removed
            label = builder.BuildLabel();
            Assert.That(label, Is.AssignableTo<LabelDecoratorBase>());
            builder.RemoveDecorator(); // the second decorator will be removed
            label = builder.BuildLabel();
            Assert.That(label, Is.AssignableTo<LabelDecoratorBase>());
            builder.RemoveDecorator(); // the first decorator will be removed, now it will be a simple label
            label = builder.BuildLabel();
            Assert.That(label, Is.AssignableTo<SimpleLabel>());
            Assert.That(label, Is.Not.AssignableTo<LabelDecoratorBase>());

            // Assert.That(label, Is.AssignableTo<HelpLabel>()); it is not a helpLabel
            builder.RemoveDecorator(); // nothing will happen as it is not a decorator
        }

        [Test]
        public void Test_LabelBuilder_ClearAllDecorators()
        {
            LabelBuilder builder = new LabelBuilder(DEFAULT_TEXT_VALUE);
            const string BASIC_LABEL_TYPE = "simple";
            builder.LabelType = BASIC_LABEL_TYPE;
            Assert.That(builder.LabelType, Is.EqualTo(BASIC_LABEL_TYPE)); // label type cannot be marked null, in order to create an object, the person needs to type it before finalization

            builder.RemoveDecorators(); // product is null nothing happens

            ILabel? label = builder.BuildLabel(); // the product is of type simpleLabel, not a decorator
            Assert.That(label, Is.AssignableTo<SimpleLabel>());
            // Assert.That(label, Is.AssignableTo<HelpLabel>()); // it is not helpLabel as it doesn't contain a helpMessage
            builder.RemoveDecorators(); // nothing will happen as it is not a decorator

            const string DEFAULT_BAD_WORD = "abc";
            builder.AddTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD);
            label = builder.BuildLabel(); // now it constructs a decorator
            builder.AddTransformation(TextTransformationFactory.LEFT_TRIMMER_TYPE);
            builder.AddTransformation(TextTransformationFactory.RIGHT_TRIMMER_TYPE);
            label = builder.BuildLabel(); // it appends the second decorator onto the first
            Assert.That(label, Is.AssignableTo<LabelDecoratorBase>());
            Assert.That(((LabelDecoratorBase)label).HelpText, Is.EqualTo(HelpLabel.MISSING_HELP_MESSAGE));

            builder.RemoveDecorators(); // the decorators will be removed
            label = builder.BuildLabel();
            Assert.That(label, Is.AssignableTo<SimpleLabel>());
            Assert.That(label, Is.Not.AssignableTo<LabelDecoratorBase>());
            // Assert.That(label, Is.AssignableTo<HelpLabel>()); it is not a helpLabel
            builder.RemoveDecorators(); // nothing will happen as it is not a decorator, all of them have been removed
        }

        [Test]
        public void Test_LabelBuilder_ApplyDecoratorFromTransformations()
        {

            LabelBuilder builder = new LabelBuilder(DEFAULT_TEXT_VALUE);
            const string DEFAULT_BAD_WORD = "abc";
            const string DEFAULT_LONG_BAD_WORD = "ABCD!";
            builder.ApplyTransformationsAsADecorator(DecoratorFactory.TEXT_DECORATOR_TYPE); // nothing will happen as no transformation have been added

            builder.AddTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD);
            builder.AddTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_LONG_BAD_WORD);
            // by creating a product with no labelType, an exception will occur
            Assert.Throws<ArgumentNullException>(() => builder.ApplyTransformationsAsADecorator(DecoratorFactory.TEXT_DECORATOR_TYPE));

            builder.LabelType = BaseLabelFactory.SIMPLE_LABEL_TYPE;
            builder.ApplyTransformationsAsADecorator("unknown"); // invalid decorator type is given, the class will cover it

            builder.ApplyTransformationsAsADecorator(DecoratorFactory.TEXT_DECORATOR_TYPE);
            Assert.That(builder.Transformations, Is.EquivalentTo(new List<ITextTransformation>()));
            ILabel? label = builder.BuildLabel(); // we have a simpleLabel, wrapped by 2 chainded textDecorators
            Assert.That(label, Is.Not.AssignableTo<SimpleLabel>());
            Assert.That(label, Is.AssignableTo<LabelDecoratorBase>());
            Assert.That(label, Is.AssignableTo<TextTransformationDecorator>());

            builder.AddTransformation(TextTransformationFactory.LEFT_TRIMMER_TYPE);
            builder.AddTransformation(TextTransformationFactory.RIGHT_TRIMMER_TYPE);
            builder.ApplyTransformationsAsADecorator(DecoratorFactory.CYCLIC_DECORATOR_TYPE);
            Assert.That(builder.Transformations, Is.EquivalentTo(new List<ITextTransformation>()));
            label = builder.BuildLabel(); // we have simpleLabel, wrapped by textDecorator,textDecorator and a cyclic one
            Assert.That(label, Is.Not.AssignableTo<SimpleLabel>());
            Assert.That(label, Is.Not.AssignableTo<TextTransformationDecorator>());
            Assert.That(label, Is.AssignableTo<LabelDecoratorBase>());
            Assert.That(label, Is.AssignableTo<CyclingTransformationsDecorator>());

            builder.AddTransformation(TextTransformationFactory.SPACE_NORMALIZATION_TYPE);
            builder.AddTransformation(TextTransformationFactory.DECORATION_TRANSFORMATION_TYPE);
            builder.ApplyTransformationsAsADecorator(DecoratorFactory.RANDOM_DECORATOR_TYPE);
            Assert.That(builder.Transformations, Is.EquivalentTo(new List<ITextTransformation>()));
            label = builder.BuildLabel(); // we have simpleLabel, wrapped by textDecorator,textDecorator, a cyclic one and a random one to top it all of
            Assert.That(label, Is.Not.AssignableTo<SimpleLabel>());
            Assert.That(label, Is.Not.AssignableTo<TextTransformationDecorator>());
            Assert.That(label, Is.Not.AssignableFrom<CyclingTransformationsDecorator>());
            Assert.That(label, Is.AssignableTo<LabelDecoratorBase>());
            Assert.That(label, Is.AssignableTo<RandomTransformationDecorator>());
        }

        [Test]
        public void Test_LabelBuilder_AddDecorator()
        {
            const string DEFAULT_BAD_WORD = "abc";
            ILabel simpleLabel = new SimpleLabel(DEFAULT_BAD_WORD);

            LabelDecoratorBase firstlyAppliedDecorator = DecoratorFactory.CreateDecoratorWithoutStyles(DecoratorFactory.CYCLIC_DECORATOR_TYPE, simpleLabel);
            firstlyAppliedDecorator.AddDecorator(TextTransformationFactory.CreateTransformation(TextTransformationFactory.SPACE_NORMALIZATION_TYPE));

            LabelBuilder builder = new LabelBuilder(DEFAULT_TEXT_VALUE);
            Assert.Throws<ArgumentNullException>(() => builder.AddDecorator(firstlyAppliedDecorator)); // no type has been chosen, it is invalid

            builder.LabelType = BaseLabelFactory.SIMPLE_LABEL_TYPE;
            builder.AddDecorator(firstlyAppliedDecorator);
            Assert.That(builder.Transformations, Is.EquivalentTo(new List<ITextTransformation>()));
            ILabel? generatedProduct = builder.BuildLabel(); // the product needs to be decorator from this point, it was chosen to be a cyclic one
            Assert.That(generatedProduct, Is.Not.Null);
            Assert.That(generatedProduct, Is.AssignableTo<CyclingTransformationsDecorator>());

            LabelDecoratorBase dec = DecoratorFactory.CreateDecoratorWithoutStyles(DecoratorFactory.TEXT_DECORATOR_TYPE, simpleLabel);
            dec.AddDecorator(TextTransformationFactory.CreateTransformation(TextTransformationFactory.LEFT_TRIMMER_TYPE));
            dec.AddDecorator(TextTransformationFactory.CreateTransformation(TextTransformationFactory.RIGHT_TRIMMER_TYPE)); // add the two styles as textDecorators

            dec = DecoratorFactory.CreateDecoratorWithoutStyles(DecoratorFactory.CYCLIC_DECORATOR_TYPE, dec);
            dec.AddDecorator(TextTransformationFactory.CreateTransformation(TextTransformationFactory.SPACE_NORMALIZATION_TYPE));
            dec.AddDecorator(TextTransformationFactory.CreateTransformation(TextTransformationFactory.SPACE_NORMALIZATION_TYPE)); // add the 2 transformations as a cyclic decorator

            dec = DecoratorFactory.CreateDecoratorWithoutStyles(DecoratorFactory.RANDOM_DECORATOR_TYPE, dec);
            const string DEFAULT_LONG_BAD_WORD = "ABCD!";
            dec.AddDecorator(TextTransformationFactory.CreateTransformation(TextTransformationFactory.REPLACER_TYPE, DEFAULT_BAD_WORD, DEFAULT_LONG_BAD_WORD));
            dec.AddDecorator(TextTransformationFactory.CreateTransformation(TextTransformationFactory.CENSORER_TYPE, DEFAULT_BAD_WORD)); // add the 2 transformations as a random decorator

            builder.AddDecorator(dec);
            Assert.That(builder.Transformations, Is.EquivalentTo(new List<ITextTransformation>()));
            generatedProduct = builder.BuildLabel();
            Assert.That(generatedProduct, Is.Not.Null);
            Assert.That(generatedProduct, Is.AssignableTo<RandomTransformationDecorator>());

            generatedProduct = ((RandomTransformationDecorator)generatedProduct).Label; // remove decorators one by one, the next one should be cyclic
            Assert.That(generatedProduct, Is.Not.Null);
            Assert.That(generatedProduct, Is.AssignableTo<CyclingTransformationsDecorator>());

            generatedProduct = ((CyclingTransformationsDecorator)generatedProduct).Label; // remove decorators one by one, the next one should be text
            Assert.That(generatedProduct, Is.Not.Null);
            Assert.That(generatedProduct, Is.AssignableTo<TextTransformationDecorator>());

            generatedProduct = ((TextTransformationDecorator)generatedProduct).Label; // remove decorators one by one, the next one should be text
            Assert.That(generatedProduct, Is.Not.Null);
            Assert.That(generatedProduct, Is.AssignableTo<TextTransformationDecorator>());

            generatedProduct = ((TextTransformationDecorator)generatedProduct).Label; // remove decorators one by one, the next one should be the first cyclic
            Assert.That(generatedProduct, Is.Not.Null);
            Assert.That(generatedProduct, Is.AssignableTo<CyclingTransformationsDecorator>());

            generatedProduct = ((CyclingTransformationsDecorator)generatedProduct).Label; // remove decorators one by one, the next one should be the first cyclic
            Assert.That(generatedProduct, Is.Not.Null);
            Assert.That(generatedProduct, Is.Not.AssignableTo<LabelDecoratorBase>()); // it is not a decorator, but a simpleLabel
            Assert.That(generatedProduct, Is.AssignableTo<SimpleLabel>());
        }

        [Test]
        public void Test_LabelBuilder_BuildLabel()
        {
            const string DEFAULT_BAD_WORD = "abc";
            ILabel simpleLabel = new SimpleLabel(DEFAULT_BAD_WORD);

            LabelDecoratorBase firstlyAppliedDecorator = DecoratorFactory.CreateDecoratorWithoutStyles(DecoratorFactory.CYCLIC_DECORATOR_TYPE, simpleLabel);
            firstlyAppliedDecorator.AddDecorator(TextTransformationFactory.CreateTransformation(TextTransformationFactory.SPACE_NORMALIZATION_TYPE));

            LabelBuilder builder = new LabelBuilder(DEFAULT_TEXT_VALUE);
            ILabel? product = builder.BuildLabel(); // no type has been chosen, it is invalid, it will be null
            Assert.That(product, Is.Null);
            // label type cannot be set as invalid one, as the .Label property controls it
            Assert.Throws<ArgumentNullException>(() => builder.LabelType = null);
            Assert.Throws<ArgumentOutOfRangeException>(() => builder.LabelType = "customLabelExpress");

            builder.LabelType = BaseLabelFactory.SIMPLE_LABEL_TYPE; // simple label with decorator on top
            builder.AddDecorator(firstlyAppliedDecorator);
            product = builder.BuildLabel();
            Assert.That(product, Is.Not.Null);
            Assert.That(product, Is.AssignableTo<LabelDecoratorBase>());
            product = ((LabelDecoratorBase)product).Label;
            Assert.That(product, Is.Not.Null);
            Assert.That(product, Is.AssignableTo<SimpleLabel>());

            builder.ResetStyles(); // try help label creation
            const string HELPFUL_TEXT_1 = "Super useful message here!";
            builder.HelpText = HELPFUL_TEXT_1; // will indulge product regeneration
            product = builder.BuildLabel();
            Assert.That(product, Is.Not.Null);
            Assert.That(product, Is.AssignableTo<HelpLabel>());
            Assert.That(((HelpLabel)product).HelpText, Is.EqualTo(HELPFUL_TEXT_1));
            product = ((HelpLabel)product).Label;
            Assert.That(product, Is.Not.Null);
            Assert.That(product, Is.AssignableTo<SimpleLabel>());

            builder.ResetLabel(); // validate rich label creation
            builder.LabelType = BaseLabelFactory.SIMPLE_LABEL_TYPE;

            const string EXAMPLE_COLOR = "yellow";
            builder.AddTextColor(EXAMPLE_COLOR);
            Assert.That(builder.TextColor, Is.EqualTo(EXAMPLE_COLOR));

            const string EXAMPLE_FONT = "Calibri";
            builder.AddFont(EXAMPLE_FONT);
            Assert.That(builder.Font, Is.EqualTo(EXAMPLE_FONT));

            const double EXAMPLE_FONT_SIZE = 10.5;
            builder.AddFontSize(EXAMPLE_FONT_SIZE);
            Assert.That(builder.FontSize, Is.EqualTo(EXAMPLE_FONT_SIZE));
            product = builder.BuildLabel(); // will return a simple label, although it has color, font and font_size
            Assert.That(product, Is.Not.Null);
            Assert.That(product, Is.AssignableTo<SimpleLabel>());
            Assert.That(product, Is.Not.AssignableTo<RichLabel>());

            builder.LabelType = BaseLabelFactory.RICH_LABEL_TYPE;
            builder.RemoveTextColor();
            product = builder.BuildLabel();
            Assert.That(product, Is.Null); // cannot create rich label without textColour

            builder.AddTextColor(EXAMPLE_COLOR);
            builder.RemoveFont();
            product = builder.BuildLabel();
            Assert.That(product, Is.Null); // cannot create rich label without font

            builder.AddFont(EXAMPLE_FONT);
            builder.RemoveFontSize();
            product = builder.BuildLabel();
            Assert.That(product, Is.Null); // cannot create rich label without font

            builder.AddFontSize(EXAMPLE_FONT_SIZE);
            product = builder.BuildLabel();
            Assert.That(product, Is.Not.Null); // finally rich label CAN be created as all of its labels have been fulfilled
            Assert.That(product, Is.Not.AssignableTo<SimpleLabel>());
            Assert.That(product, Is.AssignableTo<RichLabel>());

            const string HELPFUL_TEXT_2 = "Again, the super useful text strikes!";
            builder.HelpText = HELPFUL_TEXT_2;
            product = builder.BuildLabel();
            Assert.That(product, Is.Not.Null); // rich label with helpText (helpLabel with richLabel underneath)
            Assert.That(product, Is.AssignableTo<HelpLabel>());
            Assert.That(((HelpLabel)product).HelpText, Is.EqualTo(HELPFUL_TEXT_2));
            product = ((HelpLabel)product).Label;
            Assert.That(product, Is.AssignableTo<RichLabel>());
        }
    }
}
