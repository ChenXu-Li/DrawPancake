using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorLevel : MonoBehaviour
{
   
    public GameObject canvas;
    public GameObject buttonPrefab;
    public Sprite pointupleftSprite;
    public Sprite pointbottomrightSprite;
    public Sprite defaultSprite;

  
    public enum EditType
    {
        PaintModel = 0,           // 0000
        LocateModel = 1,           // 0001
        
    }
    private static int BlockCount = 0;
    private Color[] morandiColors = new Color[16];
    private int colorcount = 0;
    // Start is called before the first frame update

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;


    public Color bcolor;

    /*public bool PaintModel;
    public bool LocateModel;*/
    public EditType editType;
    public string levelname;
    public int steps;
    public int rows;
    public int columns;
    
    public BlockInJson[] BlocksInEditor;

    
    void Awake()
    {
        // 获取Canvas上的GraphicRaycaster组件
        raycaster = FindObjectOfType<GraphicRaycaster>();
        // 获取场景中的EventSystem
        eventSystem = FindObjectOfType<EventSystem>();
    }
    void Start()
    {
        CreateColorCollections();
        CreateButtonZone();

    }


    // Update is called once per frame
    void Update()
    {
        if(editType  == EditType.PaintModel)
        {
            if (Input.GetMouseButton(0)) // 
            {

                pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results);

                foreach (RaycastResult result in results)
                {

                    // 尝试获取Button组件
                    Button button = result.gameObject.GetComponent<Button>();
                    if (button != null)
                    {

                        button.GetComponent<Image>().color = bcolor;
                    }
                }
            }
            if (Input.GetMouseButton(1)) // 检测是否持续按住鼠标右键
            {

                pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results);

                foreach (RaycastResult result in results)
                {

                    // 尝试获取Button组件
                    Button button = result.gameObject.GetComponent<Button>();
                    if (button != null)
                    {

                        button.GetComponent<Image>().color = Color.black;
                    }
                }
            }
            if (Input.GetMouseButtonDown(2))
            {
                bcolor = morandiColors[++colorcount % 16];
                Debug.Log("colorchange" + colorcount);

            }
        }
        if (editType == EditType.LocateModel)
        {
            if (Input.GetMouseButtonDown(0)&&Input.GetKey(KeyCode.Q)) // 确定左上角角点
            {

                pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results);

                foreach (RaycastResult result in results)
                {

                    // 尝试获取Button组件
                    Button button = result.gameObject.GetComponent<Button>();
                    if (button != null)
                    {
                        //Sprite pointupleftSprite = Sprite.Create(pointimage, new Rect(0.0f, 0.0f, pointimage.width, pointimage.height), new Vector2(0.5f, 0.5f));
                        button.GetComponent<Image>().sprite = pointupleftSprite;

                        string name = result.gameObject.name; // 获取GameObject的名称
                        (bool r, int n1, int n2)  = SliceIntFromName(name);
                        if (r)
                        {
                            result.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = $"{n1},{n2}";
                        }
                        
                    }
                }
            }
            if (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.S)) 
            {

                pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results);

                foreach (RaycastResult result in results)
                {

                    Button button = result.gameObject.GetComponent<Button>();

                    if (button != null)
                    {
                        button.GetComponent<Image>().sprite = pointbottomrightSprite;

                        string name = result.gameObject.name; 
                        (bool r, int n1, int n2) = SliceIntFromName(name);
                        if (r)
                        {
                            result.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = $"{n1},{n2}";
                        }

                    }
                }
            }
            if (Input.GetMouseButtonDown(1)) 
            {

                pointerEventData = new PointerEventData(eventSystem);
                pointerEventData.position = Input.mousePosition;

                List<RaycastResult> results = new List<RaycastResult>();
                raycaster.Raycast(pointerEventData, results);

                foreach (RaycastResult result in results)
                {

                    // 尝试获取Button组件
                    Button button = result.gameObject.GetComponent<Button>();
                    if (button != null)
                    {
                        button.GetComponent<Image>().sprite = defaultSprite;
                        result.gameObject.transform.GetChild(0).gameObject.GetComponent<Text>().text = "";
                    }
                }
            }
        }
        
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

                /*  //对所有按钮设置索引
                  Global.Instance.ButtonCollection[row, col] = button;
  */
                // 设置按钮的位置
                RectTransform rectTransform = button.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(col * buttonWidth - columns * buttonWidth / 2, -row * buttonHeight + rows * buttonHeight / 2);



                // 添加按钮点击事件
                Button buttonComponent = button.GetComponent<Button>();
                int rowIndex = row;
                int colIndex = col;
                buttonComponent.onClick.AddListener(() => OnButtonClick(rowIndex, colIndex, button));

                //Sprite defaultSprite = Sprite.Create(defaultimage, new Rect(0.0f, 0.0f, defaultimage.width, defaultimage.height), new Vector2(0.5f, 0.5f));

                //defaultSprite=button.GetComponent<Image>().sprite;

            }
        }
    }
    void OnButtonClick(int rowIndex, int colIndex,GameObject button)
    {
        /*if (!PaintModel)
        {
            if (Input.GetMouseButton(0))
            {
                Sprite pointupleftSprite = Sprite.Create(pointimage, new Rect(0.0f, 0.0f, pointimage.width, pointimage.height), new Vector2(0.5f, 0.5f));

                button.GetComponent<Image>().sprite = pointupleftSprite;

            }
            if (Input.GetMouseButton(1))
            {
                Sprite defaultSprite = Sprite.Create(defaultimage, new Rect(0.0f, 0.0f, defaultimage.width, defaultimage.height), new Vector2(0.5f, 0.5f));

                button.GetComponent<Image>().sprite = defaultSprite;

            }
            Debug.Log(rowIndex + " " + colIndex);
            
        }*/


    }

    public void OnSaveButtonClick()
    {
        Debug.Log("hhhhhh"+levelname);
        SaveLevelToJson(CreataLevelData());
    }

    LevelDataInJson CreataLevelData()
    {
        LevelDataInJson leveldata = new LevelDataInJson();
        leveldata.RemainSteps = steps;
        leveldata.Rows = rows;
        leveldata.Columns = columns;
        foreach (BlockInJson b in BlocksInEditor)
        {
            Debug.Log(b.p1_UpLeft + "--" + b.p2_BottomRight + "--" + b.BlockType);
            leveldata.Blocks.Add (b);
            
        }

        return leveldata;
    }
    void SaveLevelToJson(LevelDataInJson data)
    {
        string json = JsonUtility.ToJson(data);

        // 定义文件路径。注意：这不是Resources文件夹，因为在运行时不能向其写入。
        string path = Path.Combine(Application.persistentDataPath, levelname+".json");

        File.WriteAllText(path, json);

        Debug.Log($"Data saved to {path}");
    }
    public (bool, int, int) SliceIntFromName(string input)
    {
        int n1=-1, n2=-1;
        bool success = true; // 假设的解析逻辑

        string name = input; // 获取GameObject的名称

        // 使用下划线分割名称
        string[] parts = name.Split('_');

        // 检查分割后的数组长度是否足够
        if (parts.Length >= 3) // 确保名称符合预期的格式
        {
            string n1Str = parts[parts.Length - 2]; // 倒数第二部分是n1
            string n2Str = parts[parts.Length - 1]; // 最后一部分是n2

            // 尝试转换为整数
            if (int.TryParse(n1Str, out int x1) && int.TryParse(n2Str, out int x2))
            {
                Debug.Log($"n1: {x1}, n2: {x2}");
                n1=x1 ; 
                n2=x2 ;
            }
            else
            {
                Debug.LogError("无法将名称的一部分转换为整数");
                success = false;
            }
        }
        else
        {
            Debug.LogError("GameObject的名称不符合预期的格式: " + name);
            success = false;
        }

        if (success)
        {
            return (true, n1, n2);
        }
        return (false, -1, -1);
    }
}




