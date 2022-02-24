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

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitFlowField()
    {
        mFlowField = new FlowField(mCellRadius, mSize);
        mFlowField.InitFlowGrid();
    }

    private void OnDrawGizmos()
    {
        if (mShouldDisplayGrid && mFlowField == null)
        {
            DrawGrid(mSize, mCellRadius);
        }
        else if (mShouldDisplayGrid && mFlowField != null)
        {
            foreach (FlowCell cell in mFlowField.mFlowGrid)
            {
                Handles.Label(cell.mWorldPos, cell.mCost.ToString());
            }
        }
    }

    void DrawGrid(Vector2Int size, float radius)
    {
        Gizmos.color = Color.green;

        for (int i = 0; i < size.x; ++i)
        {
            for (int j = 0; j < size.y; ++j)
            {
                Vector3 center = new Vector3((radius * 2) * i + radius, 0, (radius * 2) * j + radius);
                Gizmos.DrawCube(center, (Vector3.one * radius * 2));
            }
        }
    }
}
