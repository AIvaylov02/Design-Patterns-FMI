namespace DP_Task2.Interfaces
{
    public interface ILabelBuilder
    {
        // label type underneath
        string LabelType { get; set; }

        // with or without help text
        string HelpText { get; set; }
        // Add help text
        // remove help text if such exists - let it be always

        IReadOnlyCollection<ITextTransformation> Transformations { get; }

        public void AddTextColor(string textColor);
        public void AddFont(string font);
        public void AddFontSize(double fontSize);
        public string RemoveTextColor();
        public string RemoveFont();
        public double RemoveFontSize();

        void ClearTransformations();
        void AddTransformation(string type, string? censoredWord = null, string? replacement = null);
        void RemoveTransformation(string type, string? censoredWord = null, string? replacement = null);

        void ApplyTransformationsAsADecorator(string decoratorType);
        void RemoveDecorator();
        void RemoveDecorators();

        ILabel? BuildLabel();
    }
}
