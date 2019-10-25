using System;
using System.Collections.Generic;
using UnityEngine;

public class BricksManager : MonoBehaviour
{
    #region  Singleton

    private static BricksManager _instance;
    public static BricksManager Instance => _instance;

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
        }
    }
    #endregion

    private int maxRows = 17;
    private int maxCols = 12;
    private GameObject bricksContainer;
    private float initialBrickSpawnPositionX = -1.96f;
    private float initialBrickSpawnPositionY = 3.325f;
    private float shiftAmount = 0.365f;

    public Brick brickPrefab;
    public Sprite[] Sprites;
    public Color[] BrickColors;

    public List<int[,]> LevelsData { get; set; }
    public List<Brick> RemainingBricks { get; set; }
    public int InitialBricksCount { get; set; }





    public int CurrentLevel;








    // Start  ---------------------------------------------------------------------------------------------------------


    private void Start()
    {
        this.LevelsData = this.LoadLevelsData();
        this.bricksContainer = new GameObject("BricksContainer");
        this.RemainingBricks = new List<Brick>();
        this.GenerateBricks();
    }

    private void GenerateBricks()
    {
        int[,] currentLevelData = this.LevelsData[this.CurrentLevel];
        float currentSpawnX = initialBrickSpawnPositionX;
        float currentSpawnY = initialBrickSpawnPositionY;

        float zShift = 0;

        for (int row = 0; row < this.maxRows; row++)
        {
            {
                for (int col = 0; col < this.maxCols; col++)
                {
                    int brickType = currentLevelData[row, col];
                    if (brickType > 0)
                    {
                        Brick newBrick = Instantiate(brickPrefab, new Vector3(currentSpawnX, currentSpawnY, 0.0f - zShift), Quaternion.identity) as Brick;
                        newBrick.Init(bricksContainer.transform, this.Sprites[brickType - 1], this.BrickColors[brickType], brickType);

                        this.RemainingBricks.Add(newBrick);
                        zShift += 0.0001f;
                    }
                    currentSpawnX += shiftAmount;
                    if (col + 1 == this.maxCols)
                    {
                        currentSpawnX = initialBrickSpawnPositionX;
                    }


                }
                currentSpawnY -= shiftAmount;

            }
            this.InitialBricksCount = this.RemainingBricks.Count;
        }
    }

    private List<int[,]> LoadLevelsData()
    {


        TextAsset textFile = Resources.Load("levels") as TextAsset;
        string[] rows = textFile.text.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> tempLevelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols]; // ekrana sığan max kutu sayısı
        int currentRow = 0;

        for (int row = 0; row < rows.Length; row++)
        {
            string line = rows[row];

            if (line.IndexOf("--") == -1) // IndexOf( ) metodu, aradığını bulamazsa geriye -1 değerini döndürür. satırda -- aranıp bulamazsa demek
            {
                string[] bricks = line.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < bricks.Length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }
                currentRow++;
            }


            else
            {   // levelin sonu geldi
                currentRow = 0;
                tempLevelsData.Add(currentLevel);
                currentLevel = new int[maxRows, maxCols];

            }
        }
        return tempLevelsData;
    }

}