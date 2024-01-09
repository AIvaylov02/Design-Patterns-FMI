using DP_Task2.Interfaces;
using DP_Task2.LabelUtilizers;
using System.Text;

namespace DP_Task2.Utilizers
{
    public class BaseUserInteractor
    {
        TextReader inputStream;
        TextWriter outputStream;
        List<ILabel> labels;
        List<IHelpLabel> helpLabels;
        int productsToCreate;
        public BaseUserInteractor(TextReader inputStream, TextWriter outputStream)
        {
            this.inputStream = inputStream;
            this.outputStream = outputStream;
            labels = new List<ILabel>();
            helpLabels = new List<IHelpLabel>();
        }

        public void ReadHowManyProductsToCreate()
        {
            outputStream.WriteLine("Enter the number of products you want to create");
            bool isSucessful = int.TryParse(inputStream.ReadLine(), out productsToCreate);
            while (isSucessful == false) // the user can choose to skip product creation, but the collection will be empty then
            {
                isSucessful = int.TryParse(inputStream.ReadLine(), out productsToCreate);
            }
        }

        public void CreateProducts()
        {
            while (productsToCreate > 0)
            {
                outputStream.WriteLine("Enter the label text, that you wish to add.");
                string? labelText = inputStream.ReadLine();
                while (labelText is null)
                {
                    outputStream.WriteLine("Enter a valid, not NULL labelText!");
                    labelText = inputStream.ReadLine();
                }
                ConstructSingleProduct(labelText);
                productsToCreate--;                
            }
        }

        public void PrintResults()
        {
            outputStream.WriteLine();

            if (helpLabels.Count == 0 && labels.Count == 0)
            {
                outputStream.WriteLine("You haven't created any labels for me to display ;( ");
            }
            else // at least one collection has elements
            {

                if (helpLabels.Count > 0)
                {
                    outputStream.WriteLine("Here is a list of the helpLabels you created: ");
                    for (int i = 0; i < helpLabels.Count; i++)
                    {
                        outputStream.Write($"Number {i+1}:'s contents: ");
                        LabelPrinter.PrintLabelWithHelpText(helpLabels[i]);
                        outputStream.WriteLine();
                    }
                }
                if (labels.Count > 0)
                {
                    outputStream.WriteLine("Here is a list of the otherLabels you created: ");
                    for (int i = 0; i < labels.Count; i++)
                    {
                        outputStream.Write($"Number {i+1}:'s contents: ");
                        LabelPrinter.PrintLabel(labels[i]);
                        outputStream.WriteLine();
                    }
                }

            }
        }

        private void ConstructSingleProduct(string labelText)
        {
            LabelBuilder builder = new LabelBuilder(labelText);
            bool isProductConstructed = false;
            while (isProductConstructed == false)
            {
                outputStream.WriteLine();
                outputStream.WriteLine("Enter a command you wish the system to execute. Follow the convention command type <param1> ... <paramN>");
                outputStream.WriteLine("If you are stuck, type 'help' in the console to open the help menu");
                outputStream.WriteLine();

                string? rawCommand = inputStream.ReadLine();
                if (rawCommand is not null && rawCommand == "help")
                {
                    ListAllSupportedCommands();
                    continue;
                }

                string[]? splitCommand = ParseInputCommand(rawCommand);
                if (splitCommand is null)
                    continue;

                try
                {
                    ExecuteCommand(splitCommand, ref builder, ref isProductConstructed);
                }
                catch (Exception e)
                {
                    outputStream.WriteLine($"An exception has occured, while executing command '{rawCommand}', please refer to its message: {e.Message}");
                }
            }
        }

        private string[]? ParseInputCommand(string? rawCommand)
        {
            if (rawCommand is null)
            {
                outputStream.WriteLine("Enter a valid command, not an empty one!");
                return null;
            }

            string[] splitCommand = rawCommand.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            const int MINIMUM_COMMAND_LENGTH = 2;
            if (splitCommand.Length < MINIMUM_COMMAND_LENGTH && rawCommand != "build")
            {
                outputStream.WriteLine("Your command is a parameter too short, try entering a longer command with more options");
                return null;
            }

            return splitCommand;
        }

        private void ListAllSupportedCommands()
        {
            const string EMPTY_LINE = "|                                                                                                         |";
            outputStream.WriteLine("---------------------------------------------   HELP MENU   -----------------------------------------------");
            outputStream.WriteLine("|  build                                                                                                  |");
            outputStream.WriteLine(EMPTY_LINE);
            ListAddCommnads();
            outputStream.WriteLine(EMPTY_LINE);
            ListRemoveCommands();
            outputStream.WriteLine(EMPTY_LINE);
            ListChangeCommands();
            outputStream.WriteLine(EMPTY_LINE);
            ListResetCommands();
            outputStream.WriteLine(EMPTY_LINE);
            outputStream.WriteLine("-------------------------------------------  HELP MENU END   ----------------------------------------------");
        }

