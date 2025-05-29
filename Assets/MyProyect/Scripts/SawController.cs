using UnityEngine;

public class SawController : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Transform[] waypoints;
    [SerializeField] private int indexWaypoint = 1;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        transform.position = waypoints[0].position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, 
            waypoints[indexWaypoint].position, speed * Time.deltaTime);
        if (!(Vector2.Distance(transform.position, waypoints[indexWaypoint].position) < 0.1f)) return;
        indexWaypoint++;
        if (indexWaypoint >= waypoints.Length) indexWaypoint = 0;
        
    }
}
