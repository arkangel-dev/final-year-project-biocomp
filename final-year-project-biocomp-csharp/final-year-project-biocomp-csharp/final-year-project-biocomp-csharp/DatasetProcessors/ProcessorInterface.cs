using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp.DatasetProcessors {
    interface ProcessorInterface {
        void Train(List<ProcessableStruct> TrainingData, int iterations = 0);
        float Test(List<ProcessableStruct> TestingData);
    }
}
