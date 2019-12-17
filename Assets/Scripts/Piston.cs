using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piston : MonoBehaviour
{
    private Vector3 initPos;
    private bool isMoving = false;
    private Vector3 endPosition;

    [SerializeField] private bool extend = false;
    [SerializeField] private Vector3 destinate;
    [SerializeField] private float speed = 1;

    private void Awake()
    {
        initPos = transform.position;
    }

    void FixedUpdate()
    {
        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            if (transform.position == endPosition)
            {
                isMoving = false;
            }
        }
    }

    public void SetActive(bool isActive)
    {
        isMoving = true;

        if (isActive)
        {
            endPosition = destinate;
        }
        else
        {
            endPosition = initPos;
        }
    }
}
