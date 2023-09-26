using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Singletons;
using UnityEngine;

public class BestlapTimes : MonoBehaviour
{
    public static BestlapTimes Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        BestLapTimeData data = SaveSystem.LoadData<BestLapTimeData>("trackTimes.json");

        if (data != null)
        {
            trackBestLapTimes = constructDictionary(data);
        }
        else
        {
            trackBestLapTimes = new Dictionary<string, float>();
        }


        //trackBestLapTimes.Add("RaceTrack1Bestlap", 55.3f);
        //trackBestLapTimes.Add("RaceTrack1Overall", 205.3f);
    }

    private Dictionary<string, float> constructDictionary(BestLapTimeData data)
    {
        Dictionary<string, float> dict = new Dictionary<string, float>();

        for (int i = 0; i < data.keys.Count; i++)
        {
            dict.Add(data.keys[i], data.values[i]); 
        }
        return dict;
    }

    public Dictionary<string, float> trackBestLapTimes { get; set; }
}