using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class EndRaceMenu : MonoBehaviour
{
    [Header("LapTexts")]
    [SerializeField] private TextMeshProUGUI[] textItems;
    [Header("LapTimes")]
    [SerializeField] private TextMeshProUGUI[] timeItems;
    [Header("CurrentRace")]
    [SerializeField] private TextMeshProUGUI currentBestlapTextmesh;
    [SerializeField] private TextMeshProUGUI currentOverallTextmesh;
    [Header("AllTime")]
    [SerializeField] private TextMeshProUGUI bestLapTextmesh;
    [SerializeField] private TextMeshProUGUI bestOverallTextmesh;

    [SerializeField] private PauseMenu pauseMenu;

    public void ShowEndScreen(List<float> laptimes, float currentBestlap, float currentOverallTime, string raceTrackName)
    {
        this.gameObject.SetActive(true);
        Time.timeScale = 0f;
        pauseMenu.SetIsEnabled(false);
        pauseMenu.SetGameIsPaused(true);

        BestlapTimes bestTimes = BestlapTimes.Instance;
        string bestLapKey = raceTrackName + "Bestlap";
        string overallKey = raceTrackName + "Overall";

        float bestLap = bestTimes.trackBestLapTimes[bestLapKey];
        float overallTime = bestTimes.trackBestLapTimes[overallKey];

        Debug.Log("BestLap:" + bestLap);
        Debug.Log("Overalltime:" + overallTime);

        currentBestlapTextmesh.text = convertFloatToString(currentBestlap);
        currentOverallTextmesh.text = convertFloatToString(currentOverallTime);

        bestLapTextmesh.text = convertFloatToString(bestLap);
        bestOverallTextmesh.text= convertFloatToString(overallTime);


        for(int i = 0; i < textItems.Length; i++)
        {
            bool isItemActive = i < laptimes.Count;
            textItems[i].enabled = isItemActive;
            timeItems[i].enabled = isItemActive;

            if (isItemActive)
            {
                timeItems[i].text = convertFloatToString(laptimes[i]);
            }
        }
    }

    private string convertFloatToString(float value)
    {
        int allTimeMinute = (int)(value / 60f);
        float allTimeSecond = value % 60f;

        return allTimeMinute.ToString("00") + ":" + allTimeSecond.ToString("00.00");
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        pauseMenu.SetIsEnabled(true);
        pauseMenu.SetGameIsPaused(false);

        this.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            ContinueGame();
        }
    }
}
