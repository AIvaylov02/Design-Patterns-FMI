using DP_Task2.Interfaces;
using DP_Task2.Labels;

namespace DP_Task2.Utilizers
{
    public static class BaseLabelFactory
    {
        public const string SIMPLE_LABEL_TYPE = "simple";
        public const string RICH_LABEL_TYPE = "rich";
        public const string CUSTOM_LABEL_TYPE = "custom";

        public static ILabel CreateLabel(string type, string? text = null, string? textColor = null, string? font = null, double? fontSize = null)
        {
            switch (type)
            {
                case SIMPLE_LABEL_TYPE:
                    if (text is null)
                        throw new ArgumentNullException($"{nameof(text)} cannot be NULL!");
                    return new SimpleLabel(text);

                case RICH_LABEL_TYPE:
                    if (text is null)
                    {
                        throw new ArgumentNullException($"{nameof(text)} cannot be NULL!");
                    }
                    else if (textColor is null)
                    {
                        throw new ArgumentNullException($"{nameof(textColor)} or color cannot be NULL!");
                    }
                    else if (font is null)
                    {
                        throw new ArgumentNullException($"{nameof(font)} cannot be NULL!");
                    }
                    else if (fontSize is null)
                    {
                        throw new ArgumentNullException($"{nameof(fontSize)} cannot be NULL!");
                    }
                    return new RichLabel(text, textColor, font, (double)fontSize);
                case CUSTOM_LABEL_TYPE:
                    return new RealCustomLabel();
                default:
                    throw new ArgumentException("Given type is currently not supported by the system! Supported types are {simple, rich, custom}");
            }
        }
    }
}
