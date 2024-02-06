namespace DP_Task3.Visitors
{
    public static class VisitorResultFormatter // is used to enumerate object of the filesystem to the end output (the concrete visiting method shouldn't enumerate objects)
    {
        public static string FormatResults(string input)
        {
            string[] splitResult = input.Split(Environment.NewLine, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < splitResult.Length; i++)
            {
                string emptySpaces = "";
                if (i < 10)
                {
                    emptySpaces = "  ";
                }
                else if (i < 100)
                {
                    emptySpaces = " ";
                }
                splitResult[i] = splitResult[i].Insert(0, $"{emptySpaces}File{i}: ");
            }
            return String.Join(Environment.NewLine, splitResult);
        }
    }
}
