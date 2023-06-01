using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NitroEffect : MonoBehaviour
{
    private Rigidbody playerRB;
    public static float nitroValue;
    public static bool boosting = false;
    private bool nitroFlag = false;
    [SerializeField] private Slider nitroSlider;
    [SerializeField] private ParticleSystem nitroSmokeRight;
    [SerializeField] private ParticleSystem nitroSmokeLeft;


    // Start is called before the first frame update
    void Start()
    {
        playerRB = GetComponent<Rigidbody>();
        nitroValue = 250f;
        nitroSmokeLeft.Stop();
        nitroSmokeRight.Stop();
    }

    // Update is called once per frame
    void FixedUpdate()
    {        
        ActivateNitro();
        UpdateNitro();
    }

    private void UpdateNitro()
    {
        nitroSlider.value = nitroValue / 1000f;
    }

    public void ActivateNitro()
    {
        if (!boosting && nitroValue <= 250)
            nitroValue += Time.deltaTime * 10;
        else if (boosting)
            nitroValue -= (nitroValue <= 0) ? 0 : Time.deltaTime * 40;

        if (boosting)
        {
            if (nitroValue > 0)
                startNitroEmitter();
            else
                stopNitroEmitter();
        }
        else
            stopNitroEmitter();
    }

    public void startNitroEmitter()
    {
        if (nitroFlag) return;

        nitroSmokeLeft.Play();
        nitroSmokeRight.Play();

        playerRB.AddForce(transform.forward * 10000);
        playerRB.velocity *= 1.005f;
        nitroFlag = true;
    }

    public void stopNitroEmitter()
    {
        if (!nitroFlag) return;

        nitroSmokeLeft.Stop();
        nitroSmokeRight.Stop();

        nitroFlag = false;
    }
}
