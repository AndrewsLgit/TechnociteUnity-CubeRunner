using System;
using System.Linq;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovementController : MonoBehaviour
{
    private Transform myTransform;
    [SerializeField]
    private float _speed = 5;
    private Vector3 currentTransform;
    private PathManager pathManager;
    private int currentLaneIndex;
    [CanBeNull] private GameObject currentSegment;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myTransform = GetComponent<Transform>();
        pathManager = FindFirstObjectByType<PathManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.rightArrowKey.wasPressedThisFrame)
        {
            currentTransform = myTransform.position;
            //Debug.Log("Right arrow key pressed");
            //myTransform.position = pathManager.GetLanes()[currentLaneIndex].position;
            if (currentLaneIndex == 0)
            {
                currentTransform.x = pathManager.GetLanes()[1].position.x;
                //myTransform.position = currentTransform;
            }
            if (currentLaneIndex == 2)
            {
                currentTransform.x = pathManager.GetLanes()[0].position.x;
                //myTransform.position = currentTransform;
            }
            myTransform.position = currentTransform;
        }

        if (Keyboard.current.leftArrowKey.wasPressedThisFrame)
        {
            currentTransform = myTransform.position;
            //Debug.Log("Left arrow key pressed");
            if (currentLaneIndex == 0)
            {
                currentTransform.x = pathManager.GetLanes()[2].position.x;
            }
            if (currentLaneIndex == 1)
            {
                currentTransform.x = pathManager.GetLanes()[0].position.x;
            }
            myTransform.position = currentTransform;
        }

        if (Keyboard.current.upArrowKey.isPressed)
        {
            //Debug.Log("Up arrow key pressed");
            myTransform.position += myTransform.forward * (_speed * Time.deltaTime);
        }

        if (Keyboard.current.downArrowKey.isPressed)
        {
            //Debug.Log("Down arrow key pressed");
            myTransform.position -= myTransform.forward * (_speed * Time.deltaTime);
        }
        //myTransform.position += new Vector3(0,0, _speed * Time.deltaTime);
        //myTransform.localPosition += new Vector3(0,0, _speed * Time.deltaTime);
        //myTransform.position += myTransform.forward * (_speed * Time.deltaTime);
        //myTransform.localScale += new Vector3(_scale * Time.deltaTime, _scale * Time.deltaTime, _scale * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Lane"))
        {
            Debug.Log($"Entered trigger range for {other.gameObject.name}");
            currentLaneIndex = pathManager.GetLaneIndex(other.gameObject.name);
            Debug.Log($"Current lane's index: {currentLaneIndex}");
        }
        
        //Debug.Log($"You are on lane: {pathManager.GetLanes().FirstOrDefault(x => x.gameObject.name == other.gameObject.name).gameObject.name}");
        //myTransform.right = other.GetComponentInParent<Component>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Segment"))
        {
            Debug.Log($"Exited trigger range for {other.gameObject.name}");
            currentSegment = other.gameObject;
            Debug.Log($"Current segment: {currentSegment}");
            //Debug.Log($"Past segment's id: {currentSegment.GetComponent<SegmentScript>().GetId() /*currentSegment.GetComponentInChildren<SegmentScript>().GetId()*/}");
            
            pathManager.SetCurrentSegment();
            //Debug.Log($"Current segment's id: {pathManager.GetSegments()[0].GetComponentInChildren<SegmentScript>().GetId()}");
            Debug.Log($"New segment found at {pathManager.GetSegments()[0].name}");
            
            /*if (pathManager.GetSegments()[0].GetComponent<SegmentScript>().GetId().GetHashCode() == currentSegment.GetComponent<SegmentScript>().GetId().GetHashCode()) //does NOT work properly
            {
                pathManager.SetCurrentSegment(currentSegment.gameObject);
                Debug.Log($"New segment found at {currentSegment.gameObject.name}");
                //Vector3.Dot(this.GetComponentInChildren<GameObject>().transform.position, pathManager.GetLanes()[0].transform.position);
            }*/
        }
        
    }
}
