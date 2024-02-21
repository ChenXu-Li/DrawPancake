using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EditorLevel : MonoBehaviour
{
    public GameObject canvas;

    public GameObject buttonPrefab;

    public int rows;
    public int columns;

    public Color bcolor;

    private static int BlockCount = 0;
    private Color[] morandiColors = new Color[16];
    // Start is called before the first frame update

    private GraphicRaycaster raycaster;
    private PointerEventData pointerEventData;
    private EventSystem eventSystem;
    void Awake()
    {
        // ��ȡ��ǰ�����е�GraphicRaycaster�����EventSystem
        // ��ȡCanvas�ϵ�GraphicRaycaster���
        raycaster = FindObjectOfType<GraphicRaycaster>();
        // ��ȡ�����е�EventSystem
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
        if (Input.GetMouseButton(0)) // 
        {
            
            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);

            foreach (RaycastResult result in results)
            {
                
                // ���Ի�ȡButton���
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null)
                {
                    Debug.Log("a");
                    button.GetComponent<Image>().color = bcolor;
                    // �ı�Button����ɫ
                    /*ColorBlock colorBlock = button.colors;
                    colorBlock.normalColor = Color.red; // ����Ϊ��ɫ����ϣ�����κ���ɫ
                    button.colors = colorBlock;*/
                }
            }
        }
        if (Input.GetMouseButton(1)) // ����Ƿ������ס����Ҽ�
        {

            pointerEventData = new PointerEventData(eventSystem);
            pointerEventData.position = Input.mousePosition;

            List<RaycastResult> results = new List<RaycastResult>();
            raycaster.Raycast(pointerEventData, results);

            foreach (RaycastResult result in results)
            {

                // ���Ի�ȡButton���
                Button button = result.gameObject.GetComponent<Button>();
                if (button != null)
                {
                    Debug.Log("a");
                    button.GetComponent<Image>().color = Color.black;
                    // �ı�Button����ɫ
                    /*ColorBlock colorBlock = button.colors;
                    colorBlock.normalColor = Color.red; // ����Ϊ��ɫ����ϣ�����κ���ɫ
                    button.colors = colorBlock;*/
                }
            }
        }
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

                /*  //�����а�ť��������
                  Global.Instance.ButtonCollection[row, col] = button;
  */
                // ���ð�ť��λ��
                RectTransform rectTransform = button.GetComponent<RectTransform>();
                rectTransform.anchoredPosition = new Vector2(col * buttonWidth - columns * buttonWidth / 2, -row * buttonHeight + rows * buttonHeight / 2);



                // ��Ӱ�ť����¼�
                Button buttonComponent = button.GetComponent<Button>();
                int rowIndex = row;
                int colIndex = col;
                //buttonComponent.onClick.AddListener(() => OnButtonClick(rowIndex, colIndex, buttonComponent));
               
            }
        }
    }
    void OnButtonClick(int rowIndex, int colIndex,Button buttonComponent)
    {
        Debug.Log(rowIndex + " " + colIndex);
        buttonComponent.GetComponent<Image>().color = bcolor;

    }

}
