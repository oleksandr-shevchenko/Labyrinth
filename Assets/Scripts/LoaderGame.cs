using UnityEngine;
using System.Collections;

/// <summary>
///  This class downloads a game.
/// </summary>
public class LoaderGame : MonoBehaviour
{
    public GameObject gameManager;  // GameManager prefab to instantiate.   

    // Awake is always called before any Start functions.
    private void Awake()
    {
        //  Check if a GameManager has already been assigned to static variable GameManager.instance .
        //  or if it's still null.
        if (GameManager.instance == null)
        {
            //Instantiate gameManager prefab.
            Instantiate(gameManager);
        }
    }
}
