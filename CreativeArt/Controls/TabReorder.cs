namespace CreativeArt.Controls
{
    public class TabReorder
    {
        private int _fromIndex;

        private int _toIndex;

        public int FromIndex
        {
            get { return _fromIndex; }
            set { _fromIndex = value; }
        }

        public int ToIndex
        {
            get { return _toIndex; }
            set { _toIndex = value; }
        }

        public TabReorder(int fromIndex, int toIndex)
        {
            _fromIndex = fromIndex;
            _toIndex = toIndex;
        }
    }
}
