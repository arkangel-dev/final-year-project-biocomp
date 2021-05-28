using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.GeneticAlgorithm {
    public class ProcessableStruct {
        public List<float> Inputs;
        public float Output;
        
        public ProcessableStruct() {
            this.Inputs = new List<float>();
            this.Output = 0.0f;
        }

        public ProcessableStruct(List<float> inputs, float output) {
            this.Inputs = inputs;
            this.Output = output;
        }

        public static List<ProcessableStruct> ReadData3_Training() {
            var data1_raw = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "dataset3", "data3_Trainin.txt"));
            var data1_newLineSplit = data1_raw.Split('\n');
            var data1_parsed = new List<ProcessableStruct>();
            foreach (var line in data1_newLineSplit) {
                var split = line.Split(' ').ToList();
                var converted_split = new List<float>();
                foreach (var i in split) {
                    converted_split.Add(float.Parse(i));
                }
                data1_parsed.Add(new ProcessableStruct() {
                    Inputs = converted_split.GetRange(0, 5),
                    Output = converted_split[6]
                });
            }
            return data1_parsed;
        }

        public static List<ProcessableStruct> ReadData3_Testing() {
            var data1_raw = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "dataset3", "data3_Testin.txt"));
            var data1_newLineSplit = data1_raw.Split('\n');
            var data1_parsed = new List<ProcessableStruct>();
            foreach (var line in data1_newLineSplit) {
                var split = line.Split(' ').ToList();
                var converted_split = new List<float>();
                foreach (var i in split) {
                    converted_split.Add(float.Parse(i));
                }
                data1_parsed.Add(new ProcessableStruct() {
                    Inputs = converted_split.GetRange(0, 5),
                    Output = converted_split[6]
                });
            }
            return data1_parsed;
        }

        public static List<ProcessableStruct> ReadData2_Training() {
            var data_2_raw = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "dataset2", "data2_Trainin.txt")).Split('\n');
            var data_2_parsed = new List<ProcessableStruct>();
            foreach (var line in data_2_raw) {
                var inputs = new List<float>();
                var output = float.Parse(line.Split(' ')[1]);
                foreach (var i in line.Split(' ')[0].ToCharArray()) {
                    inputs.Add(float.Parse(i.ToString()));
                }
                data_2_parsed.Add(new ProcessableStruct(inputs, output));
            }
            return data_2_parsed;
        }

        public static List<ProcessableStruct> ReadData2_Testing() {
            var data_2_raw = System.IO.File.ReadAllText(System.IO.Path.Combine(System.IO.Directory.GetCurrentDirectory(), "dataset2", "data2_Testin.txt")).Split('\n');
            var data_2_parsed = new List<ProcessableStruct>();
            foreach (var line in data_2_raw) {
                var inputs = new List<float>();
                var output = float.Parse(line.Split(' ')[1]);
                foreach (var i in line.Split(' ')[0].ToCharArray()) {
                    inputs.Add(float.Parse(i.ToString()));
                }
                data_2_parsed.Add(new ProcessableStruct(inputs, output));
            }
            return data_2_parsed;
        }
    }
}
