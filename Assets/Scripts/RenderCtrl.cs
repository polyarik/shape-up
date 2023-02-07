using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderCtrl : MonoBehaviour
{
    public GameObject shapeObject;
    public GameObject progressBarObject;
    public GameObject progressBarBgObject;

    private static Gradient progressGradient;

    private static SpriteRenderer shapeRenderer;
    private static int shapeSize;

    private static Image progressBar;
    private static Image progressBarBg;
    private static Texture2D progressBarTexture;

    // currLvl
    // nextLvl


    private static Color[] shapeColors;

    private static Shape currShape;

    private static bool changed; //todo: track different changes

    public static TMPro.TextMeshProUGUI shapeProgressElem; //temp
    public static TMPro.TextMeshProUGUI infoElem; //temp

    void Awake()
    {
        shapeRenderer = shapeObject.GetComponent<SpriteRenderer>();
        shapeSize = (int)shapeObject.GetComponent<RectTransform>().rect.width;

        Texture2D shapeTexture = new(shapeSize, shapeSize); //todo: the whole canvas

        // shape sprite
        shapeRenderer.sprite = Sprite.Create(shapeTexture, new Rect(-shapeSize/2, -shapeSize/2, shapeSize, shapeSize), new Vector2(.5f, .5f), 1);

        // init shape colors
        shapeColors = new Color[GameConstants.shapesNum + 2]; //or shapesNum

        for (int i = 0; i < GameConstants.shapesNum + 2; i++) // or + 1 ?
        {
            shapeColors[i] = new Color(
                GameConstants.shapeColors[i][0],
                GameConstants.shapeColors[i][1],
                GameConstants.shapeColors[i][2],
            1);
        }

        //colors for backgrounds ...
        //

        progressGradient = new Gradient();

        InitProgressBar();

        changed = false;


        shapeProgressElem = transform.Find("progress_TEMP").GetComponent<TMPro.TextMeshProUGUI>(); //temp
        infoElem = transform.Find("info_TEMP").GetComponent<TMPro.TextMeshProUGUI>(); //temp
    }

    private void InitProgressBar()
    {
        progressBar = progressBarObject.GetComponent<Image>();
        progressBar.type = Image.Type.Filled;
        progressBar.fillMethod = Image.FillMethod.Horizontal;
        progressBar.fillOrigin = (int)Image.OriginHorizontal.Left;

        progressBarBg = progressBarBgObject.GetComponent<Image>();

        int width = (int)progressBar.GetComponent<RectTransform>().rect.width;
        int height = (int)progressBar.GetComponent<RectTransform>().rect.height;
        progressBarTexture = new Texture2D(width, height);
    }

    public static void UpdateShape(Shape shape)
    {
        currShape = shape;

        // update buttons?

        changed = true;
    }

    void Update()
    {
        //TRACK DIFFERENT CHANGES!

        if (changed)
        {
            changed = false;


            //todo: only on switch or lvl up
            ChangeProgressGradient(currShape.lvl);
            RenderShape();
            CreateProgressBar();


            //------------
            ColorShape();
            RenderProgressBar();

            RenderClicks(); //temp
        }
    }

    //void RenderBackground()

    private static void ChangeProgressGradient(int currLvl = 0)
    {
        progressGradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(shapeColors[currLvl], 0), new GradientColorKey(shapeColors[currLvl + 1], 1) },
            new GradientAlphaKey[] { new GradientAlphaKey(1, 0), new GradientAlphaKey(1, 1) }
        );
    }

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
    }

    private static void ColorShape()
    {
        shapeRenderer.color = progressGradient.Evaluate((float)currShape.clicks / (float)currShape.clicksForLvlUp);
    }

    private static void CreateProgressBar()
    {
        int width = progressBarTexture.width;
        int height = progressBarTexture.height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                progressBarTexture.SetPixel(x, y, progressGradient.Evaluate((float)x / width));
            }
        }
        progressBarTexture.Apply();

        progressBar.sprite = Sprite.Create(progressBarTexture, new Rect(0, 0, width, height), Vector2.zero);

        // progress bar background
        Color bgColor = progressGradient.Evaluate(0);
        bgColor.a = 0.2f;
        progressBarBg.color = bgColor;

        //todo: prepare bar shapes
    }

    private static void RenderProgressBar()
    {
        float shapeLvlProgress = (float)currShape.clicks / (float)currShape.clicksForLvlUp;
        progressBar.fillAmount = shapeLvlProgress;

        //temp
        int progressPercentage = (int)(shapeLvlProgress * 100);
        shapeProgressElem.text = string.Format("{0} => {1}% => {2}", currShape.lvl, progressPercentage, currShape.lvl + 1);
    }

    private static void RenderClicks() //temp
    {
        infoElem.text = string.Format("shape: {0}   clicks: {1}", currShape.type, currShape.clicks);
    }
}