using DP_Task2.Interfaces;

namespace DP_Task2.Transformations
{
    public class CompositeTransformation : ITextTransformation, IEquatable<CompositeTransformation>
    {
        ITextTransformation? currentTransformation;
        CompositeTransformation? nextTransformation;
        public CompositeTransformation()
        {
            currentTransformation = null;
            nextTransformation = null;
        }
        public CompositeTransformation(ITextTransformation style)
        {
            currentTransformation = style;
            nextTransformation = null;
        }

        public CompositeTransformation(List<ITextTransformation> styles)
        {
            currentTransformation = null;
            nextTransformation = null;
            if (styles is not null && styles.Count > 0)
            {
                currentTransformation = styles[0];
                nextTransformation = new CompositeTransformation(styles.Skip(1).ToList());
                if (nextTransformation.currentTransformation is null) // if there are no more transformations to stack on top of
                {
                    nextTransformation = null;
                }
            }
        }

        public bool Equals(CompositeTransformation? other)
        {
            if (other is not CompositeTransformation)
                return false;

            if (currentTransformation is null && other.currentTransformation is null)
                return true;
            else
            {
                if (currentTransformation is null || other.currentTransformation is null) // only one of the 2s are null
                    return false;

                //both are not null
                if (!currentTransformation.Equals(other.currentTransformation))
                    return false;
                else
                {
                    if (nextTransformation is null && other.nextTransformation is null) // the lists are matching, stop the recursive equality
                        return true;
                    else if (nextTransformation is null) // the other is not null
                        return false;
                    else
                        return nextTransformation.Equals(other.nextTransformation);
                }
            }
        }

        // by task definition, we cannot remove styles on the fly, but I have that covered as my TextDecorator is a composite in itself :)

        public override bool Equals(object? obj)
        {
            if (obj is null && this is null)
                return true;
            else if (obj is null)
                return false;
            else
                return Equals(obj as CompositeTransformation);
        }

        public string Transform(string text)
        {
            if (text is null)
                throw new ArgumentNullException("Text to transform cannot be null!");


            if (currentTransformation is not null)
            {
                string result = currentTransformation.Transform(text);
                if (nextTransformation is not null)
                {
                    result = nextTransformation.Transform(result);

                }
                return result;
            }
            else
                return text; // no transformation has been applied as styles are empty
            
        }
    }
}
