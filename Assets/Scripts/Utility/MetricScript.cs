﻿using UnityEngine;
using System.Collections;
using System.IO;

public class MetricScript : MonoBehaviour
{

    string createText = "";

    public int sampleMetric1, sampleMetric2;

    void Start() { }
    void Update() { }

    //When the game quits we'll actually write the file.
    void OnApplicationQuit()
    {
        GenerateMetricsString();
        string time = System.DateTime.UtcNow.ToString(); //string dateTime = System.DateTime.Now.ToString(); //Get the time to tack on to the file name
        time = time.Replace("/", "-"); //Replace slashes with dashes, because Unity thinks they are directories..
        time = time.Replace(":", "_"); // Replace : with _ cause directories
        string reportFile = "AstralApprentice_Metrics_" + time + ".txt"; 
        File.WriteAllText(reportFile, createText);
        //In Editor, this will show up in the project folder root (with Library, Assets, etc.)
        //In Standalone, this will show up in the same directory as your executable
    }

    void GenerateMetricsString()
    {
        createText =
            "Average time (UnMars) " + GameManager.instance.GetPlayerStats().TotalTimeUnmars + " | " +
            "Total Magic Points: " + GameManager.instance.GetPlayerStats().TotalMagicPoints + " | " +
            "Total Missiles Fired: " + GameManager.instance.GetPlayerStats().TotalMissilesFired + " | " +
            "Total Slashes: " + GameManager.instance.GetPlayerStats().TotalSlashes + " | ";
    }

    //Add to your set metrics from other classes whenever you want
    public void AddToSample1(int amtToAdd)
    {
        sampleMetric1 += amtToAdd;
    }
}
