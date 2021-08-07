using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.GeneticAlgorithm {
    public class Simulation {
        public List<Candidate> Candidates;

        public float CurrentGenerationScore;
        public int PopulationCount;
        public int MutateIterations;
        public int MixIterations;

        public int InputCount;

        public float MutationMagnitude;
        public float MinMutationMagnitude;
        public float MaxSearchPeriod;

        public int nodesPerLayer;
        public int layerCount;

        /// <summary>
        /// Constructor for the simulation class
        /// </summary>
        public Simulation(int inputcount) {
            VarInit();
            this.InputCount = inputcount;
            for (int i = 0; i < this.PopulationCount; i++) {
                this.Candidates.Add(new Candidate(inputcount));
            }
        }

        public Simulation(int inodesperlayer, int layercount) {
            this.nodesPerLayer = inodesperlayer;
            this.layerCount = layercount;
            VarInit();
            for (int i = 0; i < this.PopulationCount; i++) {
                this.Candidates.Add(new Candidate(layercount, inodesperlayer, 2, 6));
            }
        }

        private void VarInit() {
            this.Candidates = new List<Candidate>();
            this.InputCount = 6;
            this.PopulationCount = 15;
            this.CurrentGenerationScore = 0.0f;
            this.MutateIterations = 11;
            this.MixIterations = 1;
            this.MutationMagnitude = 0.1f;
            this.MinMutationMagnitude = 0.0001f;
            this.MaxSearchPeriod = 1000;

        }

        /// <summary>
        /// Grade the candidates and sort them by how much they scored
        /// </summary>
        /// <param name="TraininData">Training data set</param>
        public void RunExamAndSort(List<ProcessableStruct> TraininData) {
            this.CurrentGenerationScore = 0.0f;

            foreach (var candidate in this.Candidates) {
                this.CurrentGenerationScore += candidate.RunTest(TraininData);
            }
            this.Candidates.Sort();
        }

        public void PrintScores() {
            foreach (var candidate in this.Candidates) {
                Console.WriteLine(candidate.Name + " >> " + candidate.Score.ToString());
            }
        }

        /// <summary>
        /// Roulette selection
        /// </summary>
        public void RouletteSelection() {
            var WeightedBag = new WeightedRandom<Candidate>();
            foreach (var candidate in this.Candidates) {
                WeightedBag.AddEntry(candidate, candidate.Score / this.CurrentGenerationScore);
            }
            var TopCandidate = this.Candidates[0];
            this.Candidates.Clear();
            this.Candidates.Add(TopCandidate);



            for (int i = 0; i < this.PopulationCount / 2; i++) {
                var parent_a = Common.GlobalRandom.Next(0, 2) == 1 ? WeightedBag.GetRandom() : TopCandidate;
                var parent_b = WeightedBag.GetRandom();

                var mixed = MateAndMutate(
                    parent_a.Brain.GetBrainConfiguration(),
                    parent_b.Brain.GetBrainConfiguration()
                );

                var child_a = new Candidate(layerCount, nodesPerLayer);
                var child_b = new Candidate(layerCount, nodesPerLayer);

                child_a.Brain.SetBrainConfiguration(mixed.Item1);
                child_b.Brain.SetBrainConfiguration(mixed.Item2);

                this.Candidates.Add(child_a);
                this.Candidates.Add(child_b);
            }
        }

        public Tuple<List<float>, List<float>> MateAndMutate(List<float> a, List<float> b) {
            if (a.Count != b.Count) return Tuple.Create(b, a);
            for (int i = 0; i < MixIterations; i++) {
                //Console.WriteLine(a.Count);
                int intersection_point = Common.GlobalRandom.Next(0, a.Count);
                //Console.WriteLine("Intersect at : " + intersection_point.ToString());
                //Console.WriteLine("Size : " + a.Count);
                var atemp = a;
                a.RemoveRange(intersection_point, a.Count - intersection_point);
                a.InsertRange(intersection_point, b.GetRange(intersection_point, b.Count - intersection_point));

                b.RemoveRange(intersection_point, b.Count - intersection_point);
                b.InsertRange(intersection_point, atemp.GetRange(intersection_point, atemp.Count - intersection_point));
            }

            if (Common.GlobalRandom.Next(0, 10) == 0) {
                for (int i = 0; i < MutateIterations; i++) {

                    int mutate_point = Common.GlobalRandom.Next(0, a.Count);


                    if (Common.GlobalRandom.Next(0, 2) == 0) {
                        a[mutate_point] = Common.GetRandomNumber(-1, 1);
                        b[mutate_point] = Common.GetRandomNumber(-1, 1);
                    } else {
                        if (Common.GlobalRandom.Next(0, 2) == 0) {
                            a[mutate_point] += this.MutationMagnitude;
                            b[mutate_point] += this.MutationMagnitude;
                        } else {
                            a[mutate_point] -= this.MutationMagnitude;
                            b[mutate_point] -= this.MutationMagnitude;
                        }
                    }


                }
            }
            return Tuple.Create(a, b);
        }


        public int SimJumpCount = 0;
        public void RunSimulation(List<ProcessableStruct> c, int Iterations, List<float> w) {
            this.RunExamAndSort(c);
            this.Candidates[0].Brain.SetBrainConfiguration(w);
            this.RunSimulation(c, Iterations, "data3.csv", 5);
        }
        public float CompletionTimeSeconds = 0.0f;


        public int SamplingRate = 1;
        public int SamplingCounter = 0;
        public string OutputFilePath;
        public string OutputFileBuffer;

        public void RunSimulation(List<ProcessableStruct> c, int Iterations, string output = null, int samplingrate = 1) {

            Console.CancelKeyPress += ExitConsole;

            RunExamAndSort(c);

            this.OutputFilePath = output;
            this.SamplingRate = samplingrate;
            this.SamplingCounter = 0;
            if (output != null) {
                OutputFilePath = output;
                OutputFileBuffer = "Generation,BestCandidate, Average, WorstCandidate\n";
            }


            float LastScore = -1;
            var first_best_score = this.Candidates[0].Score;
            var defaultColor = Console.ForegroundColor;
            int Jumps = 0;
            int JumpIterations = 0;

            var FileOut = "";


            var StartTime = DateTime.Now;

            for (int i = 0; i < Iterations; i++) {
                JumpIterations++;


                this.RunExamAndSort(c);
                this.RouletteSelection();



                var debugl = String.Format("\r{4} | Sim : {0,-5} | Best Score : {1,-7}   ({3}) | Population Score : {2}",
                        (i + 1).ToString(),
                        this.Candidates[0].Score.ToString(),
                        (this.CurrentGenerationScore / this.Candidates.Count).ToString(),
                        this.Candidates[0].Name,
                        DateTime.Now.ToString()

                    );


                if (this.Candidates[0].Score > LastScore) {
                    SimJumpCount++;
                    Console.Write(debugl);
                    Console.WriteLine("");
                    //JumpIterations -= (int)(MaxSearchPeriod / 10);
                    Jumps += 1;
                    LastScore = this.Candidates[0].Score;
                    OverwriteBestConfiguration(this.Candidates[0]);


                } else {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write(String.Format("\rSim : {0,-5} | {1} Since last jump | Population Score : {2}",
                        (i + 1).ToString(),
                        JumpIterations.ToString(),
                        (this.CurrentGenerationScore / this.Candidates.Count).ToString()
                    ));
                    Console.ForegroundColor = defaultColor;
                }

                if (this.Candidates[0].Score > 0.9f) {
                    Console.WriteLine("");
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("Minimum threshold found!");
                    Console.ForegroundColor = defaultColor;
                    break;
                }
                if (JumpIterations > MaxSearchPeriod) {

                    JumpIterations = 0;

                    if (this.MutateIterations > 1) this.MutateIterations -= 10;
                    this.MaxSearchPeriod += 3000;


                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine("\n");

                    Console.WriteLine("Refining max search time...");
                    Console.WriteLine("Refining mutation magnitude...");
                    Console.WriteLine("New mutation rate : " + this.MutateIterations.ToString() + " cycles");
                    Console.WriteLine("New max search time : " + this.MaxSearchPeriod.ToString() + " cycles");

                    if ((MutationMagnitude / 10) > this.MinMutationMagnitude) {
                        this.MutationMagnitude = this.MutationMagnitude / 10;
                        Console.WriteLine("Refining mutation rate...");
                        Console.WriteLine("New mutation magnitude : " + this.MutationMagnitude.ToString());
                    }
                    Console.WriteLine("");
                    Console.ForegroundColor = defaultColor;
                }

                FileOut += String.Format("{0},{1},{2}\n",
                    i,
                    (this.CurrentGenerationScore / this.Candidates.Count),
                    this.Candidates[0].Score
                );

                if (OutputFilePath != null && SamplingCounter>= SamplingRate) {
                    OutputFileBuffer += $"{i}, {Candidates[0].Score}, {(this.CurrentGenerationScore / this.Candidates.Count)}, {Candidates[Candidates.Count - 1].Score}\n";
                    SamplingCounter = 0;
                }

                SamplingCounter++;
            }

            var CompletionTime = (DateTime.Now - StartTime).TotalSeconds;
            CompletionTimeSeconds = (float)CompletionTime;

            System.IO.File.WriteAllText(System.IO.Path.Combine(
                System.IO.Directory.GetCurrentDirectory(),
                "ReLU_Dataset.csv"),
                FileOut);

            Console.WriteLine("");
            Console.WriteLine("------------------------------------------------------------------");
            Console.WriteLine(String.Format("Improvement : {0}", this.Candidates[0].Score - first_best_score));
            Console.WriteLine(String.Format("Duration : {0} seconds", CompletionTime));
            RunExamAndSort(c);


            if (OutputFilePath != null) {
                File.WriteAllText(OutputFilePath, OutputFileBuffer);
            }
            //this.PrintScores();

        }

        public void OverwriteBestConfiguration(Candidate c) {
            var filename = System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "BestConfig.conf");
            if (!System.IO.File.Exists(filename)) {
                var defaultTemplate =
                    "Header this can be literally anything\n" +
                    "0.0\n" + // Score
                    "One one" + // Name
                    "0\n" + // Number of hidden layers
                    "0\n" + // Number of nodes per hidden layer
                    "0.0"; // BrainConfig Data
                System.IO.File.WriteAllText(filename, defaultTemplate);
            }



            var fileRaw = new List<string>();
            fileRaw = System.IO.File.ReadAllText(filename).Split('\n').ToList();
            if (float.Parse(fileRaw[1]) < c.Score) {
                var newTemplate =
                    "{0}\n" + // Header
                    "{1}\n" + // Score
                    "{2}\n" + // Name
                    "{3}\n" + // Number of hidden layer
                    "{4}\n" + // Number of nodes per hidden layer
                    "{5}";  // Brain

                newTemplate = String.Format(newTemplate,
                    fileRaw[0],
                    c.Score.ToString(),
                    c.Name,
                    c.BrainConf_HiddenLayerCount.ToString(),
                    c.BrainConf_NodesPerHiddenLayer.ToString(),
                    String.Join(", ", c.Brain.GetBrainConfiguration())
                );

                System.IO.File.WriteAllText(filename, newTemplate);
            }

        }

        public Candidate GetBestStoredCandidate(string File = "BestConfig.conf") {
            var configLine = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), File)).Split('\n').ToList();
            var HiddenLayerCount = int.Parse(configLine[3]);
            var NodesPerHiddenLayerCount = int.Parse(configLine[4]);
            var BrainConf = new List<float>();

            foreach (var f in configLine[5].Split(',')) {
                BrainConf.Add(float.Parse(f));
            }


            var c = new Candidate(HiddenLayerCount, NodesPerHiddenLayerCount, 6, 2);
            c.Name = configLine[2];
            c.Brain.SetBrainConfiguration(BrainConf);
            return c;
        }



        private void ExitConsole(object sender, ConsoleCancelEventArgs e) {
            Console.WriteLine("");
            Console.WriteLine(String.Join(", ", this.Candidates[0].Brain.GetBrainConfiguration()));
            Console.WriteLine("Press enter to exit");
            Console.ReadLine();
        }




    }
}
