﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

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
