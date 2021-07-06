using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.KNN {
    public class KNNHandler {
        public List<KNNPoint> ReferencePoints = new List<KNNPoint>();

        public void AddReferencePoint(float value, float[] dimensional_values) {
            this.ReferencePoints.Add(new KNNPoint(value, dimensional_values));
        }


       

        public float GetProbablePoint(float[] dimensional_values) {
            List<KNNDistanceResult> distances = new List<KNNDistanceResult>();
            SortedDictionary<float, int> HitCount = new SortedDictionary<float, int>();

            foreach (var rp in this.ReferencePoints) {
                distances.Add(rp.CalculateDistanceFrom(dimensional_values));
            }

            distances.Sort();

            //Console.WriteLine(String.Join(",", distances));

            //distances.Reverse();



            for (int i = 0; i < 5; i++) {

                if (HitCount.TryGetValue(distances[i].Value.Value, out _)) {
                    HitCount[distances[i].Value.Value]++;
                } else {
                    HitCount.Add(distances[i].Value.Value, 1);
                }
            }

            var results = HitCount.OrderBy(d => d.Value).Select(d => new {
                Point = d.Key,
                Count = d.Value
            });

            var res = results.First().Point;

            return res;


        }

        
    }
}
