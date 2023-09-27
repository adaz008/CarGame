using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TrackCheckPoints : MonoBehaviour
{
    [SerializeField] private int lapCount;
    public int currentLap = 0;
    [SerializeField] GameObject lapTextPrefab;
    private TextMeshProUGUI lapText;

    private List<CheckpointTrigger> checkpointTriggerList;
    private int nextCheckpointSingleIndex;
    private List<CheckpointMissDetector> checkpointMissDetectorList;
    private List<PreCheckpointZoneTrigger> preCheckpointZoneTriggerList;

    private TimerController timerController;
    private CountDownController countDownController;

    private void Awake()
    {
        // TimerController komponens referencia lekérése
        timerController = GameObject.FindWithTag("Timer").GetComponent<TimerController>();

        // Lap szöveg létrehozása és kezdõérték beállítása.
        lapText = Instantiate(lapTextPrefab, GameObject.FindGameObjectWithTag("UI").transform).GetComponent<TextMeshProUGUI>();
        lapText.text = "1/" + lapCount;
        lapText.gameObject.SetActive(false);

        // CheckPointSingle komponensek referencia lekérése és hozzáadása a listához.
        checkpointTriggerList = new List<CheckpointTrigger>();
        foreach (Transform checkpoint in transform.Find("CheckPoints"))
        {
            CheckpointTrigger checkpointSingle = checkpoint.GetComponent<CheckpointTrigger>();
            checkpointSingle.SetTrackCheckpoints(this);
            checkpointTriggerList.Add(checkpointSingle);
        }

        // CheckpointMissSingle komponensek referencia lekérése és hozzáadása a listához.
        checkpointMissDetectorList = new List<CheckpointMissDetector>();
        foreach (Transform checkpointMiss in transform.Find("CheckpointsMiss"))
        {
            CheckpointMissDetector checkpointmissSingle = checkpointMiss.GetComponent<CheckpointMissDetector>();
            checkpointmissSingle.SetTrackCheckpoints(this);
            checkpointMissDetectorList.Add(checkpointmissSingle);
        }

        // MissedCheckpointResetTrigger komponensek referencia lekérése és hozzáadása a listához.
        preCheckpointZoneTriggerList = new List<PreCheckpointZoneTrigger>();
        foreach (Transform checkpointMissReset in transform.Find("CheckpointsMissReset"))
        {
            PreCheckpointZoneTrigger checkpointmissSingle = checkpointMissReset.GetComponent<PreCheckpointZoneTrigger>();
            checkpointmissSingle.SetTrackCheckpoints(this);
            preCheckpointZoneTriggerList.Add(checkpointmissSingle);
        }

        // Kör és ellenõrzõpont indexek inicializálása.
        currentLap = 0;
        nextCheckpointSingleIndex = 0;
    }

    private void OnDestroy()
    {
        Destroy(lapText.gameObject);
    }


    private void Start()
    {
        // Az összes CheckPointSingle és CheckpointMissSingle inaktívvá tétele.
        foreach (CheckpointTrigger single in checkpointTriggerList)
          single.Disable();
        foreach (CheckpointMissDetector single in checkpointMissDetectorList)
            single.Disable();
        foreach (PreCheckpointZoneTrigger single in preCheckpointZoneTriggerList)
            single.Disable();

        lapText.gameObject.SetActive(true);
    }

    public void PlayerThroughCheckpoint(CheckpointTrigger checkPointSingle)
    {
        // Ellenõrizzük, hogy a játékos az aktuális ellenõrzõpontot érte-e el.
        if (checkpointTriggerList.IndexOf(checkPointSingle) == nextCheckpointSingleIndex)
        {
            checkPointSingle.Disable();

            // A jelenlegi CheckpointMissSingle inaktívvá tétele.
            checkpointMissDetectorList[nextCheckpointSingleIndex].Disable();
            // A jelenlegi ResetTrigger inaktívvá tétele.
            preCheckpointZoneTriggerList[nextCheckpointSingleIndex].Disable();

            if (nextCheckpointSingleIndex == 0)
            {
                // Ha az elsõ ellenõrzõponthoz értünk, akkor ellenõrizzük, hogy hány kört tettünk meg.
                switch (currentLap)
                {
                    case 0:
                        // Még egy kört se tettünk meg, ez az elsõ áthaladás
                        currentLap++;
                        break;
                    case int lap when lap < lapCount:
                        // Ha még nem teljesítettük az összes kört, akkor növeljük a kört,
                        // frissítjük a lap szöveget, újraindítjuk az idõzítõt.
                        currentLap++;
                        lapText.text = currentLap.ToString() + "/" + lapCount.ToString();
                        timerController.Restart();

                        break;
                    default:
                        // Ha teljesítettük az összes kört, akkor leállítjuk az idõzítõt.
                        timerController.Stop();
                        timerController.Destroy();
                        countDownController.Destroy();
                        Destroy(this.gameObject);
                        break;
                }
            }

            // A következõ ellenõrzõpont indexének frissítése.
            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % checkpointTriggerList.Count;

            if (countDownController == null)
            {
                GameObject trackParent = GameObject.FindGameObjectWithTag("TrackParent");
                countDownController = trackParent.GetComponentInChildren<CountDownController>();
            }

            countDownController.StopMissedCounter();

            // A következõ CheckpointSingle aktiválása
            checkpointTriggerList[nextCheckpointSingleIndex].Enable();
            //CheckpointMissSingle aktiválása.
            checkpointMissDetectorList[nextCheckpointSingleIndex].Enable(); ;

            // ResetTrigger aktiválása
            preCheckpointZoneTriggerList[nextCheckpointSingleIndex].Enable();
        }
    }

    public void EnableFirstCheckpoint(CheckpointTrigger checkPointSingle)
    {
        if (checkpointTriggerList.IndexOf(checkPointSingle) == 0)
            checkPointSingle.Enable();
    }

    public void EnableFirstCheckpointMiss(CheckpointMissDetector checkpointMiss)
    {
        if (checkpointMissDetectorList.IndexOf(checkpointMiss) == 0)
            checkpointMiss.Enable();
    }

    public void EnableFirstCheckpointMissReset(PreCheckpointZoneTrigger resetTrigger)
    {
        if (preCheckpointZoneTriggerList.IndexOf(resetTrigger) == 0)
            resetTrigger.Enable();
    }
}
