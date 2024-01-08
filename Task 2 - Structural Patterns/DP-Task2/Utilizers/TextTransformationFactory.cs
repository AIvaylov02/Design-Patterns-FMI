using DP_Task2.Interfaces;
using DP_Task2.Transformations;

namespace DP_Task2.Utilizers
{
    public static class TextTransformationFactory
    {
        public static ITextTransformation CreateTransformation(string type, string? badWord = null, string? replacement = null)
        {
            switch (type)
            {
                case "capitalize":
                    return new CapitalizeTransformation();
                case "compose":
                    return new CompositeTransformation(); // It has very limited implementation this way
                case "decorate":
                    return new DecorationTransformation();
                case "spacer":
                    return new SpaceNormalizationTransformation();
                case "trimLeft":
                    return new TrimLeftTransformation();
                case "trimRight":
                    return new TrimRightTransformation();
                case "censor":
                    return CensorerTransformationSingletonFactory.Instance.CreateCensorer(badWord); // this may be static class aswell
                case "replacement":
                    return new ReplacerTransformation(badWord, replacement);
                default:
                    throw new ArgumentException("Given type is currently not supported by the system!");
            }
        }
    }
}
