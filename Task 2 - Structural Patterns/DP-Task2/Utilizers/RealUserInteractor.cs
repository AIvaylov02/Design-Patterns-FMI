namespace DP_Task2.Utilizers
{
    public class RealUserInteractor : BaseUserInteractor
    {
        public RealUserInteractor() : base(Console.In, Console.Out) { }
    }
}
