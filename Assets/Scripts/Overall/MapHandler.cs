using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapHandler : MonoBehaviour
{
    List<GameObject> mapSegments;
    private GameObject player;
    public GameObject generalMapSegment;
    public GameObject destination;
    public GameObject gasStation;

    public bool destinationAdded;
    public int maxMapSegments;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("PlayerTag");
        mapSegments = new List<GameObject>();
        mapSegments.Add(Instantiate(generalMapSegment, new Vector3(player.transform.position.x, player.transform.position.y - 10.0f, player.transform.position.z - 40), player.transform.rotation));
        destinationAdded = false;
    }

    public void AddGasStation()
    {
        var prevObject = mapSegments[mapSegments.Count - 1].gameObject;
        mapSegments.Add(Instantiate(gasStation, new Vector3(prevObject.transform.position.x, prevObject.transform.position.y, prevObject.transform.position.z + 20), prevObject.transform.rotation));
        var curObject = mapSegments[mapSegments.Count - 1].gameObject;
        curObject.SetActive(true);
    }

    public void AddDestination()
    {
        
        destinationAdded = true;
        var prevObject = mapSegments[mapSegments.Count - 1].gameObject;
        mapSegments.Add(Instantiate(destination, new Vector3(prevObject.transform.position.x, prevObject.transform.position.y, prevObject.transform.position.z + 20), prevObject.transform.rotation));
        var curObject = mapSegments[mapSegments.Count - 1].gameObject;
        curObject.SetActive(true);
    }

    public void RemoveGameObject(GameObject objectToRemove)
    {
        mapSegments.Remove(objectToRemove);
    }

    // Update is called once per frame
    void Update()
    {
        if (destinationAdded) { return; }

        if(mapSegments.Count != maxMapSegments)
        {
            var prevObject = mapSegments[mapSegments.Count - 1].gameObject;
            mapSegments.Add(Instantiate(generalMapSegment, new Vector3(prevObject.transform.position.x, prevObject.transform.position.y, prevObject.transform.position.z + 20), prevObject.transform.rotation));
            var curObject = mapSegments[mapSegments.Count - 1].gameObject;
            curObject.SetActive(true);
        }
    }
}
