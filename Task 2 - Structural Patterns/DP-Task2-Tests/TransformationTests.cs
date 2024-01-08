using DP_Task2.Interfaces;
using DP_Task2.Transformations;

namespace DP_Task2_Tests
{
    [TestFixture]
    public class CapitalizeTransformationTests
    {
        CapitalizeTransformation capitalizator;
        [SetUp]
        public void SetUp()
        {
            // will get invoked before every other testing method
            capitalizator = new CapitalizeTransformation();
        }

        [Test]
        public void Test_Transformation_Capitalize_EmptyString()
        {
            const string EMPTY_STRING = "";
            string result = capitalizator.Transform(EMPTY_STRING);
            Assert.That(result, Is.EqualTo(EMPTY_STRING));
        }

        [Test]
        public void Test_Transformation_Capitalize_NULLText()
        {
            const string? NULL_STRING = null;
            Assert.Throws<ArgumentNullException>(() => capitalizator.Transform(NULL_STRING));
        }

        [Test]
        public void Test_Transformation_Capitalize_FirstLetterIsDigit()
        {
            const string WORD_WITH_STARTING_DIGIT = "6aezakmi";
            string result = capitalizator.Transform(WORD_WITH_STARTING_DIGIT);
            Assert.That(result, Is.EqualTo(WORD_WITH_STARTING_DIGIT));
        }

        [Test]
        public void Test_Transformation_Capitalize_OnlySmallLettersText()
        {
            const string INPUT = "aezakmi"; // gta san andreas cheat code for police elusion for anybody who is wondering
            string result = capitalizator.Transform(INPUT);
            const string EXPECTED_RESULT = "Aezakmi";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }


        [Test]
        public void Test_Transformation_Capitalize_AlreadyCapitalized()
        {
            const string INPUT = "Aezakmi    ";
            string result = capitalizator.Transform(INPUT);
            const string EXPECTED_RESULT = "Aezakmi    ";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }


