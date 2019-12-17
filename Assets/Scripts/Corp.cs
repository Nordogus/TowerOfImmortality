using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Corp : MonoBehaviour
{
    public static List<Corp> instance;
    public static int nbMaxCorp = 2;
    public Switch sitOn;

    private void Awake()
    {
        if (instance == null)
        {
            instance = new List<Corp>();
        }

        instance.Add(this);

        while (instance.Count > nbMaxCorp)
        {
            GameObject tmpCorp = instance[0].gameObject;
            instance.RemoveAt(0);
            Destroy(tmpCorp);
        }
    }

    private void OnDestroy()
    {
        if (sitOn != null)
        {
            sitOn.Unactive();
        }
    }
}
