using DP_Task2.Interfaces;

namespace DP_Task2.Transformations
{
    public class TrimRightTransformation : ITextTransformation, IEquatable<TrimRightTransformation>
    {
        public bool Equals(TrimRightTransformation? other)
        {
            return other is TrimRightTransformation;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null && this is null)
                return true;
            else if (obj is null)
                return false;
            else
                return Equals(obj as TrimRightTransformation);
        }

        public string Transform(string text)
        {
            if (text == null)
                throw new ArgumentNullException("Text cannot be NULL!");

            return text.TrimEnd(); // no parameterers will trim all white spaces (space, tab, etc.)
        }
    }
}
