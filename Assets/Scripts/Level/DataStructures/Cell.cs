namespace AnotherTankGame{
    public class Cell{
        private BlockType _block;

        public BlockType Block {
            get { return _block; }
            set { _block = value; }
        }

        private int _xIndex;
        private int _yIndex;

        public int XIndex {
            get { return _xIndex; }
        }

        public int YIndex {
            get { return _yIndex; }
        }

        public Cell(BlockType block, int xIndex, int yIndex) {
            _block = block;
            _xIndex = xIndex;
            _yIndex = yIndex;
        }
    }
}