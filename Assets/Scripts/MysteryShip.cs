using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MysteryShip : MonoBehaviour
{
    public GameObject mysteryShip;
    public float mysteryStartTimeDelay;
    public float mysteryContinuousTimeDelay;

    public float counter;

    // Start is called before the first frame update
    void Start()
    {
        counter = 0;
        InvokeRepeating(nameof(InstantiateMysteryShip), mysteryStartTimeDelay, mysteryContinuousTimeDelay);
    }

    // Update is called once per frame
    void Update()
    {
        counter += Time.deltaTime;
    }

    void InstantiateMysteryShip()
    {
        Instantiate(mysteryShip, transform.position, Quaternion.identity);
    }
}
