using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp {
    class Program {


        public static void Main(string[] args) {

            //var list = new List<GeneticAlgorithmLogicFiltering.CandidateSolution>();

            //for (int i = 0; i < 500; i++) {
            //    var candidate = new GeneticAlgorithmLogicFiltering.CandidateSolution(5);
            //    candidate.EvaulateGroup(ProcessableStruct.ReadData1_Training());
            //    list.Add(candidate);
            //}

            //list.Sort();
            //list.Reverse();

            //foreach (var cand in list) {
            //    Console.WriteLine($"Candidate {cand.Gene} : Score : {cand.Score}");
            //}


            //var canda = new GeneticAlgorithmLogicFiltering.CandidateSolution(5);
            //var candb = new GeneticAlgorithmLogicFiltering.CandidateSolution(5);
            //var candc = canda.MateWeith(candb);

            //Console.WriteLine($"Candidate A Gene : {canda.Gene}");
            //Console.WriteLine($"Candidate B Gene : {candb.Gene}");
            //Console.WriteLine($"Candidate C Gene : {candc.Gene}");

            for (int i = 0; i < 1; i++) {

                var Simulation = new GeneticAlgorithmLogicFiltering.Simulation(15, 6, ProcessableStruct.ReadData2_Training());
                Simulation.Run(2500, "data.csv", 5);
                var testing_score = Simulation.CurrentGeneration[0].EvaulateGroup(ProcessableStruct.ReadData2_Testing());
                var og_col = Console.ForegroundColor;
                Console.ForegroundColor = testing_score > 0.5 ? ConsoleColor.Green : ConsoleColor.Red;
                var verdict = testing_score > 0.5 ? "Acceptable" : "Unacceptable";
                Console.WriteLine($"Testing Score : {testing_score} : {verdict} ");
                Console.ForegroundColor = og_col;
            }
            Console.WriteLine("Done Training...\n");
            Console.ReadLine();

            //Tests();
        }
        
        public static void Tests() {
            Console.WriteLine("Running Dataset 1 Processor (KNN)");
            var ds1knn_p = new DatasetProcessors.Dataset1.KNNProcessor();
            ds1knn_p.Train(ProcessableStruct.ReadData1_Training());
            var ds1knn_score = ds1knn_p.Test(ProcessableStruct.ReadData1_Testing());
            Console.WriteLine($"Dataset 1 testing score : {ds1knn_score}...\nPress enter to continue...\n\n");
            Console.ReadLine();

            Console.WriteLine("Running Dataset 1 Processor (Genetic Algorithm) (1000 Iterations)");
            var ds1_p = new DatasetProcessors.Dataset1.GAProcessor();
            ds1_p.Train(ProcessableStruct.ReadData1_Training());
            var ds1_score = ds1_p.Test(ProcessableStruct.ReadData1_Testing());
            Console.WriteLine($"Dataset 1 testing score : {ds1_score}...\nPress enter to continue...\n\n");
            Console.ReadLine();

            Console.WriteLine("Running Dataset 2 Processor (Logic Gate Genetic Algorithm) (1000 Iterations)");
            var ds2_lfp = new DatasetProcessors.Dataset1.GAProcessor();
            ds2_lfp.Train(ProcessableStruct.ReadData2_Training());
            var ds2lfp_score = ds2_lfp.Test(ProcessableStruct.ReadData2_Testing());
            Console.WriteLine($"Dataset 2 testing score : {ds2lfp_score}...\nPress enter to continue...\n\n");
            Console.ReadLine();

            Console.WriteLine("Running Dataset 2 Processor (Genetic Algorithm) (1000 Iterations)");
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
