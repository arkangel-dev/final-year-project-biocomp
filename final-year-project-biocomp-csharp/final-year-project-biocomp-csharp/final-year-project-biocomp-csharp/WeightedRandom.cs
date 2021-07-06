using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace final_year_project_biocomp_csharp {
    class WeightedRandom<T> {
        private struct Entry {
            public double accumulatedWeight;
            public T item;
        }

        private List<Entry> entries = new List<Entry>();
        private double accumulatedWeight;

        public void AddEntry(T item, double weight) {
            accumulatedWeight += weight;
            entries.Add(new Entry { item = item, accumulatedWeight = accumulatedWeight });
        }

        public T GetRandom() {
            double r = Common.GlobalRandom.NextDouble() * accumulatedWeight;

            foreach (Entry entry in entries) {
                if (entry.accumulatedWeight >= r) {
                    return entry.item;
                }
            }
            return default(T); //should only happen when there are no entries
        }
    }
}