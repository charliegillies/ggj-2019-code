using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Campfire : MonoBehaviour
{
    // Charlie was here! 7.08 am, 27.01.2019.
    public static event System.Action GameOver;

    // Adjustable settings
    [SerializeField]
    private float maxTime = 15.0f;
    [SerializeField]
    private float maxCampFireIntensity = 4.0f;
    [SerializeField]
    private float maxSpotAngle = 140.0f;
    [SerializeField]
    private CharacterController player;
    [SerializeField]
    private float fireRecoveryTime = 8.0f;
    [SerializeField]
    private float fireSlowdown = 200.0f;
    [SerializeField]
    private GameObject fireButtonPrompt;
    [SerializeField]
    private ParticleSystem fireParticle;
    
    private bool isGameOver = false;
   
    // Components, timers.
    private float timer = 0.0f;
    private Light campFireLight;
    private bool fireLit = false;   // has the player lit the fire recently
    private float fireRecoveryTimer = 0.0f; // Time since the player last lit the fire

    void Start()
    {
        campFireLight = GetComponent<Light>();
        campFireLight.intensity = maxCampFireIntensity;
        campFireLight.spotAngle = maxSpotAngle;
    }

    // Update is called once per frame
    void Update()
    {
        float fireParticleScale = 1 * (campFireLight.intensity / maxCampFireIntensity);
        fireParticle.startSize = fireParticleScale;
        // Scale intensity;
        // Figure out the ratio of the current time vs the max time.
        float lightIntensity = (maxCampFireIntensity * (timer / maxTime) / fireSlowdown);
        float currentSpotAngle = (maxSpotAngle * (timer / maxTime) / fireSlowdown);
        // Fire slow down affects how fast the fire dwindles and 

        if (fireLit)
        {
            fireRecoveryTimer += Time.deltaTime;
            if(fireRecoveryTimer > fireRecoveryTime)
            {
                fireRecoveryTimer = 0.0f;
                fireLit = false;
            }
            timer += Time.deltaTime;
            campFireLight.intensity += lightIntensity;
            campFireLight.spotAngle += currentSpotAngle;
        }
        else
        {
            timer += Time.deltaTime;
            campFireLight.intensity -= lightIntensity;
            campFireLight.spotAngle -= currentSpotAngle;
        }


        if(timer < 0)
        {
            timer = 0;
        }

        if(campFireLight.intensity == 0 && !isGameOver)
        {
            isGameOver = true;
            OnGameOver();
        }
        
        if(campFireLight.intensity > maxCampFireIntensity)
        {
            campFireLight.intensity = maxCampFireIntensity;
        }

        if(campFireLight.spotAngle > maxSpotAngle)
        {
           campFireLight.spotAngle = maxSpotAngle;
        }

    }

    void feed()
    {
        if(player.isCarryingStick)
        {
            player.isCarryingStick = false;
            fireLit = true;
            timer -= fireRecoveryTime;
        }
    }


    private void OnTriggerStay(Collider other)
    {
        // Stu:
        // I figured out a small bug here earlier.
        // There are no collision layers setup in the project, so
        // the 'campfire' is colliding with anything with a box. 
        // Be careful of this while editing colliders.

        // While the player is in the trigger - and is carrying a stick
        // show our 'a' button prompt to tell them to put it in the fire
        fireButtonPrompt.SetActive(player.isCarryingStick);

        //Debug.Log("Trigger happened. Carrying stick: " + player.isCarryingStick);
        if(Input.GetButtonDown("Fire1"))
        {
            if (campFireLight.intensity < maxCampFireIntensity - 0.10)
            {
                feed();
            }
        }
    }

    // Hi Stu
    // Just invoke this method to end the game scene.
    [ContextMenu("Fake Game Over")]
    public void OnGameOver() {
        GameOver?.Invoke();
    }
}
