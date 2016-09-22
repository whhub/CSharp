namespace DataStructure.DataStructures
{
    class SeqList<T>
    {

        #region Constructors

        public SeqList(int n)
        {
            _volume = n;
            _elements = new T[_volume];
            Length = 0;
        }

        public SeqList()
        {
            _volume = DefaultVolume;
            _elements = new T[_volume];
            Length = 0;
        }

        #endregion


        #region Properties

        public int Length { get; private set; }

        public bool IsEmpty { get { return Length == 0; } }

        #endregion


        public void Clear()
        {
            
        }

        #region Fields

        private T[] _elements;
        private readonly int _volume;

        private const int DefaultVolume = 100;

        #endregion

    }

}
