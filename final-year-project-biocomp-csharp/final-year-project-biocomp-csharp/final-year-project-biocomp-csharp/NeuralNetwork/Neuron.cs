using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.NeuralNetwork {
    public class Neuron {
        public List<float> Weights;
        public float Bias = 1f;
        public float Output = 0.0f;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="InputCount">Number of inputs for this neuron</param>
        public Neuron(int InputCount) {
            this.Weights = new List<float>();
            for (int i = 0; i < InputCount; i++) {
                this.Weights.Add(Common.GetRandomNumber(-1, 1));
            }
        }

        /// <summary>
        /// Run the computation
        /// </summary>
        /// <param name="Input">List of values to compute</param>
        /// <returns>Output value</returns>
        public float CalculateOutput(List<float> Input) {
            this.Output = 0.0f;
            for (int i = 0; i < Input.Count; i++) {
                this.Output += Input[i] * this.Weights[i];
            }
            this.Output = this.Bias + Common.ActivationFunction(this.Output);
            return this.Output;
        }


        
        


    }
}
