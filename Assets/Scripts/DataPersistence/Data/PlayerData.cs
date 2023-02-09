using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData
{
    public int[] lvls;
    public int[] clicks;
    public int currShapeNum;

    // default values when there's no data to load
    public PlayerData()
    {
        lvls = new int[GameConstants.shapesNum];
        clicks = new int[GameConstants.shapesNum];

        for (int i = 0; i < GameConstants.shapesNum; i++)
        {
            lvls[i] = i;
            clicks[i] = 0;
        }

        currShapeNum = 0;
    }
}
