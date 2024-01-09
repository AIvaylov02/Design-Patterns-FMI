using DP_Task2.Interfaces;
using DP_Task2.LabelDecorators;

namespace DP_Task2.Utilizers
{
    public static class DecoratorFactory
    {
        public const string TEXT_DECORATOR_TYPE = "text";
        public const string CYCLIC_DECORATOR_TYPE = "cyclic";
        public const string RANDOM_DECORATOR_TYPE = "random";

        public static LabelDecoratorBase CreateDecorator(string decoratorType, List<ITextTransformation> transformations, ILabel labelToDecorate)
        {
            switch (decoratorType)
            {
                case TEXT_DECORATOR_TYPE:
                    TextTransformationDecorator decorator = new TextTransformationDecorator(labelToDecorate);
                    foreach (ITextTransformation style in transformations)
                    {
                        decorator.AddDecorator(style);
                    }
                    return decorator;
                case CYCLIC_DECORATOR_TYPE:
                    return new CyclingTransformationsDecorator(labelToDecorate, transformations);
                case RANDOM_DECORATOR_TYPE:
                    return new RandomTransformationDecorator(labelToDecorate, transformations);
                default:
                    throw new ArgumentException("Given type is currently not supported by the system!");
            }
        }

        public static LabelDecoratorBase CreateDecorator(string decoratorType, List<ITextTransformation> transformations, IHelpLabel helpLabelToDecorate)
        {
            switch (decoratorType)
            {
                case TEXT_DECORATOR_TYPE:
                    TextTransformationDecorator decorator = new TextTransformationDecorator(helpLabelToDecorate);
                    foreach (ITextTransformation style in transformations)
                    {
                        decorator.AddDecorator(style);
                    }
                    return decorator;
                case CYCLIC_DECORATOR_TYPE:
                    return new CyclingTransformationsDecorator(helpLabelToDecorate, transformations);
                case RANDOM_DECORATOR_TYPE:
                    return new RandomTransformationDecorator(helpLabelToDecorate, transformations);
                default:
                    throw new ArgumentException("Given type is currently not supported by the system!");
            }
        }

        public static LabelDecoratorBase CreateDecorator(string decoratorType, List<ITextTransformation> transformations, LabelDecoratorBase decoratorToDecorate)
        {
            switch (decoratorType)
            {
                case TEXT_DECORATOR_TYPE:
                    TextTransformationDecorator decorator = new TextTransformationDecorator(decoratorToDecorate);
                    foreach (ITextTransformation style in transformations)
                    {
                        decorator.AddDecorator(style);
                    }
                    return decorator;
                case CYCLIC_DECORATOR_TYPE:
                    return new CyclingTransformationsDecorator(decoratorToDecorate, transformations);
                case RANDOM_DECORATOR_TYPE:
                    return new RandomTransformationDecorator(decoratorToDecorate, transformations);
                default:
                    throw new ArgumentException("Given type is currently not supported by the system!");
            }
        }

        public static LabelDecoratorBase CreateDecoratorWithoutStyles(string decoratorType, ILabel labelToDecorate)
        {
            switch (decoratorType)
            {
                case TEXT_DECORATOR_TYPE:
                    return new TextTransformationDecorator(labelToDecorate);
                case CYCLIC_DECORATOR_TYPE:
                    return new CyclingTransformationsDecorator(labelToDecorate);
                case RANDOM_DECORATOR_TYPE:
                    return new RandomTransformationDecorator(labelToDecorate);
                default:
                    throw new ArgumentException("Given type is currently not supported by the system!");
            }
        }

        public static LabelDecoratorBase CreateDecoratorWithoutStyles(string decoratorType, IHelpLabel helpLabelToDecorate)
        {
            switch (decoratorType)
            {
                case TEXT_DECORATOR_TYPE:
                    return new TextTransformationDecorator(helpLabelToDecorate);
                case CYCLIC_DECORATOR_TYPE:
                    return new CyclingTransformationsDecorator(helpLabelToDecorate);
                case RANDOM_DECORATOR_TYPE:
                    return new RandomTransformationDecorator(helpLabelToDecorate);
                default:
                    throw new ArgumentException("Given type is currently not supported by the system!");
            }
        }

        public static LabelDecoratorBase CreateDecoratorWithoutStyles(string decoratorType, LabelDecoratorBase decoratorToDecorate)
        {
            switch (decoratorType)
            {
                case TEXT_DECORATOR_TYPE:
                    return new TextTransformationDecorator(decoratorToDecorate);
                case CYCLIC_DECORATOR_TYPE:
                    return new CyclingTransformationsDecorator(decoratorToDecorate);
                case RANDOM_DECORATOR_TYPE:
                    return new RandomTransformationDecorator(decoratorToDecorate);
                default:
                    throw new ArgumentException("Given type is currently not supported by the system!");
            }
        }
    }
}
