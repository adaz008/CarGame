using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace Assets.Scripts.CarParts
{
    public class CarInsideUIManager : CarUIManagerBase
    {
        [SerializeField] private GameObject insideSpeedoMeter;
        [SerializeField] private TMP_Text rpmText;

        private void Update()
        {
            ShowInside(UserSettings.Instance.Camera == "Inside");
            if (PauseMenu.GameIsPaused)
            {
                ShowInside(false);
            }
        }

        private void ShowInside(bool value)
        {
            Debug.Log(value);
            insideSpeedoMeter.gameObject.SetActive(value);

        }

        public override void UpdateUI(int KPH)
        {
            base.UpdateUI(KPH);

            float RPM = motor.RPM;

            rpmText.text = ((int)RPM).ToString();
        }

        public override void CheckRedLine()
        {
            base.CheckRedLine();
        }
    }
}
