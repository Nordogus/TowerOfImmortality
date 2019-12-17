using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Switch : MonoBehaviour
{
    [SerializeField] private Piston target;
    [SerializeField] private GameObject child;

    public void Activate()
    {
        target.SetActive(true);
        child.transform.position = new Vector3(0, -.2f,0) + transform.position;
    }

    public void Unactive()
    {
        target.SetActive(false);
        child.transform.position = new Vector3(0, 0, 0) + transform.position;
    }
}
