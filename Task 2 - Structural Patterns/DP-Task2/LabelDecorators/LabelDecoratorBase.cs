using DP_Task2.Interfaces;

namespace DP_Task2.LabelDecorators
{
    public abstract class LabelDecoratorBase : ILabel
    {
        protected ILabel label;

        public LabelDecoratorBase(ILabel label)
        {
            this.label = label;
        }

        // this will be the overriden strategy method
        public abstract string Text { get; }

        // text transformation means one style, while a LabelDecoratorBase can be a list of styles
        public abstract void AddDecorator(ITextTransformation style);

        public void AddDecorator(LabelDecoratorBase decoratorOnTop)
        {
            RandomTransformationDecorator? randomOther = decoratorOnTop as RandomTransformationDecorator;
            if (randomOther is not null)
            {
                AddRandomDecorator(randomOther);
            }
            else // simple label or other type
            {
                TextTransformationDecorator? textOther = decoratorOnTop as TextTransformationDecorator;
                if (textOther is not null) // it is textTransformationDecorator
                {
                    // extract its styles
                    List<ITextTransformation> styles = textOther.ExtractTransformationsToOuterWorld().ToList();
                    AddTextTransformationDecorator(styles);
                }
                else
                {
                    CyclingTransformationsDecorator? cyclicOther = decoratorOnTop as CyclingTransformationsDecorator;
                    if (cyclicOther is not null) // it is a cyclic decorator
                    {
                        AddCyclicTransformationDecorator(cyclicOther);
                    }
                    // the if clause is for pure guarding, either it is null or a simple label which are both invalid in the context
                    // cyclicTransformation is ILabel, but none of the labels are LabelDecoratorBase
                }
            }
        }

        
        protected abstract void AddRandomDecorator(RandomTransformationDecorator randomOther);
        protected abstract void AddTextTransformationDecorator(List<ITextTransformation> styles);
        protected abstract void AddCyclicTransformationDecorator(CyclingTransformationsDecorator cyclicOther);

        public abstract ILabel RemoveDecorator();

        public abstract ILabel RemoveDecorator(ITextTransformation style);

        public ILabel RemoveDecorator(LabelDecoratorBase decoratorOnTop)
        {
            RandomTransformationDecorator? randomOther = decoratorOnTop as RandomTransformationDecorator;
            if (randomOther is not null)
            {
                return RemoveRandomDecorator(randomOther);
            }
            else // simple label or other type
            {
                TextTransformationDecorator? textOther = decoratorOnTop as TextTransformationDecorator;
                if (textOther is not null) // it is textTransformationDecorator
                {
                    // extract its styles
                    List<ITextTransformation> styles = textOther.ExtractTransformationsToOuterWorld().ToList();
                    return RemoveTextTransformationDecorator(styles);
                }
                else
                {
                    CyclingTransformationsDecorator? cyclicOther = decoratorOnTop as CyclingTransformationsDecorator;
                    if (cyclicOther is not null) // it is a cyclic decorator
                    {
                        return RemoveCyclicTransformationDecorator(cyclicOther);
                    }
                    // the if clause is for pure guarding, either it is null or a simple label which are both invalid in the context
                    // cyclicTransformation is ILabel, but none of the labels are LabelDecoratorBase
                }
            }

            return label;
        }

        protected abstract ILabel RemoveRandomDecorator(RandomTransformationDecorator randomOther);

        protected abstract ILabel RemoveTextTransformationDecorator(List<ITextTransformation> styles);

        protected abstract ILabel RemoveCyclicTransformationDecorator(CyclingTransformationsDecorator cyclicOther);


        public static ILabel RemoveDecoratorFrom(ILabel label, ITextTransformation style)
        {
            if (label is not null)
            {
                LabelDecoratorBase decorator = label as LabelDecoratorBase;
                if (decorator is not null)
                {
                    decorator.RemoveDecorator(style);
                }
            }

            return label;
        }

        public static ILabel RemoveDecoratorFrom(ILabel label, LabelDecoratorBase decorationsToRemove)
        {
            if (label is not null)
            {
                LabelDecoratorBase decorator = label as LabelDecoratorBase;
                if (decorator is not null)
                {
                    decorator.RemoveDecorator(decorationsToRemove);
                }
            }

            return label;
        }


    }
}
