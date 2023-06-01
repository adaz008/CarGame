using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CountDownController : MonoBehaviour
{
    [SerializeField] private Material lightMaterial;
    [SerializeField] private int countDownTime;
    private TextMeshProUGUI countDownText;
    [SerializeField] private GameObject countDownPrefab;
    private TextMeshProUGUI missedCheckpointCountDownText;
    [SerializeField] private GameObject missedCheckpointCountPrefab;
    private TextMeshProUGUI missedCheckpointText;
    [SerializeField] private GameObject missedCheckpointTextPrefab;
    private TimerController timerController;

    //private bool isMissedCounterRunning = false;
    private Coroutine missedCheckpointCoroutine;

    void Awake() {
        GameObject UIparent = GameObject.FindGameObjectWithTag("UI");

        countDownText = Instantiate(countDownPrefab, UIparent.transform).GetComponent<TextMeshProUGUI>();

        timerController = GameObject.FindWithTag("Timer").GetComponent<TimerController>();
    }

    private void OnDestroy()
    {
        if (countDownText)
            Destroy(countDownText.gameObject);
        if (missedCheckpointCountDownText)
            Destroy(missedCheckpointCountDownText.gameObject);
        if (missedCheckpointText)
            Destroy(missedCheckpointText.gameObject);
    }

    public void Destroy()
    {
        Destroy(this.gameObject);
    }

    public void StartRace()
    {
        StartCoroutine(CountDownToStart());
    }

    public void StartMissedCounter (){
        missedCheckpointCoroutine = StartCoroutine(MissedCheckpointToStart());
    }

    public void StopMissedCounter() {
        if (missedCheckpointCoroutine != null)
        {
            StopCoroutine(missedCheckpointCoroutine);

            if (missedCheckpointCountDownText)
                Destroy(missedCheckpointCountDownText.gameObject);
            if (missedCheckpointText)
                Destroy(missedCheckpointText.gameObject);
        }
    }

    IEnumerator CountDownToStart()
    {
        while (countDownTime > 0)
        {
            lightMaterial.EnableKeyword("_EMISSION");
            if (countDownTime == 3 ||countDownTime == 2)
                lightMaterial.SetColor("_EmissionColor", Color.red * Mathf.Pow(2f, 4));
            else if (countDownTime == 1)
                lightMaterial.SetColor("_EmissionColor", Color.yellow * Mathf.Pow(2f, 2));

            countDownText.text = countDownTime.ToString();
            yield return new WaitForSeconds(1f);
            countDownTime--;
        }

        GameObject.FindWithTag("Car").GetComponent<CarMovement>().SetRaceStarting(false);

        lightMaterial.SetColor("_EmissionColor", Color.green * Mathf.Pow(2f, 1));
        countDownText.text = "GO!";

        //Start Race
        if (timerController != null)
            timerController.StartTime();

        yield return new WaitForSeconds(1f);

        Destroy(countDownText.gameObject);

        lightMaterial.SetColor("_EmissionColor", Color.black * Mathf.Pow(2f, 1));
    }

    IEnumerator MissedCheckpointToStart()
    {
        GameObject UIparent = GameObject.FindGameObjectWithTag("UI");

        CarMovement player = GameObject.FindGameObjectWithTag("Car").GetComponent<CarMovement>();

        missedCheckpointCountDownText = Instantiate(missedCheckpointCountPrefab, UIparent.transform).GetComponent<TextMeshProUGUI>();
        missedCheckpointText = Instantiate(missedCheckpointTextPrefab, UIparent.transform).GetComponent<TextMeshProUGUI>();

        countDownTime = 3;

        while (countDownTime > 0)
        {
            missedCheckpointCountDownText.text = countDownTime.ToString();
            yield return new WaitForSeconds(1f);
            countDownTime--;
        }

        player.MissedCheckpointReset(CheckpointMiss.Reset);

        TrackCheckPoints track = GameObject.FindGameObjectWithTag("Track").GetComponent<TrackCheckPoints>();

        Destroy(missedCheckpointText.gameObject);
        Destroy(missedCheckpointCountDownText.gameObject);
    }
}
