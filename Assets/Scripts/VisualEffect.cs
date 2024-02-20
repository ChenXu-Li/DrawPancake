using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.UI;


public class VisualEffect: MonoBehaviour
{
    public static GameObject ArrowPrefab;
    public static GameObject CanvasArrow;
    
    public static Text RemainStepsText;

    public static void UpdateRemainingStepsDisplay()
    {
        // 更新Text组件的内容
        RemainStepsText.text = "Remaining Steps:\n " + Global.Instance.RemainSteps;
    }


    protected static float arrowRotate(XDirection xd)
    {
        float temp = 0;
        if (xd== XDirection.LEFTDOWN) { temp = 0; }
        if (xd == XDirection.RIGHTDOWN) { temp = 90; }
        if (xd == XDirection.RIGHTUP) { temp = 180; }
        if (xd== XDirection.LEFTUP) { temp = 270; }
        return temp;
    }
    protected static float arrowRotate(IDirection id)
    {
        float temp = 0;
        if (id == IDirection.DOWN) { temp = 45; }
        if (id == IDirection.RIGHT) { temp = 135; }
        if (id == IDirection.UP) { temp = 225; }
        if (id == IDirection.LEFT) { temp = 315; }
        return temp;
    }

    protected static GameObject InstantiateArrowPrefab(Vector2 position, float zRotation)
    {
        // 实例化预制体，设置canvas为父对象
        GameObject instance = Instantiate(ArrowPrefab, CanvasArrow.transform, false);

        // 设置位置
        RectTransform rectTransform = instance.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = position;

        // 设置旋转
        rectTransform.localRotation = Quaternion.Euler(0, 0, zRotation);
        return instance;
    }
    protected static Vector2 TransformPosition(int row,int col)//矩阵xy与rect transform的xy是相反的,矩阵x从左上到左下递增，y从左上到右上递增
    {
        Vector2 anchor_position = new Vector2(col * Global.Instance.ButtonWidth - Global.Instance.Columns * Global.Instance.ButtonWidth / 2, -row * Global.Instance.ButtonHeight + Global.Instance.Rows * Global.Instance.ButtonHeight / 2);
        return  anchor_position;
    }

    public static void ArrowCreate(ITypeBlock b)
    {
        Vector2 temp_anchorposition = TransformPosition(b.Position.x, b.Position.y);
        if ((b.EnableDirs & IDirection.UP) == IDirection.UP)
        {           
            float temp_zrotation = arrowRotate(IDirection.UP);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(IDirection.UP, temp_arrow);
        }
        if ((b.EnableDirs & IDirection.DOWN) == IDirection.DOWN)
        {           
            float temp_zrotation = arrowRotate(IDirection.DOWN);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(IDirection.DOWN, temp_arrow);
        }
        if ((b.EnableDirs & IDirection.RIGHT) == IDirection.RIGHT)
        {            
            float temp_zrotation = arrowRotate(IDirection.RIGHT);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(IDirection.RIGHT, temp_arrow);
        }
        if ((b.EnableDirs & IDirection.LEFT) == IDirection.LEFT)
        {            
            float temp_zrotation = arrowRotate(IDirection.LEFT);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(IDirection.LEFT, temp_arrow);
        }
    }
    public static void ArrowCreate(XTypeBlock b)
    {
        Vector2 temp_anchorposition = TransformPosition(b.Position.x, b.Position.y);
        if ((b.EnableDirs & XDirection.LEFTDOWN) == XDirection.LEFTDOWN)
        {
            float temp_zrotation = arrowRotate(XDirection.LEFTDOWN);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(XDirection.LEFTDOWN, temp_arrow);
        }
        if ((b.EnableDirs & XDirection.RIGHTDOWN) == XDirection.RIGHTDOWN)
        {
            float temp_zrotation = arrowRotate(XDirection.RIGHTDOWN);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(XDirection.RIGHTDOWN, temp_arrow);
        }
        if ((b.EnableDirs & XDirection.RIGHTUP) == XDirection.RIGHTUP)
        {
            float temp_zrotation = arrowRotate(XDirection.RIGHTUP);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(XDirection.RIGHTUP, temp_arrow);
        }
        if ((b.EnableDirs & XDirection.LEFTUP) == XDirection.LEFTUP)
        {
            float temp_zrotation = arrowRotate(XDirection.LEFTUP);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(XDirection.LEFTUP, temp_arrow);
        }
    }

