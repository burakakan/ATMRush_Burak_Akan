using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackRoot : MonoBehaviour
{
    Collider rootCollider;
    private void Awake()
    {
        rootCollider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        StackManager.Instance.StackCollectible(collider);
        rootCollider.enabled = false;
    }
}