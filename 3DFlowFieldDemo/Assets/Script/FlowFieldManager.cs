using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FlowFieldManager : MonoBehaviour
{
    [Header("Flow Field Data")]
    public Vector2Int mSize;
    public float mCellRadius = 0.5f;
    public FlowField mFlowField;

    [Header("Debug")]
    public bool mShouldDisplayGrid;

    void Start()
    {
        InitFlowField();
        mFlowField.InitCost();
    }

    private void InitFlowField()
    {
        mFlowField = new FlowField(mCellRadius, mSize);
        mFlowField.InitFlowGrid();
    }

    private void OnDrawGizmos()
    {
        if (mShouldDisplayGrid && mFlowField == null) //Visualize grid before being made
        {
            Gizmos.color = Color.green;

            for (int i = 0; i < mSize.x; ++i)
            {
                for (int j = 0; j < mSize.y; ++j)
                {
                    Vector3 center = new Vector3((mCellRadius * 2) * i + mCellRadius, 0, (mCellRadius * 2) * j + mCellRadius);
                    Gizmos.DrawCube(center, (Vector3.one * mCellRadius * 2));
                }
            }
        }
        else if (mShouldDisplayGrid && mFlowField != null) //Visualize Cost Field for created grid
        {
            foreach (FlowCell cell in mFlowField.mFlowGrid)
            {
                Handles.Label(cell.mWorldPos, cell.mCost.ToString());
            }
        }
    }
}
