using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.GeneticAlgorithmRuleBased {
    public class GeneticAlgorithmRuleBasedSimulation {
        public List<CandidateSolution> CurrentGeneration;
        public List<CandidateSolution> NextGeneration;
        public List<ProcessableStruct> TestingData;
        public WeightedRandom<CandidateSolution> PopulationWeighted;
        public CandidateSolution BestCandidate;
        public float AverageScore;
        
        /// <summary>
        /// Constructor for this class
        /// </summary>
        /// <param name="population_size">Population Size</param>
        /// <param name="target_size">The amount of bits on the input the dataset</param>
        /// <param name="TestingData">Testing data to use in the training session</param>
        public GeneticAlgorithmRuleBasedSimulation(int population_size, int target_size, List<ProcessableStruct> TestingData) {

            this.CurrentGeneration = new List<CandidateSolution>();
            this.NextGeneration = new List<CandidateSolution>();
            this.PopulationWeighted = new WeightedRandom<CandidateSolution>();
            this.TestingData = new List<ProcessableStruct>();
            this.BestCandidate = new CandidateSolution(0);

            this.TestingData.AddRange(TestingData);
            for (int i = 0; i < population_size; i++) {
                CurrentGeneration.Add(new CandidateSolution(target_size));
            }
        }

        /// <summary>
        /// Grade and sort the population
        /// </summary>
        private void GradeAndSort() {
            foreach (var cand in this.CurrentGeneration) {
                this.PopulationWeighted.AddEntry(cand, cand.RunTestList(this.TestingData));
            }
            this.CurrentGeneration.Sort();

            if (BestCandidate.score < CurrentGeneration[0].score) {
                var og_col = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\rNew best score : {CurrentGeneration[0].score * 100}% accuracy");
                Console.ForegroundColor = og_col;
                this.BestCandidate = this.CurrentGeneration[0];
            }
        }

        /// <summary>
        /// Create the next generation of candidates
        /// </summary>
        private void CreateNextGenerationAndShift() {
            this.NextGeneration.Add(this.BestCandidate);
            while (this.NextGeneration.Count < this.CurrentGeneration.Count) {
                if (Common.GlobalRandom.Next(0, 5) == 0) {
                    this.NextGeneration.Add(PopulationWeighted.GetRandom().MateWith(BestCandidate));
                } else {
                    this.NextGeneration.Add(PopulationWeighted.GetRandom().MateWith(PopulationWeighted.GetRandom()));
                }
            }
            this.CurrentGeneration = new List<CandidateSolution>();
            this.CurrentGeneration.AddRange(this.NextGeneration);
            this.NextGeneration = new List<CandidateSolution>();
        }

        /// <summary>
        /// Run the simulation
        /// </summary>
        /// <param name="iter">Number of iterations to run</param>
        public void Run(int iter) {
            int refresh_c = 0;
            foreach (var cand in this.CurrentGeneration) {
                this.PopulationWeighted.AddEntry(cand, cand.RunTestList(this.TestingData));
            }
            this.CurrentGeneration.Sort();
            this.BestCandidate = this.CurrentGeneration[0];
            Console.WriteLine($"Best Candidate Score (Pre-training) : {this.BestCandidate.score * 100}% accurate...");
            for (int i = 0; i < iter + 1; i++) {
                //Console.WriteLine($"Iteration {i}");
                GradeAndSort();
                CreateNextGenerationAndShift();

                

                if (refresh_c == 10) {
                    var progress = (((float)i / (float)iter) * 100).ToString().PadRight(4, ' ');
                    Console.Write($"\rTraining {progress}% complete");
                    refresh_c = 0;
                }
                refresh_c++;
            }
            GradeAndSort();
            Console.WriteLine($"\nBest Candidate Score (Post-training) : {this.BestCandidate.score * 100}% accurate... ({this.BestCandidate.RuleSet})");
        }

        


    }
}
