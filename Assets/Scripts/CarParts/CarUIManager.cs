using Assets.Scripts.CarParts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CarUIManager : CarUIManagerBase
{

    [Header("UI")]
    [SerializeField] private Transform rpmNeedle;
    [SerializeField] private float minRPMNeedleRotation;
    [SerializeField] private float maxRPMNeedleRotation;

    [Header("Brake lamp")]
    [SerializeField] private Material brakeMaterial;
    [SerializeField] private Color brakingColor;
    [SerializeField] private float brakeColorIntensity;

    public override void UpdateUI(int KPH)
    {
        base.UpdateUI(KPH);
        float RPM = motor.RPM;
        float redLineEnd = motor.RedLineEnd;

        rpmNeedle.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(minRPMNeedleRotation, maxRPMNeedleRotation, RPM / redLineEnd));
    }

    public override void CheckRedLine()
    {
        base.CheckRedLine();
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
