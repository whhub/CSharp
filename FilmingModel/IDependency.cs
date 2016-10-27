namespace UnitTestExample
{
    public interface IDependency
    {
        int GetValue(string s);
        void CallMeFirst();
        int CallMeTwice(string s);
        void CallMeLast();
    }
}
