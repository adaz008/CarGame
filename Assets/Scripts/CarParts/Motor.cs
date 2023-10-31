using System.Collections;
using UnityEngine;

namespace Assets.Scripts.CarParts
{
    public enum GearState
    {
        Neutral,
        Running,
        CheckingChange,
        Changing,
        Reverse
    };

    public class Motor : MonoBehaviour
    {
        [Header("Motor settings")]
        [SerializeField] private float motorPower;
        [SerializeField] private float brakePower;
        [SerializeField] private AnimationCurve hpToRPMCurve;
        [SerializeField] private float redLineStart;
        [SerializeField] private float redLineEnd;
        [SerializeField] private float increaseGearRPM;
        [SerializeField] private float decreaseGearRPM;

        [Header("Transmission settings")]
        [SerializeField] private float[] gearRatios;
        [SerializeField] private float differentialRatio;
        [SerializeField] private float changeGearTime;


        private readonly float idleRPM = 800f;
        private readonly float tireDiameter = 25f;

        #region Getters
        public float RPM { get; set; }
        public GearState gearState { get; set; }
        public int currentGear { get; set; }
        public float clutch { get; set; }
        public bool isChanged { get; set; } = false;
        public int isEngineRunning { get; set; } = 0;
        public float BrakePower => brakePower;
        public float RedLineEnd => redLineEnd;
        public float RedLineStart => redLineStart;
        public float[] GearRatios => gearRatios;
        #endregion

        public void setRPMToIdle()
        {
            RPM = idleRPM;
        }

        public float CalculateTorque(Rigidbody playerRB, float gasInput)
        {
            float torque = 0;

            CheckForGearChange(gasInput);

            //Ha jár a motor
            if (isEngineRunning > 1)
            {
                //Ha a kuplung be van nyomva
                if (clutch == 0f)
                {
                    //Random azért kell, hogy tiltásnál ugráljon a mutató picit
                    //Illetve nézzük, hogy az alapjárat vagy az adott fordulat a nagyobb és afelé közeledik a mutató
                    RPM = Mathf.Lerp(RPM, Mathf.Max(idleRPM, redLineEnd * gasInput) + Random.Range(-50, 50), Time.deltaTime);
                }
                else
                {
                    //Motor fordulatszáma
                    //playerRB.velocity.magnitude * 2.25f = mérföld/órában a sebesség
                    //gearRatios[currentGear] =  adott fokozat áttétele
                    //336f konstans
                    //tireDiameter = kerek magassag

                    RPM = (playerRB.velocity.magnitude * 2.25f * gearRatios[currentGear] * 336f * differentialRatio) / tireDiameter;
                    RPM = RPM > redLineEnd ? (redLineEnd + Random.Range(-100, 100)) : RPM;
                    //Nyomaték newtonméterben
                    torque = (hpToRPMCurve.Evaluate(RPM / redLineEnd) * motorPower / RPM) * gearRatios[currentGear] * differentialRatio * 5252f * clutch;
                    if (RPM > redLineEnd)
                        torque = 0f;
                }
            }
            return torque;
        }

        public void CheckForGearChange(float gasInput)
        {
            //Ha megáll, üresbe kerül
            if (RPM < idleRPM + 200 && gasInput == 0 && currentGear == 0)
                gearState = GearState.Neutral;

            //Ha előre menetből egyből tolatni kezd
            if (RPM < idleRPM + 200 && gasInput < 0 && currentGear == 0)
                gearState = GearState.Reverse;

            //Ha tolatásból egyből előre megy
            if (RPM < idleRPM + 200 && gasInput > 0 && currentGear == 0)
                gearState = GearState.Running;


            //Ha jár a motor és a kuplung ki van nyomva
            if (gearState == GearState.Running && clutch> 0 && UserSettings.Instance.Transmission == "Auto")
            {
                if (RPM > increaseGearRPM)
                    StartGearChangeCoroutine(1);
                else if (RPM < decreaseGearRPM)
                    StartGearChangeCoroutine(-1);
            }
        }

        public void StartGearChangeCoroutine(int direction)
        {
            StartCoroutine(ChangeGear(direction));
        }

        IEnumerator ChangeGear(int gearChange)
        {
            if (UserSettings.Instance.Transmission == "Auto")
            {
                //Ellenőrizzük, hogy történik-e váltás
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
                isChanged = !isChanged;
            }
        }

    }
}
