using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.NeuralNetwork {
    public class NeuralNetwork {
        public int HiddenLayerCount = 0;
        public int NodesPerHiddenLayer = 0;
        public int OutputLayerCount = 0;
        public int InputLayerCount = 0;

        public List<List<Neuron>> Layers;

        /// <summary>
        /// Constructor for the neural network class. This is essentially the brain of the candidates in
        /// the genepool of the genetic algorithm.
        /// </summary>
        /// <param name="hiddenLayerCount">Number of hidden layers in the neural network</param>
        /// <param name="nodesPerHiddenLayer">Nodes per hidden layer</param>
        /// <param name="outputLayerCount">Number output nodes in the neural network</param>
        /// <param name="InputLayerCount">Number of input nodes in the neural network</param>
        public NeuralNetwork(int hiddenLayerCount, int nodesPerHiddenLayer, int outputLayerCount, int InputLayerCount) {
            this.HiddenLayerCount = hiddenLayerCount;
            this.NodesPerHiddenLayer = nodesPerHiddenLayer;
            this.OutputLayerCount = outputLayerCount;
            this.InputLayerCount = InputLayerCount;
            this.Layers = new List<List<Neuron>>();

            // Generate the first hidden layer. This layers neurons will have a
            // limited amount of inputs. Aka limited number of input weights
            this.GenerateNewLayer(this.NodesPerHiddenLayer, this.InputLayerCount);

            //Generate the other hidden layers. The neurons in this layer will have
            //a the same number of inputs as the number of neurons in the previous layer
            for (int i = 0; i < this.HiddenLayerCount - 1; i++)
                this.GenerateNewLayer(this.NodesPerHiddenLayer, this.NodesPerHiddenLayer);

            //Generate the final output layer. This layer will have the number of neurons specified
            //in the outputLayerCount parameter
            this.GenerateNewLayer(this.OutputLayerCount, this.NodesPerHiddenLayer);
        }

        /// <summary>
        /// This function will the process the input with the neural network
        /// </summary>
        /// <param name="Input">Input data</param>
        /// <returns>Output of the computation</returns>
        public List<float> ProcessData(List<float> Input) {

            // Process the first layer of the neural network
            foreach (var neuron in this.Layers[0]) neuron.CalculateOutput(Input);

            // Process the remaining layers of the neural network
            for (int i = 1; i < this.Layers.Count; i++) {
                foreach (var neuron in this.Layers[i]) neuron.CalculateOutput(GetValuesFromLayer(this.Layers[i - 1]));
            }

            // Process the output layer of the neural network
            foreach (var neuron in this.Layers[this.Layers.Count - 1]) neuron.CalculateOutput(this.GetValuesFromLayer(this.Layers[this.Layers.Count - 2]));
            
            // Return the final layer
            return this.GetValuesFromLayer(this.Layers[this.Layers.Count - 1]); 


        }

        /// <summary>
        /// Gets the output values of a layer in the neural network
        /// </summary>
        /// <param name="layer">Layer to extract values from</param>
        /// <returns>Value from the layer</returns>
        public List<float> GetValuesFromLayer(List<Neuron> layer) {
            // Just get the values from the passed in layer and
            // return it
            List<float> values = new List<float>();
            foreach (var neuron in layer)
                values.Add(neuron.Output);
            return values;
        }

        /// <summary>
        /// Generate a new layer
        /// </summary>
        /// <param name="NodeCount">Number of nodes in the layer</param>
        /// <param name="PreviousLayerOutputCount">Number of nodes in the previous layer</param>
        public void GenerateNewLayer(int NodeCount, int PreviousLayerOutputCount) {
            var newLayer = new List<Neuron>();
            for (int i = 0; i < NodeCount; i++) {
                newLayer.Add(new Neuron(PreviousLayerOutputCount));
            }
            this.Layers.Add(newLayer);
        }

        /// <summary>
        /// Get the weight values of the neural network
        /// </summary>
        /// <returns>List of the weights</returns>
        public List<float> GetBrainConfiguration() {
            var weights = new List<float>();
            foreach (var layer in this.Layers)
                foreach (var neuron in layer) {
                    weights.AddRange(neuron.Weights);
                    weights.Add(neuron.Bias);
                }
            return weights;
        }

        /// <summary>
        /// Set the weight values of the neural network
        /// </summary>
        /// <param name="weights">Weights to set</param>
        public void SetBrainConfiguration(List<float> weights) {
            foreach (var layer in this.Layers) {
                foreach (var neuron in layer) {
                    neuron.Weights = weights.GetRange(0, neuron.Weights.Count);
                    weights.RemoveRange(0, neuron.Weights.Count);
                    neuron.Bias = weights[0];
                    weights.RemoveAt(0);
                }
            }   
        }

    }
}
