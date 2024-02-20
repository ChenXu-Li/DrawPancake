using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.Progress;
public class CreateWorld : MonoBehaviour
{
    public GameObject canvas;
    public GameObject canvas_arrow;
    public GameObject buttonPrefab;
    public GameObject arrowPrefab;
    public Text remainstepsText;
    public int rows;
    public int columns;

    private static int BlockCount = 0;
    private Color[] morandiColors = new Color[16];

    // 初始化颜色数组
    
    // Start is called before the first frame update
    void Start()
    {
        InitializeVisualEffectArg();
        InitializeGlobalArg();
        CreateColorCollections();
        CreateButtonZone();
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

    // Update is called once per frame
    void Update()
    {
         
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
        Debug.Log(rowIndex + " " + colIndex);
        int numberindex = Global.Instance.StateCollection[rowIndex+1, colIndex+1];
        if (numberindex!= 0)
        {
            //Global.Instance.BlockCollection[numberindex].Position.x;
            BaseBlock a = Global.Instance.BlockCollection[numberindex - 1];
            a.Click ();
            if (a is ITypeBlock)
            {
                VisualEffect.ArrowUpdate((ITypeBlock)a);
            }
            else if (a is XTypeBlock)
            {
                VisualEffect.ArrowUpdate((XTypeBlock)a);
            }

            Global.Instance.RemainSteps--;
            VisualEffect.UpdateRemainingStepsDisplay();


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


        /* CreateBlockInLevel(0, 0, 0, IDirection.RIGHT | IDirection.DOWN, Color.gray);
         CreateBlockInLevel(7, 1, 0, IDirection.RIGHT | IDirection.UP, Color.yellow);
         CreateBlockInLevel(6, 7, 0, XDirection.LEFTUP, Color.blue);*/

        Global.Instance.RemainSteps = 15;
        CreateBlockInLevel(0, 0, 0, XDirection.RIGHTDOWN);
        CreateBlockInLevel(0, 5, 0, IDirection.LEFT | IDirection.RIGHT | IDirection.DOWN);
        CreateBlockInLevel(1, 7, 0, XDirection.LEFTDOWN);
        CreateBlockInLevel(3, 4, 0, IDirection.RIGHT | IDirection.UP);
        CreateBlockInLevel(3, 7, 0, IDirection.DOWN);
        CreateBlockInLevel(4, 0, 0, IDirection.UP);
        CreateBlockInLevel(4, 2, 0, XDirection.LEFTUP);
        CreateBlockInLevel(4, 3, 0, IDirection.RIGHT | IDirection.UP);
        CreateBlockInLevel(6, 7, 0, XDirection.LEFTUP);
        CreateBlockInLevel(7, 2, 0, XDirection.LEFTUP | XDirection.RIGHTUP);
        CreateBlockInLevel(7, 5, 0, IDirection.RIGHT | IDirection.UP);

    }

    void CreateBlockInLevel(int x, int y, int initial_step, XDirection Dirs)
    {
        ++BlockCount;
        BaseBlock a = new XTypeBlock(x, y, initial_step, BlockCount, Dirs, morandiColors[BlockCount % 16]);
        Global.Instance.ButtonCollection[x, y].GetComponent<Image>().color = a.SquareColor;
        Global.Instance.AppendBlock(a);
        VisualEffect.ArrowCreate((XTypeBlock)a);

    }
    void CreateBlockInLevel(int x, int y, int initial_step,IDirection Dirs)
    {
        ++BlockCount;
        BaseBlock a = new ITypeBlock(x, y, initial_step, BlockCount, Dirs, morandiColors[BlockCount%16]);
        Global.Instance.ButtonCollection[x, y].GetComponent<Image>().color = a.SquareColor;
        Global.Instance.AppendBlock(a);
        VisualEffect.ArrowCreate((ITypeBlock)a);
    }
}
