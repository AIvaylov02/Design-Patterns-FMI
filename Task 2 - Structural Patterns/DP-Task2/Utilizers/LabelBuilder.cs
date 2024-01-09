using DP_Task2.Interfaces;
using DP_Task2.LabelDecorators;
using DP_Task2.Labels;
using DP_Task2.Transformations;
using DP_Task2.Utilizers;

namespace DP_Task2.LabelUtilizers
{
    public class LabelBuilder : ILabelBuilder
    {
        string? labelType; // simple, rich, custom
        string text; // text is immutable and received in the constructor
        string? helpText;
        List<ITextTransformation> transformations;
        ILabel? product;
        string? textColor;
        string? font;
        double? fontSize;
        bool baseProductRegenerationIsNeeded; // will be used to keep track when we need to change the base product -> this is a heavy operations as all decorators have to be unchained

        public LabelBuilder(string text)
        {
            labelType = null;
            this.text = text;
            //helpText = HelpLabel.MISSING_HELP_MESSAGE;
            transformations = new List<ITextTransformation>();
            product = null;
            baseProductRegenerationIsNeeded = true;
        }

        public string? LabelType 
        { 
            get => labelType; 
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException($"{nameof(value)} cannot be marked as NULL");
                }
                    
                HashSet<string> allowedLabelTypes = new HashSet<string>()
                {
                "simple", "rich", "custom"
                };
                if (!allowedLabelTypes.Contains(value))
                    throw new ArgumentOutOfRangeException($"{nameof(labelType)} must be one of the following: {string.Join(" ", allowedLabelTypes)}");
                if (labelType is not null || labelType != value)
                {
                    labelType = value;
                    baseProductRegenerationIsNeeded = true;
                }
            }
        }

        // if helpText is changed, all the styles will be reset
        public string? HelpText
        {
            get => helpText;
            set
            {
                if (helpText is null)
                {
                    if (value is not null)
                    {
                        baseProductRegenerationIsNeeded = true;
                        helpText = value;
                    }
                }
                else // helpText is not null
                {
                    if (value is null || helpText != value)
                    {
                        baseProductRegenerationIsNeeded = true;
                        helpText = value;
                    }
                }
            }
        }

        public string? TextColor
        {
            get => textColor;
            set
            {
                if (labelType is not null && labelType == BaseLabelFactory.RICH_LABEL_TYPE) // we are in the interesting case
                {
                    if (textColor is null)
                    {
                        if (value is not null)
                        {
                            baseProductRegenerationIsNeeded = true;
                            textColor = value;
                        }
                    }
                    else // textColor is not null
                    {
                        if (value is null || textColor != value)
                        {
                            baseProductRegenerationIsNeeded = true;
                            textColor = value;
                        }
                    }
                }
                else
                {
                    if (value is null)
                        RemoveTextColor();
                    else
                        AddTextColor(value);
                }                
            }
        }

        public string? Font
        {
            get => font;
            set
            {
                if (labelType is not null && labelType == BaseLabelFactory.RICH_LABEL_TYPE) // we are in the interesting case
                {
                    if (font is null)
                    {
                        if (value is not null)
                        {
                            baseProductRegenerationIsNeeded = true;
                            font = value;
                        }
                    }
                    else // textColor is not null
                    {
                        if (value is null || font != value)
                        {
                            baseProductRegenerationIsNeeded = true;
                            font = value;
                        }
                    }
                }
                else
                {
                    if (value is null)
                        RemoveFont();
                    else
                        AddFont(value);
                }
            }
        }

        public double? FontSize
        {
            get => fontSize;
            set
            {
                if (labelType is not null && labelType == BaseLabelFactory.RICH_LABEL_TYPE) // we are in the interesting case
                {
                    if (fontSize is null)
                    {
                        if (value is not null)
                        {
                            baseProductRegenerationIsNeeded = true;
                            fontSize = value;
                        }
                    }
                    else // textColor is not null
                    {
                        if (value is null || fontSize != value)
                        {
                            baseProductRegenerationIsNeeded = true;
                            fontSize = value;
                        }
                    }
                }
                else
                {
                    if (value is null)
                        RemoveFontSize();
                    else
                        AddFontSize((double)value);
                }
            }
        }

        // for interactive use it is appropriate to use the add and remove methods instead of the properties
        public void AddTextColor(string textColor)
        {
            this.textColor = textColor;
        }

        public void AddFont(string font)
        {
            this.font = font;
        }

        public void AddFontSize(double fontSize)
        {
            this.fontSize = fontSize;
        }

        public string RemoveTextColor()
        {
            string previousColour = string.Empty;
            if (textColor is not null)
            {
                previousColour = textColor;
                textColor = null;
            }
            return previousColour;
        }
        public string RemoveFont()
        {
            string previousFont = string.Empty;
            if (font is not null)
            {
                previousFont = font;
                font = null;
            }
            return previousFont;
        }
        public double RemoveFontSize()
        {
            const double DEFAULT_FONT_SIZE = 0;
            double previousFontSize = DEFAULT_FONT_SIZE;
            if (fontSize is not null)
            {
                previousFontSize = (double)fontSize;
                fontSize = null;
            }
            return previousFontSize;
        }

        public IReadOnlyCollection<ITextTransformation> Transformations => transformations.AsReadOnly();

        public void ClearTransformations()
        {
            transformations.Clear();
        }

        public void AddTransformation(string type, string? censoredWord = null, string? replacement = null)
        {
            transformations.Add(TextTransformationFactory.CreateTransformation(type, censoredWord, replacement));
        }

        public void RemoveTransformation(string type, string? censoredWord = null, string? replacement = null)
        {
            transformations.Remove(TextTransformationFactory.CreateTransformation(type, censoredWord, replacement));
        }

        public LabelDecoratorBase? RemoveDecorator()
        {
            LabelDecoratorBase currentDecorator = null;

            if (product is not null)
            {
                LabelDecoratorBase? decorator = product as LabelDecoratorBase;
                if (decorator is not null)
                {
                    currentDecorator = decorator;
                    product = decorator.RemoveDecorator();
                    HelpLabel? helpProduct = product as HelpLabel;
                    // check if it contains helpText, if not revert it back to simpleLabel/richLabel or customLabel when we get to the bottom
                    if (helpProduct is not null && helpProduct.HelpText == HelpLabel.MISSING_HELP_MESSAGE) 
                    {
                        product = helpProduct.Label;
                    }
                }
            }
            return currentDecorator;
        }

        public void ApplyTransformationsAsADecorator(string decoratorType)
        {
            if (transformations.Count == 0)
                return; // the transformations are empty, no decorator can be applied

            if (product is null) // generate a base label
            {
                GenerateBaseProduct();
            }
            try // the inputted decorator type may be unsupported
            {
                // we need to dynamically pass the needed type as otherwise we risk loss of information
                if (product is LabelDecoratorBase)
                {
                    product = DecoratorFactory.CreateDecorator(decoratorType, transformations, (LabelDecoratorBase)product);
                }
                else if (product is IHelpLabel)
                {
                    product = DecoratorFactory.CreateDecorator(decoratorType, transformations, (IHelpLabel)product);
                }
                else
                {
                    product = DecoratorFactory.CreateDecorator(decoratorType, transformations, product);
                }
                transformations.Clear(); // if the application has been succesful, then clear the styles
            }
            catch (ArgumentException ex) // unsupported style
            {
                Console.WriteLine(ex.Message);
            }
        }

        public Stack<LabelDecoratorBase> RemoveDecorators()
        {
            Stack<LabelDecoratorBase> decorators = new Stack<LabelDecoratorBase>();
            while (product is LabelDecoratorBase)
            {
                LabelDecoratorBase? currentDecorator = RemoveDecorator();
                if (currentDecorator is not null)
                    decorators.Push(currentDecorator);
            }
            return decorators;
        }
        private void GenerateBaseProduct()
        {
            // generate base and apply help text if such is present
            if (labelType is null)
            {
                throw new ArgumentNullException($"type of Label must be chosen, before you create the finalize the creation of the object!");
            }

            product = BaseLabelFactory.CreateLabel(labelType, text, textColor, font, fontSize);
            if (helpText is not null) // if you have helpText you will be marked as helpLabel
            {
                product = new HelpLabel(product, helpText);
            }
            baseProductRegenerationIsNeeded = false;
        }

        public ILabel? BuildLabel()
        {
            try
            {
                if (baseProductRegenerationIsNeeded) // a dramatic circumstance has occurred
                {
                    Stack<LabelDecoratorBase> styles = RemoveDecorators();
                    GenerateBaseProduct();
                    ApplyDecoratorsFromStack(ref styles);
                }
                else if (product is null) // no decorators have been applied yet (product has not been generated yet)
                {
                    GenerateBaseProduct();
                }
                ApplyTransformationsAsADecorator(DecoratorFactory.TEXT_DECORATOR_TYPE);
                return product; // note that the object hasn't been cleared. We can pump out objects with subtle changes really easily this way.
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }

        public void AddTransformation(ITextTransformation transformation)
        {
            CensorerTransformation? censorerTransformation = transformation as CensorerTransformation;
            if (censorerTransformation is not null) // the transformation is from censorer type, copy its contents if it is more than 5 chars word long
            {
                if (censorerTransformation.BadWord.Length > CensorerTransformationSingletonFactory.UPPER_BOUND_OF_FLYWEIGHT_WORD) //copy
                {
                    transformations.Add(new CensorerTransformation(censorerTransformation.BadWord));
                    return;
                }
            }

            transformations.Add(transformation);
        }

        public void RemoveTransformation(ITextTransformation transformation)
        {
            transformations.Remove(transformation);
        }

        public void RemoveTransformation()
        {
            if (transformations.Count > 0)
            {
                transformations.RemoveAt(transformations.Count - 1); // remove the lastly added one
            }
        }

        private void ApplyDecoratorsFromStack(ref Stack<LabelDecoratorBase> decoratorsToApply)
        {
            // product is guaranteed not null here
            while (decoratorsToApply.Count > 0)
            {
                // unpack the decorator by extracting its styles
                LabelDecoratorBase currentDecorator = decoratorsToApply.Pop();
                ApplySingleDecorator(currentDecorator);
            }
        }

        // product needs to be guaranteed to be not null in time of invoking
        private void ApplySingleDecorator(LabelDecoratorBase currentDecorator)
        {
            List<ITextTransformation> styles = currentDecorator.ExtractStyles().ToList();
            if (product is LabelDecoratorBase)
            {
                if (currentDecorator is TextTransformationDecorator)
                {
                    styles = new List<ITextTransformation>() { styles[0] }; // pick only the first transformation, otherwise we create duplicate transformations thanks to the composite chaining
                    product = DecoratorFactory.CreateDecorator(DecoratorFactory.TEXT_DECORATOR_TYPE, styles, (LabelDecoratorBase)product);
                }
                else if (currentDecorator is CyclingTransformationsDecorator)
                {
                    product = DecoratorFactory.CreateDecorator(DecoratorFactory.CYCLIC_DECORATOR_TYPE, styles, (LabelDecoratorBase)product);
                }
                else
                {
                    product = DecoratorFactory.CreateDecorator(DecoratorFactory.RANDOM_DECORATOR_TYPE, styles, (LabelDecoratorBase)product);
                }
            }
            else // helpLabel or other type of label, not a decorator
            {
                if (currentDecorator is TextTransformationDecorator)
                {
                    product = DecoratorFactory.CreateDecorator(DecoratorFactory.TEXT_DECORATOR_TYPE, styles, product);
                }
                else if (currentDecorator is CyclingTransformationsDecorator)
                {
                    product = DecoratorFactory.CreateDecorator(DecoratorFactory.CYCLIC_DECORATOR_TYPE, styles, product);
                }
                else
                {
                    product = DecoratorFactory.CreateDecorator(DecoratorFactory.RANDOM_DECORATOR_TYPE, styles, product);
                }
            }
        }

        public void AddDecorator(LabelDecoratorBase decoratorToApply)
        {
            if (baseProductRegenerationIsNeeded) // a dramatic circumstance has occurred
            {
                Stack<LabelDecoratorBase> styles = RemoveDecorators();
                GenerateBaseProduct();
                ApplyDecoratorsFromStack(ref styles);
            }
            else if (product is null) // the product is yet to be generated
            {
                GenerateBaseProduct();
            }

            LabelDecoratorBase? decoratorCopy = decoratorToApply;
            Stack<LabelDecoratorBase> decoratorsToApply = new Stack<LabelDecoratorBase>();
            while (decoratorCopy is not null) 
            {
                // it removes a single decorator from each step, by using a stack we are guaranteed the order
                decoratorsToApply.Push(decoratorCopy);
                decoratorCopy = decoratorCopy.RemoveWholeDecorator() as LabelDecoratorBase; // it may be a composite
            }
            ApplyDecoratorsFromStack(ref decoratorsToApply);

        }

        public void ResetStyles()
        {
            RemoveDecorators();
            ClearTransformations();
        }

        public void ResetLabel()
        {
            labelType = null;
            helpText = null;
            textColor = null;
            font = null;
            fontSize = null;
            ResetStyles();
            product = null;
        }
    }
}