        [Test]
        public void Test_Transformation_Capitalize_OnlyCapitalLettersText()
        {
            const string INPUT = "AEZAKMI";
            string result = capitalizator.Transform(INPUT);
            const string EXPECTED_RESULT = "AEZAKMI";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_Capitalize_OnlySpaces()
        {
            const string SPACES_STRING = "        ";
            string result = capitalizator.Transform(SPACES_STRING);
            const string EXPECTED_RESULT = "        ";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_Decorate_AreEqual()
        {
            ITextTransformation another = new TrimRightTransformation(); // this symbolizes all other transformations
            Assert.IsFalse(capitalizator.Equals(another));
            another = new CapitalizeTransformation();
            Assert.IsTrue(capitalizator.Equals(another));
        }
    }

    [TestFixture]
    public class CensorerTransformationTests
    {
        const string STANDARD_BAD_WORD = "abc";
        const string BAD_WORD_IS_EMPTY_STRING = "";
        const string STANDARD_REPLACEMENT = "***";

        [Test]
        public void Test_Transformation_Censorer_ReplaceEmptyStringOfText()
        {
            // nothing should happen
            CensorerTransformation censorer = new CensorerTransformation(STANDARD_BAD_WORD);
            string result = censorer.Transform(string.Empty);
            string EXPECTED_RESULT = string.Empty;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_Censorer_ReplaceEmptyStringOfText_BadWordEmpty()
        {
            // Empty string is bad, empty string is given, what is the corresponding behaviour, it just leaves it as empty string or replaces it with one *
            const string REPLACEMENT = "*";
            CensorerTransformation censorer = new CensorerTransformation(BAD_WORD_IS_EMPTY_STRING);
            string result = censorer.Transform(string.Empty);
            Assert.That(result, Is.EqualTo(REPLACEMENT));
        }

        [Test]
        public void Test_Transformation_Censorer_BadWordEmpty_NotEmptyInputString()
        {
            CensorerTransformation censorer = new CensorerTransformation(BAD_WORD_IS_EMPTY_STRING);
            const string INPUT_TEXT = "Wakanda";
            string result = censorer.Transform(INPUT_TEXT);
            Assert.That(result, Is.EqualTo(INPUT_TEXT));
        }

        // word Permutated and case sensitive are also musts!

        [Test]
        public void Test_Transformation_Censorer_WordPermutated()
        {
            CensorerTransformation censorer = new CensorerTransformation(STANDARD_BAD_WORD);
            const string INPUT_TEXT = "abc acb bac bca cab cba";
            string result = censorer.Transform(INPUT_TEXT);
            const string EXPECTED_RESULT = "*** acb bac bca cab cba";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_Censorer_IsCaseSensitive()
        {
            const string DISALLOWED_CAPITAL_A = "A";
            CensorerTransformation censorer = new CensorerTransformation(DISALLOWED_CAPITAL_A);
            const string INPUT_TEXT = "abc acb bac bca cab cba Abc Acb bAc bcA cAb cbA";
            string result = censorer.Transform(INPUT_TEXT);
            const string EXPECTED_RESULT = "abc acb bac bca cab cba *bc *cb b*c bc* c*b cb*";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_Censorer_ReplaceAll()
        {
            const string INPUT_TEXT = "Wakanda";
            CensorerTransformation censorer = new CensorerTransformation(INPUT_TEXT);
            string result = censorer.Transform(INPUT_TEXT);
            const string EXPECTED_RESULT = "*******";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_Censorer_NULLInitialization()
        {
            const string? NULL_STRING = null;
            Assert.Throws<ArgumentNullException>(() => new CensorerTransformation(NULL_STRING));
        }

        [Test]
        public void Test_Transformation_Censorer_NULLText()
        {
            CensorerTransformation censorer = new CensorerTransformation(STANDARD_BAD_WORD);
            const string? NULL_STRING = null;
            Assert.Throws<ArgumentNullException>(() => censorer.Transform(NULL_STRING));
        }

        //[Test] - is invalid because of part 9, which uses that badWord is immutable
        //public void Test_Transformation_Censorer_Change_BadWord()
        //{
        //    const string NEW_BAD_WORD = "September";

        //    CensorerTransformation censorer = new CensorerTransformation(STANDARD_BAD_WORD);
        //    const string TEST_FOR_THIS_OCCASION = "September will be May to some of you, so be ready for September!";
        //    // nothing will happen (as bad word is still "abc", so the string remains unchanged
        //    string result = censorer.Transform(TEST_FOR_THIS_OCCASION);
        //    Assert.That(result, Is.EqualTo(TEST_FOR_THIS_OCCASION));


        //    // censorer.BadWord = NEW_BAD_WORD; is invalid because of flyweight added in part 8
        //    result = censorer.Transform(TEST_FOR_THIS_OCCASION);
        //    const string EXPECTED_RESULT = "********* will be May to some of you, so be ready for *********!";
        //    Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        //}

        [Test]
        public void Test_Transformation_Censorer_SameBadWordAsReplacement_EasyToSpot()
        {
            const string TROLL_BAD_WORD = "*";
            CensorerTransformation censorer = new CensorerTransformation(TROLL_BAD_WORD);
            // no transformation should occur, as we replace the matched text with itself
            const string STANDARD_EXAMPLE = " abc abcdef";
            string result = censorer.Transform(STANDARD_EXAMPLE);
            Assert.That(result, Is.EqualTo(STANDARD_EXAMPLE));
        }

        [Test]
        public void Test_Transformation_Censorer_SameBadWordAsReplacementAndString_TabAndSpace()
        {
            const string TROLL_BAD_WORD = "*****";
            CensorerTransformation censorer = new CensorerTransformation(TROLL_BAD_WORD);
            // No transformation should happen as replacements are same for matchers and the input string
            const string STANDARD_EXAMPLE = TROLL_BAD_WORD;
            string result = censorer.Transform(STANDARD_EXAMPLE);
            Assert.That(result, Is.EqualTo(STANDARD_EXAMPLE));
        }

        [Test]
        public void Test_Transformation_Censorer_AreEqual()
        {
            ITextTransformation another = new TrimRightTransformation(); // this symbolizes all other transformations
            CensorerTransformation censorer = new CensorerTransformation(STANDARD_BAD_WORD);
            Assert.IsFalse(censorer.Equals(another));

            const string MISSMATCH = "INVALID";
            another = new CensorerTransformation(MISSMATCH); // bad word missmatch
            Assert.IsFalse(censorer.Equals(another));
            another = new CensorerTransformation(STANDARD_BAD_WORD); // equality match
            Assert.IsTrue(censorer.Equals(another));
        }
    }

    [TestFixture]
    public class DecorationTransformationTests
    {
        DecorationTransformation decoration;
        const string PREPENDER = "-={ ";
        const string APPENDER = " }=-";

        [SetUp]
        public void SetUp()
        {
            // will get invoked before every other testing method
            decoration = new DecorationTransformation();
        }

        [Test]
        public void Test_Transformation_Decorate_EmptyString()
        {
            string result = decoration.Transform(string.Empty);
            string EXPECTED_RESULT = PREPENDER + string.Empty + APPENDER;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_Decorate_NULLText()
        {
            const string? NULL_STRING = null;
            Assert.Throws<ArgumentNullException>(() => decoration.Transform(NULL_STRING));
        }

        [Test]
        public void Test_Transformation_Decorate_StandartText()
        {
            const string STANDARD_TEXT = "beautiful abc";
            string result = decoration.Transform(STANDARD_TEXT);
            string EXPECTED_RESULT = $"{PREPENDER}{STANDARD_TEXT}{APPENDER}";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }


        [Test]
        public void Test_Transformation_Decorate_OnlySpaces()
        {
            const string SPACE_AND_TABS_INPUT = "                  ";
            string result = decoration.Transform(SPACE_AND_TABS_INPUT);
            const string EXPECTED_RESULT = $"{PREPENDER}{SPACE_AND_TABS_INPUT}{APPENDER}";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_Decorate_AreEqual()
        {
            ITextTransformation another = new TrimRightTransformation(); // this symbolizes all other transformations
            Assert.IsFalse(decoration.Equals(another));
            another = new DecorationTransformation();
            Assert.IsTrue(decoration.Equals(another));
        }

    }

    [TestFixture]
    public class ReplacerTransformationTests
    {
        const string STANDARD_BAD_WORD = "abc";
        const string BAD_WORD_IS_EMPTY_STRING = "";
        const string STANDARD_REPLACEMENT = "d";

        [Test]
        public void Test_Transformation_Replacer_ReplaceEmptyStringOfText()
        {
            // nothing should happen
            ReplacerTransformation replacer = new ReplacerTransformation(STANDARD_BAD_WORD, STANDARD_REPLACEMENT);
            string result = replacer.Transform(string.Empty);
            string EXPECTED_RESULT = string.Empty;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_Replacer_ReplaceEmptyStringOfText_BadWordEmpty()
        {
            // will it catch something?
            const string REPLACEMENT = "?FILL THE STRING?";
            ReplacerTransformation replacer = new ReplacerTransformation(BAD_WORD_IS_EMPTY_STRING, REPLACEMENT);
            string result = replacer.Transform(string.Empty);
            Assert.That(result, Is.EqualTo(REPLACEMENT));
        }

        [Test]
        public void Test_Transformation_Replacer_BadWordEmpty_NotEmptyInputString()
        {
            const string REPLACEMENT = "?FILL THE STRING?";
            ReplacerTransformation replacer = new ReplacerTransformation(BAD_WORD_IS_EMPTY_STRING, REPLACEMENT);
            const string INPUT_TEXT = "Wakanda";
            string result = replacer.Transform(INPUT_TEXT);
            Assert.That(result, Is.EqualTo(INPUT_TEXT));
        }

        [Test]
        public void Test_Transformation_Replacer_ReplacerEmpty_NotEmptyInputString_NoMatch()
        {
            const string REPLACEMENT = BAD_WORD_IS_EMPTY_STRING;
            ReplacerTransformation replacer = new ReplacerTransformation(STANDARD_BAD_WORD, REPLACEMENT);
            const string INPUT_TEXT = "Wakanda";
            string result = replacer.Transform(INPUT_TEXT);
            Assert.That(result, Is.EqualTo(INPUT_TEXT));
        }

        [Test]
        public void Test_Transformation_Replacer_ReplacerEmpty_NotEmptyInputString_Match_DeleteIt()
        {
            const string REPLACEMENT = BAD_WORD_IS_EMPTY_STRING;
            const string INPUT_TEXT = "Wakanda";
            ReplacerTransformation replacer = new ReplacerTransformation(INPUT_TEXT, REPLACEMENT);
            string result = replacer.Transform(INPUT_TEXT);
            Assert.That(result, Is.EqualTo(string.Empty));
        }

        [Test]
        public void Test_Transformation_Replacer_WordPermutated()
        {
            ReplacerTransformation replacer = new ReplacerTransformation(STANDARD_BAD_WORD, "NOT_ABC");
            const string INPUT_TEXT = "abc acb bac bca cab cba";
            string result = replacer.Transform(INPUT_TEXT);
            const string EXPECTED_RESULT = "NOT_ABC acb bac bca cab cba";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_Replacer_IsCaseSensitive()
        {
            const string DISALLOWED_CAPITAL_A = "A";
            ReplacerTransformation censorer = new ReplacerTransformation(DISALLOWED_CAPITAL_A, "_no_cap_a_");
            const string INPUT_TEXT = "abc acb bac bca cab cba Abc Acb bAc bcA cAb cbA";
            string result = censorer.Transform(INPUT_TEXT);
            const string EXPECTED_RESULT = "abc acb bac bca cab cba _no_cap_a_bc _no_cap_a_cb " +
                "b_no_cap_a_c bc_no_cap_a_ c_no_cap_a_b cb_no_cap_a_";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_Replacer_NULLInitialization()
        {
            const string? NULL_STRING = null;
            Assert.Throws<ArgumentNullException>(() => new ReplacerTransformation(NULL_STRING, STANDARD_REPLACEMENT));
            Assert.Throws<ArgumentNullException>(() => new ReplacerTransformation(STANDARD_BAD_WORD, NULL_STRING));
        }

        [Test]
        public void Test_Transformation_Replacer_NULLText()
        {
            ReplacerTransformation replacer = new ReplacerTransformation(STANDARD_BAD_WORD, STANDARD_REPLACEMENT);
            const string? NULL_STRING = null;
            Assert.Throws<ArgumentNullException>(() => replacer.Transform(NULL_STRING));
        }

        [Test]
        public void Test_Transformation_Replacer_Change_BadWord()
        {
            const string NEW_BAD_WORD = "September";

            ReplacerTransformation replacer = new ReplacerTransformation(STANDARD_BAD_WORD, STANDARD_REPLACEMENT);
            const string TEST_FOR_THIS_OCCASION = "September will be May to some of you, so be ready for September!";
            // nothing will happen (as bad word is still "abc", so the string remains unchanged
            string result = replacer.Transform(TEST_FOR_THIS_OCCASION);
            Assert.That(result, Is.EqualTo(TEST_FOR_THIS_OCCASION));

            replacer.BadWord = NEW_BAD_WORD;
            result = replacer.Transform(TEST_FOR_THIS_OCCASION);
            const string EXPECTED_RESULT = "d will be May to some of you, so be ready for d!";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_Replacer_Change_ReplacementText()
        {
            const string TEXT_TO_TRANSFORM = "DP exam will consist only of course project defenses";
            const string NEW_BAD_WORD = "only of course project defenses";
            const string NEW_REPLACEMENT = "not only of course project defenses, but some kind of a test/mouth to mouth speaking aswell";

            ReplacerTransformation replacer = new ReplacerTransformation(STANDARD_BAD_WORD, STANDARD_REPLACEMENT);
            // nothing will happen (as bad word is still "abc", so the string remains unchanged
            string result = replacer.Transform(TEXT_TO_TRANSFORM);
            Assert.That(result, Is.EqualTo(TEXT_TO_TRANSFORM));

            replacer.BadWord = NEW_BAD_WORD;
            replacer.Replacement = NEW_REPLACEMENT;
            result = replacer.Transform(TEXT_TO_TRANSFORM);
            const string EXPECTED_RESULT = "DP exam will consist " + NEW_REPLACEMENT;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_Replacer_SameBadWordAsReplacement_EasyToSpot()
        {
            const string TROLL_REPLACEMENT = STANDARD_BAD_WORD;
            ReplacerTransformation replacer = new ReplacerTransformation(STANDARD_BAD_WORD, TROLL_REPLACEMENT);
            // no transformation should occur, as we replace the text with itself
            const string STANDARD_EXAMPLE = " abc abcdef";
            string result = replacer.Transform(STANDARD_EXAMPLE);
            Assert.That(result, Is.EqualTo(STANDARD_EXAMPLE));
        }

        [Test]
        public void Test_Transformation_Replacer_SameBadWordAsReplacement_TabAndSpace()
        {
            const string TROLL_REPLACEMENT = "  ";
            ReplacerTransformation replacer = new ReplacerTransformation(BAD_WORD_IS_EMPTY_STRING, TROLL_REPLACEMENT);
            // Check if TAB is foolproof of SPACE. Otherwise it could lead to an endless recursion
            const string STANDARD_EXAMPLE = " abc abc   def ";
            string result = replacer.Transform(STANDARD_EXAMPLE);
            Assert.That(result, Is.EqualTo(STANDARD_EXAMPLE));
        }

        [Test]
        public void Test_Transformation_Replacer_AreEqual()
        {
            ITextTransformation another = new TrimRightTransformation(); // this symbolizes all other transformations
            ReplacerTransformation replacer = new ReplacerTransformation(STANDARD_BAD_WORD, STANDARD_REPLACEMENT);
            Assert.IsFalse(replacer.Equals(another));

            const string MISSMATCH = "INVALID";
            another = new ReplacerTransformation(STANDARD_BAD_WORD, MISSMATCH); // second word missmatch
            Assert.IsFalse(replacer.Equals(another));
            another = new ReplacerTransformation(MISSMATCH, STANDARD_REPLACEMENT); // first word missmatch
            Assert.IsFalse(replacer.Equals(another));
            another = new ReplacerTransformation(STANDARD_REPLACEMENT, STANDARD_BAD_WORD); // swapped words
            Assert.IsFalse(replacer.Equals(another));

            another = new ReplacerTransformation(STANDARD_BAD_WORD, STANDARD_REPLACEMENT);
            Assert.IsTrue(replacer.Equals(another));
        }
    }

    [TestFixture]
    public class SpaceNormalizationTests
    {
        SpaceNormalizationTransformation spaceNormalizer;

        [SetUp]
        public void SetUp()
        {
            // will get invoked before every other testing method
            spaceNormalizer = new SpaceNormalizationTransformation();
        }

        [Test]
        public void Test_Transformation_SpaceNormalize_EmptyString()
        {
            string result = spaceNormalizer.Transform(string.Empty);
            string EXPECTED_RESULT = string.Empty;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_SpaceNormalize_NULLText()
        {
            const string? NULL_STRING = null;
            Assert.Throws<ArgumentNullException>(() => spaceNormalizer.Transform(NULL_STRING));
        }

        [Test]
        public void Test_Transformation_SpaceNormalize_StandartText()
        {
            const string TEXT_FROM_TASK_EXAMPLE = "some   text";
            string result = spaceNormalizer.Transform(TEXT_FROM_TASK_EXAMPLE);
            string EXPECTED_RESULT = "some text";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_SpaceNormalize_NoSpaces()
        {
            // no transformation should occur
            const string UNSPACED_TEXT = "someText";
            string result = spaceNormalizer.Transform(UNSPACED_TEXT);
            string EXPECTED_RESULT = UNSPACED_TEXT;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_SpaceNormalize_OnlySingleSpaces()
        {
            const string WELL_FORMATED_TEXT = " some text, which was already well formated beforehand! ";
            string result = spaceNormalizer.Transform(WELL_FORMATED_TEXT);
            string EXPECTED_RESULT = WELL_FORMATED_TEXT;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_SpaceNormalize_Leading_WhiteSpacesAndTabs()
        {
            // all execess spaces should shrink to one
            const string TEXT_FROM_TASK_EXAMPLE_EXTENDED = "        some text";
            string result = spaceNormalizer.Transform(TEXT_FROM_TASK_EXAMPLE_EXTENDED);
            string EXPECTED_RESULT = " some text";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_SpaceNormalize_Trailing_WhiteSpacesAndTabs()
        {
            // all execess spaces should shrink to one
            const string TEXT_FROM_TASK_EXAMPLE_EXTENDED = "some text       ";
            string result = spaceNormalizer.Transform(TEXT_FROM_TASK_EXAMPLE_EXTENDED);
            string EXPECTED_RESULT = "some text ";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }


        [Test]
        public void Test_Transformation_SpaceNormalize_LeadingAndTrailing_WhiteSpacesAndTabs()
        {
            // all execess spaces should shrink to one
            const string TEXT_FROM_TASK_EXAMPLE_EXTENDED = "        some   text       ";
            string result = spaceNormalizer.Transform(TEXT_FROM_TASK_EXAMPLE_EXTENDED);
            string EXPECTED_RESULT = " some text ";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }



        [Test]
        public void Test_Transformation_SpaceNormalize_OnlySpaces()
        {
            // only one space should be left at the end
            const string SPACE_AND_TABS_INPUT = "                  ";
            string result = spaceNormalizer.Transform(SPACE_AND_TABS_INPUT);
            const string EXPECTED_RESULT = " ";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_SpaceNormalize_AreEqual()
        {
            ITextTransformation another = new TrimRightTransformation(); // this symbolizes all other transformations
            Assert.IsFalse(spaceNormalizer.Equals(another));
            another = new SpaceNormalizationTransformation();
            Assert.IsTrue(spaceNormalizer.Equals(another));
        }

    }


    [TestFixture]
    public class TrimLeftTransformationTests
    {
        TrimLeftTransformation leftTrimmer;

        [SetUp]
        public void SetUp()
        {
            // will get invoked before every other testing method
            leftTrimmer = new TrimLeftTransformation();
        }

        [Test]
        public void Test_Transformation_TrimLeft_EmptyString()
        {
            string result = leftTrimmer.Transform(string.Empty);
            string EXPECTED_RESULT = string.Empty;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_TrimLeft_NULLText()
        {
            const string? NULL_STRING = null;
            Assert.Throws<ArgumentNullException>(() => leftTrimmer.Transform(NULL_STRING));
        }

        [Test]
        public void Test_Transformation_TrimLeft_NothingToTrim()
        {
            // no transformation should occur
            const string TEXT_FROM_TASK_EXAMPLE = "some   text";
            string result = leftTrimmer.Transform(TEXT_FROM_TASK_EXAMPLE);
            const string EXPECTED_RESULT = TEXT_FROM_TASK_EXAMPLE;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_TrimLeft_NoSpaces()
        {
            // no transformation should occur
            const string UNSPACED_TEXT = "someText";
            string result = leftTrimmer.Transform(UNSPACED_TEXT);
            const string EXPECTED_RESULT = UNSPACED_TEXT;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_TrimLeft_OnlyOneLeading_WhiteSpace()
        {
            // all execess left spaces should shrink to one
            const string TEXT_FROM_TASK_EXAMPLE_EXTENDED = " some text ";
            string result = leftTrimmer.Transform(TEXT_FROM_TASK_EXAMPLE_EXTENDED);
            const string EXPECTED_RESULT = "some text ";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_TrimLeft_Leading_WhiteSpacesAndTabs()
        {
            // all execess left spaces should vanish
            const string TEXT_FROM_TASK_EXAMPLE_EXTENDED = "        some text";
            string result = leftTrimmer.Transform(TEXT_FROM_TASK_EXAMPLE_EXTENDED);
            const string EXPECTED_RESULT = "some text";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_TrimLeft_Trailing_WhiteSpacesAndTabs()
        {
            // nothing should happen
            const string TEXT_FROM_TASK_EXAMPLE_EXTENDED = "some text       ";
            string result = leftTrimmer.Transform(TEXT_FROM_TASK_EXAMPLE_EXTENDED);
            const string EXPECTED_RESULT = "some text       ";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }


        [Test]
        public void Test_Transformation_TrimLeft_LeadingAndTrailing_WhiteSpacesAndTabs()
        {
            // all execess spaces to the left should vanish
            const string TEXT_FROM_TASK_EXAMPLE_EXTENDED = "        some   text       ";
            string result = leftTrimmer.Transform(TEXT_FROM_TASK_EXAMPLE_EXTENDED);
            const string EXPECTED_RESULT = "some   text       ";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }



        [Test]
        public void Test_Transformation_TrimLeft_OnlySpaces()
        {
            // the string should be empty as all spaces are considered left trimmable
            const string SPACE_AND_TABS_INPUT = "                  ";
            string result = leftTrimmer.Transform(SPACE_AND_TABS_INPUT);
            string EXPECTED_RESULT = string.Empty;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_TrimLeft_AreEqual()
        {
            ITextTransformation another = new SpaceNormalizationTransformation(); // this symbolizes all other transformations
            Assert.IsFalse(leftTrimmer.Equals(another));
            another = new TrimLeftTransformation();
            Assert.IsTrue(leftTrimmer.Equals(another));
        }

    }

    [TestFixture]
    public class TrimRightTransformationTests
    {
        TrimRightTransformation rightTrimmer;

        [SetUp]
        public void SetUp()
        {
            // will get invoked before every other testing method
            rightTrimmer = new TrimRightTransformation();
        }

        [Test]
        public void Test_Transformation_TrimRight_EmptyString()
        {
            string result = rightTrimmer.Transform(string.Empty);
            string EXPECTED_RESULT = string.Empty;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_TrimRight_NULLText()
        {
            const string? NULL_STRING = null;
            Assert.Throws<ArgumentNullException>(() => rightTrimmer.Transform(NULL_STRING));
        }

        [Test]
        public void Test_Transformation_TrimRight_NothingToTrim()
        {
            // no transformation should occur
            const string TEXT_FROM_TASK_EXAMPLE = "some   text";
            string result = rightTrimmer.Transform(TEXT_FROM_TASK_EXAMPLE);
            const string EXPECTED_RESULT = TEXT_FROM_TASK_EXAMPLE;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_TrimRight_NoSpaces()
        {
            // no transformation should occur
            const string UNSPACED_TEXT = "someText";
            string result = rightTrimmer.Transform(UNSPACED_TEXT);
            const string EXPECTED_RESULT = UNSPACED_TEXT;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_TrimRight_OnlyOneTrailing_WhiteSpace()
        {
            // all execess left spaces should shrink to one
            const string TEXT_FROM_TASK_EXAMPLE_EXTENDED = " some text ";
            string result = rightTrimmer.Transform(TEXT_FROM_TASK_EXAMPLE_EXTENDED);
            const string EXPECTED_RESULT = " some text";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_TrimRight_Leading_WhiteSpacesAndTabs()
        {
            // all execess left spaces should vanish
            const string TEXT_FROM_TASK_EXAMPLE_EXTENDED = "        some text";
            string result = rightTrimmer.Transform(TEXT_FROM_TASK_EXAMPLE_EXTENDED);
            const string EXPECTED_RESULT = "        some text";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_TrimRight_Trailing_WhiteSpacesAndTabs()
        {
            // nothing should happen
            const string TEXT_FROM_TASK_EXAMPLE_EXTENDED = "some text       ";
            string result = rightTrimmer.Transform(TEXT_FROM_TASK_EXAMPLE_EXTENDED);
            const string EXPECTED_RESULT = "some text";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }


        [Test]
        public void Test_Transformation_TrimRight_LeadingAndTrailing_WhiteSpacesAndTabs()
        {
            // all execess spaces to the left should vanish
            const string TEXT_FROM_TASK_EXAMPLE_EXTENDED = "        some   text       ";
            string result = rightTrimmer.Transform(TEXT_FROM_TASK_EXAMPLE_EXTENDED);
            string EXPECTED_RESULT = "        some   text";
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }



        [Test]
        public void Test_Transformation_TrimRight_OnlySpaces()
        {
            // the string should be empty as all spaces are considered left trimmable
            const string SPACE_AND_TABS_INPUT = "                  ";
            string result = rightTrimmer.Transform(SPACE_AND_TABS_INPUT);
            string EXPECTED_RESULT = string.Empty;
            Assert.That(result, Is.EqualTo(EXPECTED_RESULT));
        }

        [Test]
        public void Test_Transformation_TrimRight_AreEqual()
        {
            ITextTransformation another = new SpaceNormalizationTransformation(); // this symbolizes all other transformations
            Assert.IsFalse(rightTrimmer.Equals(another));
            another = new TrimRightTransformation();
            Assert.IsTrue(rightTrimmer.Equals(another));
        }

    }

    [TestFixture]
    public class CompositeTransformationTests
    {
        CompositeTransformation compositor;

        [Test]
        public void Test_Transformation_Compose_EmptyString_DefaultInitialization()
        {
            const string EMPTY_STRING = "";
            compositor = new CompositeTransformation();
            string result = compositor.Transform(EMPTY_STRING);
            Assert.That(result, Is.EqualTo(EMPTY_STRING));
        }

        [Test]
        public void Test_Transformation_Compose_EmptyString_OneStyle()
        {
            const string EMPTY_STRING = "";
            compositor = new CompositeTransformation(new CapitalizeTransformation());
            string result = compositor.Transform(EMPTY_STRING);
            Assert.That(result, Is.EqualTo(EMPTY_STRING));
        }

        [Test]
        public void Test_Transformation_Compose_EmptyString_NStyles()
        {
            const string EMPTY_STRING = "";
            List<ITextTransformation> styles = new List<ITextTransformation>();
            styles.Add(new CapitalizeTransformation());
            styles.Add(new TrimRightTransformation());
            compositor = new CompositeTransformation(styles);
            string result = compositor.Transform(EMPTY_STRING);
            Assert.That(result, Is.EqualTo(EMPTY_STRING));
        }

        [Test]
        public void Test_Transformation_Compose_NULLText()
        {
            const string? NULL_STRING = null;
            compositor = new CompositeTransformation();
            Assert.Throws<ArgumentNullException>(() => compositor.Transform(NULL_STRING));
        }

        [Test]
        public void Test_Transformation_Compose_DefaultInitialization()
        {
            const string INPUT = "abc def";
            compositor = new CompositeTransformation();
            string result = compositor.Transform(INPUT);
            Assert.That(result, Is.EqualTo(INPUT));
        }

        [Test]
        public void Test_Transformation_Compose_OneStyle()
        {
            const string INPUT = "abc def";
            compositor = new CompositeTransformation(new CapitalizeTransformation());
            string result = compositor.Transform(INPUT);
            Assert.That(result, Is.EqualTo("Abc def"));
        }

        [Test]
        public void Test_Transformation_Compose_NStyles()
        {
            // ORDER matters!

            const string INPUT = "abc def";
            List<ITextTransformation> styles = new List<ITextTransformation>();
            styles.Add(new CapitalizeTransformation());
            styles.Add(new DecorationTransformation());
            styles.Add(new ReplacerTransformation("abc", "def"));
            compositor = new CompositeTransformation(styles);
            string result = compositor.Transform(INPUT);
            Assert.That(result, Is.EqualTo("-={ Abc def }=-"));

            styles.Clear();
            styles.Add(new DecorationTransformation());
            styles.Add(new CapitalizeTransformation()); // this won't work after decoration, so it is the same as dec, replace, cap
            styles.Add(new ReplacerTransformation("abc", "def"));
            compositor = new CompositeTransformation(styles);
            result = compositor.Transform(INPUT);
            Assert.That(result, Is.EqualTo("-={ def def }=-"));

            styles.Clear();
            styles.Add(new ReplacerTransformation("abc", "def"));
            styles.Add(new CapitalizeTransformation());
            styles.Add(new DecorationTransformation());
            compositor = new CompositeTransformation(styles);
            result = compositor.Transform(INPUT);
            Assert.That(result, Is.EqualTo("-={ Def def }=-"));
        }

        [Test]
        public void Test_Transformation_Compose_AreEqual()
        {
            ITextTransformation spacer = new SpaceNormalizationTransformation();
            List<ITextTransformation> styles = new List<ITextTransformation>();
            styles.Add(new CapitalizeTransformation());
            styles.Add(new DecorationTransformation());
            styles.Add(new ReplacerTransformation("abc", "def"));
            compositor = new CompositeTransformation(styles);
            Assert.IsFalse(compositor.Equals(spacer));

            styles.RemoveAt(2);
            ITextTransformation anotherCompositor = new CompositeTransformation(styles); // it has 2 styles but missmatches the last one
            Assert.IsFalse(compositor.Equals(anotherCompositor));

            styles.Add(new ReplacerTransformation("abc", "def"));
            anotherCompositor = new CompositeTransformation(styles); // all the styles are matched accordingly
            Assert.IsTrue(compositor.Equals(anotherCompositor));

            styles.Add(new CapitalizeTransformation());
            anotherCompositor = new CompositeTransformation(styles); // another compositor has more styles
            Assert.IsFalse(compositor.Equals(anotherCompositor));

            styles.RemoveAt(1);
            anotherCompositor = new CompositeTransformation(styles); // the 2 compositors differ at position one
            Assert.IsFalse(compositor.Equals(anotherCompositor));

        }

    }

}
