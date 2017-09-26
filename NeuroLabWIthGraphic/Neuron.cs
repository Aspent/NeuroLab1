using System;

namespace NeuroNetLab1
{
    class Neuron
    {
        private float[] _weights;
        bool _weightChanged = true;

        public Neuron(int inputCount)
        {
            _weights = new float[inputCount+1];
            for (var i = 0; i < _weights.Length; i++)
            {
                _weights[i] = -1;
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
                Console.WriteLine("y = {0}", y);
                if (Math.Sign(y) != elem.Desire)
                {
                    _weightChanged = true;
                    ChangeWeights(elem);
                }
            }
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
    }
}
