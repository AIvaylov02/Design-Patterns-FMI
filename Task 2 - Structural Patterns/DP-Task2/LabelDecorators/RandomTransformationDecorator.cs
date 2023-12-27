using DP_Task2.Interfaces;

namespace DP_Task2.LabelDecorators
{
    public class RandomTransformationDecorator : MultipleTransformationLabelDecorator
    {
        private Random generator;
        public RandomTransformationDecorator(ILabel label) : base(label) 
        {
            generator = new Random();
        }

        public RandomTransformationDecorator(ILabel label, List<ITextTransformation> transformationToClone) : base(label, transformationToClone) 
        {
            generator = new Random();
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

                    // since we have a valid list of transformations we can generate and apply a random transformation from it
                    decoratedLabelContent = transformations[GenerateRandomValidNumber()].Transform(decoratedLabelContent);
                    // we want to keep the state after style removal, so we should store the string as an intermediary result
                    return decoratedLabelContent;
                }
            }
        }

        // it is guaranteed that only non-empty list will be searched from
        private int GenerateRandomValidNumber()
        {
            int generatedNum = generator.Next();
            return generatedNum % transformations.Count;
        }
    }
}
