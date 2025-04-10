using System.Collections.Generic;
using System.Linq;
using Unity.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    [SerializeField]
    private GameObject currentSegment;
    [SerializeField] 
    private GameObject[] segmentPrefabs;
    
    private Transform[] lanes = new Transform[3];
    private GameObject[] segments = new GameObject[3];

    public Transform[] GetLanes()
    {
        //SetLanes();
        return lanes;
    }

    public int GetLaneIndex(string lane)
    {
        int index = 0;
        for (int i = 0; i < lanes.Length; i++)
        {
            if (lanes[i].name == lane) index = i;
        }
        return index;
    }

    public GameObject[] GetSegments()
    {
        return segments;
    }

    public int GetSegmentIndex(string segment)
    {
        int index = 0;
        for (int i = 0; i < segments.Length; i++)
        {
            if (segments[i].name == segment) index = i;
        }
        return index;
    }

    public void SetCurrentSegment(/*GameObject newSegment*/)
    {
        //currentSegment.gameObject.SetActive(false);
        //currentSegment = newSegment; 
        //int index = GetSegmentIndex(newSegment.name); 
        segments[0] = segments[1];
        Destroy(currentSegment.gameObject);
        currentSegment = segments[0];
        segments[1] = segments[^1]; 
        segments[^1] = GenerateSingleSegment(segments[1].transform.position.z);
        SetLanes();
    }

    private void SetLanes()
    {
        
        //Transform[] newLanes = currentSegment.GetComponentsInChildren<Transform>();
        if (segments[0].gameObject.GetComponentsInChildren<Transform>()
                .FirstOrDefault(x => x.gameObject.CompareTag("Lane")) != null)
        {
            //lanes = segments[0].gameObject.GetComponentsInChildren<Transform>().Where(x => x.gameObject.CompareTag("Lane") && !x.IsUnityNull()).ToArray();
            for (int i = 0; i < lanes.Length; i++)
            {
                //lanes[i] = currentSegment.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.gameObject.CompareTag("Lane") && !lanes.Contains(x)).transform;
                lanes[i] = segments[0].gameObject.GetComponentsInChildren<Transform>().FirstOrDefault(x => x.gameObject.CompareTag("Lane") && !lanes.Contains(x)).transform;
                Debug.Log("Lane found in segment: " +lanes[i].gameObject.name);
            }
        }
        else
        {
            Debug.Log($"Lanes not found in segment: {currentSegment.name}");
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GenerateSegments();
        SetLanes();
    }

    // Update is called once per frame
    void Update()
    {
        //currentSegment = segments[0];
    }

    private void GenerateSegments()
    {
        //int range = Random.Range(0, segmentPrefabs.Length);
        //segments[0] = currentSegment;
        for (int i = 0; i < segments.Length; i++)
        {
            //GameObject segment = Instantiate(segmentPrefabs[range], new Vector3(0, 0, segmentZ + 20/*segmentPrefabs[range].gameObject.transform.localScale.z*/), Quaternion.identity);
            segments[i] = i == 0 ? currentSegment : GenerateSingleSegment(segments[i-1].transform.position.z);
            //segment.transform.SetParent(this.gameObject.transform);
        }
        //floor.transform.Rotate(0, 90, 0);
    }

    private GameObject GenerateSingleSegment(float segmentZ)
    {
        int range = Random.Range(0, segmentPrefabs.Length);
        //segments[0] = currentSegment;
        GameObject segment = Instantiate(segmentPrefabs[range],
            new Vector3(0, 0, segmentZ + 20 /*segmentPrefabs[range].gameObject.transform.localScale.z*/),
            Quaternion.identity);
        
        segment.transform.SetParent(this.gameObject.transform);
        Debug.Log($"Generated segment: {segment.gameObject.name}");
        return segment;
    }
}
