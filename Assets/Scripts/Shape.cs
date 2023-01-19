using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shape
{
    public int type;
    public int lvl;
    public int clicks;
    public int clicksForLvlUp;

    public Shape(int shapeType, int shapeLvl, int shapeClicks = 0)
    {
        type = shapeType;
        lvl = shapeLvl;
        clicks = shapeClicks;

        clicksForLvlUp = CalcClicksForLvlUp();
    }

    public void AddClick()
    {
        clicks++;

        if (clicks >= clicksForLvlUp)
        {
            // lvl up
            clicks -= clicksForLvlUp;
            lvl++;
            clicksForLvlUp = CalcClicksForLvlUp();
        }
    }

    private int CalcClicksForLvlUp()
    {
        return (int)System.Math.Pow(type + 2, lvl - type + 4);
    }
}