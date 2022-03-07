using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public FlowFieldManager mFFM;

    [Header("Unit Data")]
    public GameObject mUnitPrefab;
    public int mSpawnNum = 50;
    public float mSpeed = 4;

    [Header("Key Code")]
    public KeyCode mSpawnKey = KeyCode.E;
    public KeyCode mDestroyKey = KeyCode.Q;

    private List<GameObject> mUnitList;

    // Start is called before the first frame update
    void Start()
    {
        mUnitList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(mSpawnKey))
        {
            SpawnUnit();
        }

        if (Input.GetKeyDown(mDestroyKey))
        {
            DestroyAllUnits();
        }
    }

    void FixedUpdate()
    {
        if (mFFM.mFlowField == null)
        {
            return;
        }

        foreach(GameObject unit in mUnitList)
        {
            FlowCell occupiedCell = mFFM.mFlowField.ConvertWorldToCellPos(unit.transform.position);
            Vector3 dir = new Vector3(occupiedCell.mBestDir.mDirVector.x, 0, occupiedCell.mBestDir.mDirVector.y);
            Rigidbody rb = unit.GetComponent<Rigidbody>();
            rb.velocity = dir * mSpeed;
        }
    }

    private void SpawnUnit()
    {
        Vector2Int size = mFFM.mSize;
        float rad = mFFM.mCellRadius;
        Vector2 spawnPos = new Vector2(size.x * (rad * 2) + rad, size.y * (rad * 2) + rad);
        int layer = LayerMask.GetMask("Impassable", "Terrian", "RoughTerrian");
        
        Vector3 newPos = Vector3.zero;
        GameObject newUnit = Instantiate(mUnitPrefab);
        newUnit.transform.parent = transform;
        mUnitList.Add(newUnit);

        newPos = new Vector3(Random.Range(0, spawnPos.x - 1), 0, Random.Range(0, spawnPos.y - 1));
        newUnit.transform.position = newPos + new Vector3(0.0f, 0.4f, 0.0f);
    }

    private void DestroyAllUnits()
    {
        foreach(GameObject unit in mUnitList)
        {
            Destroy(unit);
        }
        mUnitList.Clear();
    }
}
