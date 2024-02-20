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

    // ��ʼ����ɫ����
    
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

        // ��ӡ��������
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
