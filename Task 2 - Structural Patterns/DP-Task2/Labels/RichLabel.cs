using DP_Task2.Interfaces;

namespace DP_Task2.Labels
{
    public class RichLabel : IRichLabel
    {
        string text;
        string textColor;
        string font;
        double fontSize;

        public RichLabel(string text, string textColor, string font, double fontSize)
        {
            this.text = text;
            this.textColor = textColor;
            this.font = font;
            FontSize = fontSize;
        }

        public string TextColor => textColor;
        public string Font => font;
        public double FontSize
        {
            get => fontSize;
            set
            {
                if (value < 0 || value < 0.0001)
                    throw new ArgumentException("Font size must be greater than 0!");
                fontSize = value;
            }
        }

        public string Text => text;
    }
}
