using DP_Task2.Interfaces;


namespace DP_Task2.Transformations
{
    abstract public class BadWordTransformation : IBadWordTransformation
    {
        protected string badWord;

        public BadWordTransformation(string badWord)
        {
            BadWord = badWord;
        }
        public string BadWord
        {
            get => badWord;
            set
            {
                if (value == null)
                    throw new ArgumentNullException($"{nameof(value)} cannot be NULL!");

                badWord = value;
            }
        }

        public string Transform(string text) // this is not virtual and will be used as base of a template method
        {
            if (text == null)
                throw new ArgumentNullException("Text for transformation cannot be NULL!");
                
            string replacement = GenerateCensorshipReplacement();
            // if the whole word is empty string and we want to replace the occurances of empty string, then just return replacement
            if (BadWord == string.Empty && text == string.Empty)
                return replacement;

            // if text ABC is given and bad word is ABC and we want to switch it with ABC then, it is as nothing has been applied
            // as well we cannot search for empty string within a full one
            if (replacement == BadWord || BadWord == string.Empty)
                return text;
                
            return text.Replace(BadWord, replacement);
        }

        abstract protected string GenerateCensorshipReplacement();
    }
}
