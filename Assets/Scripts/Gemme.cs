using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gemme : MonoBehaviour
{
    [SerializeField] private float rotor = 0;
    [SerializeField] private float speedRotation = 100;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Euler(0, rotor, 0);
        rotor += Time.deltaTime * speedRotation;
    }
}
