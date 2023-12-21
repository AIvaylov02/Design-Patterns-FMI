namespace DP_Task2.Interfaces
{
    public interface IBadWordTransformation : ITextTransformation
    {
        // all methods are abstract by default
        public string BadWord { get; set; }
    }
}
