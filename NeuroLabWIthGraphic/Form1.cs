using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using NeuroNetLab1;

namespace NeuroLabWIthGraphic
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private float calcY(float x, float k, float b)
        {
            return k*x + b;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var trainingSet = new TrainingSet("input.txt");
            var points = trainingSet.ToInputPointsList();
            chart1.Series[0].ChartType = SeriesChartType.Point;

            chart1.Series.Add("1");
            chart1.Series["1"].Color = Color.Red;;
            chart1.Series["1"].ChartType = SeriesChartType.Point;

            chart1.Series.Add("2");
            chart1.Series["2"].Color = Color.Green; ;
            chart1.Series["2"].ChartType = SeriesChartType.Line;

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

            var neuron = new Neuron(2);
            while (neuron.WeightChanged)
            {
                neuron.Learn(trainingSet);
            }

            float x1 = -10;
            float x2 = 10;
            var k = -neuron.Weights[0]/neuron.Weights[1];
            var b = -neuron.Weights[2] / neuron.Weights[1];
            var y1 = calcY(x1, k, b);
            var y2 = calcY(x2, k, b);


            chart1.Series["2"].Points.AddXY(x1, y1);
            chart1.Series["2"].Points.AddXY(x2, y2);
            //chart1.Series[0].Points.AddXY(0, 0);
            //chart1.Series["1"].Points.AddXY(3, 4);

        }
    }
}
