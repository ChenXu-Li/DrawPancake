using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

[Flags]
public enum IDirection
{
    None = 0,           // 0000
    UP = 1,           // 0001
    DOWN = 2,          // 0010
    LEFT = 4,         // 0100
    RIGHT = 8      
}

[Flags]
public enum XDirection
{
    None = 0,           // 0000
    LEFTUP = 1,           // 0001
    RIGHTUP = 2,          // 0010
    LEFTDOWN = 4,         // 0100
    RIGHTDOWN = 8           //1000
}


public class BaseBlock
{
    public int ExpandedStep;
    protected int NumberIndex;
    public int2 Position;//从（0，0）到（n-1，m-1）
    public int2 PositionTopLeft;//从（0，0）到（n-1，m-1）
    public int2 PositionBottomRight;//从（0，0）到（n-1，m-1）

    public bool ifExpanded;
    public Color SquareColor;

    protected virtual void Expand()
    {
        
    }
    protected virtual void Shrink()
    {

    }

    public void Click()
    {
        if (ifExpanded)
        {
            Shrink ();
            ifExpanded = false;
        }
        else
        {
            Expand ();
            ifExpanded =  true;
        }
        Debug.Log("click ");
    }
}
public class ITypeBlock : BaseBlock
{
    public IDirection EnableDirs;
    public Dictionary<IDirection, GameObject> ArrowDictionary = new Dictionary<IDirection, GameObject>();

    /*public ITypeBlock(int x, int y, int initial_step,int num,IDirection Dirs,Color c)
    {
        Position =  new int2(x,y);
        ExpandedStep = initial_step;
        EnableDirs = Dirs;
        NumberIndex = num;
        if (initial_step == 0) { ifExpanded = false; } else { ifExpanded = true; }
        Global.Instance.StateCollection[x + 1, y + 1] = num;
        SquareColor = c;
    }*/
    public ITypeBlock(int2 p1, int2 p2, int initial_step, int num, IDirection Dirs, Color c)
    {
        //Position = new int2(x, y);
        PositionTopLeft = p1;
        PositionBottomRight = p2;
        
        ExpandedStep = initial_step;
        EnableDirs = Dirs;
        NumberIndex = num;
        if (initial_step == 0) { ifExpanded = false; } else { ifExpanded = true; }
        
        for( int i = PositionTopLeft.x; i <= PositionBottomRight.x; i++ )
        {
            for  ( int j = PositionTopLeft.y; j <= PositionBottomRight.y; j++ )
            {
                Global.Instance.StateCollection[i + 1, j + 1] = num;
            }
        }
        SquareColor = c;
    }

