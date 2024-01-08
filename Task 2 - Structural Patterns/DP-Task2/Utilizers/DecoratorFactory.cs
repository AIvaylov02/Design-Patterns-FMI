using DP_Task2.Interfaces;
using DP_Task2.LabelDecorators;

namespace DP_Task2.Utilizers
{
    public static class DecoratorFactory
    {
        public static LabelDecoratorBase CreateDecorator(string decoratorType, List<ITextTransformation> transformations, ILabel labelToDecorate)
        {
            switch (decoratorType)
            {
                case "text":
                    TextTransformationDecorator decorator = new TextTransformationDecorator(labelToDecorate);
                    foreach (ITextTransformation style in transformations)
                    {
                        decorator.AddDecorator(style);
                    }
                    return decorator;
                case "cyclic":
                    return new CyclingTransformationsDecorator(labelToDecorate, transformations);
                case "random":
                    return new RandomTransformationDecorator(labelToDecorate, transformations);
                default:
                    throw new ArgumentException("Given type is currently not supported by the system!");
            }
        }
    }
}
