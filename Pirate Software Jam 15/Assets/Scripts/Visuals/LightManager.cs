using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class LightManager : MonoBehaviour
{
    public static LightManager instance;

    public GameObject player;
    public float decaySpeed;
    public float startRadius;
    public float damageRate = 1.5f;
    public float tickDamage = 1;
    private float delay;
    private Light2D lamp;

    void Awake()
    {
        instance = this;

        lamp = player.GetComponent<Light2D>();
    }

    public void AddLight(float amount)
    {
        lamp.pointLightOuterRadius += amount;
        //lamp.pointLightInnerRadius += amount;
    }

    void Update()
    {
        delay -= Time.deltaTime;

        //lamp.pointLightInnerRadius -= Time.deltaTime * decaySpeed;
        lamp.pointLightOuterRadius -= Time.deltaTime * decaySpeed;

        if (lamp.pointLightOuterRadius <= 0 && delay <= 0)
        {
            int direction = 0;
            while (direction == 0)
            {
                float random = Random.Range(-1,2);
                direction = (int)Mathf.Round(random);
            }


            player.GetComponent<HealthComponent>().TakeDamage(tickDamage, new Vector2(direction, 0));

            delay = damageRate;
        }
    }
}
