using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;

[Serializable]
public class ReportOfGame : System.Object
{
    public string name;
    public int coins;
    public string reasonOfLosing;
    public int time;

    public ReportOfGame() { }

    public ReportOfGame(string name, int coins, string reasonOfLosing, int time)
    {
        this.name = name;
        this.coins = coins;
        this.reasonOfLosing = reasonOfLosing;
        this.time = time;
    }
    public string GetReportOfGame()
    {
        return name + " " + coins + " " + reasonOfLosing + " " + time + "\n";
    }
}

public class ReportManager 
{

    public List<ReportOfGame> listReportOfGames;

    public ReportManager()
    {
        listReportOfGames = new List<ReportOfGame>();
        LoadReport();
    }

    public string GetReport()
    {
        string reportOfGame = "";
        foreach (ReportOfGame report in listReportOfGames)
        {
            reportOfGame += report.GetReportOfGame();
        }
        return reportOfGame;
    }

    public void AddReport(string name, int coins, string reasonOfLosing, int time)
    {
        listReportOfGames.Add(new ReportOfGame(name, coins, reasonOfLosing, time));
    }
    public void SaveReport()
    {
        XmlSerializer formatter = new XmlSerializer(typeof(List<ReportOfGame>));

        using (FileStream fileStream = new FileStream("report.xml", FileMode.OpenOrCreate))
        {
            formatter.Serialize(fileStream, listReportOfGames);
        }
    }
    private void LoadReport()
    {
        XmlSerializer formatter = new XmlSerializer(typeof(List<ReportOfGame>));

        using (FileStream fileStream = new FileStream("report.xml", FileMode.OpenOrCreate))
        {
            if (fileStream.Length == 0)
                return;

            this.listReportOfGames = (List<ReportOfGame>)formatter.Deserialize(fileStream);
        }
    }
}