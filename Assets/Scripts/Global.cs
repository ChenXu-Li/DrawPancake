using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.UI;
public class Global
{
    //私有构造函数
    private Global() { }
    //私有对象
    private static Global instance;
    //公有对象
    public static Global Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new Global();
            }
            return instance;
        }
    }
    public GameObject[,] ButtonCollection;
    public int[,] StateCollection;
    public List<BaseBlock> BlockCollection = new List<BaseBlock>();


    public int Rows;
    public int Columns;
    public float ButtonWidth;
    public float ButtonHeight;

    public int RemainSteps;

      
    public void InitializeButtonCollection(int row,int col)
    {
        Instance.ButtonCollection = new GameObject[row, col];
 
    }
    public void InitializeStateCollection(int row, int col)
    {
        Instance.StateCollection = new int[row, col];
        for (int i = 0; i < col; i++) {
            Instance.StateCollection[0,i] = -1;
            Instance.StateCollection[row-1, i] = -1;
        }
        for (int i = 0; i < row; i++)
        {
            Instance.StateCollection[i, 0] = -1;
            Instance.StateCollection[i, col-1] = -1;
        }
        // Debug.Log("Instance.StateCollection" + Instance.StateCollection[10, 10]);
    }
    public void AppendBlock(BaseBlock block)
    {
        BlockCollection.Add(block);
    }
 /*   public int CountEmptyLand()
    {

    }*/


}
