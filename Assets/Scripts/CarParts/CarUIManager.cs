using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarUIManager : MonoBehaviour
{

    [Header("UI")]
    [SerializeField] private TMP_Text speedText;
    [SerializeField] private TMP_Text gearText;
    [SerializeField] private Transform rpmNeedle;
    [SerializeField] private float minRPMNeedleRotation;
    [SerializeField] private float maxRPMNeedleRotation;
    [SerializeField] private Image needleCircle;

    [Header("Brake lamp")]
    [SerializeField] private Material brakeMaterial;
    [SerializeField] private Color brakingColor;
    [SerializeField] private float brakeColorIntensity;

    [SerializeField] Transmission transmission;
    [SerializeField] Motor motor;


    public void UpdateUI(int KPH)
    {
        GearState gearState = transmission.GearState;
        int currentGear = transmission.CurrentGear;
        float RPM = motor.GetRPM();
        float redLineEnd = motor.RedLineEnd;

        rpmNeedle.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(minRPMNeedleRotation, maxRPMNeedleRotation, RPM / redLineEnd));

        speedText.text = KPH.ToString();

        if (gearState == GearState.Neutral)
            gearText.text = "N";
        else if (gearState == GearState.Reverse)
            gearText.text = "R";
        else
            gearText.text = (currentGear + 1).ToString();
    }

    public void CheckRedLine()
    {
        float RPM = motor.GetRPM();
        float redLineStart = motor.RedLineStart;
        if (RPM > redLineStart)
        {
            needleCircle.color = Color.red;
            gearText.color = Color.red;
        }
        else
        {
            needleCircle.color = Color.white;
            gearText.color = Color.white;
        }
    }

    public void BrakeLampChange(bool handBrake, float brakeInput)
    {
        if (!handBrake && brakeMaterial)
        {
            if (brakeInput > 0)
            {
                brakeMaterial.EnableKeyword("_EMISSION");
                brakeMaterial.SetColor("_EmissionColor", brakingColor * Mathf.Pow(2f, brakeColorIntensity));
            }
            else
            {
                brakeMaterial.DisableKeyword("_EMISSION");
                brakeMaterial.SetColor("_EmissionColor", Color.black);
            }
        }
    }
}
