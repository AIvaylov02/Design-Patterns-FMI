using DP_Task2.Interfaces;

namespace DP_Task2.LabelDecorators
{
    public class CyclingTransformationsDecorator : MultipleTransformationLabelDecorator
    {
        private int nextTransformationIndex;

        public CyclingTransformationsDecorator(ILabel label) : base(label)
        {
            nextTransformationIndex = 0;
        }

        public CyclingTransformationsDecorator(ILabel label, List<ITextTransformation> transformationToClone) : base(label, transformationToClone)
        {
            nextTransformationIndex = 0;
        }

        public override string Text
        {
            get
            {
                if (transformations.Count == 0) // no valid transformations to apply
                {
                    return label.Text;
                }
                else
                {
                    // the idea is that the final result state will be unchanged, even after removing a style
                    if (decoratedLabelContent is null) // the first transformation is yet to be applied
                        decoratedLabelContent = label.Text;

                    decoratedLabelContent = transformations[nextTransformationIndex++].Transform(decoratedLabelContent);
                    nextTransformationIndex %= transformations.Count;
                    return decoratedLabelContent;
                }
            }
        }

        public override void AddDecorator(ITextTransformation style)
        {
            throw new NotImplementedException();
        }

        public override void AddDecorator(LabelDecoratorBase decoratorOnTop)
        {
            throw new NotImplementedException();
        }

        public override ILabel RemoveDecorator()
        {
            throw new NotImplementedException();
        }

        public override ILabel RemoveDecorator(ITextTransformation style)
        {
            throw new NotImplementedException();
        }

        public override ILabel RemoveDecorator(LabelDecoratorBase decoratorOnTop)
        {
            throw new NotImplementedException();
        }
    }
}
