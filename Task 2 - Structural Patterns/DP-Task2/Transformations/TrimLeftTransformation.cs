using DP_Task2.Interfaces;

namespace DP_Task2.Transformations
{
    public class TrimLeftTransformation : ITextTransformation
    {
        public string Transform(string text)
        {
            if (text == null)
                throw new ArgumentNullException("Text cannot be NULL!");

            return text.TrimStart(); // no parameterers will trim all white spaces (space, tab, etc.)
        }
    }
}
