using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Singletons
{
    [Serializable]
    public class BestLapTimeData
    {
        public List<string> keys;
        public List<float> values;

        public BestLapTimeData(BestlapTimes laptimes)
        {
            keys = laptimes.trackBestLapTimes.Keys.ToList();
            values = laptimes.trackBestLapTimes.Values.ToList();
        }
    }
}
