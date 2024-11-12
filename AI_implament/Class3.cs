using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AI_implament
{ 
        public class Perceptron
        {
            private double[] weights;
            private double bias;
            private double learningRate;

            public Perceptron(int inputSize, double learningRate)
            {
                weights = new double[inputSize];
                bias = 0;
                this.learningRate = learningRate;

                // Initialize weights randomly
                Random random = new Random();
                for (int i = 0; i < inputSize; i++)
                {
                    weights[i] = random.NextDouble() * 2 - 1;
                }
            }

            public double Predict(double[] inputs)
            {
                double sum = 0;
                for (int i = 0; i < inputs.Length; i++)
                {
                    sum += weights[i] * inputs[i];
                }
                sum += bias;

                // Use a simple step function as the activation function
                return sum >= 0 ? 1 : 0;
            }

            public void Train(double[][] inputs, int[] targets)
            {
                for (int i = 0; i < inputs.Length; i++)
                {
                    double prediction = Predict(inputs[i]);
                    double error = targets[i] - prediction;

                    for (int j = 0; j < weights.Length; j++)
                    {
                        weights[j] += learningRate * error * inputs[i][j];
                    }
                    bias += learningRate * error;
                }
            }
        }
}
