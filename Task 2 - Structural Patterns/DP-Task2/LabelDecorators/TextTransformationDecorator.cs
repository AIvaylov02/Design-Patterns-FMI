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

    }
}
