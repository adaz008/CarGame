using System.Collections;
using UnityEngine;

public enum GearState
{
    Neutral,
    Running,
    CheckingChange,
    Changing,
    Reverse
};

public class Transmission : MonoBehaviour
{
    private GearState gearState;
    [SerializeField] private float clutch;
    [SerializeField] private int currentGear;

    [SerializeField] private float[] gearRatios;
    [SerializeField] private float differentialRatio;

    [SerializeField] private float changeGearTime;
    [SerializeField] Motor motor;
    [SerializeField] CarMovement carmovement;

    public GearState GearState => gearState;
    public void SetGearState(GearState value) { gearState = value; }
    public int CurrentGear => currentGear;
    public void SetCurrentGear(int value) { currentGear = value; }

    public float[] GearRatios => gearRatios;
    public void SetGearRatios(float[] value) { gearRatios = value; }
    public float DifferentialRatio => differentialRatio;
    public void SetDifferentialRatio(float value) { differentialRatio = value; }

    public float Clutch => clutch;
    public void SetClutch(float value) { clutch = value; }


    public void StartGearChangeCoroutine(int direction)
    {
        StartCoroutine(ChangeGear(direction));
    }

    IEnumerator ChangeGear(int gearChange)
    {
        float increaseGearRPM = motor.IncreaseGearRPM;
        float decreaseGearRPM = motor.DecreaseGearRPM;
        float RPM = motor.GetRPM();
        if (UserSettings.Instance.Transmission == "Auto")
        {
            //Ellenõrizzük, hogy történik-e váltás
            gearState = GearState.CheckingChange;
            if (currentGear + gearChange >= 0)
            {
                if (gearChange > 0)
                {
                    //Növelni szeretnénk a fokozatot
                    //Várunk egy picit hogy tényleg kell-e váltani
                    if (currentGear == 1)
                        yield return new WaitForSeconds(0.4f);
                    else
                        yield return new WaitForSeconds(0.05f);
                    //Ha a fordulatszám kisebb mint a felfele váltás fordulatszáma vagy elértük a maximális sebességi fokozatot akkor nem váltunk
                    if (RPM < increaseGearRPM || currentGear >= gearRatios.Length - 1)
                    {
                        gearState = GearState.Running;
                        yield break;
                    }
                }
                else
                {
                    //Csökkenteni szeretnénk a fokozatot
                    //Várunk egy picit hogy tényleg kell-e váltani
                    yield return new WaitForSeconds(0.1f);

                    //Ha a fordulatszám nagyobb mint a lefele váltás fordulatszáma vagy nem tudunk már lefele váltani(üresben vagyunk)
                    if (RPM > decreaseGearRPM || currentGear <= 0)
                    {
                        gearState = GearState.Running;
                        yield break;
                    }
                }
                //Váltás történik
                gearState = GearState.Changing;
                yield return new WaitForSeconds(changeGearTime);
                //Váltottunk
                currentGear += gearChange;
            }
            if (gearState != GearState.Neutral)
                gearState = GearState.Running;
        }
        else
        {
            gearState = GearState.Changing;
            yield return new WaitForSeconds(changeGearTime);
            //Váltottunk
            currentGear += gearChange;
            gearState = GearState.Running;
            carmovement.SetIsChanged(!carmovement.IsChanged);
        }
    }
}
