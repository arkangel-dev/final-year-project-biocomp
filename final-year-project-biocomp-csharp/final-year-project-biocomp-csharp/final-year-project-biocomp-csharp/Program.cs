using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp {
    class Program {


        public static void Main(string[] args) {
            Console.WriteLine("Starting program...");



            var sim = new GeneticAlgorithm.Simulation(3,2);
            //training session
            sim.RunSimulation(GeneticAlgorithm.ProcessableStruct.ReadData3_Training(), 10000);

            //var cand = sim.Candidates[0];
            //Console.WriteLine(String.Format("Candidate '{0}' scored {1}% accuracy", cand.Name, cand.RunTest(GeneticAlgorithm.ProcessableStruct.ReadData3_Testing()) * 100));


            ////Console.WriteLine("Done program");
            ////Console.ReadLine();

            //BrainConfigurationTest();
        }

        private static void BrainConfigurationTest() {
            int maxNeuronPerLayerCount = 10;
            int minNeuronPerLayerCount = 1;
            int maxLayerCount = 10;
            int minLayerCount = 1;
            int AverageCount = 3;

            string csv_string = "";

            for (int layer = minLayerCount; layer < maxLayerCount; layer++) {
                for (int neuron = minNeuronPerLayerCount; neuron < maxNeuronPerLayerCount; neuron++) {
                    float total_score = 0.0f;
                    float total_time = 0.0f;
                    int total_jumps = 0;
                    for (int i = 0; i < AverageCount; i++) {
                        
                        var sim = new GeneticAlgorithm.Simulation(neuron, layer);
                        sim.RunSimulation(GeneticAlgorithm.ProcessableStruct.ReadData3_Training(), 500);
                        total_score += sim.Candidates[0].Score;
                        total_time += sim.CompletionTimeSeconds;
                        total_jumps += sim.SimJumpCount;
                    }
                    float average_score = total_score / AverageCount;
                    float average_time = total_time / AverageCount;
                    float average_jumps = total_jumps / AverageCount;

                    Console.WriteLine("\n================================================================");
                    Console.WriteLine(
                            String.Format("Layers : {0} | Nodes : {1} | Time : {2} | Score {3} | Jumps {4}",
                            layer,
                            neuron,
                            average_time,
                            average_score,
                            average_jumps
                        ));

                    Console.WriteLine("\n\n\n\n");

                    csv_string += String.Format("{0},{1},{2},{3},{4}\n",
                        layer,
                        neuron, 
                        average_time,
                        average_score,
                        average_jumps
                        );
                    System.IO.File.WriteAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "BrainStructConfig.csv"), csv_string);
                }
            }
            //System.IO.File.WriteAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "BrainStructConfig.csv"), csv_string);
        }



      
    }
}
