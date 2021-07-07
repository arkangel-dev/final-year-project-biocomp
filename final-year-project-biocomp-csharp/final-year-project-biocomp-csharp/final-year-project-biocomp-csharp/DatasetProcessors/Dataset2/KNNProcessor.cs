using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.DatasetProcessors.Dataset2 {
    public class KNNProcessor : ProcessorInterface {

        KNN.KNNHandler KProcessor;
        public float Test(List<ProcessableStruct> TestingData) {
            int correct = 0;
            foreach (var tp in TestingData) {

                var expected = tp.Output;
                var actual = KProcessor.GetProbablePoint(tp.Inputs.ToArray());

                Console.WriteLine($"Expecting {expected} --> Actual {actual}");

                if (expected == actual) {
                    correct++;
                }
            }

            Console.WriteLine($"{correct} correct out of {TestingData.Count} queries...");
            return (float)correct / (float)TestingData.Count;
        }

        public void Train(List<ProcessableStruct> TrainingData, int iterations = 0) {
            this.KProcessor = new KNN.KNNHandler();
            foreach (var rp in TrainingData) {
                KProcessor.AddReferencePoint(rp.Output, rp.Inputs.ToArray());
            }
        }
    }
}
