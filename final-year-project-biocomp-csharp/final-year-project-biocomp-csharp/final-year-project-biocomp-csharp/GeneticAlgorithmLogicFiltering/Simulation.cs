using System;
using System.Collections.Generic;
using System.IO;
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
        public float AveragePopulationScore = 0.0f;

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

        private string OutputFileBuffer = null;
        private string OutputFilePath = null;
        private int SamplingRate = 1;
        private int SamplingRateCounter = 0;

        public void Run(int iterations, string outputfile = null, int sampling_rate = 1) {
            this.GenerationIterations = iterations;
            this.LimitReached = false;

            this.SamplingRate = sampling_rate;
            this.SamplingRateCounter = 0;

            if (outputfile != null) {
                OutputFilePath = outputfile;
                OutputFileBuffer = "Generation,BestCandidate, Average, WorstCandidate\n";
            }

            for (int i = 0; i < this.GenerationIterations; i++) {

                // Grade all the candidates
                // and sort them
                var total = 0.0f;
                foreach (var c in CurrentGeneration) {
                    //Console.WriteLine(".");
                    var score = c.EvaulateGroup(TrainingData);
                    if (BestScore < score) {
                        BestScore = score;
                        
                        Console.WriteLine($"\rNew Score : {BestScore.ToString().PadRight(10, ' ')} | Population : {AveragePopulationScore.ToString().PadRight(10,' ')} | Iteration {i.ToString().PadRight(5, ' ')} | Gene : {c.GetGeneString()}");
                    }

                    total += score;

                    if (BestScore == 1) {
                        LimitReached = true;
                        break;
                    }
                    WeightBag.AddEntry(c, score);
                }
                AveragePopulationScore = total / PopulationSize;
                if (LimitReached ) {
                    Console.WriteLine("Maximum score reached!");
                    break;
                }
                CurrentGeneration.Sort();
                NextGeneration.Add(CurrentGeneration[0]);



                if (OutputFilePath != null && SamplingRateCounter >= SamplingRate) {
                    OutputFileBuffer += $"{i}, {CurrentGeneration[0].Score}, {AveragePopulationScore}, {CurrentGeneration[PopulationSize - 1].Score}\n";
                    SamplingRateCounter = 0;
                } 

                SamplingRateCounter++;

                for (int z = 0; z < 1; z++) {
                    var clone = this.CurrentGeneration[0].Clone();
                    clone.Mutate(1);
                    this.NextGeneration.Add(clone);
                }


                while (NextGeneration.Count < PopulationSize) {
                    var ca = WeightBag.GetRandom();
                    var cb = WeightBag.GetRandom();
                    var child_a = ca.MateWith(cb);
             
                    if (Common.GlobalRandom.NextDouble() < 0.1) {
                        child_a.Mutate(1);
                    }
                   
                    NextGeneration.Add(child_a);
                  
                }
                TransferGeneration();

                Console.ForegroundColor = ConsoleColor.Green;
                Console.Write($"\rIteration : {(i + 1).ToString().PadRight(10, ' ')} | Population : {AveragePopulationScore}");
                Console.ResetColor();

            }

            if (OutputFilePath != null) {
                File.WriteAllText(OutputFilePath, OutputFileBuffer);
            }

            Console.WriteLine("\n");
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
