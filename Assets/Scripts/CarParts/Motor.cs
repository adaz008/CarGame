using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

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
		private const float MAGNITUDE_TO_KMH = 3.6f;
		private const float TORQUE_CONVERSION_FACTOR = 5252f;
		private const float IDLE_RPM = 800f;
		private const float TIRE_DIAMETER = 25f;
		private const float INCHES_PER_KM_TO_RPM = 210f;


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
		[SerializeField] private float axleRatio;
		[SerializeField] private float changeGearTime;


		#region Properties
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
			RPM = IDLE_RPM;
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
					RPM = Mathf.Lerp(RPM, Mathf.Max(IDLE_RPM, redLineEnd * gasInput) + Random.Range(-50, 50), Time.deltaTime);
				}
				else
				{
					//Motor fordulatszáma
					RPM = (playerRB.velocity.magnitude * MAGNITUDE_TO_KMH * gearRatios[currentGear] * INCHES_PER_KM_TO_RPM * axleRatio) / TIRE_DIAMETER;
					RPM = RPM > redLineEnd ? (redLineEnd + Random.Range(-100, 100)) : RPM;
					//Nyomaték newtonméterben

					//torque = hpToRPMCurve.Evaluate(RPM / redLineEnd) * motorPower * RPM / TORQUE_CONVERSION_FACTOR * clutch;
					torque = (hpToRPMCurve.Evaluate(RPM / redLineEnd) * motorPower / RPM) * gearRatios[currentGear] * axleRatio * TORQUE_CONVERSION_FACTOR * clutch;

					if (RPM > redLineEnd)
						torque = 0f;
				}
			}
			return torque;
		}

		public void EngineBrake(ref Rigidbody playerRB, float gasInput)
		{
			if ((gasInput == 0 &&
			!float.IsNaN(playerRB.velocity.x) &&
			!float.IsNaN(playerRB.velocity.y) &&
			!float.IsNaN(playerRB.velocity.z) &&
			playerRB.velocity.magnitude != 0f)
			||
			(UserSettings.Instance.Transmission == "Manual") &&
			RPM > RedLineEnd)
			{
				float speed = playerRB.velocity.magnitude * 3.6f;
				float newSpeed = playerRB.velocity.magnitude * 3.6f - 0.295f;

				if (speed > 120f)
					newSpeed = playerRB.velocity.magnitude * 3.6f - 0.32f;
				else if (speed > 180f)
					newSpeed = playerRB.velocity.magnitude * 3.6f - 0.35f;

				float ratio = Mathf.Pow(newSpeed, 2) / Mathf.Pow(speed, 2);

				playerRB.velocity *= ratio < 1f ? ratio : 1f;
			}

			//Ha benyomjuk a kuplungot akkor 0 egyébként az idő függvényében váltózik 0 és 1 között
			//Ahogyan csusztatnánk
			if (gearState != GearState.Changing)
			{
				if (gearState == GearState.Neutral)
				{
					clutch = 0;
					if (gasInput > 0)
						gearState = GearState.Running;
					else if (gasInput < 0)
						gearState = GearState.Reverse;
				}
				else
					clutch = Mathf.Lerp(clutch, 1, Time.deltaTime);
			}
			else
				clutch = 0;
		}

		public void CheckForGearChange(float gasInput)
		{
			//Ha megáll, üresbe kerül
			if (RPM < IDLE_RPM + 200 && gasInput == 0 && currentGear == 0)
				gearState = GearState.Neutral;

			//Ha előre menetből egyből tolatni kezd
			if (RPM < IDLE_RPM + 200 && gasInput < 0 && currentGear == 0)
				gearState = GearState.Reverse;

			//Ha tolatásból egyből előre megy
			if (RPM < IDLE_RPM + 200 && gasInput > 0 && currentGear == 0)
				gearState = GearState.Running;


			//Ha jár a motor és a kuplung ki van nyomva
			if (gearState == GearState.Running && clutch > 0 && UserSettings.Instance.Transmission == "Auto")
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
