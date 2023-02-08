using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RenderCtrl : MonoBehaviour
{
    public GameObject shapeObject;
    public GameObject progressBarObject;
    public GameObject progressBarBgObject;
    public GameObject barCurrLvlObject;
    public GameObject barNextLvlObject;
    public GameObject prevShapeButton;
    public GameObject nextShapeButton;

    private static SpriteRenderer shapeRenderer;
    private static int shapeSize;

    private static Image progressBar;
    private static Image progressBarBg;
    private static SpriteRenderer barCurrLvlRenderer;
    private static SpriteRenderer barNextLvlRenderer;
    private static int progressBarShapeSize;

    private static Texture2D shapeTexture;
    private static Texture2D progressBarTexture;

    private static Gradient progressGradient;
    private static Color[] shapeColors;

    private static Shape currShape;

    private static bool shapeChange;
    private static bool progressChange;

    private static Image prevShapeButtonRenderer; //temp -> SpriteRenderer
    private static Image nextShapeButtonRenderer; //temp

    void Awake()
    {
        int screenWidth = (int)gameObject.GetComponent<RectTransform>().rect.width;
        int screenHeight = (int)gameObject.GetComponent<RectTransform>().rect.height;
        shapeTexture = new(screenWidth, screenHeight);

        shapeRenderer = shapeObject.GetComponent<SpriteRenderer>();
        shapeSize = (int)shapeObject.GetComponent<RectTransform>().rect.width;

        // shape sprite
        shapeRenderer.sprite = Sprite.Create(shapeTexture, new Rect(-shapeSize/2, -shapeSize/2, shapeSize, shapeSize), new Vector2(.5f, .5f), 1);

        InitProgressBar();

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

        currShape = new Shape(-1, -1);
        shapeChange = false;
        progressChange = false;


        prevShapeButtonRenderer = prevShapeButton.GetComponent<Image>(); //temp
        nextShapeButtonRenderer = nextShapeButton.GetComponent<Image>(); //temp
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

        // progress bar shapes
        barCurrLvlRenderer = barCurrLvlObject.GetComponent<SpriteRenderer>();
        barNextLvlRenderer = barNextLvlObject.GetComponent<SpriteRenderer>();
        progressBarShapeSize = (int)barCurrLvlObject.GetComponent<RectTransform>().rect.width;

        // progress bar current lvl shape sprite
        barCurrLvlRenderer.sprite = Sprite.Create(
            shapeTexture,
            new Rect(-progressBarShapeSize / 2, -progressBarShapeSize / 2, progressBarShapeSize, progressBarShapeSize),
            new Vector2(0, .5f), 1
        );

        // progress bar next lvl shape sprite
        barNextLvlRenderer.sprite = Sprite.Create(
            shapeTexture,
            new Rect(-progressBarShapeSize / 2, -progressBarShapeSize / 2, progressBarShapeSize, progressBarShapeSize),
            new Vector2(1, .5f), 1
        );
    }

    public static void UpdateShape(Shape shape)
    {
        if (currShape.type != shape.type || currShape.lvl != shape.lvl)
            shapeChange = true;

        currShape.type = shape.type;
        currShape.lvl = shape.lvl;
        currShape.clicks = shape.clicks;
        currShape.clicksForLvlUp = shape.clicksForLvlUp;
        currShape.upgradable = shape.upgradable;

        progressChange = true;
    }

    void Update()
    {
        if (progressChange)
        {
            progressChange = false;

            // shape switch or lvl up
            if (shapeChange)
            {
                shapeChange = false;

                ChangeProgressGradient(currShape.lvl);

                RenderShape();
                CreateProgressBar();

                UpdateSwitchButtons();
            }

            ColorShape();
            RenderProgressBar();
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

        Vector2[] vertices = GenerateVertices(numOfSides, shapeSize);
        ushort[] triangles = GenerateTriangles(numOfSides);

        shapeRenderer.sprite.OverrideGeometry(vertices, triangles);
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

        //progress bar shapes
        int numOfSides = currShape.type + 3;

        Vector2[] vertices = GenerateVertices(numOfSides, progressBarShapeSize);
        ushort[] triangles = GenerateTriangles(numOfSides);

        barCurrLvlRenderer.sprite.OverrideGeometry(vertices, triangles);
        barNextLvlRenderer.sprite.OverrideGeometry(vertices, triangles);

        if (currShape.upgradable)
        {
            Color bgColor = progressGradient.Evaluate(0);
            bgColor.a = 0.2f;
            progressBarBg.color = bgColor;

            barCurrLvlRenderer.color = progressGradient.Evaluate(0);
            barNextLvlRenderer.color = progressGradient.Evaluate(1);
        }
        else
        {
            progressBarBg.color = Color.clear;

            barCurrLvlRenderer.color = Color.clear;
            barNextLvlRenderer.color = Color.clear;
        }
    }

    private static void RenderProgressBar()
    {
        float shapeLvlProgress = (float)currShape.clicks / (float)currShape.clicksForLvlUp;
        progressBar.fillAmount = shapeLvlProgress;
    }

    private static Vector2[] GenerateVertices(int numOfSides, int size)
    {
        Vector2[] vertices = new Vector2[numOfSides];
        int halfSize = (int)(size / 2);

        float angle = 0f;
        float angleIncrement = 360f / numOfSides;

        for (int i = 0; i < numOfSides; i++)
        {
            float x = (Mathf.Sin(angle * Mathf.Deg2Rad) * halfSize) + halfSize;
            float y = (Mathf.Cos(angle * Mathf.Deg2Rad) * halfSize) + halfSize;

            vertices[i] = new Vector2(x, y);
            angle += angleIncrement;
        }

        return vertices;
    }

    private static ushort[] GenerateTriangles(int numOfSides)
    {
        ushort[] triangles = new ushort[numOfSides * 3];

        for (int i = 0; i < numOfSides - 2; i++)
        {
            triangles[i * 3] = 0;
            triangles[i * 3 + 1] = (ushort)(i + 1);
            triangles[i * 3 + 2] = (ushort)(i + 2);
        }

        return triangles;
    }

    private static void UpdateSwitchButtons()
    {
        prevShapeButtonRenderer.color = (currShape.type > 0) ? Color.white : new Color(0, 0, 0, .2f);
        nextShapeButtonRenderer.color = (currShape.lvl > currShape.type) ? Color.white : new Color(0, 0, 0, .2f);
    }
}