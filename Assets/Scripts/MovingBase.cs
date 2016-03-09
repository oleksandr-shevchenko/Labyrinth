using UnityEngine;
using System.Collections;

public abstract class MovingBase : MonoBehaviour
{
    public float moveTime;
    public LayerMask blockingLayer;
    public BoxCollider2D boxCollider;
    public Rigidbody2D rb2D;
    public Animator animator;
    public bool isMoving;   //

    //  Start is called on the frame when a script is enabled just before any of 
    //  the Update methods is called the first time.
    //  Use this for initialization.
    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        rb2D = GetComponent<Rigidbody2D>();
        moveTime = 3f;
        animator = GetComponent<Animator>();
        isMoving = false;
    }

    // This method 
    protected virtual bool AttemptMove<T>(int xDirection, int yDirection)
        where T : Component
    {
        RaycastHit2D hit;
        bool canMove = Move(xDirection, yDirection, out hit);

        // Check if can take a next step.
        if (hit.transform == null)
        {
            return true;
        }

        T hitComponent = hit.transform.GetComponent<T>();

        // If can't move and met other object T.
        if (!canMove && hitComponent != null)
        {
            OnCantMove(hitComponent);
        }

        return false;
    }

    protected bool Move(int xDirection, int yDirection, out RaycastHit2D hit)
    {
        Vector2 start = transform.position;
        Vector2 end = start + new Vector2(xDirection, yDirection);

        boxCollider.enabled = false;
        hit = Physics2D.Linecast(start, end, blockingLayer);
        boxCollider.enabled = true;

        // Check if can take a next step.
        if (hit.transform == null)
        {
            // Do it.
            StartCoroutine(SmoothMovement(end));
            return true;
        }

        return false;
    }

    // This method make smooth moving of zombie from a first position to a last position.
    protected abstract IEnumerator SmoothMovement(Vector3 end);

    // This method will call if a object met other object.
    protected abstract void OnCantMove<T>(T component)
        where T : Component;
}
