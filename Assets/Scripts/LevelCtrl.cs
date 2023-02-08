using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCtrl : MonoBehaviour
{
    private static Shape[] shapes;
    private static int currShapeNum;
    private static int maxLvl;

    void Awake()
    {
        shapes = new Shape[GameConstants.shapesNum];

        // check if this is the first time
        bool isNew = true;

        if (isNew)
        {
            for (int i = 0; i < GameConstants.shapesNum; i++)
            {
                shapes[i] = new Shape(i, i);
            }

            shapes[0].upgradable = true;
            currShapeNum = 0;
            maxLvl = 0;
        }
        else
        {
            //load all the progress from google

            //temp: from local
        }
    }

    private void Start()
    {
        //
    }

    public static int Click()
    {
        Shape currShape = shapes[currShapeNum];

        if (currShape.upgradable)
        {
            currShape.AddClick();

            if (currShape.type < maxLvl && currShape.lvl == maxLvl || currShape.type == maxLvl && currShape.lvl > maxLvl)
            {
                currShape.upgradable = false;
            }

            if (currShape.type == maxLvl && currShape.lvl > maxLvl)
            {
                maxLvl++;
                shapes[currShapeNum + 1].upgradable = true;

                for (int i = 0; i < currShape.type; i++)
                {
                    if (shapes[i].lvl < maxLvl)
                    {
                        shapes[i].upgradable = true;
                    }
                }
            }
        }

        int shapeTrueLvl = currShape.lvl - currShape.type;
        return shapeTrueLvl;
    }

    public static int GotoNextShape()
    {
        if (shapes[currShapeNum].lvl > currShapeNum && currShapeNum + 1 < GameConstants.shapesNum)
        {
            currShapeNum++;
            return currShapeNum;
        }

        return -1;
    }

    public static int GotoPrevShape()
    {
        if (currShapeNum - 1 >= 0)
        {
            currShapeNum--;
            return currShapeNum;
        }

        return -1;
    }

    public static Shape GetCurrShape()
    {
        return shapes[currShapeNum];
    }
}