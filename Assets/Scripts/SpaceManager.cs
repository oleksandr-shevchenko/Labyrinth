using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

/// <summary>
/// This class creates space of game.
/// </summary>
public class SpaceManager : MonoBehaviour
{
    // Using Serializable allows us to embed a class with sub properties in the inspector.
    [Serializable]
    public class Count
    {
        public int minimum; //Minimum value for our Count class.
        public int maximum; //Maximum value for our Count class.

        public Count(int minimum, int maximum)
        {
            this.minimum = minimum;
            this.maximum = maximum;
        }
    }
    private int _columns = 16;    //Number of columns in our game board.
    private int _rows = 8;    //Number of rows in our game board.

    //Lower and upper limit for our random number of walls per level.
    public Count wallCount = new Count(9, 40);

    // The objects to create space of game.

    public GameObject groundTile;
    public GameObject wallTile;
    public GameObject coinTile;
    public GameObject zombieTile;
    public GameObject exitGroundTile;

    public static int[,] matrixBoard; // A matrix to store positions of the walls. 0 - a element is set 
                                      //                                            1 - a element isn't set
    private Transform _boardHolder;  // A variable to store a reference to the transform of our Board object.
    public static List<Vector3> gridPositions = new List<Vector3>();  // A list of possible locations to place tiles.

    private void InitialiseList()
    {
        //Clear our list gridPositions.
        gridPositions.Clear();
        //Loop through x axis (columns).
        for (int x = 1; x < _columns - 1; x++)
        {
            //Within each column, loop through y axis (rows).
            for (int y = 1; y < _rows - 1; y++)
            {
                //At each index add a new Vector3 to our list with the x and y coordinates of that position.
                gridPositions.Add(new Vector3(x, y, 0f));
            }
        }
        matrixBoard = new int[_columns, _rows];
    }

    //Sets up the outer walls and floor (background) of the game board.
    private void BoardSetup()
    {
        //Instantiate Board and set boardHolder to its transform.
        _boardHolder = new GameObject("Space").transform;

        //Loop along x axis, starting from -1 (to fill corner) with floor or outerwall edge tiles.
        for (int x = -1; x < _columns + 1; x++)
        {
            //Loop along y axis, starting from -1 to place floor or outerwall tiles.
            for (int y = -1; y < _rows + 1; y++)
            {
                //Choose a random tile from our array of floor tile prefabs and prepare to instantiate it.
                GameObject toInstantiate = groundTile;

                //Check if we current position is at board edge, if so choose a random wall 
                // prefab from our array of outer wall tiles.
                if (x == -1 || x == _columns || y == -1 || y == _rows)
                {
                    toInstantiate = wallTile;
                }

                if (x == -1 && y == 0)
                {
                    toInstantiate = exitGroundTile;
                }
                else if (x == _columns && (y == _rows / 3 || y == _rows - _rows / 3))
                {
                    toInstantiate = exitGroundTile;
                }

                // Instantiate the GameObject instance using the prefab chosen for toInstantiate 
                // at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance =
                    Instantiate(toInstantiate, new Vector3(x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of our newly instantiated object instance to boardHolder, 
                // this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent(_boardHolder);
            }
        }
    }
    // This method returns a random position from our list gridPositions.
    Vector3 RandomPosition()
    {
        //Declare an integer randomIndex, set it's value to a random number between 0 and the count of items in our List gridPositions.
        int randomIndex = Random.Range(0, gridPositions.Count);

        //Declare a variable of type Vector3 called randomPosition, set it's value to the entry at randomIndex from our List gridPositions.
        Vector3 randomPosition = gridPositions[randomIndex];

        //Remove the entry at randomIndex from the list so that it can't be re-used.
        gridPositions.RemoveAt(randomIndex);

        return randomPosition;
    }

    // This method calculates matrixBord
    private void CalculateMatrixBord()
    {
        for (int i = 0; i < matrixBoard.GetLength(0); i++)
            for (int j = 0; j < matrixBoard.GetLength(1); j++)
            {
                // If the position is the extreme elements of the board
                if (i == 0 || j == 0 || i == matrixBoard.GetLength(0) - 1 ||
                    j == matrixBoard.GetLength(1) - 1)
                {
                    matrixBoard[i, j] = 1;
                }
            }
        // Set other free position.
        foreach (var vector in gridPositions)
        {
            matrixBoard[(int)vector.x, (int)vector.y] = 1;
        }
    }

    // This method accepts an array of game objects to choose from along with
    // a minimum and maximum range for the number of objects to create.
    private void LayoutObjectAtRandom(GameObject tile, int minimum, int maximum)
    {
        //Choose a random number from minimum to maximum
        int objectCount = Random.Range(minimum, maximum + 1);

        //Instantiate objects until the randomly chosen limit objectCount is reached
        for (int i = 0; i < objectCount; i++)
        {
            // Choose a position for randomPosition by getting a random position from our 
            // list of available Vector3s stored in gridPosition
            Vector3 randomPosition = RandomPosition();

            //Instantiate object tile at the position returned by RandomPosition 
            Instantiate(tile, randomPosition, Quaternion.identity);
        }
    }

    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public int[,] SetupScene()
    {
        // Creates the walls and floor.
        BoardSetup();

        // Reset our list of gridpositions.
        InitialiseList();

        // Instantiate a random number of wall tiles based on minimum and maximum, at randomized positions.
        LayoutObjectAtRandom(wallTile, wallCount.minimum, wallCount.maximum);

        // Calculate matrixBord.
        CalculateMatrixBord();

        //Instantiate a random number of one enemie
        LayoutObjectAtRandom(zombieTile, 1, 0);

        return matrixBoard;
    }

    // This method adds a new coin.
    public void AddNewCoin()
    {
        Vector3 randomPosition = RandomPosition();

        //Instantiate coinTile at the position returned by RandomPosition 
        Instantiate(coinTile, randomPosition, Quaternion.identity);
    }

    //This method adds a new zombie.
    public void AddNewZombie()
    {
        Vector3 randomPosition = RandomPosition();

        //Instantiate zombieTile at the position returned by RandomPosition 
        Instantiate(zombieTile, randomPosition, Quaternion.identity);
    }

}
