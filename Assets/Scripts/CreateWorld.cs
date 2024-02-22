using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Mathematics;
using static UnityEditor.Progress;
using System;
public class CreateWorld : MonoBehaviour
{
    public string levelname;
    public GameObject canvas;
    public GameObject canvas_arrow;
    public GameObject buttonPrefab;
    public GameObject arrowPrefab;
    public Text remainstepsText;
    public Text remainemptyText;
    protected int rows;
    protected int columns;

    private static int BlockCount = 0;
    private Color[] morandiColors = new Color[16];

    // 初始化颜色数组
    
    // Start is called before the first frame update
    void Start()
    {     
        CreateLevel();            
    }
    private void CreateColorCollections()
    {
       
        morandiColors[0] = new Color32(255, 0, 0, 255);       // 红色
        morandiColors[1] = new Color32(0, 255, 0, 255);       // 绿色
        morandiColors[2] = new Color32(0, 0, 255, 255);       // 蓝色
        morandiColors[3] = new Color32(255, 255, 0, 255);     // 黄色
        morandiColors[4] = new Color32(0, 255, 255, 255);     // 青色
        morandiColors[5] = new Color32(255, 0, 255, 255);     // 品红
        morandiColors[6] = new Color32(192, 192, 192, 255);   // 银色
        morandiColors[7] = new Color32(128, 0, 0, 255);       // 暗红
        morandiColors[8] = new Color32(128, 128, 0, 255);     // 橄榄色
        morandiColors[9] = new Color32(0, 128, 0, 255);       // 暗绿
        morandiColors[10] = new Color32(128, 0, 128, 255);    // 紫色
        morandiColors[11] = new Color32(0, 128, 128, 255);    // 暗青
        morandiColors[12] = new Color32(0, 0, 128, 255);      // 深蓝
        morandiColors[13] = new Color32(255, 165, 0, 255);    // 橙色
        morandiColors[14] = new Color32(255, 192, 203, 255);  // 粉红
        morandiColors[15] = new Color32(105, 105, 105, 255);  // 暗灰
    } 

