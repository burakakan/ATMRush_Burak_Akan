using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackedItem : MonoBehaviour
{
    public ObjectPooler.ObjectType type;

    private GameObject trigObject;
    private int trigLayer;
    private void Start()
    {
        
    }
    
    private void OnTriggerEnter(Collider collider)
    {
        trigLayer = collider.gameObject.layer;
        switch (trigLayer)
        {
            case 6:      //if the object is in the collectible layer
                StackManager.Instance.StackCollectible(collider);
                break;
            case 8:     //upgrade layer
                Upgrade();
                break;
            case 10:    //atm layer
                //Deposit(collider.gameObject);
                break;
            default:
                break;
        }
            
    }

    private void Upgrade()
    {
        ObjectPooler.Instance.Kill(gameObject);
        GameObject upgrade = ObjectPooler.Instance.Spawn(type + 1, transform.position, transform.rotation);
        upgrade.transform.SetParent(StackManager.Instance.transform, true);
        StackManager.Instance.Replace(gameObject, upgrade);
    }
    private void Deposit()
    {
        ObjectPooler.Instance.Kill(gameObject);
    }
}
