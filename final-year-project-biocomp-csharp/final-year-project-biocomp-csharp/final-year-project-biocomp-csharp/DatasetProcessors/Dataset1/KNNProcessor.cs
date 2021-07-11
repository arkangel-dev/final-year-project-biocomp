
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.DatasetProcessors.Dataset1 {
    public class KNNProcessor : ProcessorInterface {

        KNN.KNNHandler knn = new KNN.KNNHandler();

        public float Test(List<ProcessableStruct> TestingData) {
            var correct = 0;
            foreach (var td in TestingData) {
                var result = knn.GetProbablePoint(td.Inputs.ToArray());
                if (result == td.Output) {
                    correct++;
                }
            }
            Console.WriteLine($"{correct} correct out of {TestingData.Count} queries...");
            return (float)correct / (float)TestingData.Count;
        }

        public void Train(List<ProcessableStruct> TrainingData, int iterations = 0) {
            foreach (var td in TrainingData) {
                knn.AddReferencePoint(td.Output, td.Inputs.ToArray());
            }
        }
    }
}
