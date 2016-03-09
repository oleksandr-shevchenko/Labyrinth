using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/// <summary>
///  This class controls gameplay.
/// </summary>
public class GameManager : MonoBehaviour
{

    public static GameManager instance = null;    // Static instance of GameManager which allows it 
                                                  // to be accessed by any other script.
    public float speedOfZombie = 1f;
    public int minCoinsToTurnOnSmartMovingZombie = 30;
    public int minCoinsToAddNewZombie = 10;
    public int numberOfCoins;  //The current number of coins in the game.
    private SpaceManager _spaceScript;  //Store a reference to our BoardManager which will set up the level.
    private List<Zombie> _zombies;
    private float timeToAddNewCoin = 1f;
    private int maxCoinsInGame = 10;


    // Awake is always called before any Start functions.
    private void Awake()
    {
        // If instance already exists
        if (instance == null)
        {
            //if not, set instance to this
            instance = this;
        }

        // Get a component reference to the attached BoardManager script
        _spaceScript = GetComponent<SpaceManager>();
        _zombies = new List<Zombie>();
        numberOfCoins = 0;

        InitGame();
    }

    // This method initializes the game.
    private void InitGame()
    {
        _spaceScript.SetupScene();

        // Start coroutines.

        StartCoroutine(AddCoin());
        StartCoroutine(MoveZombie());
    }

    //Update is called every frame.
    private void Update()
    {

        if (Player.scoreCoins > minCoinsToAddNewZombie && _zombies.Count < 2)
            _spaceScript.AddNewZombie();

        // Set current speed of zombie
        for (int i = 0; i < _zombies.Count; i++)
            _zombies[i].moveTime = speedOfZombie;

        // If player is loser
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            // Call method FinishGame after 2 sec
            Invoke("FinishGame", 2f);
        }

        // If if the button Escape is pressed
        if (Input.GetKey(KeyCode.Escape))
        {
            // Make new report of game.
            AddReport(LoadSaveDataBuffer.LoadPlayerName(), Player.scoreCoins, "Exit", (int)Player.timeGame);
            Application.LoadLevel(0);
        }
    }

    public void AddZombieToList(Zombie script)
    {
        _zombies.Add(script);
    }

    private void AddReport(string name, int coins, string reasonOfLosing, int time)
    {
        ReportManager reportManager = new ReportManager();
        reportManager.AddReport(name, coins, reasonOfLosing, time);
        reportManager.SaveReport();
    }

    // This method loads scene MainMeny
    private void FinishGame()
    {
        // Make new report of game.
        AddReport(LoadSaveDataBuffer.LoadPlayerName(), Player.scoreCoins, "Death", (int)Player.timeGame);
        Application.LoadLevel(0);
    }

    // This method adds coins to a game every 'timeToAddNewCoin' sec
    private IEnumerator AddCoin()
    {
        while (true)
        {
            if (numberOfCoins < maxCoinsInGame)
            {
                _spaceScript.AddNewCoin();
                numberOfCoins++;
            }
            yield return new WaitForSeconds(timeToAddNewCoin);
        }
    }

    // This method controls moving zombie
    private IEnumerator MoveZombie()
    {
        while (true)
        {
            if (_zombies.Count == 0)
                yield return new WaitForSeconds(0.1f);
            for (int i = 0; i < _zombies.Count; i++)
            {
                _zombies[i].NextStep();
                yield return new WaitForSeconds(1f / speedOfZombie);
            }
        }
    }
}