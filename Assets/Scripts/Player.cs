using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

/// <summary>
///  This class describes the behavior of a player object.
/// </summary>
public class Player : MovingBase
{

    public static int scoreCoins;   // The number of collected coins.
    public static float timeGame;   // The game time.

    //  Start is called on the frame when a script is enabled just before any of 
    //  the Update methods is called the first time.
    //  Use this for initialization.
    protected override void Start()
    {
        scoreCoins = 0;
        timeGame = 0;
        base.Start();
    }

    // Update is called every frame.
    private void Update()
    {
        timeGame += Time.deltaTime;

        int horizontal = (int)Input.GetAxisRaw("Horizontal");
        int vertical = (int)Input.GetAxisRaw("Vertical");

        if (isMoving)
            return;

        if (horizontal != 0 || vertical != 0)
            AttemptMove<Zombie>(horizontal, vertical);
    }

    // Output on display information about a player.
    private void OnGUI()
    {
        GUI.Label(new Rect(5, 5, 80, 30), "Coins = " + scoreCoins);
        GUI.Label(new Rect(5, 20, 80, 50), "Time = " + timeGame);
    }

    public void GameOver()
    {
        isMoving = false;
        this.gameObject.SetActive(false);
    }

    protected override void OnCantMove<T>(T component)
    {
        Zombie hitPlayer = component as Zombie;

        // Starts animation Attacking.
        hitPlayer.animator.SetTrigger("Attacking");
        GameOver();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        // If object is Coin.
        if (collider.gameObject.tag == "Coin")
        {
            scoreCoins++;
            GameManager.instance.numberOfCoins--;

            // Delete this object of a game.
            collider.gameObject.SetActive(false);

            SpaceManager.gridPositions.Add(new Vector3(collider.transform.position.x,
                collider.transform.position.y, 0f));
            if (scoreCoins > 30)
            {
                // Add to speed of a zombie.
                GameManager.instance.speedOfZombie += 0.05f * GameManager.instance.speedOfZombie;
            }
        }
    }

    protected override bool AttemptMove<T>(int xDir, int yDir)
    {
        // If a player can do the next step.
        if (base.AttemptMove<T>(xDir, yDir))
        {
            isMoving = true;
            return true;
        }
        else
            return false;
    }

    protected override IEnumerator SmoothMovement(Vector3 end)
    {
        // Calculates squared remaining distance.
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            // Starts animation Moving.
            animator.SetTrigger("Moving");

            // Get new position and move.
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, moveTime * Time.deltaTime);
            rb2D.MovePosition(newPosition);
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;

            // When a player is moving.
            if (sqrRemainingDistance <= float.Epsilon)
            {
                isMoving = false;
            }
        }
    }
}