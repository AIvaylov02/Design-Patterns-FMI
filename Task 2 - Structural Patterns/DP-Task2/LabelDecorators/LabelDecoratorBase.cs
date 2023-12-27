using DP_Task2.Interfaces;

namespace DP_Task2.LabelDecorators
{
    public abstract class LabelDecoratorBase : ILabel
    {
        protected ILabel label;
        
        public LabelDecoratorBase(ILabel label)
        {
            this.label = label;
        }

        // this will be the overriden strategy method
        public abstract string Text { get; }
    }
}
