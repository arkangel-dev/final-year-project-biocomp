using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp {
    class Program {


        public static void Main(string[] args) {

            Console.WriteLine("Running Dataset 1 Processor (KNN)");
            var ds1_p = new DatasetProcessors.Dataset1.KNNProcessor();
            ds1_p.Train(ProcessableStruct.ReadData1_Training());
            var ds1_score = ds1_p.Test(ProcessableStruct.ReadData1_Testing());
            Console.WriteLine($"Dataset 1 testing score : {ds1_score}...\nPress enter to continue...\n\n");
            Console.ReadLine();

            Console.WriteLine("Running Dataset 2 Processor (KNN)");
            var ds2_p = new DatasetProcessors.Dataset2.KNNProcessor();
            ds2_p.Train(ProcessableStruct.ReadData2_Training());
            var ds2_score = ds2_p.Test(ProcessableStruct.ReadData2_Testing());
            Console.WriteLine($"Dataset 2 testing score : {ds2_score}...\nPress enter to continue...\n\n");
            Console.ReadLine();

            Console.WriteLine("Running Dataset 3 Processor (Genetic Algorithm) (2000 Iterations)...");
            var ds3_p = new DatasetProcessors.Dataset3.FromScratchProcessor();
            ds3_p.Train(ProcessableStruct.ReadData3_Training(), 2000);
            var ds3_score = ds3_p.Test(ProcessableStruct.ReadData3_Testing());
            Console.WriteLine($"Dataset 3 testing score : {ds3_score}...\nPress enter to continue...\n\n");
            Console.ReadLine();

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