    /*    protected override void Expand()
        {
            Debug.Log("ITypeBlock Expand ");
            int s=ExpandForesee();
            ExpandedStep += s;

            int temp = 1;
            while (temp <= ExpandedStep)
            {
                if ((EnableDirs & IDirection.UP) == IDirection.UP)
                {
                    Global.Instance.StateCollection[Position.x - temp+1, Position.y+1] = NumberIndex;
                    Global.Instance.ButtonCollection[Position.x - temp, Position.y].GetComponent<Image>().color = SquareColor;
                }
                if ((EnableDirs & IDirection.DOWN) == IDirection.DOWN)
                {
                    Global.Instance.StateCollection[Position.x + temp + 1, Position.y + 1] = NumberIndex;
                    Global.Instance.ButtonCollection[Position.x + temp, Position.y].GetComponent<Image>().color = SquareColor;
                }
                if ((EnableDirs & IDirection.RIGHT) == IDirection.RIGHT)
                {
                    Global.Instance.StateCollection[Position.x + 1, Position.y + temp + 1] = NumberIndex;
                    Global.Instance.ButtonCollection[Position.x, Position.y + temp].GetComponent<Image>().color = SquareColor;
                }
                if ((EnableDirs & IDirection.LEFT) == IDirection.LEFT)
                {
                    Global.Instance.StateCollection[Position.x + 1, Position.y - temp + 1] = NumberIndex;
                    Global.Instance.ButtonCollection[Position.x, Position.y - temp].GetComponent<Image>().color = SquareColor;
                }
                temp++;
            }
            ifExpanded = true;

        }*/
    protected override void Expand()
    {
        Debug.Log("ITypeBlock Expand ");
        int s = ExpandForesee();
        ExpandedStep += s;

        if ((EnableDirs & IDirection.UP) == IDirection.UP)
        {
            int2 tempp1 = new int2(PositionTopLeft.x - s, PositionTopLeft.y);
            int2 tempp2 = new int2(PositionTopLeft.x - 1, PositionBottomRight.y);

            Global.Instance.FillStateCollection(new int2(tempp1.x+1,tempp1.y+1), new int2(tempp2.x + 1, tempp2.y + 1),NumberIndex);
            VisualEffect.FillButtonColor(tempp1, tempp2, SquareColor);       
        }
        if ((EnableDirs & IDirection.DOWN) == IDirection.DOWN)
        {
            int2 tempp1 = new int2(PositionBottomRight.x + 1, PositionTopLeft.y);
            int2 tempp2 = new int2(PositionBottomRight.x + s, PositionBottomRight.y);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), NumberIndex);
            VisualEffect.FillButtonColor(tempp1, tempp2, SquareColor);
        }
        if ((EnableDirs & IDirection.RIGHT) == IDirection.RIGHT)
        {
            int2 tempp1 = new int2(PositionTopLeft.x, PositionBottomRight.y+1);
            int2 tempp2 = new int2(PositionBottomRight.x, PositionBottomRight.y+s);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), NumberIndex);
            VisualEffect.FillButtonColor(tempp1, tempp2, SquareColor);
        }
        if ((EnableDirs & IDirection.LEFT) == IDirection.LEFT)
        {
            int2 tempp1 = new int2(PositionTopLeft.x, PositionTopLeft.y - s);
            int2 tempp2 = new int2(PositionBottomRight.x, PositionTopLeft.y - 1);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), NumberIndex);
            VisualEffect.FillButtonColor(tempp1, tempp2, SquareColor);
        }

        ifExpanded = true;

    }
    /*    protected int ExpandForesee()
        {
            int s=999;
            if ((EnableDirs & IDirection.UP) == IDirection.UP)
            {
                int temp = 0;
                while( true)
                {
                    temp++;
                    if (Global.Instance.StateCollection[Position.x - temp + 1, Position.y + 1] != 0)
                    {
                        break;
                    }

                }
                s = (s < temp-1) ? s: temp-1;
            }
            if ((EnableDirs & IDirection.DOWN) == IDirection.DOWN)
            {
                int temp = 0;
                while (true)
                {
                    temp++;
                    if (Global.Instance.StateCollection[Position.x + temp + 1, Position.y + 1] != 0)
                    {
                        break;
                    }

                }
                s = (s < temp - 1) ? s : temp - 1;
            }
            if ((EnableDirs & IDirection.RIGHT) == IDirection.RIGHT)
            {
                int temp = 0;
                while (true)
                {
                    temp++;
                    if (Global.Instance.StateCollection[Position.x + 1, Position.y + temp + 1] != 0)
                    {
                        break;
                    }

                }
                s = (s < temp - 1) ? s : temp - 1;
            }
            if ((EnableDirs & IDirection.LEFT) == IDirection.LEFT)
            {
                int temp = 0;
                while (true)
                {
                    temp++;
                    if (Global.Instance.StateCollection[Position.x + 1, Position.y - temp + 1] != 0)
                    {
                        break;
                    }

                }
                s = (s < temp - 1) ? s : temp - 1;
            }
            Debug.Log("ITypeBlock ForeSee "+s);
            return s;
        }*/
    protected int ExpandForesee()
    {
 
       

        int s = 0;
        while (true)
        {
            s += 1;
            bool crushed = false;
            if ((EnableDirs & IDirection.UP) == IDirection.UP)
            {
                for (int i = PositionTopLeft.x - s, j = PositionTopLeft.y; j <= PositionBottomRight.y; j++)//上
                {
                    if (Global.Instance.StateCollection[i + 1, j + 1] != 0)
                    {
                        crushed = true;
                    }
                }

            }
            if ((EnableDirs & IDirection.DOWN) == IDirection.DOWN)
            {
                for (int i = PositionBottomRight.x + s, j = PositionTopLeft.y; j <= PositionBottomRight.y; j++)//下
                {
                    if (Global.Instance.StateCollection[i + 1, j + 1] != 0)
                    {
                        crushed = true;
                    }
                }
   

            }
            if ((EnableDirs & IDirection.RIGHT) == IDirection.RIGHT)
            {
                for (int i = PositionTopLeft.x, j = PositionBottomRight.y + s; i <= PositionBottomRight.x; i++)//右
                {
                    if (Global.Instance.StateCollection[i + 1, j + 1] != 0)
                    {
                        crushed = true;
                    }
                }
      

            }
            if ((EnableDirs & IDirection.LEFT) == IDirection.LEFT)

            {
                for (int i = PositionTopLeft.x, j = PositionTopLeft.y - s; i <= PositionBottomRight.x; i++)//左
                {
                    if (Global.Instance.StateCollection[i + 1, j + 1] != 0)
                    {
                        crushed = true;
                    }
                }


            }
            if (crushed)
            {
                break;
            }

        }

        Debug.Log("ITypeBlock ForeSee " + (s-1));
        return s - 1;
    }
   /* protected override void Shrink()
    {
        Debug.Log("ITypeBlock Shrink ");
        int temp = 1;
        while (temp <= ExpandedStep)
        {
            if ((EnableDirs & IDirection.UP) == IDirection.UP)
            {
                Global.Instance.StateCollection[Position.x - temp + 1, Position.y + 1] = 0;
                Global.Instance.ButtonCollection[Position.x - temp, Position.y].GetComponent<Image>().color = Color.black;
            }
            if ((EnableDirs & IDirection.DOWN) == IDirection.DOWN)
            {
                Global.Instance.StateCollection[Position.x + temp + 1, Position.y + 1] = 0;
                Global.Instance.ButtonCollection[Position.x + temp, Position.y].GetComponent<Image>().color = Color.black;
            }
            if ((EnableDirs & IDirection.RIGHT) == IDirection.RIGHT)
            {
                Global.Instance.StateCollection[Position.x + 1, Position.y + temp + 1] = 0;
                Global.Instance.ButtonCollection[Position.x, Position.y + temp].GetComponent<Image>().color = Color.black;
            }
            if ((EnableDirs & IDirection.LEFT) == IDirection.LEFT)
            {
                Global.Instance.StateCollection[Position.x + 1, Position.y - temp + 1] = 0;
                Global.Instance.ButtonCollection[Position.x, Position.y - temp].GetComponent<Image>().color = Color.black;
            }
            temp++;
        }
        ExpandedStep = 0;
        ifExpanded = false;
    }*/
    protected override void Shrink()
    {
        Debug.Log("ITypeBlock Shrink ");
        int s = ExpandedStep;
        if ((EnableDirs & IDirection.UP) == IDirection.UP)
        {
            
            int2 tempp1 = new int2(PositionTopLeft.x - s, PositionTopLeft.y);
            int2 tempp2 = new int2(PositionTopLeft.x - 1, PositionBottomRight.y);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), 0);
            VisualEffect.FillButtonColor(tempp1, tempp2, Color.black);

            
        }
        if ((EnableDirs & IDirection.DOWN) == IDirection.DOWN)
        {
            int2 tempp1 = new int2(PositionBottomRight.x + 1, PositionTopLeft.y);
            int2 tempp2 = new int2(PositionBottomRight.x + s, PositionBottomRight.y);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), 0);
            VisualEffect.FillButtonColor(tempp1, tempp2, Color.black);
        }
        if ((EnableDirs & IDirection.RIGHT) == IDirection.RIGHT)
        {
            int2 tempp1 = new int2(PositionTopLeft.x, PositionBottomRight.y + 1);
            int2 tempp2 = new int2(PositionBottomRight.x, PositionBottomRight.y + s);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), 0);
            VisualEffect.FillButtonColor(tempp1, tempp2, Color.black);
        }
        if ((EnableDirs & IDirection.LEFT) == IDirection.LEFT)
        {
            int2 tempp1 = new int2(PositionTopLeft.x, PositionTopLeft.y - s);
            int2 tempp2 = new int2(PositionBottomRight.x, PositionTopLeft.y - 1);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), 0);
            VisualEffect.FillButtonColor(tempp1, tempp2, Color.black);
        }

        ExpandedStep = 0;
        ifExpanded = false;
    }



}
public class XTypeBlock : BaseBlock
{
    public XDirection EnableDirs;
    public Dictionary<XDirection, GameObject> ArrowDictionary = new Dictionary<XDirection, GameObject>();
    public XTypeBlock(int2 p1, int2 p2,int initial_step, int num, XDirection Dirs, Color c)
    {
        PositionTopLeft = p1;
        PositionBottomRight = p2;
        ExpandedStep = initial_step;
        EnableDirs = Dirs;
        NumberIndex = num;
        if (initial_step == 0) { ifExpanded = false; } else { ifExpanded = true; }
        for (int i = PositionTopLeft.x; i <= PositionBottomRight.x; i++)
        {
            for (int j = PositionTopLeft.y; j <= PositionBottomRight.y; j++)
            {
                Global.Instance.StateCollection[i + 1, j + 1] = num;
            }
        }
        SquareColor = c;
    }

