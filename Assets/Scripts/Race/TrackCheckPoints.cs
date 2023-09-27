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
        // TimerController komponens referencia lek�r�se
        timerController = GameObject.FindWithTag("Timer").GetComponent<TimerController>();

        // Lap sz�veg l�trehoz�sa �s kezd��rt�k be�ll�t�sa.
        lapText = Instantiate(lapTextPrefab, GameObject.FindGameObjectWithTag("UI").transform).GetComponent<TextMeshProUGUI>();
        lapText.text = "1/" + lapCount;
        lapText.gameObject.SetActive(false);

        // CheckPointSingle komponensek referencia lek�r�se �s hozz�ad�sa a list�hoz.
        checkpointTriggerList = new List<CheckpointTrigger>();
        foreach (Transform checkpoint in transform.Find("CheckPoints"))
        {
            CheckpointTrigger checkpointSingle = checkpoint.GetComponent<CheckpointTrigger>();
            checkpointSingle.SetTrackCheckpoints(this);
            checkpointTriggerList.Add(checkpointSingle);
        }

        // CheckpointMissSingle komponensek referencia lek�r�se �s hozz�ad�sa a list�hoz.
        checkpointMissDetectorList = new List<CheckpointMissDetector>();
        foreach (Transform checkpointMiss in transform.Find("CheckpointsMiss"))
        {
            CheckpointMissDetector checkpointmissSingle = checkpointMiss.GetComponent<CheckpointMissDetector>();
            checkpointmissSingle.SetTrackCheckpoints(this);
            checkpointMissDetectorList.Add(checkpointmissSingle);
        }

        // MissedCheckpointResetTrigger komponensek referencia lek�r�se �s hozz�ad�sa a list�hoz.
        preCheckpointZoneTriggerList = new List<PreCheckpointZoneTrigger>();
        foreach (Transform checkpointMissReset in transform.Find("CheckpointsMissReset"))
        {
            PreCheckpointZoneTrigger checkpointmissSingle = checkpointMissReset.GetComponent<PreCheckpointZoneTrigger>();
            checkpointmissSingle.SetTrackCheckpoints(this);
            preCheckpointZoneTriggerList.Add(checkpointmissSingle);
        }

        // K�r �s ellen�rz�pont indexek inicializ�l�sa.
        currentLap = 0;
        nextCheckpointSingleIndex = 0;
    }

    private void OnDestroy()
    {
        Destroy(lapText.gameObject);
    }


    private void Start()
    {
        // Az �sszes CheckPointSingle �s CheckpointMissSingle inakt�vv� t�tele.
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
        // Ellen�rizz�k, hogy a j�t�kos az aktu�lis ellen�rz�pontot �rte-e el.
        if (checkpointTriggerList.IndexOf(checkPointSingle) == nextCheckpointSingleIndex)
        {
            checkPointSingle.Disable();

            // A jelenlegi CheckpointMissSingle inakt�vv� t�tele.
            checkpointMissDetectorList[nextCheckpointSingleIndex].Disable();
            // A jelenlegi ResetTrigger inakt�vv� t�tele.
            preCheckpointZoneTriggerList[nextCheckpointSingleIndex].Disable();

            if (nextCheckpointSingleIndex == 0)
            {
                // Ha az els� ellen�rz�ponthoz �rt�nk, akkor ellen�rizz�k, hogy h�ny k�rt tett�nk meg.
                switch (currentLap)
                {
                    case 0:
                        // M�g egy k�rt se tett�nk meg, ez az els� �thalad�s
                        currentLap++;
                        break;
                    case int lap when lap < lapCount:
                        // Ha m�g nem teljes�tett�k az �sszes k�rt, akkor n�velj�k a k�rt,
                        // friss�tj�k a lap sz�veget, �jraind�tjuk az id�z�t�t.
                        currentLap++;
                        lapText.text = currentLap.ToString() + "/" + lapCount.ToString();
                        timerController.Restart();

                        break;
                    default:
                        // Ha teljes�tett�k az �sszes k�rt, akkor le�ll�tjuk az id�z�t�t.
                        timerController.Stop();
                        timerController.Destroy();
                        countDownController.Destroy();
                        Destroy(this.gameObject);
                        break;
                }
            }

            // A k�vetkez� ellen�rz�pont index�nek friss�t�se.
            nextCheckpointSingleIndex = (nextCheckpointSingleIndex + 1) % checkpointTriggerList.Count;

            if (countDownController == null)
            {
                GameObject trackParent = GameObject.FindGameObjectWithTag("TrackParent");
                countDownController = trackParent.GetComponentInChildren<CountDownController>();
            }

            countDownController.StopMissedCounter();

            // A k�vetkez� CheckpointSingle aktiv�l�sa
            checkpointTriggerList[nextCheckpointSingleIndex].Enable();
            //CheckpointMissSingle aktiv�l�sa.
            checkpointMissDetectorList[nextCheckpointSingleIndex].Enable(); ;

            // ResetTrigger aktiv�l�sa
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
