using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.KNN {
    public class KNNDistanceResult : IComparable<KNNDistanceResult> {

        public KNNPoint Value;
        public double Distance;

        public KNNDistanceResult(KNNPoint v, double d) {
            this.Value = v;
            this.Distance = d;
        }
        public int CompareTo(KNNDistanceResult other) {
            if (other.Distance == this.Distance) return 0;
            if (other.Distance > this.Distance) {
                return -1;
            } else {
                return 1;
            }
        }

        public override string ToString() {
            return Distance.ToString();
        }
    }
}