    void InitializeGlobalArg()
    {
        Global.Instance.InitializeButtonCollection(rows, columns);
        Global.Instance.InitializeStateCollection(rows+2, columns+2);
        Global.Instance.Columns = columns;
        Global.Instance.Rows = rows;
        Global.Instance.ButtonHeight = buttonPrefab.GetComponent<RectTransform>().rect.height;
        Global.Instance.ButtonWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;
    }
    void InitializeVisualEffectArg()
    {
        VisualEffect.ArrowPrefab = arrowPrefab;
        VisualEffect.CanvasArrow = canvas_arrow;
        VisualEffect.RemainStepsText = remainstepsText;
        VisualEffect.RemainEmptyText = remainemptyText;


    }
    void CreateButtonZone()
    {
        // 计算按钮的宽度和高度
        
        float buttonWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;
        float buttonHeight = buttonPrefab.GetComponent<RectTransform>().rect.height;

        // 创建按钮矩阵
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // 实例化按钮
                GameObject button = Instantiate(buttonPrefab);
                button.name = $"Button_{row}_{col}";
                button.transform.SetParent(canvas.transform, false);

                //对所有按钮设置索引
                Global.Instance.ButtonCollection[row, col] = button;

                // 设置按钮的位置
                RectTransform rectTransform = button.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(col * buttonWidth - columns*buttonWidth / 2, -row * buttonHeight+ rows*buttonHeight / 2);



                // 添加按钮点击事件
                Button buttonComponent = button.GetComponent<Button>();
                int rowIndex = row;
                int colIndex = col;
                buttonComponent.onClick.AddListener(() => OnButtonClick(rowIndex, colIndex));
            }
        }
    }
    void OnButtonClick(int rowIndex, int colIndex)
    {
        //Debug.Log(rowIndex + " " + colIndex);
        int numberindex = Global.Instance.StateCollection[rowIndex+1, colIndex+1];
        if (numberindex!= 0&& numberindex != -1)
        {
            //Global.Instance.BlockCollection[numberindex].Position.x;
            BaseBlock a = Global.Instance.BlockCollection[numberindex - 1];


            a.Click ();
            /*if (a is ITypeBlock)
            {
                VisualEffect.ArrowUpdate((ITypeBlock)a);
            }
            else if (a is XTypeBlock)
            {
                VisualEffect.ArrowUpdate((XTypeBlock)a);
            }

            Global.Instance.RemainSteps--;
            VisualEffect.UpdateRemainingStepsDisplay();*/


        }


        string arrayAsString = "";
        for (int i = 0; i < Global.Instance.StateCollection.GetLength(0); i++)
        {
            for (int j = 0; j < Global.Instance.StateCollection.GetLength(1); j++)
            {
                arrayAsString += Global.Instance.StateCollection[i, j] + " ";
            }
            arrayAsString += "\n";
        }

        // 打印数组内容
        Debug.Log("Array Contents:\n" + arrayAsString);
    }
  
    void CreateLevel()
    {
        LoadLevelFromJson(levelname);
        VisualEffect.UpdateProgressBarDisplay();
    }


    void CreateBlockInLevel(int2 p1, int2 p2, int initial_step, XDirection Dirs)
    {
        ++BlockCount;
        BaseBlock a = new XTypeBlock(p1, p2, initial_step, BlockCount, Dirs, morandiColors[BlockCount % 16]);
        //Global.Instance.ButtonCollection[x, y].GetComponent<Image>().color = a.SquareColor;
        Global.Instance.AppendBlock(a);
        VisualEffect.FillButtonColor(p1, p2, a.SquareColor);
        VisualEffect.ArrowCreate((XTypeBlock)a);

    }

    void CreateBlockInLevel(int2 p1, int2 p2, int initial_step, IDirection Dirs)
    {
        ++BlockCount;
        BaseBlock a = new ITypeBlock(p1, p2, initial_step, BlockCount, Dirs, morandiColors[BlockCount % 16]);
 
        Global.Instance.AppendBlock(a);
        VisualEffect.FillButtonColor(p1, p2, a.SquareColor);
        VisualEffect.ArrowCreate((ITypeBlock)a);
    }
    void LoadLevelFromJson(string resourceName)
    {
        TextAsset jsonTextFile = Resources.Load<TextAsset>(resourceName);
        if (jsonTextFile != null)
        {
            LevelData levelData = JsonUtility.FromJson<LevelData>(jsonTextFile.text);
            Debug.Log("Remain Steps: " + levelData.RemainSteps);

            rows = levelData.Rows;
            columns = levelData.Columns;

            InitializeVisualEffectArg();
            InitializeGlobalArg();
            CreateColorCollections();
            CreateButtonZone();

            Global.Instance.RemainSteps = levelData.RemainSteps;


            foreach (var block in levelData.Blocks)
            {
                //Debug.Log($"Block Position: {block.position.x1}, {block.position.y1}, {block.position.x2}, {block.position.y2}");

                if (block.type == "XType")
                {
                    XDirection tempd = XDirection.None;
                    foreach (var dir in block.directions)
                    {
                        if (dir == "LEFTUP") { tempd |= XDirection.LEFTUP; }
                        if (dir == "RIGHTUP") { tempd |= XDirection.RIGHTUP; }
                        if (dir == "LEFTDOWN") { tempd |= XDirection.LEFTDOWN; }
                        if (dir == "RIGHTDOWN") { tempd |= XDirection.RIGHTDOWN; }
                    }
                    CreateBlockInLevel(new int2(block.position.x1, block.position.y1), new int2(block.position.x2, block.position.y2), 0, tempd);
                    
                }
                if (block.type == "IType")
                {
                    IDirection tempd = IDirection.None;
                    foreach (var dir in block.directions)
                    {
                    
                        if (dir == "UP") { tempd |= IDirection.UP; }
                        if (dir == "DOWN") { tempd |= IDirection.DOWN; }
                        if (dir == "LEFT") { tempd |= IDirection.LEFT; }
                        if (dir == "RIGHT") { tempd |= IDirection.RIGHT; }
                    }
                    CreateBlockInLevel(new int2(block.position.x1, block.position.y1), new int2(block.position.x2, block.position.y2), 0, tempd);

                }
                // 你可以在这里添加更多的逻辑来处理每个块的创建和配置
               

            }
        }
        else
        {
            Debug.LogError("Cannot load level data!");
        }
    }

}
[Serializable]
public class LevelData
{
    public int RemainSteps;
    public int Rows;
    public int Columns;
    public Block[] Blocks;
}

[Serializable]
public class Block
{
    public Position position;
    public string[] directions;
    public string type;
}

[Serializable]
public class Position
{
    public int x1;
    public int y1;
    public int x2;
    public int y2;
}