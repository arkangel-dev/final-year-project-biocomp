using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.GeneticAlgorithmLogicFiltering {
    public class Simulation {
        public List<CandidateSolution> CurrentGeneration;
        public List<CandidateSolution> NextGeneration;
        public List<ProcessableStruct> TrainingData;

        public WeightedRandom<CandidateSolution> WeightBag;

        public int InputBitCount;
        public int PopulationSize;
        public int GenerationIterations;

        public float BestScore = 0.0f;

        public Simulation(int population_count, int inputCount, List<ProcessableStruct> training_data) {
            this.InputBitCount = inputCount;
            this.PopulationSize = population_count;

            this.CurrentGeneration = new List<CandidateSolution>();
            this.NextGeneration = new List<CandidateSolution>();
            this.WeightBag = new WeightedRandom<CandidateSolution>();
            this.TrainingData = training_data;

            this.GeneratePopulation();
        }

        bool LimitReached = false;
        public void Run(int iterations) {
            this.GenerationIterations = iterations;
            this.LimitReached = false;
            for (int i = 0; i < this.GenerationIterations; i++) {

                // Grade all the candidates
                // and sort them
                foreach (var c in CurrentGeneration) {
                    //Console.WriteLine(".");
                    var score = c.EvaulateGroup(TrainingData);
                    if (BestScore < score) {
                        BestScore = score;
                        
                        Console.WriteLine($"\rNew Best Score : {BestScore.ToString().PadRight(10, ' ')} | Iteration {i.ToString().PadRight(5, ' ')} | Gene : {c.GetGeneString()}");
                    }
                    if (BestScore == 1) {
                        LimitReached = true;
                        break;
                    }
                    WeightBag.AddEntry(c, score);
                }
                if (LimitReached) {
                    Console.WriteLine("Maximum score reached!");
                    break;
                }
                CurrentGeneration.Sort();
                NextGeneration.Add(CurrentGeneration[0]);
                while (NextGeneration.Count < PopulationSize) {
                    var ca = WeightBag.GetRandom();
                    var cb = WeightBag.GetRandom();
                    var child = ca.MateWeith(cb);
                    if (Common.GlobalRandom.Next(0, 200) == 0) {
                        child.Mutate(2);
                    }
                    NextGeneration.Add(child);
                }
                TransferGeneration();

                Console.Write($"\rIteration : {(i + 1).ToString().PadRight(6, ' ')}");


            }
            Console.Write("\n");
        }

        /// <summary>
        /// Generate the population
        /// </summary>
        public void GeneratePopulation() {
            for (int i = 0; i < PopulationSize; i++) {
                this.CurrentGeneration.Add(new CandidateSolution(this.InputBitCount));
            }
        }

        /// <summary>
        /// Transfer the candidates from the next generation
        /// to the current generation
        /// </summary>
        public void TransferGeneration() {
            CurrentGeneration.Clear();
            foreach (var c in this.NextGeneration) {
                CurrentGeneration.Add(c);
            }
            NextGeneration.Clear();
        }
    }
}