        private void ListAddCommnads()
        {
            outputStream.WriteLine("|  add:                                                                                                   |");
            outputStream.WriteLine("|       transformation <transformationType>, <badWord->optional>, <replacement->optional>, where          |");
            outputStream.WriteLine("|  transformationType = {capitalize, compose, decorate, spacer, trimLeft, trimRight, censor, replacement} |");
            outputStream.WriteLine("|       decorator <dec_type>, where <dec_type> is {text, cyclic, random}                                  |");
            ListSimpleTypesAddition();
        }

        private void ListChangeCommands()
        {
            outputStream.WriteLine("|  change:                                                                                                |");
            ListSimpleTypesAddition();
        }

        private void ListSimpleTypesAddition()
        {
            outputStream.WriteLine("|       color <color>                                                                                     |");
            outputStream.WriteLine("|       font <font>                                                                                       |");
            outputStream.WriteLine("|       fontSize <fontSize>, where <fontSize> is a non-negative fraction                                  |");
            outputStream.WriteLine("|       labelType <labelType>, where <labelType> is {simple, rich, custom}                                |");
            outputStream.WriteLine("|       helpText <helpText>                                                                               |");
        }

        private void ListRemoveCommands()
        {
            outputStream.WriteLine("|  remove:                                                                                                |");
            outputStream.WriteLine("|       transformation - removes the lastly added transformation                                          |");
            outputStream.WriteLine("|       transformation <transformationType>, <badWord->optional>, <replacement->optional>, where          |");
            outputStream.WriteLine("|  transformationType = {capitalize, compose, decorate, spacer, trimLeft, trimRight, censor, replacement} |");
            outputStream.WriteLine("|       decorator - removes the lastly added decorator                                                    |");
            outputStream.WriteLine("|       decorators - removes all decorators from the label                                                |");
            outputStream.WriteLine("|       color                                                                                             |");
            outputStream.WriteLine("|       font                                                                                              |");
            outputStream.WriteLine("|       fontSize                                                                                          |");
            outputStream.WriteLine("|       helpText                                                                                          |");
        }

        private void ListResetCommands()
        {
            outputStream.WriteLine("|  reset:                                                                                                 |");
            outputStream.WriteLine("|       transformations - removes all of the added transformations                                        |");
            outputStream.WriteLine("|       styles - removes all of the added transformations together with the decorators                    |");
            outputStream.WriteLine("|       label - resets all of the labels attributes, except for the text                                  |");
        }

        private void ExecuteCommand(string[] splitCommand, ref LabelBuilder builder, ref bool isProductConstructed)
        {
            if (splitCommand[0] == "build")
            {
                isProductConstructed = ExecuteBuildCommand(splitCommand, ref builder);
            }
            else
            {
                switch (splitCommand[0]) // different action from build
                {
                    case "add":
                        ExecuteAddCommand(splitCommand, ref builder);
                        break;
                    case "remove":
                        ExecuteRemoveCommand(splitCommand, ref builder);
                        break;
                    case "change":
                        ExecuteChangeCommand(splitCommand, ref builder);
                        break;
                    case "reset":
                        ExecuteResetCommand(splitCommand, ref builder);
                        break;
                    default:
                        throw new ArgumentException("Command of inputted command is not recognized in the system. Valid type are {add, remove, change, reset, build}");
                }
                outputStream.WriteLine("Success. Please enter your next command!");
            }
        }

        private void ExecuteAddCommand(string[] splitCommand, ref LabelBuilder builder)
        {
            const int MINIMUM_COMMAND_LENGTH_FOR_ADDITION = 3;
            if (splitCommand.Length < MINIMUM_COMMAND_LENGTH_FOR_ADDITION)
            {
                outputStream.WriteLine("The given command has far too few parameters. Expected length of it is at least 3 words.");
                return;
            }

            // position 0 is for command type
            switch (splitCommand[1])
            {
                case "transformation":
                    string? secondParameter = null;
                    string? thirdParameter = null;
                    try
                    {
                        secondParameter = splitCommand[3]; // the second needs to be valid in order for the third to be valid. The idea is to greedily assign them
                        thirdParameter = splitCommand[4];
                    } catch (Exception) { }
                        builder.AddTransformation(splitCommand[2], secondParameter, thirdParameter); // this may throw but will be catched later
                    break;

                case "decorator":
                    builder.ApplyTransformationsAsADecorator(splitCommand[2]);
                    break;
                default: 
                    ManipulateSimpleType(splitCommand, ref builder); 
                    break;
            }
        }

