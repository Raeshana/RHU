using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class movingPlatformsVertical : MonoBehaviour
{
    private float _moveSpeed = 2f;
    public int _startingPoint;
    public Transform[] _points;
    private int i;
    
    // Start is called before the first frame update
    void Start()
    {
        transform.position = _points[_startingPoint].position;
    }

    // Update is called once per frame
    void Update()
    { 
        if(Vector2.Distance(transform.position,_points[i].position) < 0.02f)
        {
            i++;
            if(i == _points.Length)
            {
                i = 0;
            }
        }
        transform.position = Vector2.MoveTowards(transform.position, _points[i].position,_moveSpeed * Time.deltaTime);
    
    }
}
