using System.Text;

namespace DP_Task2.Transformations
{
    public class CensorerTransformation : BadWordTransformation, IEquatable<CensorerTransformation>
    {
        public CensorerTransformation(string? badWord) : base(badWord) { }

        public bool Equals(CensorerTransformation? other)
        {
            if (other is not CensorerTransformation)
                return false;

            return BadWord == other.BadWord;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null && this is null)
                return true;
            else if (obj is null)
                return false;
            else
                return Equals(obj as CensorerTransformation);
        }

        // redefine the specific part of the template method Transform
        protected override string GenerateCensorshipReplacement()
        {
            const char CENSORSHIP_TOKEN = '*';
            if (BadWord == string.Empty)
                return CENSORSHIP_TOKEN.ToString(); // if the string is Empty than, the replacement should be '*', a.k.a no empty strings allowed

            StringBuilder sb = new StringBuilder();
            foreach (char ch in BadWord)
                sb.Append(CENSORSHIP_TOKEN);
            return sb.ToString();
        }

        // since it will be used as flyweight, we should prohibit the ability to change the underlying badWord
        public new string BadWord
        {
            get => badWord;
            private set
            {
                if (value is null)
                    throw new ArgumentNullException($"{nameof(value)} cannot be NULL!");

                badWord = value;
            }
        }
    }
}
