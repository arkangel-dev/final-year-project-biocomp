using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.DatasetProcessors.Dataset1 {
    class GAProcessor : ProcessorInterface {

        GeneticAlgorithmLogicFiltering.Simulation sim;
        public float Test(List<ProcessableStruct> TestingData) {
            var BestCandidateTestingScore = sim.CurrentGeneration[0].EvaulateGroup(TestingData);
            return BestCandidateTestingScore;
        }

        public void Train(List<ProcessableStruct> TrainingData, int iterations = 1000) {
            sim = new GeneticAlgorithmLogicFiltering.Simulation(20, 5, ProcessableStruct.ReadData1_Training());
            sim.Run(iterations);
        }
    }
}
