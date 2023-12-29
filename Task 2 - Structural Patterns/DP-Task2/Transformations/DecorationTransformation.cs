using DP_Task2.Interfaces;
using System.Text;

namespace DP_Task2.Transformations
{
    public class DecorationTransformation : ITextTransformation, IEquatable<DecorationTransformation>
    {
        public bool Equals(DecorationTransformation? other)
        {
            return other is DecorationTransformation;
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as DecorationTransformation);
        }

        public string Transform(string text)
        {
            if (text == null)
                throw new ArgumentNullException("Text for transformation cannot be NULL!");

            const string PREPENDER = "-={ ";
            char[] reversed = PREPENDER.Reverse().ToArray();
            reversed[1] = '}';
            string APPENDER = string.Join("", reversed); // reverses the string

            StringBuilder sb = new StringBuilder(PREPENDER);
            sb.Append(text);
            sb.Append(APPENDER);
            return sb.ToString();
        }
    }
}
