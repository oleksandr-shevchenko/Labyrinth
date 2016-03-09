using UnityEngine;
using System.Collections;
using System;
using System.IO;
using System.Xml.Serialization;

/// <summary>
///  This class implements a data buffer between scenes.
/// </summary>
public static class LoadSaveDataBuffer
{
    public static string LoadPlayerName()
    {
        string tempName = "Name";
        XmlSerializer formatter = new XmlSerializer(typeof(string));

        using (FileStream fileStream = new FileStream("playername.xml", FileMode.OpenOrCreate))
        {
            if (fileStream.Length == 0)
                return tempName;
            tempName = (string)formatter.Deserialize(fileStream);
        }

        return tempName;
    }

    public static void SavePlayerName(string playerName)
    {
        XmlSerializer formatter = new XmlSerializer(typeof(string));

        using (FileStream fileStream = new FileStream("playername.xml", FileMode.Create))
        {
            formatter.Serialize(fileStream, playerName);
        }
    }
}
