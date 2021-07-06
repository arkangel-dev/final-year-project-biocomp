using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.GeneticAlgorithm {
    public class Candidate : IComparable<Candidate> {
        public NeuralNetwork.NeuralNetwork Brain;
        public float Score;
        public string Name;


        public int BrainConf_HiddenLayerCount = 5;
        public int BrainConf_NodesPerHiddenLayer = 2;
        public int BrainConf_OutputLayerCount = 2;
        public int BrainConf_InputLayerCount = 6;

        public Candidate(int hiddenLayerCount, int nodesPerLayer, int outputLayerCount, int InputLayerCount) {
            this.BrainConf_HiddenLayerCount = hiddenLayerCount;
            this.BrainConf_NodesPerHiddenLayer = nodesPerLayer;
            this.BrainConf_InputLayerCount = InputLayerCount;
            this.BrainConf_OutputLayerCount = outputLayerCount;
            Init();
        }

        public Candidate(int hiddenLayerCount, int nodesPerLayer) {
            this.BrainConf_HiddenLayerCount = hiddenLayerCount;
            this.BrainConf_NodesPerHiddenLayer = nodesPerLayer;
            Init();
        }

        public Candidate() {
            Init();
        }

        public Candidate(int inputCount) {
            this.BrainConf_InputLayerCount = inputCount;
            Init();
        }

        private void Init() {
            this.Brain = new NeuralNetwork.NeuralNetwork(
                    hiddenLayerCount: BrainConf_HiddenLayerCount,
                    nodesPerHiddenLayer: BrainConf_NodesPerHiddenLayer,
                    outputLayerCount: BrainConf_OutputLayerCount,
                    InputLayerCount: BrainConf_InputLayerCount
                );

            this.Score = 0;

            var prng = new System.Security.Cryptography.RNGCryptoServiceProvider();
            var bytes = new byte[4];
            prng.GetNonZeroBytes(bytes);
            this.Name = string.Join("", bytes.Select(b => b.ToString("x2"))).ToUpper();
        }


        public int CompareTo(Candidate other) {
            if (other == null) return 0;
            if (this.Score > other.Score) return -1;
            if (this.Score == other.Score) return 0;
            return 1;
        }

        public float RunTest(List<ProcessableStruct> TestSet) {
            this.Score = 0.0f;
            foreach (var Test in TestSet) {
                var expectedOutput = Test.Output;
                var actualOutputRaw = Brain.ProcessData(Test.Inputs);
                var actualOutput = actualOutputRaw[0] > actualOutputRaw[1] ? 1 : 0;

                if (actualOutput == expectedOutput)
                    this.Score += 1;
            }
            this.Score = this.Score / TestSet.Count;
            return this.Score;
        }
    }
}
