using DP_Task2.Interfaces;

namespace DP_Task2.Transformations
{
    public class TrimLeftTransformation : ITextTransformation, IEquatable<TrimLeftTransformation>
    {
        public bool Equals(TrimLeftTransformation? other)
        {
            return other is TrimLeftTransformation;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null && this is null)
                return true;
            else if (obj is null)
                return false;
            else
                return Equals(obj as TrimLeftTransformation);
        }

        public string Transform(string text)
        {
            if (text == null)
                throw new ArgumentNullException("Text cannot be NULL!");

            return text.TrimStart(); // no parameterers will trim all white spaces (space, tab, etc.)
        }
    }
}
