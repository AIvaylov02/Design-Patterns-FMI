using DP_Task2.LabelDecorators;

namespace DP_Task2.Interfaces
{
    public interface ILabelBuilder
    {
        string LabelType { get; set; }
        string HelpText { get; set; }
        public string? TextColor { get; set; }
        public string? Font { get; set; }
        public double? FontSize { get; set; }

        IReadOnlyCollection<ITextTransformation> Transformations { get; }

        public void AddTextColor(string textColor);
        public void AddFont(string font);
        public void AddFontSize(double fontSize);
        public string RemoveTextColor();
        public string RemoveFont();
        public double RemoveFontSize();

        void ClearTransformations();
        void AddTransformation(string type, string? censoredWord = null, string? replacement = null);
        
        void AddTransformation(ITextTransformation transformation);
        void RemoveTransformation(string type, string? censoredWord = null, string? replacement = null);

        void RemoveTransformation(ITextTransformation transformation);
        void RemoveTransformation(); // remove the lastly added transformation

        void ApplyTransformationsAsADecorator(string decoratorType);
        void AddDecorator(LabelDecoratorBase decoratorToApply);
        LabelDecoratorBase? RemoveDecorator(); // remove the lastly added decorator
        Stack<LabelDecoratorBase> RemoveDecorators(); // clear all decorators

        void ResetStyles();

        void ResetLabel();
        ILabel? BuildLabel();
    }
}
