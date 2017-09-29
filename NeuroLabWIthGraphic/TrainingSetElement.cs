using System.Globalization;

namespace NeuroLabWIthGraphic
{
    class TrainingSetElement
    {
        private float[] _inputs;
        private int _desire;

        public TrainingSetElement(string fileString)
        {
            var elems = fileString.Split('\t');
            var desireIndex = elems.Length - 1;
            elems[desireIndex] = elems[desireIndex].Replace("(", "");
            elems[desireIndex] = elems[desireIndex].Replace(")", "");

            _inputs = new float[elems.Length];

            for (var i = 0; i < _inputs.Length-1; i++)
            {
                _inputs[i] = float.Parse(elems[i], CultureInfo.InvariantCulture);
            }
            _inputs[desireIndex] = 1;

            _desire = 1;

            if (int.Parse(elems[desireIndex]) == 1)
            {
                _desire = -1;
            }
        }

        public InputPoint ToPoint()
        {
            var classNumber = 2;
            if (_desire == -1)
            {
                classNumber = 1;
            }
            return new InputPoint(_inputs[0], _inputs[1], classNumber);
        }

        public float[] Inputs
        {
            get { return _inputs; }
        }

        public int Desire
        {
            get { return _desire; }
        }
    }
}
