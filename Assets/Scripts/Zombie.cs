using UnityEngine;
using System.Collections;
using OtherScripts;

/// <summary>
///  This class describes the behavior of a zombie object.
/// </summary>
public class Zombie : MovingBase
{
    private Transform _target;   // A position of a enemy.

    //  Start is called on the frame when a script is enabled just before any of 
    //  the Update methods is called the first time.
    //  Use this for initialization.
    protected override void Start()
    {
        GameManager.instance.AddZombieToList(this);
        _target = GameObject.FindGameObjectWithTag("Player").transform;
        base.Start();
    }

    public void NextStep()
    {
        // If can't move.
        if (isMoving)
            return;

        int xDirection = 0;
        int yDirection = 0;

        // If number of coins is smaller than this value.
        if (Player.scoreCoins < GameManager.instance.minCoinsToTurnOnSmartMovingZombie)
        {
            int randomIndex = Random.Range(1, 4);
            // choose random index and set a new direction.
            switch (randomIndex)
            {
                case 1:
                    xDirection = -1;
                    yDirection = 0;
                    break;
                case 2:
                    xDirection = 1;
                    yDirection = 0;
                    break;
                case 3:
                    xDirection = 0;
                    yDirection = -1;
                    break;
                case 4:
                    xDirection = 0;
                    yDirection = 1;
                    break;
            }
        }
        else
        {
            Dijkstra d = new Dijkstra();
            // Calculate the shortest path and set the optimal direction
            Point p = d.GetNextStep(new Point((int)_target.position.x, (int)_target.position.y, 0), new Point((int)transform.position.x, (int)transform.position.y, 0), SpaceManager.matrixBoard);
            // Set the best direction.
            xDirection = p.x - (int)transform.position.x;
            yDirection = p.y - (int)transform.position.y;
        }
        AttemptMove<Player>(xDirection, yDirection);
    }

    protected override bool AttemptMove<T>(int xDir, int yDir)
    {
        if (base.AttemptMove<T>(xDir, yDir))
        {
            isMoving = true;
            return true;
        }
        return false;
    }

    protected override void OnCantMove<T>(T component)
    {
        Player hitPlayer = component as Player;

        // Starts animation Attacking
        animator.SetTrigger("Attacking");
        hitPlayer.GameOver();
    }

    protected override IEnumerator SmoothMovement(Vector3 end)
    {
        float sqrRemainingDistance = (transform.position - end).sqrMagnitude;

        while (sqrRemainingDistance > float.Epsilon)
        {
            // Starts animation Moving
            animator.SetTrigger("Moving");

            // Set new position.
            Vector3 newPosition = Vector3.MoveTowards(rb2D.position, end, moveTime * Time.deltaTime);

            // Move the rigidbody to new position.
            rb2D.MovePosition(newPosition);

            // Calculate distance to last position.
            sqrRemainingDistance = (transform.position - end).sqrMagnitude;
            yield return null;

            if (sqrRemainingDistance <= float.Epsilon)
            {
                isMoving = false;
            }
        }
    }
}
