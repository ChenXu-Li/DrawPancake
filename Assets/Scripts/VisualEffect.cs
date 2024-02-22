using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
//using UnityEngine.UIElements;
using UnityEngine.UI;
using Unity.Mathematics;
using System;


public class VisualEffect: MonoBehaviour
{
    public static GameObject ArrowPrefab;
    public static GameObject CanvasArrow;
    
    public static Text RemainStepsText;

    public static void UpdateRemainingStepsDisplay()
    {
        // 更新Text组件的内容
        RemainStepsText.text = "Remaining Steps:\n "+ Global.Instance.RemainSteps;
        //Debug.Log("step----"/*+ RemainStepsText.text*/);
    }
    public static void FillButtonColor(int2 p1,int2 p2,Color c)

    {
        if (p1.x <= p2.x && p1.y <= p2.y)
        {
            for (int i = p1.x; i <= p2.x; i++)
            {
                for (int j = p1.y; j <= p2.y; j++)
                {
                    Global.Instance.ButtonCollection[i, j].GetComponent<Image>().color = c;
                }
            }
        }
        else
        {
            Debug.Log("sfsagsabxcgag");
        }

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
    protected static Vector2 TransformPosition(float row,float col)//矩阵xy与rect transform的xy是相反的,矩阵x从左上到左下递增，y从左上到右上递增
    {
        Vector2 anchor_position = new Vector2(col * Global.Instance.ButtonWidth - Global.Instance.Columns * Global.Instance.ButtonWidth / 2, -row * Global.Instance.ButtonHeight + Global.Instance.Rows * Global.Instance.ButtonHeight / 2);
        return  anchor_position;
    }

/*    public static void ArrowCreate(ITypeBlock b)
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
    }*/
    public static void ArrowCreate(ITypeBlock b)
    {
        
        if ((b.EnableDirs & IDirection.UP) == IDirection.UP)
        {
            Vector2 temp_anchorposition = TransformPosition(b.PositionTopLeft.x, (b.PositionBottomRight.y + b.PositionTopLeft.y) / 2f);
            float temp_zrotation = arrowRotate(IDirection.UP);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(IDirection.UP, temp_arrow);
        }
        if ((b.EnableDirs & IDirection.DOWN) == IDirection.DOWN)
        {
            Vector2 temp_anchorposition = TransformPosition(b.PositionBottomRight.x , (b.PositionBottomRight.y + b.PositionTopLeft.y) / 2f);
            float temp_zrotation = arrowRotate(IDirection.DOWN);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(IDirection.DOWN, temp_arrow);
        }
        if ((b.EnableDirs & IDirection.RIGHT) == IDirection.RIGHT)
        {
            Vector2 temp_anchorposition = TransformPosition((b.PositionBottomRight.x + b.PositionTopLeft.x) / 2f, b.PositionBottomRight.y);
            float temp_zrotation = arrowRotate(IDirection.RIGHT);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(IDirection.RIGHT, temp_arrow);
        }
        if ((b.EnableDirs & IDirection.LEFT) == IDirection.LEFT)
        {
            Vector2 temp_anchorposition = TransformPosition((b.PositionBottomRight.x + b.PositionTopLeft.x) / 2f, b.PositionTopLeft.y);
            float temp_zrotation = arrowRotate(IDirection.LEFT);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(IDirection.LEFT, temp_arrow);
        }
    }
    public static void ArrowCreate(XTypeBlock b)
    {
        
        if ((b.EnableDirs & XDirection.LEFTDOWN) == XDirection.LEFTDOWN)
        {
            Vector2 temp_anchorposition = TransformPosition(b.PositionBottomRight.x, b.PositionTopLeft.y);
            float temp_zrotation = arrowRotate(XDirection.LEFTDOWN);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(XDirection.LEFTDOWN, temp_arrow);
        }
        if ((b.EnableDirs & XDirection.RIGHTDOWN) == XDirection.RIGHTDOWN)
        {
            Vector2 temp_anchorposition = TransformPosition(b.PositionBottomRight.x, b.PositionBottomRight.y);
            float temp_zrotation = arrowRotate(XDirection.RIGHTDOWN);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(XDirection.RIGHTDOWN, temp_arrow);
        }
        if ((b.EnableDirs & XDirection.RIGHTUP) == XDirection.RIGHTUP)
        {
            Vector2 temp_anchorposition = TransformPosition(b.PositionTopLeft.x, b.PositionBottomRight.y);
            float temp_zrotation = arrowRotate(XDirection.RIGHTUP);
            GameObject temp_arrow = InstantiateArrowPrefab(temp_anchorposition, temp_zrotation);
            b.ArrowDictionary.Add(XDirection.RIGHTUP, temp_arrow);
        }
        if ((b.EnableDirs & XDirection.LEFTUP) == XDirection.LEFTUP)
        {
            Vector2 temp_anchorposition = TransformPosition(b.PositionTopLeft.x, b.PositionTopLeft.y);
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
                float temp_x = b.PositionTopLeft.x - b.ExpandedStep;
                float temp_y = (b.PositionBottomRight.y + b.PositionTopLeft.y) / 2f;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.UP];

                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & IDirection.DOWN) == IDirection.DOWN)
            {
                float temp_x = b.PositionBottomRight.x + b.ExpandedStep;
                float temp_y = (b.PositionBottomRight.y + b.PositionTopLeft.y) / 2f;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.DOWN];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & IDirection.RIGHT) == IDirection.RIGHT)
            {
                float temp_x = (b.PositionBottomRight.x + b.PositionTopLeft.x) / 2f;
                float temp_y = b.PositionBottomRight.y + b.ExpandedStep;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.RIGHT];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;

            }
            if ((b.EnableDirs & IDirection.LEFT) == IDirection.LEFT)
            {
                float temp_x = (b.PositionBottomRight.x + b.PositionTopLeft.x) / 2f;
                float temp_y = b.PositionTopLeft.y - b.ExpandedStep;
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
                float temp_x = b.PositionTopLeft.x;
                float temp_y = (b.PositionBottomRight.y + b.PositionTopLeft.y) / 2f;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.UP];

                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & IDirection.DOWN) == IDirection.DOWN)
            {
                float temp_x = b.PositionBottomRight.x;
                float temp_y = (b.PositionBottomRight.y + b.PositionTopLeft.y) / 2f;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.DOWN];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & IDirection.RIGHT) == IDirection.RIGHT)
            {
                float temp_x = (b.PositionBottomRight.x + b.PositionTopLeft.x) / 2f;
                float temp_y = b.PositionBottomRight.y; 
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[IDirection.RIGHT];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;

            }
            if ((b.EnableDirs & IDirection.LEFT) == IDirection.LEFT)
            {
                float temp_x = (b.PositionBottomRight.x + b.PositionTopLeft.x) / 2f;
                float temp_y = b.PositionTopLeft.y;
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
                int temp_x = b.PositionTopLeft.x - b.ExpandedStep;
                int temp_y = b.PositionTopLeft.y - b.ExpandedStep;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.LEFTUP];

                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & XDirection.RIGHTDOWN) == XDirection.RIGHTDOWN)
            {
                int temp_x = b.PositionBottomRight.x + b.ExpandedStep;
                int temp_y = b.PositionBottomRight.y + b.ExpandedStep;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.RIGHTDOWN];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & XDirection.RIGHTUP) == XDirection.RIGHTUP)
            {
                int temp_x = b.PositionTopLeft.x - b.ExpandedStep;
                int temp_y = b.PositionBottomRight.y + b.ExpandedStep;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.RIGHTUP];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, 180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;

            }
            if ((b.EnableDirs & XDirection.LEFTDOWN) == XDirection.LEFTDOWN)
            {
                int temp_x = b.PositionBottomRight.x + b.ExpandedStep;
                int temp_y = b.PositionTopLeft.y - b.ExpandedStep;
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
                int temp_x = b.PositionTopLeft.x; 
                int temp_y = b.PositionTopLeft.y;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.LEFTUP];

                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & XDirection.RIGHTDOWN) == XDirection.RIGHTDOWN)
            {
                int temp_x = b.PositionBottomRight.x;
                int temp_y = b.PositionBottomRight.y;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.RIGHTDOWN];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;


            }
            if ((b.EnableDirs & XDirection.RIGHTUP) == XDirection.RIGHTUP)
            {
                int temp_x = b.PositionTopLeft.x;
                int temp_y = b.PositionBottomRight.y;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.RIGHTUP];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;

            }
            if ((b.EnableDirs & XDirection.LEFTDOWN) == XDirection.LEFTDOWN)
            {
                int temp_x = b.PositionBottomRight.x; 
                int temp_y = b.PositionTopLeft.y;
                Vector2 temp_position = TransformPosition(temp_x, temp_y);
                GameObject temp_arrow = b.ArrowDictionary[XDirection.LEFTDOWN];
                temp_arrow.GetComponent<RectTransform>().Rotate(0, 0, -180);
                temp_arrow.GetComponent<RectTransform>().anchoredPosition = temp_position;

            }
        }
    }


}
