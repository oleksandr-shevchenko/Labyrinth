using UnityEngine;
using System.Collections;

/// <summary>
///  This class draws a main menu.
/// </summary>
public class MainMenu : MonoBehaviour
{
    private int _window;    // Index of the current window to the drawing window.
    private string _playerName;
    private string _report;
    private Vector2 _scrollPosition = Vector2.zero;
    private ReportManager _reportManager;

    //  Start is called on the frame when a script is enabled just before any of 
    //  the Update methods is called the first time.
    //  Use this for initialization
    private void Start()
    {
        _window = 1;
        _playerName = LoadSaveDataBuffer.LoadPlayerName();
        _reportManager = new ReportManager();
        _report = _reportManager.GetReport();
    }

    // OnGui is called every frame.
    private void OnGUI()
    {
        GUI.BeginGroup(new Rect(Screen.width / 2 - 150, Screen.height / 2 - 150, 300, 300));

        // Depending on the selected index, GUI draws elements in the block.

        if (_window == 1)
        {
            if (GUI.Button(new Rect(50, 30, 200, 30), "Play"))
            {
                Application.LoadLevel("Game");
                _window = 1;
            }

            if (GUI.Button(new Rect(50, 70, 200, 30), "Settings"))
            {
                _window = 2;
            }

            if (GUI.Button(new Rect(50, 110, 200, 30), "About Game"))
            {
                _window = 3;
            }

            if (GUI.Button(new Rect(50, 150, 200, 30), "Exit"))
            {
                _window = 4;
            }
        }

        if (_window == 2)
        {
            GUI.Label(new Rect(25, 40, 200, 30), "Please enter your name");
            _playerName = GUI.TextField(new Rect(50, 80, 200, 20), _playerName, 25);

            if (GUI.Button(new Rect(50, 160, 200, 30), "Ok."))
            {
                LoadSaveDataBuffer.SavePlayerName(_playerName);
                _window = 1;
            }
        }

        if (_window == 3)
        {
            GUI.Label(new Rect(145, 0, 180, 30), "Statistics");

            _scrollPosition = GUI.BeginScrollView(new Rect(50, 20, 200, 250),
                _scrollPosition, new Rect(0, 0, 0, _report.Length + 10));

            GUI.Label(new Rect(0, 10, 200, _report.Length + 10), _report);
            GUI.EndScrollView();

            if (GUI.Button(new Rect(50, 275, 200, 25), "Back"))
            {
                _window = 1;
            }
        }

        if (_window == 4)
        {
            GUI.Label(new Rect(100, 10, 180, 30), "Are you sure?");

            if (GUI.Button(new Rect(50, 50, 200, 30), "Yes"))
            {
                Application.Quit();
            }

            if (GUI.Button(new Rect(50, 90, 200, 30), "No"))
            {
                _window = 1;
            }
        }

        GUI.EndGroup();
    }
}

