using DP_Task2.Interfaces;

namespace DP_Task2.Labels
{
    public class SimpleLabel : ILabel
    {
        string value;
        public SimpleLabel(string value)
        {
            // this is unchangeable code. By the task definition there are no NULL CHECKS, so we don't check for it but it also won't be NULL.
            // It is guaranteed valid
            this.value = value;
        }

        public string Text => value;

    }
}
