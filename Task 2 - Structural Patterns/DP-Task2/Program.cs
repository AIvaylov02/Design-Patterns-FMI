using DP_Task2.Interfaces;
using DP_Task2.LabelDecorators;
using DP_Task2.Labels;
using DP_Task2.Transformations;

namespace DP_Task2
{
    public class Program
    {
        static void Main(string[] args)
        {
            List<ITextTransformation> transformations = new List<ITextTransformation>()
            {
                new CapitalizeTransformation(),
                new CensorerTransformation("wakanda"),
                new DecorationTransformation(),
                new ReplacerTransformation("wakanda", "forever"),
                new SpaceNormalizationTransformation(),
                new TrimLeftTransformation(),
                new TrimRightTransformation()
            };
            List<ITextTransformation> transformations2 = new List<ITextTransformation>()
            {
                new CapitalizeTransformation(),
                new CensorerTransformation("waSkanda"),
                new DecorationTransformation(),
                new ReplacerTransformation("BAGA", "forever"),
                new SpaceNormalizationTransformation(),
                new TrimLeftTransformation(),
                new TrimRightTransformation()
            };

            for (int i = 0; i < transformations2.Count; i++)
            {
                bool result = transformations[i].Equals(transformations2[i]);
                Console.WriteLine(result);
            }
        }

    }
}
