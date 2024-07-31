using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBoss : MonoBehaviour
{
    public HealthComponent hp;

    // Update is called once per frame
    void Update()
    {
        if (hp.health <= 0 || gameObject == null)
        {
            LevelLoader.instance.MainMenu();
        }
    }
}
