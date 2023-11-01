using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.CarParts
{
    public class CarUIManagerBase : MonoBehaviour
    {
        [SerializeField] private TMP_Text speedText;
        [SerializeField] private TMP_Text gearText;
        [SerializeField] private Image needleCircle;

        [SerializeField] private Color Limit;
        [SerializeField] private Color Default;

        protected Motor motor;

        private void Start()
        {
            motor = gameObject.GetComponent<Motor>();
        }

        public virtual void UpdateUI(int KPH)
        {
            GearState gearState = motor.gearState;
            int currentGear = motor.currentGear;

            speedText.text = KPH.ToString();

            if (gearState == GearState.Neutral)
                gearText.text = "N";
            else if (gearState == GearState.Reverse)
                gearText.text = "R";
            else
                gearText.text = (currentGear + 1).ToString();
        }

        public virtual void CheckRedLine()
        {
            float RPM = motor.RPM;
            float redLineStart = motor.RedLineStart;
            if (RPM > redLineStart)
            {
                needleCircle.color = Limit;
                gearText.color = Limit;
            }
            else
            {
                needleCircle.color = Default;
                gearText.color = Default;
            }
        }
    }
}
