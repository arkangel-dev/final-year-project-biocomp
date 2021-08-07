using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp {
    class Program {


        public static void Main(string[] args) {
            Console.WriteLine("UWE | BSC (Hons) Comp Sci | Bio Computation\n");
            Tests();
        }

        public static void Tests() {


            Console.WriteLine("Running Dataset 1 Processor (Genetic Algorithm) (1000 Iterations)");
            var ds1_p = new DatasetProcessors.Dataset1.GALogicFilterProcessor();
            ds1_p.Train(ProcessableStruct.ReadData1_Training(), 1500);
            Console.WriteLine($"Dataset 1 testing done...\nPress enter to continue...\n\n");
            Console.ReadLine();

            Console.WriteLine("Running Dataset 2 Processor (Logic Gate Genetic Algorithm) (1000 Iterations)");
            var ds2_lfp = new DatasetProcessors.Dataset2.GALogicFilterProcessor();
            ds2_lfp.Train(ProcessableStruct.ReadData2_Training(), 1500);
            Console.WriteLine($"Dataset 2 testing done...\nPress enter to continue...\n\n");
            Console.ReadLine();

            Console.WriteLine("Running Dataset 3 Processor (Genetic Algorithm) (2000 Iterations)...");
            var ds3_p = new DatasetProcessors.Dataset3.FromScratchProcessor();
            ds3_p.Train(ProcessableStruct.ReadData3_Training(), 10000);
            var ds3_score = ds3_p.Test(ProcessableStruct.ReadData3_Testing());
            Console.WriteLine($"Dataset 3 testing score : {ds3_score}...\nPress enter to continue...\n\n");
            Console.ReadLine();
        }

    }
}
