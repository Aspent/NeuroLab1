using System;

namespace NeuroLabWIthGraphic
{
    class Neuron
    {
        private float[] _weights;
        bool _weightChanged = true;
        private int _currentIteration = 0;

        public Neuron(int inputCount)
        {     
            var random = new Random();
            _weights = new float[inputCount+1];
            for (var i = 0; i < _weights.Length; i++)
            {
                _weights[i] = Convert.ToSingle(random.NextDouble());
            }
        }

        private float MultiplyInputsOnWeights(float[] inputs)
        {
            float result = 0;
            for (var i = 0; i < inputs.Length; i++)
            {
                result += inputs[i]*_weights[i];
            }
            return result;

        }

        private void ChangeWeights(TrainingSetElement elem)
        {
            for (var i = 0; i < _weights.Length; i++)
            {
                _weights[i] += elem.Desire*elem.Inputs[i];
            }
        }

        public void Learn(TrainingSet trainingSet)
        {
            _weightChanged = false;

            foreach (var elem in trainingSet.Elements)
            {
                var y = MultiplyInputsOnWeights(elem.Inputs);
                //Console.WriteLine("y = {0}", y);
                if (Math.Sign(y) != elem.Desire)
                {
                    _weightChanged = true;
                    ChangeWeights(elem);
                }
            }
            _currentIteration++;
        }

        public float[] Weights
        {
            get { return _weights; }
        }


        public bool WeightChanged
        {
            get
            {
                return _weightChanged;
            }

        }

        public int CurrentIteration
        {
            get { return _currentIteration; }
        }
    }
}
