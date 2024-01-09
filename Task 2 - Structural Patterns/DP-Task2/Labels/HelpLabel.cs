using DP_Task2.Interfaces;

namespace DP_Task2.Labels
{
    // this will be decorated by the whole decorators, they will now expect everything to be HelpLabel, but not all helpLabels will implement help text(that way they remain just ILabel)
    public class HelpLabel : IHelpLabel
    {
        public const string MISSING_HELP_MESSAGE = "Sorry, but the given label has got no help clues included!";
        string helpText;
        ILabel underlyingLabel; // here go SimpleLabel, RichLabel, CustomLabel and etc.
        // By doing it this way, there can still exist Labels without them being IHelpLabel

        public HelpLabel(ILabel underlyingLabel)
        {
            this.underlyingLabel = underlyingLabel;
            helpText = MISSING_HELP_MESSAGE;
        }
        // since the text in the original label cannot change, this is the way 2 go. Alternative would be to receive 2 strings, but this will restrict HelpLabel to work
        // with a concrete label instead of the abstraction
        public HelpLabel(ILabel underlyingLabel, string? helpText)
        {
            this.underlyingLabel = underlyingLabel;
            HelpText = helpText;
        }

        public string HelpText
        {
            get => helpText;
            set
            {
                if (value is null)
                    throw new ArgumentNullException($"{nameof(value)} cannot be NULL!");
                if (value == string.Empty)
                    helpText = MISSING_HELP_MESSAGE;
                else
                    helpText = value;
            }
            
        }

        public string Text
        {
            get => underlyingLabel.Text;
        }

        public ILabel Label
        {
            get => underlyingLabel;
        }
    }
}
