﻿
namespace DP_Task2.Transformations
{
    public class ReplacerTransformation : BadWordTransformation, IEquatable<ReplacerTransformation>
    {
        private string replacement;
        public ReplacerTransformation(string? badWord, string? replacement) : base(badWord)
        {
            Replacement = replacement;
        }

        public bool Equals(ReplacerTransformation? other)
        {
            if (other is not ReplacerTransformation)
                return false;

            return BadWord == other.BadWord && Replacement == other.Replacement;
        }

        public override bool Equals(object? obj)
        {
            if (obj is null && this is null)
                return true;
            else if (obj is null)
                return false;
            else
                return Equals(obj as ReplacerTransformation);
        }


        public string Replacement
        {
            get => replacement;
            set
            {
                if (value is null)
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
