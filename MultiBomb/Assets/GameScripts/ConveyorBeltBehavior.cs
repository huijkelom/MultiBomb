using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBeltBehavior : MonoBehaviour
{
    public bool goingRight = false;
    private Rigidbody2D rb;
    private SpriteRenderer sr;
    public float beltSpeed = .8f;
    public bool newBeltSpawned;
    public GameObject ConveyorBelt;
    private float multiplier;
    public bool multiplied;
    public int arrayIndex = 0;
    private float maxMultiplySpeed = 1.6f;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        newBeltSpawned = false;
        multiplier = 2;
        multiplied = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        MoveBelt();
    }

    //Moves the belt with the given speed
    public void MoveBelt()
    {
        rb.velocity = new Vector2(beltSpeed, 0);
    }

    public void MultiplyBeltSpeed()
    {
        if (!multiplied)
        {
            beltSpeed *= multiplier;
            if(beltSpeed > maxMultiplySpeed)
            {
                beltSpeed = maxMultiplySpeed;
            }
            else if(beltSpeed < maxMultiplySpeed*-1)
            {
                beltSpeed = maxMultiplySpeed*-1;
            }
            multiplied = true;
        }
    }

    //Changes the belt direction and keeps track of the direction of the belt
    public void ChangeBeltDirection(bool newBelt, bool startGoingRight)
    {
        goingRight = startGoingRight;

        if (goingRight)
        {
            beltSpeed = 1f;
        }
        else if (!goingRight)
        {
            beltSpeed = -1f;
        }
        multiplied = false;

        //To make sure belts can keep spawning if the belt direction changes after the belt has been spawned
        if (!newBelt)
        {
            newBeltSpawned = false;
        }
    }
}
