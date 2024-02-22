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

    // ��ʼ����ɫ����
    
    // Start is called before the first frame update
    void Start()
    {     
        CreateLevel();            
    }
    private void CreateColorCollections()
    {
       
        morandiColors[0] = new Color32(255, 0, 0, 255);       // ��ɫ
        morandiColors[1] = new Color32(0, 255, 0, 255);       // ��ɫ
        morandiColors[2] = new Color32(0, 0, 255, 255);       // ��ɫ
        morandiColors[3] = new Color32(255, 255, 0, 255);     // ��ɫ
        morandiColors[4] = new Color32(0, 255, 255, 255);     // ��ɫ
        morandiColors[5] = new Color32(255, 0, 255, 255);     // Ʒ��
        morandiColors[6] = new Color32(192, 192, 192, 255);   // ��ɫ
        morandiColors[7] = new Color32(128, 0, 0, 255);       // ����
        morandiColors[8] = new Color32(128, 128, 0, 255);     // ���ɫ
        morandiColors[9] = new Color32(0, 128, 0, 255);       // ����
        morandiColors[10] = new Color32(128, 0, 128, 255);    // ��ɫ
        morandiColors[11] = new Color32(0, 128, 128, 255);    // ����
        morandiColors[12] = new Color32(0, 0, 128, 255);      // ����
        morandiColors[13] = new Color32(255, 165, 0, 255);    // ��ɫ
        morandiColors[14] = new Color32(255, 192, 203, 255);  // �ۺ�
        morandiColors[15] = new Color32(105, 105, 105, 255);  // ����
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
        // ���㰴ť�Ŀ�Ⱥ͸߶�
        
        float buttonWidth = buttonPrefab.GetComponent<RectTransform>().rect.width;
        float buttonHeight = buttonPrefab.GetComponent<RectTransform>().rect.height;

        // ������ť����
        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                // ʵ������ť
                GameObject button = Instantiate(buttonPrefab);
                button.name = $"Button_{row}_{col}";
                button.transform.SetParent(canvas.transform, false);

                //�����а�ť��������
                Global.Instance.ButtonCollection[row, col] = button;

                // ���ð�ť��λ��
                RectTransform rectTransform = button.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(col * buttonWidth - columns*buttonWidth / 2, -row * buttonHeight+ rows*buttonHeight / 2);



                // ��Ӱ�ť����¼�
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

        // ��ӡ��������
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
                // �������������Ӹ�����߼�������ÿ����Ĵ���������
               

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