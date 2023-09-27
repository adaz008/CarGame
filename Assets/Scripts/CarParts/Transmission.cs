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
            //Ellen�rizz�k, hogy t�rt�nik-e v�lt�s
            gearState = GearState.CheckingChange;
            if (currentGear + gearChange >= 0)
            {
                if (gearChange > 0)
                {
                    //N�velni szeretn�nk a fokozatot
                    //V�runk egy picit hogy t�nyleg kell-e v�ltani
                    if (currentGear == 1)
                        yield return new WaitForSeconds(0.4f);
                    else
                        yield return new WaitForSeconds(0.05f);
                    //Ha a fordulatsz�m kisebb mint a felfele v�lt�s fordulatsz�ma vagy el�rt�k a maxim�lis sebess�gi fokozatot akkor nem v�ltunk
                    if (RPM < increaseGearRPM || currentGear >= gearRatios.Length - 1)
                    {
                        gearState = GearState.Running;
                        yield break;
                    }
                }
                else
                {
                    //Cs�kkenteni szeretn�nk a fokozatot
                    //V�runk egy picit hogy t�nyleg kell-e v�ltani
                    yield return new WaitForSeconds(0.1f);

                    //Ha a fordulatsz�m nagyobb mint a lefele v�lt�s fordulatsz�ma vagy nem tudunk m�r lefele v�ltani(�resben vagyunk)
                    if (RPM > decreaseGearRPM || currentGear <= 0)
                    {
                        gearState = GearState.Running;
                        yield break;
                    }
                }
                //V�lt�s t�rt�nik
                gearState = GearState.Changing;
                yield return new WaitForSeconds(changeGearTime);
                //V�ltottunk
                currentGear += gearChange;
            }
            if (gearState != GearState.Neutral)
                gearState = GearState.Running;
        }
        else
        {
            gearState = GearState.Changing;
            yield return new WaitForSeconds(changeGearTime);
            //V�ltottunk
            currentGear += gearChange;
            gearState = GearState.Running;
            carmovement.SetIsChanged(!carmovement.IsChanged);
        }
    }
}
