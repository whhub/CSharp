using System.Threading;

namespace UnitTestExample
{
    public interface ILongRunningLibrary
    {
        string RunForALongTime(int interval);
    }

    public class LongRunningLibrary : ILongRunningLibrary
    {

        #region Implementation of ILongRunningLibrary

        public string RunForALongTime(int interval)
        {
            var timeToWait = interval*1000;
            Thread.Sleep(timeToWait);
            return string.Format("Waited {0} seconds", interval);
        }

        #endregion
    }
}
