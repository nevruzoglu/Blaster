using System.Collections;
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

    public Sprite[] Sprites;

    public List<int[,]> LevelsData { get; set; }

    private int maxRows = 17;
    private int maxCols = 12;

    private void Start()
    {
        this.LevelsData = this.LoadLevelsData();
    }

    private List<int[,]> LoadLevelsData()
    {
        TextAsset lvlText = Resources.Load("levels") as TextAsset;
        string[] rows = lvlText.text.Split(new string[] { Enviroment.NewLine }, StringSplitOptions.RemoveEmptyEntries);

        List<int[,]> levelsData = new List<int[,]>();
        int[,] currentLevel = new int[maxRows, maxCols]; // ekrana sığan max kutu sayısı
        int currentRow = 0;

        for (int row = 0; row < rows.length; row++)
        {
            string line = rows[row];

            if (line.IndexOf("--") == -1)
            { // IndexOf( ) metodu, aradığını bulamazsa geriye -1 değerini döndürür. satırda -- aranıp bulamazsa demek
                string[] bricks = line.Split(new char[] { "," }, StringSplitOptions.RemoveEmptyEntries);
                for (int col = 0; col < bricks.length; col++)
                {
                    currentLevel[currentRow, col] = int.Parse(bricks[col]);
                }
                currentRow++;
            }


            else
            {   // levelin sonu geldi
                currentRow = 0;
                levelsData.Add(currentLevel);
                currentLevel = new int[maxRows, maxCols];

            }
        }
        return levelsData;
    }

}