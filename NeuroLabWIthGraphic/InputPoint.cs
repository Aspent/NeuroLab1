namespace NeuroLabWIthGraphic
{
    class InputPoint
    {
        float _x;
        float _y;
        int _classNumber;

        public InputPoint(float x, float y, int classNum)
        {
            _x = x;
            _y = y;
            _classNumber = classNum;
        }

        public float X
        {
            get
            {
                return _x;
            }
        
        }

        public float Y
        {
            get
            {
                return _y;
            }

        }

        public int ClassNumber
        {
            get
            {
                return _classNumber;
            }

        }

    }
}
