using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.KNN {
    public class KNNPoint {

        public List<float> DimensionValues = new List<float>();
        public float Value;

        public KNNPoint(float value, float[] dimension_values) {
            Value = value;
            foreach (var d in dimension_values) {
                DimensionValues.Add(d);
            }
        }

        public KNNDistanceResult CalculateDistanceFrom(float[] ReferenceDimensionValues) {

           
            if (ReferenceDimensionValues.Count() != this.DimensionValues.Count) {
                throw new ArgumentException("Cannot calculate distance between 2 points in different dimensions");
            }
            
            double total = 0;
            for (int i = 0; i < this.DimensionValues.Count; i++) {
                total += Math.Pow((this.DimensionValues[i] + ReferenceDimensionValues[i]), 2);
            }

     

            return new KNNDistanceResult(this, Math.Sqrt(total));
        }
    }
}