        private void ExecuteRemoveCommand(string[] splitCommand, ref LabelBuilder builder)
        {
            // position 0 is for command type
            switch (splitCommand[1])
            {
                case "transformation":
                    if (splitCommand.Length == 2) // no more additional information is present, remove the lastly added one
                    {
                        builder.RemoveTransformation();
                    }
                    string? secondParameter = null;
                    string? thirdParameter = null;
                    try
                    {
                        secondParameter = splitCommand[3]; // the second needs to be valid in order for the third to be valid. The idea is to greedily assign them
                        thirdParameter = splitCommand[4];
                    }
                    catch (Exception) { }
                    builder.RemoveTransformation(splitCommand[2], secondParameter, thirdParameter); // this may throw but will be catched later
                    break;

                case "decorator":
                    builder.RemoveDecorator();
                    break;
                case "decorators":
                    builder.RemoveDecorators();
                    break;
                case "color":
                    builder.RemoveTextColor();
                    break;
                case "font":
                    builder.RemoveFont();
                    break;
                case "fontSize":
                    builder.RemoveFontSize();
                    break;
                case "helpText":
                    builder.HelpText = null;
                    break;
                default:
                    throw new ArgumentException("The supplied type is not currently valid for removal!");
            }
        }

        private void ManipulateSimpleType(string[] splitCommand, ref LabelBuilder builder)
        {
            // position 0 is for command type
            switch (splitCommand[1])
            {
                case "labelType":
                    builder.LabelType = splitCommand[2];
                    break;

                case "color":
                    // if the color consists of more than one word
                    StringBuilder colorBuilder = new StringBuilder(splitCommand[2]);
                    for (int i = 3; i < splitCommand.Length; i++)
                        colorBuilder.Append($" {splitCommand[i]}");
                    builder.AddTextColor(colorBuilder.ToString());
                    break;

                case "font":
                    // if the font consists of more than one word
                    StringBuilder fontBuilder = new StringBuilder(splitCommand[2]);
                    for (int i = 3; i < splitCommand.Length; i++)
                        fontBuilder.Append($" {splitCommand[i]}");
                    builder.AddFont(fontBuilder.ToString());
                    break;

                case "fontSize":
                    builder.AddFontSize(double.Parse(splitCommand[2]));
                    break;

                case "helpText":
                    // if the help text consists of more than one word
                    StringBuilder helpTextBuilder = new StringBuilder(splitCommand[2]);
                    for (int i = 3; i < splitCommand.Length; i++)
                        helpTextBuilder.Append($" {splitCommand[i]}");
                    builder.HelpText = helpTextBuilder.ToString();
                    break;
                default:
                    throw new ArgumentException("The supplied type is not currently valid for addition/changing to the label!");
            }
        }

        private void ExecuteChangeCommand(string[] splitCommand, ref LabelBuilder builder)
        {
            const int MINIMUM_COMMAND_LENGTH_FOR_ADDITION = 3;
            if (splitCommand.Length < MINIMUM_COMMAND_LENGTH_FOR_ADDITION)
            {
                outputStream.WriteLine("The given command has far too few parameters. Expected length of it is at least 3 words.");
                return;
            }
            ManipulateSimpleType(splitCommand, ref builder);
        }

        private void ExecuteResetCommand(string[] splitCommand, ref LabelBuilder builder)
        {
            // position 0 is for command type
            switch (splitCommand[1])
            {
                case "styles":
                    builder.ResetStyles();
                    break;
                case "transformations":
                    builder.ClearTransformations();
                    break;
                case "label":
                    builder.ResetLabel();
                    break;
                default:
                    throw new ArgumentException("The supplied type is not valid for resetting!");
            }
        }

        private bool ExecuteBuildCommand(string[] splitCommand, ref LabelBuilder builder)
        {
            ILabel? product = builder.BuildLabel();
            if (product is not null)
            {
                if (product is IHelpLabel)
                {
                    helpLabels.Add((IHelpLabel)product);
                }
                else
                {
                    labels.Add(product);
                }
                outputStream.WriteLine("Success. The label you have created has been added to the collection");
                return true;
            }
            else
            {
                outputStream.WriteLine("Sorry, the label you created seems to be invalid. Please refer to the outputted error message for more clear explanation.");
                return false;
            }
        }
    }
}