    /*protected override void Expand()
    {
        Debug.Log("XTypeBlock Expand ");
        int s = ExpandForesee();
        ExpandedStep += s;

        int temp = 1;

        while (temp <= ExpandedStep)
        {
            if ((EnableDirs & XDirection.RIGHTUP) == XDirection.RIGHTUP)
            {
                for (int i = Position.x - temp, j = Position.y + temp; i <= Position.x; i++)//右上到右下
                {
                    Global.Instance.StateCollection[i + 1, j + 1] = NumberIndex;
                    Global.Instance.ButtonCollection[i, j].GetComponent<Image>().color = SquareColor;

                }
                for (int i = Position.x - temp, j = Position.y; j <= Position.y + temp; j++)//左上到右上
                {
                    Global.Instance.StateCollection[i + 1, j + 1] = NumberIndex;
                    Global.Instance.ButtonCollection[i, j].GetComponent<Image>().color = SquareColor;

                }

            }
            if ((EnableDirs & XDirection.LEFTUP) == XDirection.LEFTUP)
            {
                for (int i = Position.x - temp, j = Position.y - temp; i <= Position.x; i++)//左上到左下
                {
                    Global.Instance.StateCollection[i + 1, j + 1] = NumberIndex;
                    Global.Instance.ButtonCollection[i, j].GetComponent<Image>().color = SquareColor;

                }
                for (int i = Position.x - temp, j = Position.y - temp; j <= Position.y; j++)//左上到右上
                {
                    Global.Instance.StateCollection[i + 1, j + 1] = NumberIndex;
                    Global.Instance.ButtonCollection[i, j].GetComponent<Image>().color = SquareColor;

                }

            }
            if ((EnableDirs & XDirection.LEFTDOWN) == XDirection.LEFTDOWN)
            {
                for (int i = Position.x, j = Position.y - temp; i <= Position.x + temp; i++)//左上到左下
                {
                    Global.Instance.StateCollection[i + 1, j + 1] = NumberIndex;
                    Global.Instance.ButtonCollection[i, j].GetComponent<Image>().color = SquareColor;

                }
                for (int i = Position.x + temp, j = Position.y - temp; j <= Position.y; j++)//左下到右下
                {
                    Global.Instance.StateCollection[i + 1, j + 1] = NumberIndex;
                    Global.Instance.ButtonCollection[i, j].GetComponent<Image>().color = SquareColor;

                }

            }
            if ((EnableDirs & XDirection.RIGHTDOWN) == XDirection.RIGHTDOWN)
            {
                for (int i = Position.x, j = Position.y + temp; i <= Position.x + temp; i++)//右上到右下
                {
                    Global.Instance.StateCollection[i + 1, j + 1] = NumberIndex;
                    Global.Instance.ButtonCollection[i, j].GetComponent<Image>().color = SquareColor;

                }
                for (int i = Position.x + temp, j = Position.y; j <= Position.y + temp; j++)//左下到右下
                {
                    Global.Instance.StateCollection[i + 1, j + 1] = NumberIndex;
                    Global.Instance.ButtonCollection[i, j].GetComponent<Image>().color = SquareColor;

                }

            }
            temp++;

        }
        ifExpanded = true;
    }*/
    protected override void Expand()
    {
        Debug.Log("XTypeBlock Expand ");
        int s = ExpandForesee();
        ExpandedStep += s;


        if ((EnableDirs & XDirection.RIGHTUP) == XDirection.RIGHTUP)
        {

            int2 tempp1 = new int2(PositionTopLeft.x - s, PositionBottomRight.y + 1);
            int2 tempp2 = new int2(PositionBottomRight.x, PositionBottomRight.y + s);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), NumberIndex);
            VisualEffect.FillButtonColor(tempp1, tempp2, SquareColor);

