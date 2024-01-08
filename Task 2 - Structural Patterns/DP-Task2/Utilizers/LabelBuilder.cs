using DP_Task2.Interfaces;
using DP_Task2.LabelDecorators;
using DP_Task2.Labels;
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

        public LabelBuilder(string text)
        {
            labelType = null;
            this.text = text;
            //helpText = HelpLabel.MISSING_HELP_MESSAGE;
            transformations = new List<ITextTransformation>();
            product = null;
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
                labelType = value;
            }
        }
        public string HelpText
        {
            get => helpText;
            set
            {
                if (value is null)
                    throw new ArgumentNullException($"{nameof(value)} cannot be marked as NULL");
                else
                    helpText = value;
            }
        }

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

        public void RemoveDecorator()
        {
            if (product is not null)
            {
                LabelDecoratorBase decorator = product as LabelDecoratorBase;
                if (decorator is not null)
                {
                    product = decorator.RemoveDecorator();
                }
                // current product can be a decorator/ iLabel or helpLabel
            }
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
                product = DecoratorFactory.CreateDecorator(decoratorType, transformations, product);
                transformations.Clear(); // if the application has been succesful, then clear the styles
            }
            catch (ArgumentException ex) // unsupported style
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void RemoveDecorators()
        {
            product = null;
        }
        private void GenerateBaseProduct()
        {
            // generate base and apply help text if such is present
            product = BaseLabelFactory.CreateLabel(labelType, text, textColor, font, fontSize);
            if (helpText is not null) // if you have helpText you will be marked as helpLabel
            {
                product = new HelpLabel(product, helpText);
            }

        }

        public ILabel? BuildLabel()
        {
            try
            {
                if (product is null) // no decorators have been applied yet (product has not been generated yet)
                {
                    GenerateBaseProduct();
                }
                const string DEFAULT_DECORATOR_TYPE = "simple";
                ApplyTransformationsAsADecorator(DEFAULT_DECORATOR_TYPE);
                return product;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return null;
            }
        }
    }
}
