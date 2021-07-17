using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.GeneticAlgorithmRuleBased {
    public class CandidateSolution : IComparable<CandidateSolution> {
        public string RuleSet = "";
        public float score = 0.0f;

        public CandidateSolution(int target_set_size) {
            this.GenerateInitialSet(target_set_size);
        }

        // Generate an initial rule set
        private void GenerateInitialSet(int size) {
            size = size * 2;
            for (int i = 0; i < size; i++) {
                if (i % 2 == 0) {
                    RuleSet += Common.GlobalRandom.Next(0, 2).ToString();
                } else {
                    RuleSet += Common.GlobalRandom.Next(0, 6).ToString();
                }
            }
        }

        // Mate with another candidate
        public CandidateSolution MateWith(CandidateSolution c) {
            var intersection_point = -1;
            var child = new CandidateSolution(this.RuleSet.Length);
            while (intersection_point == -1 || intersection_point % 2 != 0) {
                intersection_point = Common.GlobalRandom.Next(1, this.RuleSet.Length);
            }
            child.RuleSet = "";
            if (Common.GlobalRandom.Next(0, 2) == 1) {
                child.RuleSet += this.RuleSet.Substring(0, intersection_point);
                child.RuleSet += c.RuleSet.Substring(intersection_point, this.RuleSet.Length - intersection_point);
            } else {
                child.RuleSet += c.RuleSet.Substring(0, intersection_point);
                child.RuleSet += this.RuleSet.Substring(intersection_point, this.RuleSet.Length - intersection_point);
            }
            //Console.WriteLine($"Intersection Point : {intersection_point}");
            //Console.WriteLine($"Parent A ruleset : {c.RuleSet}");
            //Console.WriteLine($"Parent B ruleset : {this.RuleSet}");
            //Console.WriteLine($"Child ruleset    : {child.RuleSet}");
            return child;
        }

        // Run a list of tests
        public bool RunSingleTest(ProcessableStruct t) {
            if (t.Inputs.Count != (this.RuleSet.Length / 2)) {
                throw new Exception("Process incompatible test data : Bit size mismatch");
            }
            var input_bit_index = 0;
            var result = "";
            for (int i = 0; i < this.RuleSet.Length; i += 2) {
                var inputbit = t.Inputs[input_bit_index];
                var rulebit = this.RuleSet[i];
                var rulelogic = this.RuleSet[i + 1];
                var resultbit = HandleLogic(inputbit.ToString().ToCharArray()[0], rulebit, rulelogic);
                if (resultbit != '1') {
                    return t.Output == 0;
                }
                result += resultbit;
                input_bit_index++;
            }
            return t.Output == 1;
        }

        // Run a list of tests
        public float RunTestList(List<ProcessableStruct> tests, bool stop_on_mismatch = true) {
            var correct = 0;
            foreach (var t in tests) {
                if (this.RunSingleTest(t)) {
                    correct++;
                }
            }
            this.score = (float)correct / (float)tests.Count;
            return score;
        }

        // This function will handle the boolean logic
        private static char HandleLogic(char inputbit, char rulebit, char ruleindex) {
            var inputbitp = ConvertStringToBool(inputbit);
            var rulebitp = ConvertStringToBool(rulebit);

            switch (ruleindex) {
                case '0':
                    return ConvertBoolToString(inputbitp & rulebitp);

                case '1':
                    return ConvertBoolToString(inputbitp | rulebitp);

                case '2':
                    return ConvertBoolToString(inputbitp ^ rulebitp);

                case '3':
                    return ConvertBoolToString(!inputbitp);

                case '4':
                    return inputbit;

                case '5':
                    return rulebit;

            }
            throw new Exception($"Cannot find operation index {ruleindex}");
        }

        // Convert strings to bools
        private static bool ConvertStringToBool(char bit) {
            return bit == '1';
        } 

        // Convert bools to strings
        private static char ConvertBoolToString(bool bit) {
            return bit ? '1' : '0';
        }

        // Comparision Data
        public int CompareTo(CandidateSolution other) {
            if (other.score > this.score) return 1;
            if (other.score < this.score) return -1;
            return 0;
        }
    }
}