            tempp1 = new int2(PositionTopLeft.x - s, PositionTopLeft.y);
            tempp2 = new int2(PositionTopLeft.x - 1, PositionBottomRight.y + s);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), NumberIndex);
            VisualEffect.FillButtonColor(tempp1, tempp2, SquareColor);

        }
        if ((EnableDirs & XDirection.LEFTUP) == XDirection.LEFTUP)
        {
            int2 tempp1 = new int2(PositionTopLeft.x - s, PositionTopLeft.y - s);
            int2 tempp2 = new int2(PositionTopLeft.x - 1, PositionBottomRight.y);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), NumberIndex);
            VisualEffect.FillButtonColor(tempp1, tempp2, SquareColor);

            tempp1 = new int2(PositionTopLeft.x - s, PositionTopLeft.y - s);
            tempp2 = new int2(PositionBottomRight.x, PositionTopLeft.y - 1);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), NumberIndex);
            VisualEffect.FillButtonColor(tempp1, tempp2, SquareColor);

        }
        if ((EnableDirs & XDirection.LEFTDOWN) == XDirection.LEFTDOWN)
        {
            int2 tempp1 = new int2(PositionTopLeft.x, PositionTopLeft.y - s);
            int2 tempp2 = new int2(PositionBottomRight.x + s, PositionTopLeft.y -1);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), NumberIndex);
            VisualEffect.FillButtonColor(tempp1, tempp2, SquareColor);

            tempp1 = new int2(PositionBottomRight.x + 1, PositionTopLeft.y - s);
            tempp2 = new int2(PositionBottomRight.x + s, PositionBottomRight.y );

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), NumberIndex);
            VisualEffect.FillButtonColor(tempp1, tempp2, SquareColor);


        }
        if ((EnableDirs & XDirection.RIGHTDOWN) == XDirection.RIGHTDOWN)
        {
            int2 tempp1 = new int2(PositionBottomRight.x + 1, PositionTopLeft.y);
            int2 tempp2 = new int2(PositionBottomRight.x + s, PositionBottomRight.y + s);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), NumberIndex);
            VisualEffect.FillButtonColor(tempp1, tempp2, SquareColor);

            tempp1 = new int2(PositionTopLeft.x, PositionBottomRight.y + 1);
            tempp2 = new int2(PositionBottomRight.x + s, PositionBottomRight.y + s);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), NumberIndex);
            VisualEffect.FillButtonColor(tempp1, tempp2, SquareColor);



        }
        ifExpanded = true;
    }
    protected int ExpandForesee()
    {
        int s = 0;
        while( true)
        {
            s += 1;
            bool crushed = false;
            if ((EnableDirs & XDirection.RIGHTUP) == XDirection.RIGHTUP)
            {
                for (int i = PositionTopLeft.x-s, j = PositionBottomRight.y + s; i <= PositionBottomRight.x; i++)//右上到右下
                {
                    if(Global.Instance.StateCollection[i + 1, j + 1] != 0)
                    {
                         crushed = true;
                    }
                }
                for (int i = PositionTopLeft.x-s, j = PositionTopLeft.y; j <= PositionBottomRight.y + s; j++)//左上到右上
                {
                    if (Global.Instance.StateCollection[i + 1, j + 1] != 0)
                    {
                        crushed = true;
                    }
                }

            }
            if ((EnableDirs & XDirection.LEFTUP) == XDirection.LEFTUP)
            {
                for (int i = PositionTopLeft.x - s, j = PositionTopLeft.y - s; i <= PositionBottomRight.x; i++)//左上到左下
                {
                    if (Global.Instance.StateCollection[i + 1, j + 1] != 0)
                    {
                        crushed = true;
                    }
                }
                for (int i = PositionTopLeft.x - s, j = PositionTopLeft.y-s; j <= PositionBottomRight.y; j++)//左上到右上
                {
                    if (Global.Instance.StateCollection[i + 1, j + 1] != 0)
                    {
                        crushed = true;
                    }
                }

            }
            if ((EnableDirs & XDirection.LEFTDOWN) == XDirection.LEFTDOWN)
            {
                for (int i = PositionTopLeft.x , j = PositionTopLeft.y - s; i <= PositionBottomRight.x+s; i++)//左上到左下
                {
                    if (Global.Instance.StateCollection[i + 1, j + 1] != 0)
                    {
                        crushed = true;
                    }
                }
                for (int i = PositionBottomRight.x + s, j = PositionTopLeft.y - s; j <= PositionBottomRight.y; j++)//左下到右下
                {
                    if (Global.Instance.StateCollection[i + 1, j + 1] != 0)
                    {
                        crushed = true;
                    }
                }

            }
            if ((EnableDirs & XDirection.RIGHTDOWN) == XDirection.RIGHTDOWN)
            {
                for (int i = PositionTopLeft.x, j = PositionBottomRight.y + s; i <= PositionBottomRight.x + s; i++)//右上到右下
                {
                    if (Global.Instance.StateCollection[i + 1, j + 1] != 0)
                    {
                        crushed = true;
                    }
                }
                for (int i = PositionBottomRight.x + s, j = PositionTopLeft.y ; j <= PositionBottomRight.y + s; j++)//左下到右下
                {
                    if (Global.Instance.StateCollection[i + 1, j + 1] != 0)
                    {
                        crushed = true;
                    }
                }

            }
            if (crushed)
            {
                break;
            }

        }
        
        Debug.Log("XTypeBlock ForeSee " + (s - 1));
        return s-1;
    }
    protected override void Shrink()
    {
        Debug.Log("XTypeBlock Shrink ");
        int s = ExpandedStep;
        if ((EnableDirs & XDirection.RIGHTUP) == XDirection.RIGHTUP)
        {
            int2 tempp1 = new int2(PositionTopLeft.x - s, PositionBottomRight.y + 1);
            int2 tempp2 = new int2(PositionBottomRight.x, PositionBottomRight.y + s);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), 0);
            VisualEffect.FillButtonColor(tempp1, tempp2, Color.black);

            tempp1 = new int2(PositionTopLeft.x - s, PositionTopLeft.y);
            tempp2 = new int2(PositionTopLeft.x - 1, PositionBottomRight.y + s);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), 0);
            VisualEffect.FillButtonColor(tempp1, tempp2, Color.black);

        }
        if ((EnableDirs & XDirection.LEFTUP) == XDirection.LEFTUP)
        {
            int2 tempp1 = new int2(PositionTopLeft.x - s, PositionTopLeft.y - s);
            int2 tempp2 = new int2(PositionTopLeft.x - 1, PositionBottomRight.y);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), 0);
            VisualEffect.FillButtonColor(tempp1, tempp2, Color.black);

            tempp1 = new int2(PositionTopLeft.x - s, PositionTopLeft.y - s);
            tempp2 = new int2(PositionBottomRight.x, PositionTopLeft.y - 1);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), 0);
            VisualEffect.FillButtonColor(tempp1, tempp2, Color.black);

        }
        if ((EnableDirs & XDirection.LEFTDOWN) == XDirection.LEFTDOWN)
        {
            int2 tempp1 = new int2(PositionTopLeft.x, PositionTopLeft.y - s);
            int2 tempp2 = new int2(PositionBottomRight.x + s, PositionTopLeft.y - 1);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), 0);
            VisualEffect.FillButtonColor(tempp1, tempp2, Color.black);

            tempp1 = new int2(PositionBottomRight.x + 1, PositionTopLeft.y - s);
            tempp2 = new int2(PositionBottomRight.x + s, PositionBottomRight.y);


            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), 0);
            VisualEffect.FillButtonColor(tempp1, tempp2, Color.black);

        }
        if ((EnableDirs & XDirection.RIGHTDOWN) == XDirection.RIGHTDOWN)
        {
            int2 tempp1 = new int2(PositionBottomRight.x + 1, PositionTopLeft.y);
            int2 tempp2 = new int2(PositionBottomRight.x + s, PositionBottomRight.y + s);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), 0);
            VisualEffect.FillButtonColor(tempp1, tempp2, Color.black);

            tempp1 = new int2(PositionTopLeft.x, PositionBottomRight.y + 1);
            tempp2 = new int2(PositionBottomRight.x + s, PositionBottomRight.y + s);

            Global.Instance.FillStateCollection(new int2(tempp1.x + 1, tempp1.y + 1), new int2(tempp2.x + 1, tempp2.y + 1), 0);
            VisualEffect.FillButtonColor(tempp1, tempp2, Color.black);

        }
            

        ExpandedStep = 0;
        ifExpanded = false;
    }


}

