using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BombMovement
{
    SpedUp,
    ChangedRight,
    ChangedLeft
}

public class BombBehavior : MonoBehaviour
{
    public float bombSpeed = .8f;
    float speedDoublingAmount = 1.6f;
    public bool goingRight = true;
    public BombMovement lastChangedBombMovement;
    bool speedUp = false;

    private Rigidbody2D rb;
    private ScoreAndTimer scoreAndTimer;
    private Startup startup;
    private bool exploding;

    private SpriteRenderer sr;
    public Sprite goldBomb;
    public Sprite standardBomb;
    public float randomValue;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        scoreAndTimer = FindObjectOfType<ScoreAndTimer>();
        startup = FindObjectOfType<Startup>();
        anim = GetComponent<Animator>();

        sr = GetComponent<SpriteRenderer>();
        randomValue = 0;
        lastChangedBombMovement = BombMovement.ChangedRight;
        exploding = false;

    }

    void Update()
    {
        //The way to check if the game is over or if the bomb should be moving
        if(!scoreAndTimer.gameOver && !exploding)
        {
            MoveBomb();
        }
        else
        {
            rb.velocity = Vector3.zero;
            //Resetting the bombs position and sprite after exploding
            if (exploding && !anim.GetBool("explode"))
            {
                exploding = false;
                transform.position = new Vector2(0, transform.position.y);
                if (scoreAndTimer.Team1Score.GetComponent<Animator>().GetBool("ScoreGained"))
                {
                    scoreAndTimer.Team1Score.GetComponent<Animator>().SetBool("ScoreGained", false);
                }
                if (scoreAndTimer.Team2Score.GetComponent<Animator>().GetBool("ScoreGained"))
                {
                    scoreAndTimer.Team2Score.GetComponent<Animator>().SetBool("ScoreGained", false);
                }
            }
        }
    }

    //Move the bomb with the bombSpeed on x axis
    public void MoveBomb()
    {
        rb.velocity = new Vector2(bombSpeed, 0);
    }

    /// <summary>
    /// Changes the bomb's direction
    /// </summary>
    /// <param name="leftButton">to see if the button on the left or the right was clicked</param>
    /// <returns>the current bomb position as boolean goingRight</returns>
    /// <returns>a boolean to see if the bomb was sped up or changed in direction</returns>
    public BombMovement ChangeBombMovement(bool leftButton)
    {
        //Table:
        //LeftButton    True        True        False       False
        //GoingRight    True        False       True        False
        //Result        Speed up    Turn around Turn around Speed up
        lastChangedBombMovement = BombMovement.ChangedRight;
        if (leftButton != goingRight)
        {
            goingRight = !goingRight;
            if (speedUp)
            {
                bombSpeed /= speedDoublingAmount;
                speedUp = false;
            }
            bombSpeed *= -1;

            if (goingRight)
            {
                lastChangedBombMovement = BombMovement.ChangedRight;
            }
            else if (!goingRight)
            {
                lastChangedBombMovement = BombMovement.ChangedLeft;
            }
        }
        else if(leftButton == goingRight)
        {
            if (!speedUp)
            {
                bombSpeed *= speedDoublingAmount;
                speedUp = true;
            }
            lastChangedBombMovement = BombMovement.SpedUp;
        }
        //return goingRight;
        return lastChangedBombMovement;
    }

    //Everything that happens when the bomb reaches a player
    public void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Button")
        {
            anim.SetBool("explode", true);
            exploding = true;
            //Blow up animation to be added
            //Audio or sound addition
            int score = 1;
            if(sr.sprite == goldBomb)
            {
                score = 3;
                scoreAndTimer.amountGoldBombs--;
            }

            if (goingRight)
            {
                scoreAndTimer.team1Score += score;
                if (!scoreAndTimer.Team1Score.GetComponent<Animator>().GetBool("ScoreGained"))
                {
                    scoreAndTimer.Team1Score.GetComponent<Animator>().SetBool("ScoreGained", true);
                }
                
            }
            else
            {
                scoreAndTimer.team2Score += score;
                if (!scoreAndTimer.Team2Score.GetComponent<Animator>().GetBool("ScoreGained"))
                {
                    scoreAndTimer.Team2Score.GetComponent<Animator>().SetBool("ScoreGained", true);
                }
            }
            
            //The parameter that is given to changeBombMovement has to result in the bomb Turning around
            //This means that the new direction should always be the opposite of what boolean goingRight currently is
            BombMovement bombMovement = ChangeBombMovement(!goingRight);

            //To compensate for startup not being able to call this function after ChangeBombMovement in this scenario
            startup.ChangeButtonRow(transform.position, bombMovement, bombSpeed);
            

            if (Random.Range(0, 3) == 0 && scoreAndTimer.amountGoldBombs < scoreAndTimer.maxGoldBombsAllowed)
            {
                sr.sprite = goldBomb;
                anim.SetBool("goldBomb", true);
                scoreAndTimer.amountGoldBombs++;
            }
            else
            {
                anim.SetBool("goldBomb", false);
                sr.sprite = standardBomb;
            }
        }
    }
}
