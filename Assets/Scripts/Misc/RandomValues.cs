using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomValues
{
    private static RandomValues instance;
    private List<string> strings = new List<string>();

    public RandomValues()
    {
        LoadStringsFromFile();
    }

    public static RandomValues INSTANCE()
    {
        if(instance == null)
        {
            instance = new RandomValues();
        }

        return instance;
    }

    public string GetRandomString()
    {
        int rand = UnityEngine.Random.Range(0, strings.Count);
        return strings[rand];
    }

    private void LoadStringsFromFile()
    {
        try
        {
            TextAsset textFromData = (TextAsset)Resources.Load("strings");
            string textAsString = textFromData.text;
            string[] lines = textAsString.Split(new string[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string s in lines)
            {
                strings.Add(s);
            }
        }
        catch(Exception e)
        {
            Debug.LogError("Could not load strings from file 'strings'. Error: " + e);
        }
    }
}
