using DP_Task2.Interfaces;
using DP_Task2.Labels;

namespace DP_Task2.Utilizers
{
    public static class BaseLabelFactory
    {
        public static ILabel CreateLabel(string type, string? text = null, string? textColor = null, string? font = null, double? fontSize = null)
        {
            switch (type)
            {
                case "simple":
                    if (text is null)
                        throw new ArgumentNullException($"{nameof(fontSize)} cannot be NULL!");
                    return new SimpleLabel(text);

                case "rich":
                    if (text is null || textColor is null || font is null || fontSize is null)
                        throw new ArgumentNullException($"{nameof(fontSize)} cannot be NULL!");
                    return new RichLabel(text, textColor, font, (double)fontSize);
                case "custom":
                    return new RealCustomLabel();
                default:
                    throw new ArgumentException("Given type is currently not supported by the system! Supported types are {simple, rich, custom}");
            }
        }
    }
}
