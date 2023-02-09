using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelCtrl : MonoBehaviour, IDataPersistence
{
    private static Shape[] shapes;
    private static int currShapeNum;
    private static int maxLvl;

    void Awake()
    {
        shapes = new Shape[GameConstants.shapesNum];
    }

    public void LoadData(PlayerData data)
    {
        maxLvl = 0;

        for (int i = 0; i < data.lvls.Length; i++)
        {
            shapes[i] = new Shape(i, data.lvls[i], data.clicks[i]);

            if (data.lvls[i] > i)
            {
                maxLvl++;
            }
        }

        foreach (Shape shape in shapes)
        {
            shape.upgradable = shape.lvl < maxLvl;
        }

        shapes[maxLvl].upgradable = true; // the last unlocked shape
        currShapeNum = data.currShapeNum;
    }

    public void SaveData(ref PlayerData data)
    {
        int[] lvls = new int[shapes.Length];
        int[] clicks = new int[shapes.Length];

        for (int i = 0; i < data.lvls.Length; i++)
        {
            lvls[i] = shapes[i].lvl;
            clicks[i] = shapes[i].clicks;
        }

        data.lvls = lvls;
        data.clicks = clicks;
        data.currShapeNum = currShapeNum;
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