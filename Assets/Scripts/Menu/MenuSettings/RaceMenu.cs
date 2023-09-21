using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Menu.MenuSettings
{
    public class RaceMenu : MonoBehaviour
    {
        [Header("Prefabs")]
        [SerializeField] private GameObject timerPrefab;
        [SerializeField] private GameObject countDownPrefab;

        [Header("StartRace")]
        [SerializeField] private Image startRace;
        [SerializeField] private Image endRace;
        [SerializeField] private Button PauseMenuBack;
        private bool isRace = false;

        private GameObject timerGameObject;
        private GameObject trackGameObject;
        private GameObject countDownGameObject;

        public void Start() { }

        public void StartRace(GameObject trackPrefab)
        {
            isRace = true;
            startRace.gameObject.SetActive(false);
            endRace.gameObject.SetActive(true);

            InitializeRace(trackPrefab);
        }

        private void InitializeRace(GameObject trackPrefab)
        {
            GameObject UIparent = GameObject.FindGameObjectWithTag("UI");
            GameObject Trackparent = GameObject.FindGameObjectWithTag("TrackParent");

            GameObject Car = GameObject.FindGameObjectWithTag("Car");

            timerGameObject = Instantiate(timerPrefab, Trackparent.transform);
            trackGameObject = Instantiate(trackPrefab);

            GameObject gridline = trackGameObject.GetComponentInChildren<Transform>().Find("prop_gridline").gameObject;

            countDownGameObject = Instantiate(countDownPrefab, Trackparent.transform);

            Car.GetComponent<CarMovement>().StartRacePos(gridline.transform.position);

            countDownGameObject.GetComponent<CountDownController>().StartRace();
        }

        public void EndRace()
        {
            isRace = false;
            startRace.gameObject.SetActive(true);
            endRace.gameObject.SetActive(false);

            DestroyRaceElements();

            PauseMenuBack.onClick.Invoke();
        }

        private void DestroyRaceElements()
        {
            Destroy(timerGameObject);
            Destroy(trackGameObject);
            Destroy(countDownGameObject);
        }

        public void RaceChangerSelected(GameObject raceSelectorUI)
        {
            if (isRace)
                EndRace();
            else
                raceSelectorUI.SetActive(true);
        }
    }
}
