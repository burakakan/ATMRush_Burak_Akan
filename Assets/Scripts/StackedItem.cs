using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedItem : MonoBehaviour
{
    public ObjectPooler.ObjectType type;

    private void Start()
    {

    }
    
    private void OnTriggerEnter(Collider collider)
    {
        int triglayer = collider.gameObject.layer;
        if (triglayer == 6) //if the object is in the collectible layer
            StackManager.Instance.StackCollectible(collider);

        if (triglayer == 8 && (int)type < 2) //upgrade layer
            Upgrade();
    }

    public void Upgrade()
    {
        //ObjectPooler.Instance.Kill(gameObject);
        //ObjectPooler.Instance.Spawn(type + 1, transform.position, transform.rotation);
    }

}
