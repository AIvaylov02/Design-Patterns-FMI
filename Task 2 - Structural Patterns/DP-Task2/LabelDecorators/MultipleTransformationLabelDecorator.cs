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

        public IReadOnlyList<ITextTransformation> Transformations
        {
            get => transformations;
        }

        public override sealed void AddDecorator(ITextTransformation style)
        {
            if (style is not null)
            {
                transformations.Add(style);
            }
        }

        protected override sealed void AddCyclicTransformationDecorator(CyclingTransformationsDecorator cyclicOther)
        {
            transformations.AddRange(cyclicOther.Transformations);
        }

        protected override sealed void AddRandomDecorator(RandomTransformationDecorator randomOther)
        {
            // add all of its member styles(applied and not) to the unapplied transformations of current
            transformations.AddRange(randomOther.Transformations);
            transformations.AddRange(randomOther.AlreadyApplied);
        }

        protected override sealed void AddTextTransformationDecorator(List<ITextTransformation> styles)
        {
            transformations.AddRange(styles);
        }

        protected override sealed ILabel RemoveRandomDecorator(RandomTransformationDecorator randomOther)
        {
            List<ITextTransformation> stylesToRemove = randomOther.Transformations.ToList();
            stylesToRemove.AddRange(randomOther.AlreadyApplied);
            foreach (ITextTransformation style in stylesToRemove)
            {
                RemoveDecorator(style);
            }
            return this;
        }

        protected override sealed ILabel RemoveTextTransformationDecorator(List<ITextTransformation> styles)
        {
            foreach (ITextTransformation style in styles)
            {
                RemoveDecorator(style);
            }
            return this;
        }

        protected override sealed ILabel RemoveCyclicTransformationDecorator(CyclingTransformationsDecorator cyclicOther)
        {
            List<ITextTransformation> stylesToRemove = cyclicOther.Transformations.ToList();
            foreach (ITextTransformation style in stylesToRemove)
            {
                RemoveDecorator(style);
            }
            return this;
        }

    }
}
