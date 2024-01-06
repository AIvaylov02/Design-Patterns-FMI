using DP_Task2.Interfaces;

namespace DP_Task2.Labels
{
    public abstract class BaseCustomLabel : ILabel
    {
        private class InteractiveLabel : ILabel
        {
            TextReader inputStream;
            TextWriter outputStream;
            public InteractiveLabel(ref TextReader inputStream, ref TextWriter outputStream)
            {
                this.inputStream = inputStream;
                this.outputStream = outputStream;
            }
            public string Text
            {
                get
                {
                    outputStream.WriteLine("Please enter the labels name, which you want to use for this label");
                    outputStream.Write("Your label name: ");


                    string? result = inputStream.ReadLine();
                    return result is null ? "" : result;
                }
            }
        }


        string? value;
        int timeout;
        int timeoutInterval; // the const interval timer
        const int UNCHANGING_LABEL_TIMER = -1;
        TextReader inputStream;
        TextWriter outputStream;
        InteractiveLabel interactiveLabel;

        public BaseCustomLabel(TextReader inputStream, TextWriter outputStream)
        {
            interactiveLabel = new InteractiveLabel(ref inputStream, ref outputStream);

            this.inputStream = inputStream;
            this.outputStream = outputStream;
            value = null;
            // when text is requested , then we ask value = inputFile.ReadLine();
            ReadTimeoutNumber(inputStream, outputStream);
        }

        public string Text
        {
            get
            {
                if (value == null) // set the text for the first time
                {
                    value = interactiveLabel.Text;

                }
                else // we are already in motion in the applications
                {
                    if (timeoutInterval != UNCHANGING_LABEL_TIMER) // if it is a changing timer, and not a static label
                    {
                        timeout--;
                        if (timeout == 0) // the timeout has reached zero
                        {
                            outputStream.WriteLine("Do you want to change the label for this item? If so type one of the following chars { 'y', 'Y', 'yes', 'Yes', 'YES' }");
                            outputStream.Write("Your choice: ");
                            string willValueBeKeptForAnotherRoll = inputStream.ReadLine();

                            if (DoesUserWantToRenameLabel(willValueBeKeptForAnotherRoll)) // the user wants a new value, ask the real label interacter
                            {
                                value = interactiveLabel.Text;
                            }

                            timeout = timeoutInterval; // reset the counter
                        }

                    }
                }
                
                return value;
            }
        }

        private void ReadTimeoutNumber(TextReader inputStream, TextWriter outputStream)
        {
            outputStream.WriteLine("Please add an timeout option after which many calls you will be bothered again!");
            outputStream.WriteLine("Enter '0' if you don't wish a timeout, otherwise enter an positive integer");
            outputStream.Write("Your entered number: ");
            string rawNumber = inputStream.ReadLine();
            bool isNumber = int.TryParse(rawNumber, out int readNumber);
            if (!isNumber || readNumber == 0)
            {
                timeoutInterval = UNCHANGING_LABEL_TIMER;
            }
            else
            {
                timeoutInterval = readNumber;
            }
            timeout = timeoutInterval;

        }

        private static bool DoesUserWantToRenameLabel(string wish)
        {
            HashSet<string> confirmations = new HashSet<string>
            {
                "y",
                "Y",
                "yes",
                "Yes",
                "YES"
            };
            return confirmations.Contains(wish);
        }
    }
}
