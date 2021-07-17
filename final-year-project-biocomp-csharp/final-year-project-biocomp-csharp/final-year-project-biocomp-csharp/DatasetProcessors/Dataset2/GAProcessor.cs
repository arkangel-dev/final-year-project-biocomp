using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.DatasetProcessors.Dataset2 {
    class GAProcessor : ProcessorInterface {

        GeneticAlgorithmRuleBased.GeneticAlgorithmRuleBasedSimulation sim;
        public float Test(List<ProcessableStruct> TestingData) {
            var BestCandidateTestingScore = sim.BestCandidate.RunTestList(ProcessableStruct.ReadData2_Testing());
            Console.WriteLine($"Candidate testing result : {BestCandidateTestingScore * 100}% accurate");
            return BestCandidateTestingScore;
        }

        public void Train(List<ProcessableStruct> TrainingData, int iterations = 1000) {
            sim = new GeneticAlgorithmRuleBased.GeneticAlgorithmRuleBasedSimulation(25, 6, TrainingData);
            sim.Run(iterations);
        }


        
    }
}
