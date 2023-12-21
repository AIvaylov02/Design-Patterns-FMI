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
            // this is unchangeable code by design. By the task definition there are no NULL CHECKS, so we don't check for it but it also won't be NULL.
            // It is guaranteed valid
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
