using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderCtrl : MonoBehaviour
{
    private const bool _debug = true; //TEMP

    private static TMPro.TextMeshProUGUI shapeElem; //temp
    private static TMPro.TextMeshProUGUI shapeProgressElem; //temp
    private static TMPro.TextMeshProUGUI shapeClicksElem; //temp

    private static bool changed;

    private static Shape currShape;

    void Start()
    {
        shapeElem = transform.GetChild(0).GetComponent<TMPro.TextMeshProUGUI>(); //temp
        shapeProgressElem = transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>(); //temp
        shapeClicksElem = transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>(); //temp

        //colors for every lvl, backgrounds and shapes

        changed = false;
    }

    public static void UpdateShape(Shape shape)
    {
        currShape = shape;

        // update buttons?

        changed = true;
    }

    void Update()
    {
        if (changed)
        {
            changed = false;

            RenderShape();
            RenderProgressBar();

            RenderClicks(); //temp
        }
    }

    //void RenderBackground()

    //void CreateProgressBar

    private static void RenderShape()
    {
        //temp
        shapeElem.text = "shape: " + currShape.type.ToString();
    }

    private static void RenderProgressBar()
    {
        float shapeLvlProgress = (float)currShape.clicks / (float)currShape.clicksForLvlUp;

        if (_debug)
        {
            Debug.Log(
                string.Format("type:{0}, lvl:{1}, clicks:{2}, lvlup:{3}",
                    currShape.type, currShape.lvl, currShape.clicks, currShape.clicksForLvlUp)
            );
        }

        //temp
        int progressPercentage = (int)(shapeLvlProgress * 100);

        shapeProgressElem.text = string.Format("{0} => {1}% => {2}", currShape.lvl, progressPercentage, currShape.lvl + 1);
    }

    private static void RenderClicks() //temp
    {
        shapeClicksElem.text = currShape.clicks.ToString();
    }
}