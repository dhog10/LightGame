﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : BaseInteractable {

    public float fireIntensity = 1;
    public float fireRange = 5;
    public float flickerAmount = 0.1f;

    public bool fireOn { get; private set; }

    private GameObject particleObject;
    private ParticleSystem fireParticleSystem;
    private GameObject lightObject;
    private Light fireLight;
    private float fireAlpha = 0;
    private float flicker = 0;
    private float flickerChange = 0;

    private Inventory playerInventory;
    private float fireStartTime;
    public float fuelConsumptionRate;
    public float fuel;

    private PickUpItem playerItem;

    private GameObject player;

    AudioSource campFireSound;

    protected override void Start()
    {
        Transform lightTransform = transform.Find("Light");
        if (lightTransform)
        {
            lightObject = lightTransform.gameObject;
            fireLight = lightObject.GetComponent<Light>();
        }

        Transform particleTransform = transform.Find("Particles");
        if (particleTransform)
        {
            particleObject = particleTransform.gameObject;
            fireParticleSystem = particleObject.GetComponent<ParticleSystem>();
        }

        playerInventory = GameManager.instance.playerObject.GetComponent<Inventory>();
        if (!playerInventory)
        {
            Debug.Log("Could not get player Inventory");
        }

        player = GameManager.instance.playerObject;

        campFireSound = GetComponent<AudioSource>();

        ExtinguishFire();


        base.Start();
    }
    protected override void Update()
    {
        base.Update();

        fuel = Mathf.Lerp(fuel, 0.0f, (Time.time - fireStartTime) * fuelConsumptionRate * Time.deltaTime * 0.01f);

        if(fuel > 0.1)
        {
            fireOn = true;
        }

        if (fireOn)
        {
            fireAlpha = Mathf.Min(1, fireAlpha + (Time.deltaTime * 4));
        }
        else
        {
            fireAlpha = Mathf.Max(0, fireAlpha - Time.deltaTime);
        }

        flickerChange += Random.Range(-flickerAmount, flickerAmount);
        flickerChange = Mathf.Clamp(flickerChange, -flickerAmount, flickerAmount);
        if(flickerChange == flickerAmount || flickerChange == flickerAmount)
        {
            flickerChange = 0;
        }

        flicker = Mathf.Clamp(flicker + flickerChange, -flickerAmount, flickerAmount);

        if (fireLight)
        {

            

            if(fireAlpha > 0)
            {
                fireLight.intensity = fireAlpha + flicker * fuel;
            }
            else
            {
                fireLight.intensity = 0;
                
            }


            fireLight.range = fireRange * fuel;

            if(fuel <= 0.1f)
            {
                ExtinguishFire();
            }
        }
    }

    protected override void OnInteractableStart(GameObject invokerObject)
    {
        base.OnInteractableStart(invokerObject);

        if(invokerObject == null)
        {
            LightFire();
            return;
        }

        if (playerInventory.GetHolding())
        {
            playerItem = player.GetComponentInChildren<PickUpItem>();

            if (fuel >= 0.9f)
            {
                GameManager.instance.Notify("Fire is fully fueled!", 3);
            }
            else
            {
                if (playerItem.itemDefinition == PickUpItem.items.WOODPILE && fuel <= 0.1f)
                {
                    LightFire();
                    playerInventory.DestroyHeldItem();
                }
                else if (playerItem.itemDefinition == PickUpItem.items.WOODPILE && fuel >= 0.1f)
                {
                    playerInventory.DestroyHeldItem();
                    fuel += 0.3f;
                }
            }
        }        
    }

    void LightFire()
    {
        fireOn = true;
        fuel++;
        fireStartTime = Time.time;

        if (fireParticleSystem)
        {
            fireParticleSystem.Play();
        }

        campFireSound.Play();
    }

    void ExtinguishFire()
    {
        fireOn = false;

        if (fireParticleSystem)
        {
            fireParticleSystem.Stop();
        }
        campFireSound.Stop();
    }

    protected override string GetHoverText(GameObject invokerObject)
    {
        if(invokerObject == null) { return ""; }

        Character character = invokerObject.GetComponent<Character>();
        if (!character) { return ""; }

        Inventory inv = invokerObject.GetComponent<Inventory>();
        Transform heldTransform = inv.GetHeldItem();
        GameObject holdingObject = heldTransform ? heldTransform.gameObject : null;
    
        if (!holdingObject) { return ""; }

        PickUpItem pickup = holdingObject.GetComponent<PickUpItem>();
        if(pickup == null) { return "NULL PICKUP"; }

        if (pickup.itemDefinition == PickUpItem.items.FIRE_TORCH) { return "Light Torch"; }
        
        return "";
    }
}
