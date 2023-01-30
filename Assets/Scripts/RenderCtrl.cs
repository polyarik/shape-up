using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderCtrl : MonoBehaviour
{
    public GameObject shapeObject;
    private static SpriteRenderer shapeRenderer;
    private static int shapeSize;

    private static Color[] shapeColors;

    public static TMPro.TextMeshProUGUI shapeProgressElem; //temp
    public static TMPro.TextMeshProUGUI infoElem; //temp

    private static bool changed; //todo: track different changes

    private static Shape currShape;

    void Awake()
    {
        shapeRenderer = shapeObject.GetComponent<SpriteRenderer>();
        shapeSize = (int)shapeObject.GetComponent<RectTransform>().rect.width;


        Texture2D texture = new(shapeSize, shapeSize);

        /*Color[] pixels = texture.GetPixels();

        for (int i = 0, l = pixels.Length; i < l; i++)
        {
            pixels[i] = Color.black;
        }

        texture.SetPixels(pixels);
        texture.Apply();*/

        // shape sprite
        shapeRenderer.sprite = Sprite.Create(texture, new Rect(-shapeSize/2, -shapeSize/2, shapeSize, shapeSize), new Vector2(.5f, .5f), 1);

        shapeProgressElem = transform.GetChild(1).GetComponent<TMPro.TextMeshProUGUI>(); //temp
        infoElem = transform.GetChild(2).GetComponent<TMPro.TextMeshProUGUI>(); //temp

        // init shape colors
        shapeColors = new Color[GameConstants.shapesNum + 1]; //or shapesNum

        for (int i = 0; i < GameConstants.shapesNum + 1; i++)
        {
            shapeColors[i] = new Color(
                GameConstants.shapeColors[i][0],
                GameConstants.shapeColors[i][1],
                GameConstants.shapeColors[i][2],
            1);
        }


        //colors for backgrounds ...


        changed = false;
    }

    void Start()
    {
        //
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

    //void CreateProgressBar //on switch or lvlup

    private static void RenderShape()
    {
        int numOfSides = currShape.type + 3;

        ushort[] triangles = new ushort[numOfSides * 3];
        Vector2[] uvs = new Vector2[numOfSides];

        float angle = 0f;
        float angleIncrement = 360f / numOfSides;

        for (int i = 0; i < numOfSides; i++)
        {
            float x = (Mathf.Sin(angle * Mathf.Deg2Rad) * shapeSize / 2) + shapeSize / 2;
            float y = (Mathf.Cos(angle * Mathf.Deg2Rad) * shapeSize / 2) + shapeSize / 2;

            uvs[i] = new Vector2(x, y);
            angle += angleIncrement;
        }

        for (int i = 0; i < numOfSides - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = (ushort)(i + 1);
            triangles[i * 3 + 2] = (ushort)(i + 2);
        }

        shapeRenderer.sprite.OverrideGeometry(uvs, triangles);

        shapeRenderer.color = shapeColors[currShape.lvl];
    }

    private static void ColorShape()
    {
        //
    }

    private static void RenderProgressBar()
    {
        float shapeLvlProgress = (float)currShape.clicks / (float)currShape.clicksForLvlUp;

        if (GameConstants._debug)
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
        infoElem.text = string.Format("shape: {0}   clicks: {1}", currShape.type, currShape.clicks);
    }
}