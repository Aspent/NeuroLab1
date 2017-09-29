using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.IO;

namespace NeuroLabWIthGraphic
{
    public partial class Form1 : Form
    {      
        private delegate float ActivationFunction(float x, float amplification);

        private Neuron _neuron;
        private string _inputFileName = "input.txt";
        private TrainingSet _trainingSet;
        private ActivationFunction _chosenFunction;
        private Dictionary<int, ActivationFunction> _functions = new Dictionary<int,ActivationFunction>(); 

        public Form1()
        {
            InitializeComponent();

            openFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            saveFileDialog1.Filter = "Text files(*.txt)|*.txt|All files(*.*)|*.*";
            _trainingSet = new TrainingSet(_inputFileName);
            _neuron = new Neuron(2);

            chart1.Series[0].ChartType = SeriesChartType.Point;
            chart2.Series[0].ChartType = SeriesChartType.Line;
            chart2.ChartAreas[0].AxisX.IsStartedFromZero = true; // начинать с 0
            chart2.ChartAreas[0].AxisX.Interval = 1;
            chart2.ChartAreas[0].AxisX.LabelStyle.Format = "f{0,2}";

            chart1.Series.Add("1");
            chart1.Series["1"].Color = Color.Red; ;
            chart1.Series["1"].ChartType = SeriesChartType.Point;

            chart1.Series.Add("2");
            chart1.Series["2"].Color = Color.Green; ;
            chart1.Series["2"].ChartType = SeriesChartType.Line;

            _functions[0] = LinearFunction;
            _functions[1] = BipolarThresholdFunction;
            _functions[2] = SigmoidFunction;

            listBox1.SelectedIndexChanged += SelectedIndexChanged;
            maxIterationsInput.TextChanged += MaxIter_TextChanged;
        }

        private float LinearFunction(float x, float k)
        {
            return k*x;
        }

        private float BipolarThresholdFunction(float x, float k)
        {
            if (x <= 0)
            {
                return -1;
            }
            return 1;
        }

        private float SigmoidFunction(float x, float k)
        {
            return 1.0f/(1 + (float)Math.Exp(-k*x));
        }

        private float calcY(float x, float k, float b)
        {
            return k*x + b;
        }

        private float CalculateNeuronResult(float firstWeight, float secondWeight, float offset,
            float x, float y, float ampl, ActivationFunction actFunction)
        {
            var sum = firstWeight*x + secondWeight*y + offset;
            return actFunction(sum, ampl);
        }

        private bool AllFieldsFilled()
        {
            float value;
            if (!float.TryParse(xInput.Text, out value))
            {
                return false;
            }
            if (!float.TryParse(xInput.Text, out value))
            {
                return false;
            }
            if (!float.TryParse(yInput.Text, out value))
            {
                return false;
            }
            if (!float.TryParse(firstWeightInput.Text, out value))
            {
                return false;
            }
            if (!float.TryParse(secondWeightInput.Text, out value))
            {
                return false;
            }
            if (listBox1.SelectedIndex == -1)
            {
                return false;
            }
            if (listBox1.SelectedIndex != 1)
            {
                if (!float.TryParse(amplificationInput.Text, out value))
                {
                    return false;
                }
            }
            return true;
        }

        public void DrawLearning()
        {
            chart1.Series[0].Points.Clear();
            chart1.Series["1"].Points.Clear();
            chart1.Series["2"].Points.Clear();
            //var trainingSet = new TrainingSet(_inputFileName);
            var points = _trainingSet.ToInputPointsList();


            foreach (var point in points)
            {
                if (point.ClassNumber == 1)
                {
                    chart1.Series[0].Points.AddXY(point.X, point.Y);
                }
                else
                {
                    chart1.Series["1"].Points.AddXY(point.X, point.Y);
                }
            }

            float x1 = -10;
            float x2 = 10;
            var k = -_neuron.Weights[0] / _neuron.Weights[1];
            var b = -_neuron.Weights[2] / _neuron.Weights[1];
            var y1 = calcY(x1, k, b);
            var y2 = calcY(x2, k, b);


            chart1.Series["2"].Points.AddXY(x1, y1);
            chart1.Series["2"].Points.AddXY(x2, y2);

            textBox2.Text = _neuron.Weights[0].ToString();
            textBox3.Text = _neuron.Weights[1].ToString();
            textBox4.Text = _neuron.Weights[2].ToString();
        }


