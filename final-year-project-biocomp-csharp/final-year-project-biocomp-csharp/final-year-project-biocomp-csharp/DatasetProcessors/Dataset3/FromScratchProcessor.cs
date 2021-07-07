using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.DatasetProcessors.Dataset3 {
    public class FromScratchProcessor : ProcessorInterface {

        public GeneticAlgorithm.Simulation sim;


        public float Test(List<ProcessableStruct> TestingData) {
            var BestCandidate = sim.Candidates[0];
            return BestCandidate.RunTest(TestingData);
        }

        public void Train(List<ProcessableStruct> TrainingData, int iterations = 0) {
            Console.WriteLine($"Running Training for {iterations} iterations...");
            this.sim = new GeneticAlgorithm.Simulation(2, 5);
            this.sim.RunSimulation(TrainingData, iterations);
        }
    }
}
