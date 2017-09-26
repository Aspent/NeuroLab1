using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace NeuroNetLab1
{
    class TrainingSet
    {
        private List<TrainingSetElement> _elements = new List<TrainingSetElement>();

        public TrainingSet(string filePath)
        {
            var strings = File.ReadAllLines(filePath);
            foreach (var str in strings)
            {
                _elements.Add(new TrainingSetElement(str));
            }
        }

        public List<InputPoint> ToInputPointsList()
        {
            return _elements.Select(elem => elem.ToPoint()).ToList();
        }

        public List<TrainingSetElement> Elements
        {
            get { return _elements; }
        }
    }
}
