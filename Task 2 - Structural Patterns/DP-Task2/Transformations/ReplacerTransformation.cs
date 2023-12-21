using DP_Task2.Interfaces;

namespace DP_Task2.Transformations
{
    public class ReplacerTransformation : BadWordTransformation
    {
        private string replacement;
        public ReplacerTransformation(string badWord, string replacement) : base(badWord)
        {
            Replacement = replacement;
        }


        public string Replacement
        {
            get => replacement;
            set
            {
                if (value == null)
                    throw new ArgumentNullException($"{nameof(value)} cannot be NULL!");

                replacement = value;
            }
        }
        // redefine the specific part of the template method Transform
        protected override string GenerateCensorshipReplacement()
        {
            return Replacement;
        }
    }
}
