using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCtrl : MonoBehaviour
{
    private const bool _debug = true; //TEMP
    private const bool _cheats = true; //TEMP

    public bool interactable;
    public int currShapeTrueLvl;

    void Start()
    {
        interactable = true;

        UpdateShape();
    }

    public void Click()
    {
        if (interactable)
        {
            int newShapeTrueLvl = LevelCtrl.Click();

            if (currShapeTrueLvl == 0 && newShapeTrueLvl == 1)
            {
                // first lvl up

                //todo: delay + animation
                GotoNextShape();
            }
            else if (newShapeTrueLvl > currShapeTrueLvl)
            {
                // lvl up
                UpdateShape();
            }
            else
            {
                UpdateShape(); //optimize?
            }
        }
    }

    public void GotoNextShape()
    {
        if (interactable)
        {
            interactable = false;

            //todo: animation

            LevelCtrl.GotoNextShape();
            UpdateShape();

            interactable = true;
        }
    }

    public void GotoPrevShape()
    {
        if (interactable)
        {
            interactable = false;

            //todo: animation

            LevelCtrl.GotoPrevShape();
            UpdateShape();

            interactable = true;
        }
    }

    public void UpdateShape()
    {
        Shape currShape = LevelCtrl.GetCurrShape();
        RenderCtrl.UpdateShape(currShape);

        currShapeTrueLvl = currShape.lvl - currShape.type;
    }

    //  -------------cheats--------------
    public void SUPPER_CLICK()
    {
        if (_cheats)
        {
            if (_debug)
            {
                Debug.Log("CHEAT ;>");
            }

            Shape currShape = LevelCtrl.GetCurrShape();
            currShape.clicks = currShape.clicksForLvlUp - 1;

            UpdateShape();
        }
    }
}
