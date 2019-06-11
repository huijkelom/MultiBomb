using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Startup : MonoBehaviour
{
    public int AmountOfRows;
    public GameObject ConveyorButton;
    public GameObject Bomb;
    public GameObject ConveyorBelt;

    /*public Sprite LeftTurnButton;
    public Sprite RightTurnButton;
    public Sprite SpeedUpLeftButton;
    public Sprite SpeedUpRightButton;*/

    public Sprite TurnButton;
    public Sprite SpeedUpButton;

    public GameObject[] bombs;
    public GameObject[] buttons;
    public GameObject[] conveyors;

    //With this you can set the X position where the buttons spawn
    public float buttonXposOffset;
    //This value is used to determine where the first row of buttons spawn
    public float buttonYStartPos;

    private float conveyorBelt1StartXPos;
    private float conveyorBelt2StartXPos;
    private float spawnXPositionConveyorLeft;
    private float spawnXPositionConveyorRight;

    // Start is called before the first frame update
    void Start()
    {
        if(AmountOfRows >= 4)
        {
            AmountOfRows = 4;
        }
        else if(AmountOfRows <= 0)
        {
            AmountOfRows = 1;
        }
        
        CreateRows();
        bombs = GameObject.FindGameObjectsWithTag("Bomb");
        //conveyors = new List<GameObject>();
        conveyors = GameObject.FindGameObjectsWithTag("ConveyorBelt");

        conveyorBelt1StartXPos = -1.25f;
        conveyorBelt2StartXPos = 4.145f;
        spawnXPositionConveyorLeft = 7.16f;
        spawnXPositionConveyorRight = -7.16f;
    }

    // Update is called once per frame
    void Update()
    {
        //foreach(GameObject conveyor in conveyors)
        List<GameObject> conveyorsToDelete = new List<GameObject>();
        for (int i = 0; i < conveyors.Length; i++)
        {
            ConveyorBeltBehavior cbb = conveyors[i].GetComponent<ConveyorBeltBehavior>();
            cbb.arrayIndex = i;
            if (cbb.goingRight)
            {   
                //Spawn a new conveyor to go right
                if (!cbb.newBeltSpawned && conveyors[i].transform.position.x >= -1f && conveyors[i].transform.position.x < -0.5f)
                {
                    float xPos = -0.85f - conveyors[i].transform.position.x;
                    GameObject newConveyor = Instantiate(ConveyorBelt, new Vector2(spawnXPositionConveyorRight - xPos, conveyors[i].transform.position.y), Quaternion.identity);
                    //newConveyor.GetComponent<ConveyorBeltBehavior>().ChangeBeltDirection(true);

                    BombBehavior bb = GetBombOfRow(conveyors[i].transform.position.y).GetComponent<BombBehavior>();
                    newConveyor.GetComponent<ConveyorBeltBehavior>().ChangeBeltDirection(true, bb.goingRight);
                    newConveyor.GetComponent<ConveyorBeltBehavior>().beltSpeed = bb.bombSpeed;

                    if (bb.lastChangedBombMovement == BombMovement.SpedUp)
                    {
                        //newConveyor.GetComponent<ConveyorBeltBehavior>().MultiplyBeltSpeed();
                        newConveyor.GetComponent<ConveyorBeltBehavior>().multiplied = true;
                    }
                    //conveyors[i].GetComponent<ConveyorBeltBehavior>().ChangeBeltDirection(false);
                    conveyors = AddObjectToArray(conveyors, newConveyor);
                    cbb.newBeltSpawned = true;
                    //CreateNewConveyor(xPos, i, cbb);
                }
            }
            else
            {
                //Spawn a new conveyor to go left
                if (!cbb.newBeltSpawned && conveyors[i].transform.position.x <= 1f && conveyors[i].transform.position.x > 0.5f)
                {
                    float xPos = 0.85f - conveyors[i].transform.position.x;
                    GameObject newConveyor = Instantiate(ConveyorBelt, new Vector2(spawnXPositionConveyorLeft - xPos, conveyors[i].transform.position.y), Quaternion.identity);
                    BombBehavior bb = GetBombOfRow(conveyors[i].transform.position.y).GetComponent<BombBehavior>();
                    newConveyor.GetComponent<ConveyorBeltBehavior>().ChangeBeltDirection(true, bb.goingRight);
                    newConveyor.GetComponent<ConveyorBeltBehavior>().beltSpeed = bb.bombSpeed;
                    if (bb.lastChangedBombMovement == BombMovement.SpedUp)
                    {
                        //newConveyor.GetComponent<ConveyorBeltBehavior>().MultiplyBeltSpeed();
                        newConveyor.GetComponent<ConveyorBeltBehavior>().multiplied = true;
                    }
                    //conveyors[i].GetComponent<ConveyorBeltBehavior>().ChangeBeltDirection(false);
                    conveyors = AddObjectToArray(conveyors, newConveyor);
                    cbb.newBeltSpawned = true;
                    //CreateNewConveyor(xPos, i, cbb);
                }
            }

            //Collect all conveyors to set inactive after the loop
            if (conveyors[i].transform.position.x >= 8f || conveyors[i].transform.position.x <= -8f)
            {
                conveyorsToDelete.Add(conveyors[i]);
            }
        }

        foreach(GameObject conveyor in conveyorsToDelete)
        {
            conveyor.SetActive(false);
        }
        conveyorsToDelete.Clear();
    }

    private void CreateRows()
    {
        //float xOffset = Screen.width / 2 / 100 * 80;
        for (int i = 0; i < AmountOfRows; i++)
        {
            //float yPosition = Screen.height / 2 / 100 * (70 -(20*i));

            //Spawning a row with two buttons and a bomb
            Vector2 buttonPosLeft = new Vector2(buttonXposOffset * -1, buttonYStartPos);
            //Vector2 buttonPosLeft = new Vector2(xOffset * -1, buttonYStartPos);
            GameObject buttonLeft = Instantiate(ConveyorButton, buttonPosLeft, Quaternion.identity);
            //buttonLeft.transform.localPosition = new Vector2((xOffset * -1), yPosition);
            

            Vector2 buttonPosRight = new Vector2(buttonXposOffset, buttonYStartPos);
            //Vector2 buttonPosRight = new Vector2(xOffset, buttonYStartPos);
            GameObject buttonRight = Instantiate(ConveyorButton, buttonPosRight, Quaternion.identity);
            //buttonRight.transform.localPosition = new Vector2(xOffset, yPosition);

            //Vector2 bombPos = new Vector2(0, buttonYStartPos);
            Vector2 bombPos = new Vector2(0, buttonYStartPos);
            GameObject bomb = Instantiate(Bomb, bombPos, Quaternion.identity);

            //Vector2 conveyor1Pos = new Vector2(conveyorBelt1StartXPos, buttonYStartPos);
            //GameObject conveyor1 = Instantiate(ConveyorBelt, conveyor1Pos, Quaternion.identity);
            //conveyor1.transform.position = new Vector2(conveyorBelt1StartXPos, buttonYStartPos);
            //conveyor1.transform.position.x = conveyorBelt1StartXPos;
            GameObject conveyor1 = Instantiate(ConveyorBelt, new Vector2(-1.35f, buttonYStartPos), Quaternion.identity);
            //conveyors.Add(conveyor1);
            GameObject conveyor2 = Instantiate(ConveyorBelt, new Vector2(4.965f, buttonYStartPos), Quaternion.identity);
            //conveyors.Add(conveyor2);

            /*Vector2 conveyor2Pos = new Vector2(conveyorBelt2StartXPos, buttonYStartPos);
            GameObject conveyor2 = Instantiate(ConveyorBelt, new Vector2(conveyorBelt2StartXPos, buttonYStartPos), Quaternion.identity);*/
            //conveyor2.transform.position = new Vector2(conveyorBelt2StartXPos, buttonYStartPos);

            //With this every even numbered bombs will start to go left and the other bombs go right 
            if (i % 2 == 0)
            {
                //Setting up the buttons (and bomb) for the situation of the bomb going right
                bomb.GetComponent<BombBehavior>().ChangeBombMovement(false);
                buttonRight.GetComponent<SpriteRenderer>().sprite = SpeedUpButton;
                buttonRight.GetComponent<ButtonBehavior>().buttonState = ButtonState.TurnAroundPressed; 
                buttonLeft.GetComponent<SpriteRenderer>().sprite = TurnButton;
            }
            else
            {
                //Setting up the buttons for the situation of the bomb going left
                buttonLeft.GetComponent<SpriteRenderer>().sprite = SpeedUpButton;
                buttonLeft.GetComponent<ButtonBehavior>().buttonState = ButtonState.TurnAroundPressed;
                buttonLeft.transform.Rotate(0, 180, 0);

                buttonRight.GetComponent<SpriteRenderer>().sprite = TurnButton;
                buttonRight.transform.Rotate(0, 180, 0);

                //BombBehavior bombScript = bomb.GetComponent<BombBehavior>();
                conveyor1.GetComponent<ConveyorBeltBehavior>().ChangeBeltDirection(true, true/*, bombScript.bombSpeed*/);
                conveyor2.GetComponent<ConveyorBeltBehavior>().ChangeBeltDirection(true, true/*, bombScript.bombSpeed*/);
            }

            buttons = GameObject.FindGameObjectsWithTag("Button");

            //Setting the y position for the next row
            buttonYStartPos -= 2f;
        }
    }

    public void ButtonClicked(Vector3 pos)
    {
        //Here we check on which row the bomb is
        GameObject bomb = null;
        for(int i = 0; i < (AmountOfRows); i++)
        {
            if(bombs[i].transform.position.y == pos.y)
            {
                bomb = bombs[i];
            }
        }

        //Here is the check which of the two buttons in the row was checked
        bool leftButton = true;
        if(pos.x >= 0)
        {
            leftButton = false;
        }

        //By giving the correct button, this function will figure out what movement will occur from the button press
        if(bomb != null)
        {
            BombBehavior bombBehavior = bomb.GetComponent<BombBehavior>();
            ChangeButtonRow(bomb.transform.position, bombBehavior.ChangeBombMovement(leftButton), bombBehavior.bombSpeed);
        }
    }

    //With this function we can make use of the information gotten from ChangeBombMovement to select the two buttons in the same bomb's row
    public void ChangeButtonRow(Vector3 bombPos, BombMovement bombMovement, float bombSpeed)
    {
        int leftButtonNumber = -1;
        int rightButtonNumber = -1;
        for(int i = 0; i < buttons.Length; i++)
        {
            if(buttons[i].transform.position.y == bombPos.y)
            {
                if (buttons[i].transform.position.x > 0)
                {
                    rightButtonNumber = i;
                }
                else
                {
                    leftButtonNumber = i;
                }
            }
        }
        
        if(leftButtonNumber == -1 || rightButtonNumber == -1)
        {
            return;
        }

        if(bombMovement != BombMovement.SpedUp)
        {
            ChangeButtonSprites(leftButtonNumber, rightButtonNumber, bombMovement);
            BombBehavior bombBehavior = GetBombOfRow(bombPos.y).GetComponent<BombBehavior>();
            ChangeConveyorDirection(bombPos.y, bombBehavior.goingRight);
        }
        else
        {
            SpeedUpConveyors(bombPos.y);
        }
    }

    //This function is responsible for making sure the button sprites stay correct after the bomb changes its route
    private void ChangeButtonSprites(int leftButtonNumber, int rightButtonNumber, BombMovement bombMovement)
    {
        if (bombMovement == BombMovement.ChangedRight)
        {
            //Right
            buttons[rightButtonNumber].GetComponent<SpriteRenderer>().sprite = TurnButton;
            buttons[rightButtonNumber].GetComponent<ButtonBehavior>().ResetButtonState(false);


            //Left
            buttons[leftButtonNumber].GetComponent<SpriteRenderer>().sprite = SpeedUpButton;
            
            buttons[leftButtonNumber].GetComponent<ButtonBehavior>().buttonState = ButtonState.TurnAroundPressed;
        }
        else
        {
            //Right
            buttons[rightButtonNumber].GetComponent<SpriteRenderer>().sprite = SpeedUpButton;
            buttons[rightButtonNumber].GetComponent<ButtonBehavior>().buttonState = ButtonState.TurnAroundPressed;

            //Left
            buttons[leftButtonNumber].GetComponent<SpriteRenderer>().sprite = TurnButton;
            //Rotate turn button
            buttons[leftButtonNumber].GetComponent<ButtonBehavior>().ResetButtonState(true);
        }

        buttons[rightButtonNumber].transform.Rotate(0, 180, 0);
        buttons[leftButtonNumber].transform.Rotate(0, 180, 0);
    }

    //Determine which conveyors need to be switched in direction
    private void ChangeConveyorDirection(float yPos, bool goRight)
    {
        foreach(GameObject conveyor in conveyors)
        {
            if(conveyor.transform.position.y == yPos)
            {
                conveyor.GetComponent<ConveyorBeltBehavior>().ChangeBeltDirection(false, goRight);
            }
        }
    }

    private void SpeedUpConveyors(float yPos)
    {
        foreach (GameObject conveyor in conveyors)
        {
            if (conveyor.transform.position.y == yPos)
            {
                conveyor.GetComponent<ConveyorBeltBehavior>().MultiplyBeltSpeed();
            }
        }
    }

    //Function to add an already existing object to an array of GameObjects
    private GameObject[] AddObjectToArray(GameObject[] array, GameObject newObject)
    {
        int arrayLength = array.Length + 1;
        GameObject[] newArray = new GameObject[arrayLength];
        for(int i = 0; i < arrayLength; i++)
        {
            if(i != array.Length)
            {
                newArray[i] = array[i];
            }
            else
            {
                newArray[i] = newObject;
            }
        }
        return newArray;
    }

    //Function to remove an object by index from an array of GameObjects
    private GameObject[] RemoveObjectFromArray(GameObject[] array, int indexToRemove)
    {
        int arrayLength = array.Length - 1;
        GameObject[] newArray = new GameObject[arrayLength];
        for (int i = 0; i < array.Length; i++)
        {
            if (i != indexToRemove)
            {
                //To prevent putting something in an out of range index
                if(indexToRemove < i)
                {
                    newArray[i - 1] = array[i];
                }
                else
                {
                    newArray[i] = array[i];
                }
            }
        }
        return newArray;
    }

    private GameObject GetBombOfRow(float yPos)
    {
        GameObject correctBomb = null;
        for(int i = 0; i < bombs.Length; i++)
        {
            if(bombs[i].transform.position.y == yPos)
            {
                correctBomb = bombs[i];
            }
        }
        return correctBomb;
    }

    public void DisableButtons()
    {
        for(int i = 0; i < buttons.Length; i++)
        {
            buttons[i].SetActive(false);
        }
    }

    public void DisableBombs()
    {
        for(int i = 0; i < bombs.Length; i++)
        {
            bombs[i].SetActive(false);
        }
    }

    public void DisableConveyors()
    {
        for(int i = 0; i < conveyors.Length; i++)
        {
            if (conveyors[i].activeSelf)
            {
                conveyors[i].SetActive(false);
            }
        }
    }
}
