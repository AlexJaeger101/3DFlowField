using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlowCell
{
    public Vector3 mWorldPos;
    public Vector2Int mIndex;
    public int mCost;
    public int mBestCost;

    const int MAX_COST = 100;

    public FlowCell(Vector3 worldPos, Vector2Int index)
    {
        mWorldPos = worldPos;
        mIndex = index;
        mCost = 1;
        mBestCost = int.MaxValue;
    }

    public void IncCost(int incAmout)
    {
        if (mCost == MAX_COST)
        {
            return;
        }
        else if (incAmout + mCost >= MAX_COST)
        {
            mCost = MAX_COST;
        }
        else
        {
            mCost += incAmout;
        }
    }
}

public class FlowCellDirection
{
    public readonly Vector2Int mDirVector;

    //Directions
    public static readonly FlowCellDirection NoDir = new FlowCellDirection(new Vector2Int(0, 0));
    public static readonly FlowCellDirection UpDir = new FlowCellDirection(new Vector2Int(0, 1));
    public static readonly FlowCellDirection DownDir = new FlowCellDirection(new Vector2Int(0, -1));
    public static readonly FlowCellDirection RightDir = new FlowCellDirection(new Vector2Int(1, 0));
    public static readonly FlowCellDirection LeftDir = new FlowCellDirection(new Vector2Int(-1, 0));
    public static readonly FlowCellDirection TopRightDir = new FlowCellDirection(new Vector2Int(1, 1));
    public static readonly FlowCellDirection TopLeftDir = new FlowCellDirection(new Vector2Int(-1, 1));
    public static readonly FlowCellDirection BottomRightDir = new FlowCellDirection(new Vector2Int(1, -1));
    public static readonly FlowCellDirection BottomLeftDir = new FlowCellDirection(new Vector2Int(-1, -1));

    //Direction List
    public static readonly List<FlowCellDirection> DirectionList = new List<FlowCellDirection>
    { 
        UpDir,
        DownDir,
        RightDir,
        LeftDir,
        TopRightDir,
        TopLeftDir,
        BottomRightDir,
        BottomLeftDir
    };

    private FlowCellDirection(Vector2Int vec)
    {
        mDirVector = vec;
    }

    public static FlowCellDirection GetDirection(Vector2Int dirVector)
    {
        foreach(FlowCellDirection dir in DirectionList)
        {
            if (dirVector == dir.mDirVector)
            {
                return dir;
            }
        }

        return NoDir;
    }

    //Vector2 conversions
    public static implicit operator Vector2Int(FlowCellDirection dir)
    {
        return dir.mDirVector;
    }
}
