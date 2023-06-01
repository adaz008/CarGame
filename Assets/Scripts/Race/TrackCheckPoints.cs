using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.EventSystems;

public class TrackCheckPoints : MonoBehaviour
{
    [SerializeField] private int lapCount;
    public int currentLap = 0;
    [SerializeField] GameObject lapTextPrefab;
    private TextMeshProUGUI lapText;

    private List<CheckPointSingleTrigger> checkpointSingleList;
    private int nextCheckpointSingleIndex;
    private List<CheckpointMissSingleTrigger> checkpointmissSingleList;
    private List<MissedCheckpointResetTrigger> checkpointmissResetList;

    [SerializeField] private GameObject[] enable;
    [SerializeField] private GameObject[] disable;

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
        checkpointSingleList = new List<CheckPointSingleTrigger>();
        foreach (Transform checkpoint in transform.Find("CheckPoints"))
        {
            CheckPointSingleTrigger checkpointSingle = checkpoint.GetComponent<CheckPointSingleTrigger>();
            checkpointSingle.SetTrackCheckpoints(this);
            checkpointSingleList.Add(checkpointSingle);
        }

        // CheckpointMissSingle komponensek referencia lekérése és hozzáadása a listához.
        checkpointmissSingleList = new List<CheckpointMissSingleTrigger>();
        foreach (Transform checkpointMiss in transform.Find("CheckpointsMiss"))
        {
            CheckpointMissSingleTrigger checkpointmissSingle = checkpointMiss.GetComponent<CheckpointMissSingleTrigger>();
            checkpointmissSingle.SetTrackCheckpoints(this);
            checkpointmissSingleList.Add(checkpointmissSingle);
        }

        // MissedCheckpointResetTrigger komponensek referencia lekérése és hozzáadása a listához.
        checkpointmissResetList = new List<MissedCheckpointResetTrigger>();
        foreach (Transform checkpointMissReset in transform.Find("CheckpointsMissReset"))
        {
            MissedCheckpointResetTrigger checkpointmissSingle = checkpointMissReset.GetComponent<MissedCheckpointResetTrigger>();
            checkpointmissSingle.SetTrackCheckpoints(this);
            checkpointmissResetList.Add(checkpointmissSingle);
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
        foreach (CheckPointSingleTrigger single in checkpointSingleList)
          single.Disable();
        foreach (CheckpointMissSingleTrigger single in checkpointmissSingleList)
            single.Disable();
        foreach (MissedCheckpointResetTrigger single in checkpointmissResetList)
            single.Disable();

        lapText.gameObject.SetActive(true);
    }

    public void PlayerThroughCheckpoint(CheckPointSingleTrigger checkPointSingle)
    {
        // Ellenõrizzük, hogy a játékos az aktuális ellenõrzõpontot érte-e el.
        if (checkpointSingleList.IndexOf(checkPointSingle) == nextCheckpointSingleIndex)
        {
            checkPointSingle.Disable();

            // A jelenlegi CheckpointMissSingle inaktívvá tétele.
            checkpointmissSingleList[nextCheckpointSingleIndex].Disable();
            // A jelenlegi ResetTrigger inaktívvá tétele.
            checkpointmissResetList[nextCheckpointSingleIndex].Disable();

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

                        if(currentLap == lapCount)
                        {
                            foreach (GameObject obj in enable)
                            {
                                    obj?.SetActive(true);
                            }

                            foreach (GameObject obj in disable)
                            {
                                    obj?.SetActive(false);
                            }
                        }

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
            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % checkpointSingleList.Count;

            if (countDownController == null)
            {
                GameObject trackParent = GameObject.FindGameObjectWithTag("TrackParent");
                countDownController = trackParent.GetComponentInChildren<CountDownController>();
            }

            countDownController.StopMissedCounter();

            // A következõ CheckpointSingle aktiválása
            checkpointSingleList[nextCheckpointSingleIndex].Enable();
            //CheckpointMissSingle aktiválása.
            checkpointmissSingleList[nextCheckpointSingleIndex].Enable(); ;

            // ResetTrigger aktiválása
            checkpointmissResetList[nextCheckpointSingleIndex].Enable();
        }
    }

    public void EnableFirstCheckpoint(CheckPointSingleTrigger checkPointSingle)
    {
        if (checkpointSingleList.IndexOf(checkPointSingle) == 0)
            checkPointSingle.Enable();
    }

    public void EnableFirstCheckpointMiss(CheckpointMissSingleTrigger checkpointMiss)
    {
        if (checkpointmissSingleList.IndexOf(checkpointMiss) == 0)
            checkpointMiss.Enable();
    }

    public void EnableFirstCheckpointMissReset(MissedCheckpointResetTrigger resetTrigger)
    {
        if (checkpointmissResetList.IndexOf(resetTrigger) == 0)
            resetTrigger.Enable();
    }
}
