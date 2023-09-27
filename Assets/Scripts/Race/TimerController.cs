using System.Collections.Generic;
using Assets.Scripts.Menu.MenuSettings;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimerController : MonoBehaviour
{
    private TextMeshProUGUI timerText;
    private Image timerObject;
    [SerializeField] private GameObject timerTextPrefab;
    private TextMeshProUGUI bestLapText;
    private Image bestObject;
    [SerializeField] private GameObject bestLapPrefab;
    private TextMeshProUGUI currentLapText;
    private Image currentObject;
    [SerializeField] private GameObject currentLapPrefab;
    
    
    private float currentTime = 0f;
    private float allTime = 0f;
    private float bestLap = 0f;
    private bool isStarted = false;
    private bool isEnded = false;

    private List<float> lapTimes = new List<float>();
    private RaceMenu raceMenu;

    private void Awake()
    {
        GameObject UIparent = GameObject.FindGameObjectWithTag("UI");

        timerObject = Instantiate(timerTextPrefab, UIparent.transform).GetComponent<Image>();
        timerText = timerObject.GetComponentsInChildren<TextMeshProUGUI>()[1];

        bestObject = Instantiate(bestLapPrefab, UIparent.transform).GetComponent<Image>();
        bestLapText = bestObject.GetComponentsInChildren<TextMeshProUGUI>()[1];

        currentObject = Instantiate(currentLapPrefab, UIparent.transform).GetComponent<Image>();
        currentLapText = currentObject.GetComponentsInChildren<TextMeshProUGUI>()[1];

        timerText.text = "00:00.000";
        bestLapText.text = "00:00.000";
        currentLapText.text = "00:00.000";
    }

    public void setRaceMenu(RaceMenu menu)
    {
        raceMenu = menu;
    }

    private void OnDestroy()
    {
        Destroy(timerObject.gameObject);
        Destroy(bestObject.gameObject);
        Destroy(currentObject.gameObject);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
    void Start() {}

    // Update is called once per frame
    void Update()
    {
        if (isStarted)
        {
            currentTime += Time.deltaTime;
            allTime += Time.deltaTime;
        }
        int allTimeMinute = (int)(allTime / 60f);
        float allTimeSecond = allTime % 60f;

        string allTimeStr = allTimeMinute.ToString("00") + ":" + allTimeSecond.ToString("00.000");

        timerText.text = allTimeStr;

        int currentMinute = (int)(currentTime / 60f);
        float currentSecond = currentTime % 60f;

        string currentTimeStr = currentMinute.ToString("00") + ":" + currentSecond.ToString("00.000");

        currentLapText.text = currentTimeStr;
    }

    public void StartTime()
    {
        isStarted = true;
    }
    public void Restart()
    {
        if (bestLap == 0f || bestLap > currentTime)
            bestLap = currentTime;

        int minute = (int)(bestLap / 60f);
        float second = bestLap % 60f;

        string timeStr = minute.ToString("00") + ":" + second.ToString("00.000");
        bestLapText.text = timeStr;

        lapTimes.Add(currentTime);

        currentTime = 0f;
    }


    public void Stop()
    {
        Restart();
        raceMenu?.FinishRace(lapTimes, bestLap, allTime);
        isStarted = false;
        isEnded = true;
    }
}
