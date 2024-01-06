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

        public RandomTransformationDecorator(IHelpLabel label) : base(label)
        {
            generator = new Random();
            alreadyApplied = new List<ITextTransformation>();
        }

        public RandomTransformationDecorator(ILabel label, List<ITextTransformation> transformationToClone) : base(label, transformationToClone)
        {
            generator = new Random();
            alreadyApplied = new List<ITextTransformation>();
        }

        public RandomTransformationDecorator(IHelpLabel label, List<ITextTransformation> transformationToClone) : base(label, transformationToClone)
        {
            generator = new Random();
            alreadyApplied = new List<ITextTransformation>();
        }

        public IReadOnlyList<ITextTransformation> AlreadyApplied
        {
            get => alreadyApplied;
        }


        public override string Text
        {
            get
            {
                if (transformations.Count == 0 && alreadyApplied.Count == 0) // no valid transformations to apply
                {
                    decoratedLabelContent = label.Text;
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
                }

                return decoratedLabelContent;
            }
        }

        // remove a random already applied one
        public override ILabel RemoveDecorator()
        {
            if (alreadyApplied.Count != 0) // remove from the already appllied as they are less interesting
            {
                alreadyApplied.RemoveAt(alreadyApplied.Count - 1); // the style just added seems the most boring
            }
            else
            {
                if (transformations.Count != 0)
                {
                    transformations.RemoveAt(0); // the first style of all the list seems the least interesting as it gets repeated the most times
                }
            }

            return this;
        }

        public override ILabel RemoveDecorator(ITextTransformation style)
        {
            if (style is not null)
            {
                bool hasBeenRemoved = alreadyApplied.Remove(style);
                if (!hasBeenRemoved)
                {
                    transformations.Remove(style);
                }
            }
            return this;
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
            transformations.AddRange(alreadyApplied);
            alreadyApplied.Clear();
        }

        public override void ResetStyles()
        {
            transformations.Clear();
            alreadyApplied.Clear();
        }
    }
}
