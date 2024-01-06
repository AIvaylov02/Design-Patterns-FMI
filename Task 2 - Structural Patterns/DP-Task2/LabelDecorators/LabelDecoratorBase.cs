using DP_Task2.Interfaces;
using DP_Task2.Labels;

namespace DP_Task2.LabelDecorators
{
    public abstract class LabelDecoratorBase : IHelpLabel
    {
        protected IHelpLabel label; // if it is just a plain ILabel, then the helpLabel option will return an empty value. This way We can have ILabel Decorators and IHelpLabelDecorators

        public LabelDecoratorBase(ILabel label)
        {
            this.label = new HelpLabel(label);
        }
        public LabelDecoratorBase(IHelpLabel label)
        {
            this.label = label;
        }

        // this will be the overriden strategy method
        public abstract string Text { get; }
        public string HelpText
        {
            get => label.HelpText;
            set => label.HelpText = value;
        }

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

        public void ApplyStylesFromList(List<ITextTransformation> styles)
        {
            List<ITextTransformation> copy = new List<ITextTransformation>(styles);
            while (copy.Count > 0)
            {
                AddDecorator(copy[0]);
                copy.RemoveAt(0);
            }
        }

        public void RemoveStylesFromList(List<ITextTransformation> styles)
        {
            List<ITextTransformation> copy = new List<ITextTransformation>(styles);
            while (copy.Count > 0)
            {
                RemoveDecorator(copy[0]);
                copy.RemoveAt(0);
            }
        }

    }
}
