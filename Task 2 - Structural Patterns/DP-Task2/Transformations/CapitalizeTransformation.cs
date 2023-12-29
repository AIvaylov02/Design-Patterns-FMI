using DP_Task2.Interfaces;

namespace DP_Task2.Transformations
{
    public class CapitalizeTransformation : ITextTransformation, IEquatable<CapitalizeTransformation>
    {
        public bool Equals(CapitalizeTransformation? other)
        {
            return other is CapitalizeTransformation;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as CapitalizeTransformation);
        }

        public string Transform(string text)
        {
            if (text == null)
                throw new ArgumentNullException("Text for transformation cannot be NULL!");

            if (text == string.Empty) // str == ""
                return string.Empty;

            string firstLetter = char.ToUpper(text[0]).ToString(); // StringBuilder will be overkill here as we just concatenate once
            return firstLetter + text.Substring(1); // if there are no more characters it will return just firstLetter capitalized(if it is a letter)
        }
    }
}
