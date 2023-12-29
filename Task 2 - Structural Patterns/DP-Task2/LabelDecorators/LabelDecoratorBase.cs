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

        public abstract void AddDecorator(LabelDecoratorBase decoratorOnTop);

        public abstract ILabel RemoveDecorator();

        public abstract ILabel RemoveDecorator(ITextTransformation style);

        // not yet tested
        public abstract ILabel RemoveDecorator(LabelDecoratorBase decoratorOnTop);

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
