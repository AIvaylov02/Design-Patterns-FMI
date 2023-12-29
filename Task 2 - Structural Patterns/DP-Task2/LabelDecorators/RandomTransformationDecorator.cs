using DP_Task2.Interfaces;

namespace DP_Task2.LabelDecorators
{
    public class RandomTransformationDecorator : MultipleTransformationLabelDecorator
    {
        private Random generator;
        private List<ITextTransformation> alreadyApplied;
        public RandomTransformationDecorator(ILabel label) : base(label)
        {
            generator = new Random();
            alreadyApplied = new List<ITextTransformation>();
        }

        public RandomTransformationDecorator(ILabel label, List<ITextTransformation> transformationToClone) : base(label, transformationToClone)
        {
            generator = new Random();
            alreadyApplied = new List<ITextTransformation>();
        }

        public override string Text
        {
            get
            {
                if (transformations.Count == 0 && alreadyApplied.Count == 0) // no valid transformations to apply
                {
                    return label.Text;
                }
                else
                {
                    if (transformations.Count == 0) // all the transformations have already been applied, reroll them back
                    {
                        RerollTransformations();
                    }

                    // the idea is that the final result state will be unchanged, even after removing a style
                    if (decoratedLabelContent is null) // the first transformation is yet to be applied
                        decoratedLabelContent = label.Text;

                    // since we have a valid list of transformations we can generate and apply a random transformation from it
                    ITextTransformation randomTransformation = transformations[GenerateRandomValidNumber()];
                    ApplyTransformation(randomTransformation);

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

        private void ApplyTransformation(ITextTransformation randomTransformation)
        {
            decoratedLabelContent = randomTransformation.Transform(decoratedLabelContent);
            alreadyApplied.Add(randomTransformation);
            transformations.Remove(randomTransformation);
        }

        private void RerollTransformations()
        {
            // for instead of foreach will grant us faster removal
            ITextTransformation style;
            for (int i = 0; i < alreadyApplied.Count; i++)
            {
                style = alreadyApplied[i];
                alreadyApplied.RemoveAt(i);
                transformations.Add(style);
            }
        }


        public override ILabel RemoveDecorator()
        {
            throw new NotImplementedException();
        }

        public override ILabel RemoveDecorator(ITextTransformation style)
        {
            throw new NotImplementedException();
        }


        public override void AddDecorator(ITextTransformation style)
        {
            throw new NotImplementedException();
        }

        public override void AddDecorator(LabelDecoratorBase decoratorOnTop)
        {
            throw new NotImplementedException();
        }

        public override ILabel RemoveDecorator(LabelDecoratorBase decoratorOnTop)
        {
            throw new NotImplementedException();
        }
    }
}
