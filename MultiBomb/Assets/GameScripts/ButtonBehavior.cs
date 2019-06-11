using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ButtonState { Unpressed, TurnAroundPressed, SpeedUpPressed };

public class ButtonBehavior : Interactable
{
    private Startup startup;
   
    public ButtonState buttonState;

    // Start is called before the first frame update
    void Start()
    {
        buttonState = ButtonState.Unpressed;
        startup = FindObjectOfType<Startup>();
    }

    protected override void Click(Vector3 clickposition)
    {
        startup.ButtonClicked(transform.position);
    }

    //Resets the ButtonState of the button to the state SpeedUpPressed 
    //so that the next time the buttonstate gets updated will switch back to Unpressed
    public void ResetButtonState(bool leftButton)
    {
        buttonState = ButtonState.SpeedUpPressed;
    }
}
