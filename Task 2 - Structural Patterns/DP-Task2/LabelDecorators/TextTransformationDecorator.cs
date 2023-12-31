using DP_Task2.Interfaces;

namespace DP_Task2.LabelDecorators
{
    public class TextTransformationDecorator : LabelDecoratorBase
    {
        private ITextTransformation? transformation;
        public TextTransformationDecorator(ILabel label, ITextTransformation transformation) : base(label)
        {
            this.transformation = transformation;
        }

        public TextTransformationDecorator(ILabel label) : base(label)
        {
            transformation = null;
        }

        public override string Text
        {
            get
            {
                if (transformation is null)
                    return label.Text;
                else
                    return transformation.Transform(label.Text);
            }
        }

        private ITextTransformation Transformation
        {
            get => transformation;
            set => transformation = value;
        }



        // transformation is a single style, while a decorator can consist of chaining 2 decorators together
        public override void AddDecorator(ITextTransformation style)
        {
            if (transformation is null)
                transformation = style;
            else // Chain the next transformation (resembles composite pattern)
            {
                label = new TextTransformationDecorator(label, transformation);
                transformation = style;
            }
        }

        // remove the lastly chained/added style
        public override ILabel RemoveDecorator()
        {
            if (transformation is not null)
            {
                transformation = null;
            }
            else // we have to dig into a deeper chaining of removal, remove the next one
            {
                TextTransformationDecorator nextChainedDecorator = label as TextTransformationDecorator; // nextChainded.transformation is the next operation to remove
                while (nextChainedDecorator is not null && nextChainedDecorator.transformation is null) // innerContent is not only label, it is a valid decorator
                {
                    // while we have a valid decorators but they all contain null transformations, skip them

                    label = nextChainedDecorator.label; // get the next label, if it is a decorator dwellve deeper
                    nextChainedDecorator = label as TextTransformationDecorator;

                } // either the current nextChained contains a valid transformation or it is null(simpleLabel)

                if (nextChainedDecorator is not null) // it is a transformation, skip it one more time
                {
                    label = nextChainedDecorator.label;
                    nextChainedDecorator = label as TextTransformationDecorator;
                    transformation = nextChainedDecorator is not null ? nextChainedDecorator.transformation : null;
                }
            }
            return label;
        }

        public override ILabel RemoveDecorator(ITextTransformation style)
        {
            // by keeping all the transformations in a stack we can recursively put them back in place after the style is removed
            // note that this operation may take O(N) to find and O(N) to put back but it leads to O(N) in the end
            Stack<ITextTransformation> transformationToApplyBack = new Stack<ITextTransformation>();

            TextTransformationDecorator? decoratorTraverser = new TextTransformationDecorator(label, transformation);
            ILabel endLabel = decoratorTraverser.label; // it is necessary in order to catch the corner case of the decorator being the last valid decorator
            bool matchFound = IsStyleMatched(style, ref decoratorTraverser, ref endLabel, ref transformationToApplyBack);

            if (matchFound) // reapply the transformations if we have removed a matching transformation
            {
                ApplyStylesFromCertainPointOnward(ref transformationToApplyBack, ref decoratorTraverser, endLabel);
            }

            return this;
        }
        
        public IReadOnlyCollection<ITextTransformation> ExtractTransformationsToOuterWorld()
        {
            Stack<ITextTransformation> extractedStyles = new Stack<ITextTransformation>();
            TextTransformationDecorator? decoratorTraverser = new TextTransformationDecorator(this);
            while (decoratorTraverser is not null)
            {
                ITextTransformation? currentStyle = decoratorTraverser.Transformation;
                if (currentStyle is not null) // to be able to deal with troll transformations added
                {
                    extractedStyles.Push(decoratorTraverser.Transformation);
                }
                decoratorTraverser = decoratorTraverser.label as TextTransformationDecorator;
            }
            return extractedStyles;
        }

        private bool IsStyleMatched(ITextTransformation style, ref TextTransformationDecorator? decoratorTraverser, ref ILabel endLabel, ref Stack<ITextTransformation> transformationsSkipped)
        {
            while (decoratorTraverser is not null)
            {
                if (decoratorTraverser.Transformation.Equals(style))
                {
                    endLabel = decoratorTraverser.label;
                    decoratorTraverser = endLabel as TextTransformationDecorator; // decorator will be null if endLabel is not a decorator
                    return true;
                }

                transformationsSkipped.Push(decoratorTraverser.Transformation);
                endLabel = decoratorTraverser.label;
                decoratorTraverser = endLabel as TextTransformationDecorator;
            }

            return false;
        }

        private TextTransformationDecorator AddManyStylesOntoDecorator(Stack<ITextTransformation> transformationToApply, TextTransformationDecorator whereToApply)
        {
            while (transformationToApply.Count > 0)
            {
                whereToApply = new TextTransformationDecorator(whereToApply, transformationToApply.Pop());
            }
            return whereToApply;
        }

        private void ApplyStylesFromCertainPointOnward(ref Stack<ITextTransformation> transformationToApply, ref TextTransformationDecorator startDecorator, ILabel startPointLabel)
        {
            if (startDecorator is null)
                startDecorator = new TextTransformationDecorator(startPointLabel);
            startDecorator = AddManyStylesOntoDecorator(transformationToApply, startDecorator);
            transformation = startDecorator.Transformation;
            label = startDecorator.label;
        }

        protected override void AddRandomDecorator(RandomTransformationDecorator randomOther)
        {
            List<ITextTransformation> styles = randomOther.Transformations.ToList();
            styles.AddRange(randomOther.AlreadyApplied);
            ApplyStylesFromList(styles);
        }

        protected override void AddTextTransformationDecorator(List<ITextTransformation> styles)
        {
            ApplyStylesFromList(styles);
        }

        protected override void AddCyclicTransformationDecorator(CyclingTransformationsDecorator cyclicOther)
        {
            List<ITextTransformation> styles = cyclicOther.Transformations.ToList();
            ApplyStylesFromList(styles);
        }

        
        protected override ILabel RemoveRandomDecorator(RandomTransformationDecorator randomOther)
        {
            List<ITextTransformation> styles = randomOther.Transformations.ToList();
            styles.AddRange(randomOther.AlreadyApplied);
            RemoveStylesFromList(styles);
            return this;
        }

        // remove N styles from a linked list(This is very slow - N transformations from a linked list of N elements)
        protected override ILabel RemoveTextTransformationDecorator(List<ITextTransformation> styles)
        {
            RemoveStylesFromList(styles);
            return this;
        }

        protected override ILabel RemoveCyclicTransformationDecorator(CyclingTransformationsDecorator cyclicOther)
        {
            List<ITextTransformation> styles = cyclicOther.Transformations.ToList();
            RemoveStylesFromList(styles);
            return this;
        }
    }
}
