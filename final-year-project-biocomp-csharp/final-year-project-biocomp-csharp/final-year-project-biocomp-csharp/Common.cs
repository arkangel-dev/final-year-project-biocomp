using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp {
    public class Common {
        public static Random GlobalRandom = new Random();

        public static float ActivationFunction(float value) {
            //return 1.0f / (1.0f + (float)Math.Exp(-value));
            return value > 0 ? value : 0.0f;
        }

        public static float GetRandomNumber(double minimum, double maximum) {
            return (float)(Common.GlobalRandom.NextDouble() * (maximum - minimum) + minimum);
        }
    }
}