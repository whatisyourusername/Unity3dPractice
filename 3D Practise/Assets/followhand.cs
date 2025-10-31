using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followhand : MonoBehaviour
{
    [SerializeField] private GameObject hand;

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = hand.transform.position;
    }
}
