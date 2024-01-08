using DP_Task2.Transformations;

namespace DP_Task2.Utilizers
{
    // lazy singleton - I got an F on operating systems last semester, so I don't have a clue on the domain of threads and threadsafety
    // this could be a static class as well. No need to be strictly bound to the interface. 
    public sealed class CensorerTransformationSingletonFactory
    {
        private static CensorerTransformationSingletonFactory? instance;
        private static Dictionary<string, CensorerTransformation>? censorships;
        private const int UPPER_BOUND_OF_FLYWEIGHT_WORD = 4;

        private CensorerTransformationSingletonFactory()
        { }

        public static CensorerTransformationSingletonFactory Instance
        {
            get
            {
                if (instance is null)
                {
                    instance = new CensorerTransformationSingletonFactory();
                    censorships = new Dictionary<string, CensorerTransformation>(); // lazy initialization
                }
                return instance;
            }
        }

        // retrieve the current censorships list(this will be used for testing)
        public IReadOnlyCollection<KeyValuePair<string, CensorerTransformation>>? Censorships
        {
            get => censorships;
        }

        public CensorerTransformation CreateCensorer(string? badWord) // valid instance is given
        {
            if (badWord is null)
            {
                throw new ArgumentNullException($"{nameof(badWord)} cannot be NULL!");
            }

            if (badWord.Length > UPPER_BOUND_OF_FLYWEIGHT_WORD) // always create the object, don't store it
            {
                return new CensorerTransformation(badWord);
            }
            else // it is a potential flyweight object
            {
                if (!censorships.ContainsKey(badWord)) // it is yet to have been created
                {
                    CensorerTransformation censorer = new CensorerTransformation(badWord);
                    censorships.Add(badWord, censorer);
                }
                return censorships[badWord];
            }
        }
    }

    // This should do the same trick. The difference is in the lazy initialization and in the fact that my above code is not thread-safe. It depents - 
    // memory management vs data integrity :D

    //public static class CensorerTransformationStaticFactory
    //{
    //    private static Dictionary<string, CensorerTransformation>? censorships;
    //    private const int UPPER_BOUND_OF_FLYWEIGHT_WORD = 4;

    //    public static ITextTransformation CreateCensorer(string badWord) // valid instance is given
    //    {
    //        if (badWord.Length > UPPER_BOUND_OF_FLYWEIGHT_WORD) // always create the object, don't store it
    //        {
    //            return new CensorerTransformation(badWord);
    //        }
    //        else // it is a potential flyweight object
    //        {
    //            if (!censorships.ContainsKey(badWord)) // it is yet to have been created
    //            {
    //                CensorerTransformation censorer = new CensorerTransformation(badWord);
    //                censorships.Add(badWord, censorer);
    //            }
    //            return censorships[badWord];
    //        }
    //    }
    //}
}
