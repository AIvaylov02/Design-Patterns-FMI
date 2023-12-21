using DP_Task2.Interfaces;
using System.Text;

namespace DP_Task2.Transformations
{
    public class CensorerTransformation : BadWordTransformation
    {
        public CensorerTransformation(string badWord) : base(badWord) { }

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
    }
}
