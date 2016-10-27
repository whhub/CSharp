namespace UnitTestExample
{
    public class StringUtilities
    {
        public int CountOccurences(string stringToCheck, string stringToFind)
        {
            if (stringToCheck == null) return -1;
            var stringAsCharArray = stringToCheck.ToUpper().ToCharArray();
            var stringToCheckAsChar = stringToFind.ToUpper().ToCharArray()[0];
            var occurenceCount = 0;
            for (int characterIndex = 0; characterIndex <= stringAsCharArray.GetUpperBound(0); characterIndex++)
            {
                if (stringAsCharArray[characterIndex] == stringToCheckAsChar)
                {
                    occurenceCount++;
                }
            }
            return occurenceCount;
        }
    }
}
