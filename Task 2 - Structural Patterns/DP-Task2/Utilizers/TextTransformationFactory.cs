using DP_Task2.Interfaces;
using DP_Task2.Transformations;

namespace DP_Task2.Utilizers
{
    public static class TextTransformationFactory
    {
        public const string CAPITALIZER_TYPE = "capitalize";
        public const string COMPOSITION_TRANSFORMATION_TYPE = "compose";
        public const string DECORATION_TRANSFORMATION_TYPE = "decorate";
        public const string SPACE_NORMALIZATION_TYPE = "spacer";
        public const string LEFT_TRIMMER_TYPE = "trimLeft";
        public const string RIGHT_TRIMMER_TYPE = "trimRight";
        public const string CENSORER_TYPE = "censor";
        public const string REPLACER_TYPE = "replacement";

        public static ITextTransformation CreateTransformation(string type, string? badWord = null, string? replacement = null)
        {
            switch (type)
            {
                case CAPITALIZER_TYPE:
                    return new CapitalizeTransformation();
                case COMPOSITION_TRANSFORMATION_TYPE:
                    return new CompositeTransformation(); // It has very limited implementation this way
                case DECORATION_TRANSFORMATION_TYPE:
                    return new DecorationTransformation();
                case SPACE_NORMALIZATION_TYPE:
                    return new SpaceNormalizationTransformation();
                case LEFT_TRIMMER_TYPE:
                    return new TrimLeftTransformation();
                case RIGHT_TRIMMER_TYPE:
                    return new TrimRightTransformation();
                case CENSORER_TYPE:
                    return CensorerTransformationSingletonFactory.Instance.CreateCensorer(badWord); // this may be static class aswell
                case REPLACER_TYPE:
                    return new ReplacerTransformation(badWord, replacement);
                default:
                    throw new ArgumentException("Given type is currently not supported by the system!");
            }
        }
    }
}
