using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp {
    class Program {


        public static void Main(string[] args) {

            Console.WriteLine("Running Dataset Processor...");

            var ds1_p = new DatasetProcessors.Dataset3.FromScratchProcessor();
            ds1_p.Train(ProcessableStruct.ReadData3_Training(), 1000);
            var score = ds1_p.Test(ProcessableStruct.ReadData3_Testing());

            Console.WriteLine($"Data set 3 score : {score}...");
        }

        public static void RunKNNTest() {
            Console.WriteLine("Starting program...");


            var data2Training = ProcessableStruct.ReadData2_Training();
            var data2Testing = ProcessableStruct.ReadData2_Testing();
            var knn = new KNN.KNNHandler();

            foreach (var rp in data2Training) {
                knn.AddReferencePoint(rp.Output, rp.Inputs.ToArray());
            }

            int correct = 0;
            foreach (var tp in data2Testing) {

                var expected = tp.Output;
                var actual = knn.GetProbablePoint(tp.Inputs.ToArray());

                Console.WriteLine($"Expecting {expected} --> Actual {actual}");

                if (expected == actual) {
                    correct++;
                }
            }

            Console.WriteLine($"Correct : {correct} correct out of {data2Testing.Count}");
            Console.ReadLine();

        }






    }
}
