using DP_Task2.Interfaces;
using System.Text;

namespace DP_Task2.Transformations
{
    public class SpaceNormalizationTransformation : ITextTransformation, IEquatable<SpaceNormalizationTransformation>
    {
        public bool Equals(SpaceNormalizationTransformation? other)
        {
            return other is SpaceNormalizationTransformation;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null && this is null)
                return true;
            else if (obj is null)
                return false;
            else
                return Equals(obj as SpaceNormalizationTransformation);
        }

        public string Transform(string text)
        {
            if (text == null)
                throw new ArgumentNullException("Text for transformation cannot be NULL!");

            if (string.Empty == text)
                return string.Empty;

            const char WHITE_SPACE = ' ';
            // split by ' ', if 2 consecutive spaces are met an empty entry will be made, thus ignoring all repeating spaces.
            // Example "abc  ab" leads to arr{"abc", "", "ab"} and we will ommit the second entry. Thus having only "abc" and "ab" which we join
            string[] splitText = text.Split(WHITE_SPACE, StringSplitOptions.RemoveEmptyEntries); 
            StringBuilder sb = new StringBuilder(string.Join(WHITE_SPACE, splitText));

            // Corner case checks for leading or trailing white spaces in the original
            ushort timesEndSpaceHasBeenAdded = 0;
            if (text[0] == WHITE_SPACE)
            {
                sb.Insert(0, WHITE_SPACE);
                timesEndSpaceHasBeenAdded++;
            }
            if (text[text.Length - 1] == WHITE_SPACE)
            {
                sb.Append(WHITE_SPACE);
                timesEndSpaceHasBeenAdded++;
            }

            // check if the string consists only of 2 whitespaces, that would be unappropriate! It should shrink to one
            if (timesEndSpaceHasBeenAdded == 2 && sb.Length == 2) 
                sb.Remove(0, 1); // remove the first space |_|

           return sb.ToString();
        }
    }
}
