using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedItem : MonoBehaviour
{
    private void Start()
    {

    }
    private void OnTriggerEnter(Collider collider)
    {
        int triglayer = collider.gameObject.layer;
        //Debug.Log("stacked: " + transform.gameObject.name + ", collided w: " + collider.gameObject.name);
        if (triglayer == 6) //if the object is in the collectible layer
            StackManager.Instance.StackCollectible(collider);
        if (triglayer == 8)
        {

        }

    }

}
