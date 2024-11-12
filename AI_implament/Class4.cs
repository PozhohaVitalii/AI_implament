using System;


public class MLP
{
    private int inputSize;
    private int hiddenSize;
    private int outputSize;
    private double[,] weightsInputHidden;
    private double[,] weightsHiddenOutput;
    private double[] hiddenBias;
    private double[] outputBias;
    private double learningRate;

    public MLP(int inputSize, int hiddenSize, int outputSize, double learningRate)
    {
        this.inputSize = inputSize;
        this.hiddenSize = hiddenSize;
        this.outputSize = outputSize;
        this.learningRate = learningRate;

        weightsInputHidden = InitializeWeights(inputSize, hiddenSize);
        weightsHiddenOutput = InitializeWeights(hiddenSize, outputSize);
        hiddenBias = InitializeBias(hiddenSize);
        outputBias = InitializeBias(outputSize);
    }

    private double[,] InitializeWeights(int rows, int cols)
    {
        var random = new Random();
        double[,] weights = new double[rows, cols];
        for (int i = 0; i < rows; i++)
            for (int j = 0; j < cols; j++)
                weights[i, j] = random.NextDouble() - 0.5; // Random values between -0.5 and 0.5
        return weights;
    }

    private double[] InitializeBias(int size)
    {
        double[] bias = new double[size];
        for (int i = 0; i < size; i++)
            bias[i] = 0;
        return bias;
    }

    private double[] ForwardPass(double[] inputs, out double[] hiddenLayerOutputs)
    {
        hiddenLayerOutputs = new double[hiddenSize];

        // Input -> Hidden
        for (int i = 0; i < hiddenSize; i++)
        {
            hiddenLayerOutputs[i] = hiddenBias[i];
            for (int j = 0; j < inputSize; j++)
                hiddenLayerOutputs[i] += inputs[j] * weightsInputHidden[j, i];
            hiddenLayerOutputs[i] = Sigmoid(hiddenLayerOutputs[i]);
        }

        // Hidden -> Output
        double[] outputLayerOutputs = new double[outputSize];
        for (int i = 0; i < outputSize; i++)
        {
            outputLayerOutputs[i] = outputBias[i];
            for (int j = 0; j < hiddenSize; j++)
                outputLayerOutputs[i] += hiddenLayerOutputs[j] * weightsHiddenOutput[j, i];
            outputLayerOutputs[i] = Sigmoid(outputLayerOutputs[i]);
        }

        return outputLayerOutputs;
    }

    private double Sigmoid(double x)
    {
        return 1.0 / (1.0 + Math.Exp(-x));
    }

    private double SigmoidDerivative(double x)
    {
        return x * (1.0 - x); // Assuming x is already sigmoid-activated
    }

    public void Train(double[][] inputs, double[][] expectedOutputs, int epochs)
    {
        for (int epoch = 0; epoch < epochs; epoch++)
        {
            for (int k = 0; k < inputs.Length; k++)
            {
                // Forward pass
                double[] hiddenLayerOutputs;
                double[] predictedOutputs = ForwardPass(inputs[k], out hiddenLayerOutputs);

                // Compute output layer error and deltas
                double[] outputDeltas = new double[outputSize];
                for (int i = 0; i < outputSize; i++)
                {
                    double error = expectedOutputs[k][i] - predictedOutputs[i];
                    outputDeltas[i] = error * SigmoidDerivative(predictedOutputs[i]);
                }

                // Compute hidden layer error and deltas
                double[] hiddenDeltas = new double[hiddenSize];
                for (int i = 0; i < hiddenSize; i++)
                {
                    double error = 0.0;
                    for (int j = 0; j < outputSize; j++)
                        error += outputDeltas[j] * weightsHiddenOutput[i, j];
                    hiddenDeltas[i] = error * SigmoidDerivative(hiddenLayerOutputs[i]);
                }

                // Update weights and biases for hidden -> output layer
                for (int i = 0; i < hiddenSize; i++)
                {
                    for (int j = 0; j < outputSize; j++)
                    {
                        weightsHiddenOutput[i, j] += learningRate * outputDeltas[j] * hiddenLayerOutputs[i];
                    }
                }
                for (int i = 0; i < outputSize; i++)
                {
                    outputBias[i] += learningRate * outputDeltas[i];
                }

                // Update weights and biases for input -> hidden layer
                for (int i = 0; i < inputSize; i++)
                {
                    for (int j = 0; j < hiddenSize; j++)
                    {
                        weightsInputHidden[i, j] += learningRate * hiddenDeltas[j] * inputs[k][i];
                    }
                }
                for (int i = 0; i < hiddenSize; i++)
                {
                    hiddenBias[i] += learningRate * hiddenDeltas[i];
                }
            }

            if (epoch % 100 == 0) // Print error every 100 epochs
                Console.WriteLine($"Epoch {epoch}: Error = {CalculateTotalError(inputs, expectedOutputs)}");
        }
    }

    private double CalculateTotalError(double[][] inputs, double[][] expectedOutputs)
    {
        double totalError = 0;
        for (int k = 0; k < inputs.Length; k++)
        {
            double[] hiddenLayerOutputs;
            double[] predictedOutputs = ForwardPass(inputs[k], out hiddenLayerOutputs);

            for (int i = 0; i < outputSize; i++)
            {
                double error = expectedOutputs[k][i] - predictedOutputs[i];
                totalError += error * error;
            }
        }
        return totalError / 2.0;
    }

    public double[] Predict(double[] inputs)
    {
        double[] hiddenLayerOutputs;
        return ForwardPass(inputs, out hiddenLayerOutputs);
    }
}
