using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp {
    class Program {


        public static void Main(string[] args) {



            
            Tests();
        }
        
        public static void Tests() {
            Console.WriteLine("Running Dataset 1 Processor (KNN)");
            var ds1_p = new DatasetProcessors.Dataset1.KNNProcessor();
            ds1_p.Train(ProcessableStruct.ReadData1_Training());
            var ds1_score = ds1_p.Test(ProcessableStruct.ReadData1_Testing());
            Console.WriteLine($"Dataset 1 testing score : {ds1_score}...\nPress enter to continue...\n\n");
            Console.ReadLine();

            Console.WriteLine("Running Dataset 2 Processor (KNN)");
            var ds2_p = new DatasetProcessors.Dataset2.GAProcessor();
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

    }
}
