using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCtrl : MonoBehaviour
{
    private const int shapesNum = 10;
    private static Shape[] shapes;
    private static int currShapeNum;

    void Start()
    {
        shapes = new Shape[shapesNum];

        // check if this is the first time
        bool isNew = true;

        if (isNew)
        {
            for (int i = 0; i < shapesNum; i++)
            {
                shapes[i] = new Shape(i, i);
            }

            currShapeNum = 0;
        }
        else
        {
            //load all the progress from google

            //temp: from local
        }
    }

    public static int Click()
    {
        shapes[currShapeNum].AddClick();

        int shapeTrueLvl = shapes[currShapeNum].lvl - shapes[currShapeNum].type;
        return shapeTrueLvl;
    }

    public static int GotoNextShape()
    {
        if (shapes[currShapeNum].lvl > currShapeNum && currShapeNum + 1 < shapesNum)
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