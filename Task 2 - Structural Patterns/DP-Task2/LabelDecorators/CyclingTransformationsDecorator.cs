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

        public CyclingTransformationsDecorator(IHelpLabel label) : base(label)
        {
            nextTransformationIndex = 0;
        }

        public CyclingTransformationsDecorator(ILabel label, List<ITextTransformation> transformationToClone) : base(label, transformationToClone)
        {
            nextTransformationIndex = 0;
        }

        public CyclingTransformationsDecorator(IHelpLabel label, List<ITextTransformation> transformationToClone) : base(label, transformationToClone)
        {
            nextTransformationIndex = 0;
        }

        public override string Text
        {
            get
            {
                if (transformations.Count == 0) // no valid transformations to apply
                {
                    decoratedLabelContent = label.Text;
                }
                else
                {
                    // the idea is that the final result state will be unchanged, even after removing a style
                    if (decoratedLabelContent is null) // the first transformation is yet to be applied
                        decoratedLabelContent = label.Text;

                    decoratedLabelContent = transformations[nextTransformationIndex++].Transform(decoratedLabelContent);
                    nextTransformationIndex %= transformations.Count;
                }

                return decoratedLabelContent;
            }
        }

        public override ILabel RemoveDecorator()
        {
            if (transformations.Count != 0)
            {
                int removeIndex = transformations.Count - 1;
                if (nextTransformationIndex == removeIndex)
                    nextTransformationIndex = 0;
                transformations.RemoveAt(removeIndex);
            }
            return this;
        }

        public override ILabel RemoveDecorator(ITextTransformation style)
        {
            if (style is not null)
            {
                int removeIndex = transformations.LastIndexOf(style);
                if (removeIndex != -1) // style is found in the collection
                {
                    if (removeIndex <= nextTransformationIndex)
                    {
                        nextTransformationIndex--;
                        if (nextTransformationIndex < 0)
                            nextTransformationIndex = 0;
                    }

                    transformations.RemoveAt(removeIndex);
                }
                
            }
            return this;
        }

        public override void ResetStyles()
        {
            transformations.Clear();
            nextTransformationIndex = 0;
        }
    }
}
