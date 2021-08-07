using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.GeneticAlgorithmLogicFiltering {
    public class CandidateSolution : IComparable<CandidateSolution> {
        // Gene 
        // 0121422
        // First Bit : Bit A Position
        // Second Bit : Bit B Position
        // Third Bit : Logical Operation to use

        public List<int> Gene;
        public int Size = 0;
        public float Score = 0.0f;


        public CandidateSolution(int size) {
            this.Size = size;
            this.Gene = new List<int>();
            GenerateGene();
        }

        public CandidateSolution MateWith(CandidateSolution c) {
            var intersectionPoint = Common.GlobalRandom.Next(1, c.Gene.Count);
            var newGene = new List<int>();
            newGene.AddRange(Gene.GetRange(0, intersectionPoint));
            newGene.AddRange(c.Gene.GetRange(intersectionPoint, c.Gene.Count - intersectionPoint));
            var child = new CandidateSolution(this.Size);
            child.Gene = newGene;
            return child;
        }

        public CandidateSolution MateWith2Point(CandidateSolution c) {
            var intersection_points = new List<int>();
            intersection_points.Add(Common.GlobalRandom.Next(1, c.Gene.Count));
            intersection_points.Add(Common.GlobalRandom.Next(1, c.Gene.Count));

            intersection_points.Sort();

            var newGene = new List<int>();
            newGene.AddRange(Gene.GetRange(0, intersection_points[0]));
            newGene.AddRange(c.Gene.GetRange(intersection_points[0], intersection_points[1] - intersection_points[0]));
            newGene.AddRange(Gene.GetRange(intersection_points[1], Gene.Count - intersection_points[1]));

            var child = new CandidateSolution(this.Size);
            child.Gene = newGene;
            return child;
        }

        public void Mutate(int iter) {
            for (int i = 0; i < iter; i++) {
                var mutationPoint = Common.GlobalRandom.Next(1, Gene.Count);
                if (mutationPoint > 15) {
                    Gene[mutationPoint] = Common.GlobalRandom.Next(0, 6);
                } else {
                    Gene[mutationPoint] = Common.GlobalRandom.Next(0, Size);
                }
            }
        }

        public bool Evaluate(ProcessableStruct p) {            
            var pa_a = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[0].ToString())]).ToString()[0]);
            var pa_b = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[1].ToString())]).ToString()[0]);
            var pb_a = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[2].ToString())]).ToString()[0]);
            var pb_b = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[3].ToString())]).ToString()[0]);
            var pc_a = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[4].ToString())]).ToString()[0]);
            var pc_b = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[5].ToString())]).ToString()[0]);
            var pd_a = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[6].ToString())]).ToString()[0]);
            var pd_b = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[7].ToString())]).ToString()[0]);
            var pe_a = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[8].ToString())]).ToString()[0]);
            var pe_b = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[9].ToString())]).ToString()[0]);
            var pf_a = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[10].ToString())]).ToString()[0]);
            var pf_b = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[11].ToString())]).ToString()[0]);
            var pg_a = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[12].ToString())]).ToString()[0]);
            var pg_b = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[13].ToString())]).ToString()[0]);
            var ph_a = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[14].ToString())]).ToString()[0]);
            var ph_b = ConvertIntToBool(((int)p.Inputs[Int32.Parse(this.Gene[15].ToString())]).ToString()[0]);

            
            var pa_rule = this.Gene[16];
            var pb_rule = this.Gene[17];
            var pc_rule = this.Gene[18];
            var pd_rule = this.Gene[19];
            var pe_rule = this.Gene[20];
            var pf_rule = this.Gene[21];
            var pg_rule = this.Gene[22];
            var ph_rule = this.Gene[23];

            var ab_rule = this.Gene[24];
            var cd_rule = this.Gene[25];
            var ef_rule = this.Gene[26];
            var gh_rule = this.Gene[27];

            var ab_cd_rule = this.Gene[28];
            var ef_gh_rule = this.Gene[29];
            var ab_cd_ef_gh_rule = this.Gene[30];

            return (HandleLogic(
                        HandleLogic(
                            HandleLogic(
                                HandleLogic(pa_a, pa_b, pa_rule),
                                HandleLogic(pb_a, pb_b, pb_rule),
                                ab_rule),

                            HandleLogic(
                                HandleLogic(pc_a, pc_b, pc_rule),
                                HandleLogic(pd_a, pd_b, pd_rule),
                                cd_rule),
                            ab_cd_rule
                        ),
                        HandleLogic(
                            HandleLogic(
                                HandleLogic(pe_a, pe_b, pe_rule),
                                HandleLogic(pf_a, pf_b, pf_rule),
                                ef_rule),

                            HandleLogic(
                                HandleLogic(pg_a, pg_b, pg_rule),
                                HandleLogic(ph_a, ph_b, ph_rule),
                                gh_rule),
                            ef_gh_rule
                        ), ab_cd_ef_gh_rule) ? 1 : 0) == p.Output;
        }

        public float EvaulateGroup(List<ProcessableStruct> s) {
            var correct = 0;
            foreach (var t in s) {
                if (Evaluate(t))
                    correct++;
            }
            this.Score = (float)correct / (float)s.Count;
            return this.Score;
        }

        public string GetGeneString() {
            var output_string = "";

            output_string += String.Join("-", Gene.GetRange(0, 17)); //Inputs
            
            for (int i = 17; i < Gene.Count; i++) {
                output_string += Gene[i].ToString();
            }
            return output_string;
        }

        private static bool HandleLogic(bool bita, bool bitb, int ruleindex) {
            var bitap = bita;
            var bitbp = bitb;

            switch (ruleindex) {
                case 0: return (bitap & bitbp);
                case 1: return (bitap | bitbp);
                case 2: return (bitap ^ bitbp);
                case 3: return !(bitap & bitbp);
                case 4: return !(bitap | bitbp);
                case 5: return !(bitap ^ bitbp);

            }
            throw new Exception($"Cannot find operation index {ruleindex}");
        }

        public void GenerateGene() {
            this.Gene = new List<int>();

            for (int i = 0; i < 16; i++) {
                this.Gene.Add(Common.GlobalRandom.Next(0, Size));
            }

            for (int i = 0; i < 15; i++) {
                this.Gene.Add(Common.GlobalRandom.Next(0, 6));
            }
        }

        private static bool ConvertIntToBool(int bit) {
            return bit == '1';
        }

        private static char ConvertBoolToString(bool bit) {
            return bit ? '1' : '0';
        }

        public int CompareTo(CandidateSolution other) {

            if (other.Score > this.Score) return 1;
            if (other.Score < this.Score) return -1;
            return 0;

        }

        public CandidateSolution Clone() {
            var clone = new CandidateSolution(this.Size);
            clone.Gene = new List<int>();
            clone.Gene.AddRange(this.Gene);
            return clone;
        }
    }
}