    public static void ArrowUpdate(ITypeBlock b)
    {
        if (b.ifExpanded)
        {
            if ((b.EnableDirs & IDirection.UP) == IDirection.UP)
            {
                int temp_x = b.Position.x - b.ExpandedStep;
                int temp_y = b.Position.y;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.UP];

                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & IDirection.DOWN) == IDirection.DOWN)
            {
                int temp_x = b.Position.x + b.ExpandedStep;
                int temp_y = b.Position.y;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.DOWN];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & IDirection.RIGHT) == IDirection.RIGHT)
            {
                int temp_x = b.Position.x ;
                int temp_y = b.Position.y + b.ExpandedStep;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.RIGHT];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;

            }
            if ((b.EnableDirs & IDirection.LEFT) == IDirection.LEFT)
            {
                int temp_x = b.Position.x;
                int temp_y = b.Position.y - b.ExpandedStep;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.LEFT];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;

            }
        }
        else
        {
            if ((b.EnableDirs & IDirection.UP) == IDirection.UP)
            {
                int temp_x = b.Position.x;
                int temp_y = b.Position.y;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.UP];

                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & IDirection.DOWN) == IDirection.DOWN)
            {
                int temp_x = b.Position.x ;
                int temp_y = b.Position.y;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.DOWN];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & IDirection.RIGHT) == IDirection.RIGHT)
            {
                int temp_x = b.Position.x;
                int temp_y = b.Position.y; ;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.RIGHT];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;

            }
            if ((b.EnableDirs & IDirection.LEFT) == IDirection.LEFT)
            {
                int temp_x = b.Position.x;
                int temp_y = b.Position.y;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.LEFT];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;

            }
        }
    }

    public static void ArrowUpdate(XTypeBlock b)
    {
        if (b.ifExpanded)
        {
            if ((b.EnableDirs & XDirection.LEFTUP) == XDirection.LEFTUP)
            {
                int temp_x = b.Position.x - b.ExpandedStep;
                int temp_y = b.Position.y - b.ExpandedStep;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.LEFTUP];

                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & XDirection.RIGHTDOWN) == XDirection.RIGHTDOWN)
            {
                int temp_x = b.Position.x + b.ExpandedStep;
                int temp_y = b.Position.y + b.ExpandedStep;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.RIGHTDOWN];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & XDirection.RIGHTUP) == XDirection.RIGHTUP)
            {
                int temp_x = b.Position.x - b.ExpandedStep;
                int temp_y = b.Position.y + b.ExpandedStep;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.RIGHTUP];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;

            }
            if ((b.EnableDirs & XDirection.LEFTDOWN) == XDirection.LEFTDOWN)
            {
                int temp_x = b.Position.x + b.ExpandedStep;
                int temp_y = b.Position.y - b.ExpandedStep;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.LEFTDOWN];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;

            }
        }
        else
        {
            if ((b.EnableDirs & XDirection.LEFTUP) == XDirection.LEFTUP)
            {
                int temp_x = b.Position.x;
                int temp_y = b.Position.y;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.LEFTUP];

                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & XDirection.RIGHTDOWN) == XDirection.RIGHTDOWN)
            {
                int temp_x = b.Position.x;
                int temp_y = b.Position.y;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.RIGHTDOWN];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & XDirection.RIGHTUP) == XDirection.RIGHTUP)
            {
                int temp_x = b.Position.x;
                int temp_y = b.Position.y; ;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.RIGHTUP];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;

            }
            if ((b.EnableDirs & XDirection.LEFTDOWN) == XDirection.LEFTDOWN)
            {
                int temp_x = b.Position.x;
                int temp_y = b.Position.y;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.LEFTDOWN];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;

            }
        }
    }
}
