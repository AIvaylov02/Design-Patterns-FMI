using DP_Task2.Interfaces;

namespace DP_Task2.LabelDecorators
{
    public abstract class MultipleTransformationLabelDecorator : LabelDecoratorBase
    {
        // by keeping a state of the decoratedContent we can easily access the result, without the need of repeating the transformations
        // this is especially useful when the transformations are of count N
        protected string? decoratedLabelContent;
        protected List<ITextTransformation> transformations;

        public MultipleTransformationLabelDecorator(ILabel label) : base(label) 
        {
            decoratedLabelContent = null;
            transformations = new List<ITextTransformation>();
        }
        public MultipleTransformationLabelDecorator(ILabel label, List<ITextTransformation> transformations) : base(label)
        {
            // TODO check for deep copying if necessary
            decoratedLabelContent = null;
            this.transformations = new List<ITextTransformation>(transformations);
        }

    }
}