        private void button1_Click(object sender, EventArgs e)
        {
            label12.Text = "";
            var maxIterationsCount = int.Parse(maxIterationsInput.Text);
            
            while (_neuron.WeightChanged || _neuron.CurrentIteration < maxIterationsCount)
            {
                _neuron.Learn(_trainingSet);
            }
            if (_neuron.CurrentIteration >= maxIterationsCount)
            {
                label12.Text = "Достигнуто максимальное число итераций";
            }
            DrawLearning();

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click_1(object sender, EventArgs e)
        {

        }

        private void SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex != -1)
            {
                _chosenFunction = _functions[listBox1.SelectedIndex];
                amplificationInput.Enabled = true;
            }
            if (listBox1.SelectedIndex == 1)
            {
                
                //amplificationInput.Text = "";
                amplificationInput.Enabled = false;
            }
            DrawActivationFunction();
        }

        private void DrawActivationFunction()
        {
            chart2.Series[0].Points.Clear();
            var xCoords = new List<float>();
            var yCoords = new List<float>();
            for (float x = -2; x <= 2; x += 0.01f)
            {
                xCoords.Add(x);
                float ampl = 1;
                float.TryParse(amplificationInput.Text, out ampl);
                var y = _chosenFunction(x, ampl);
                Console.WriteLine(y);
                yCoords.Add(y);
            }
            for (var i = 0; i < xCoords.Count; i++)
            {
                chart2.Series[0].Points.AddXY(xCoords[i], yCoords[i]);
            }
            //chart2.Series[0].Points.A;
            //chart2.Series[0].Points.Add(0.6, 0.8);
            //chart2.ChartAreas[0].AxisX.Minimum = xCoords.Select(x => x).Min();
            //chart2.ChartAreas[0].AxisX.Maximum = xCoords.Select(x => x).Max();
            //chart2.ChartAreas[0].AxisY.Minimum = yCoords.Select(x => x).Min();
            //chart2.ChartAreas[0].AxisY.Maximum = yCoords.Select(x => x).Max();
            //chart2.ChartAreas[0].AxisX.MajorGrid.Enabled = false; //убрать сетку по оси X
            //chart2.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            ////chart2.Series[0].AxisLabel
            //chart2.DataBind();
            //chart2.Invalidate();

        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            errorLabel.Text = "";

            if (!AllFieldsFilled())
            {
                errorLabel.Text = "Неверно заполнены поля ввода";
                return;
            }
            var x = float.Parse(xInput.Text);
            var y = float.Parse(yInput.Text);
            var firstWeight = float.Parse(firstWeightInput.Text);
            var secondWeight = float.Parse(secondWeightInput.Text);
            var offset = float.Parse(offsetInput.Text);
            float amplification = 0;
            if (_chosenFunction != BipolarThresholdFunction)
            {
                amplification = float.Parse(amplificationInput.Text);
            }
            var result = CalculateNeuronResult(firstWeight, secondWeight, offset, x, y,
                amplification, _chosenFunction);
            resultText.Text = result.ToString();



        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            _inputFileName = openFileDialog1.FileName;
            _neuron = new Neuron(2);
            _trainingSet = new TrainingSet(_inputFileName);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            var filename = saveFileDialog1.FileName;
            var lines = new List<string>();
            foreach (var weight in _neuron.Weights)
            {
                lines.Add(weight.ToString());
            }
            File.AppendAllLines(filename, lines);            
        }

        private void button5_Click(object sender, EventArgs e)
        {
            label12.Text = "";
            var maxIterationsCount = int.Parse(maxIterationsInput.Text);
          
            if (_neuron.CurrentIteration >= maxIterationsCount)
            {
                label12.Text = "Достигнуто максимальное число итераций";
                return;
            }
            _neuron.Learn(_trainingSet);
            DrawLearning();
        }

        private void MaxIter_TextChanged(object sender, EventArgs e)
        {
            int val;
            button1.Enabled = int.TryParse(maxIterationsInput.Text, out val);
            button5.Enabled = int.TryParse(maxIterationsInput.Text, out val);
        }
    }
}
