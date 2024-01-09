using DP_Task2.Interfaces;
using DP_Task2.LabelDecorators;
using DP_Task2.Labels;
using DP_Task2.LabelUtilizers;
using DP_Task2.Transformations;
using DP_Task2.Utilizers;

namespace DP_Task2
{
    public class Program
    {
        static void Main(string[] args)
        {
            RealUserInteractor userInteractor = new RealUserInteractor();
            userInteractor.ReadHowManyProductsToCreate();
            userInteractor.CreateProducts();
            userInteractor.PrintResults();
        }

    }
}
