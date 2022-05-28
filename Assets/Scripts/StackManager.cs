using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.Mathf;

public class StackManager : MonoBehaviour
{
    [SerializeField]
    private Transform stackRoot;
    [SerializeField]
    private float offset = 0.5f, punchScaleDelay = 0.05f, fetchUpTime = 0.1f;

    public static StackManager Instance;
    private void Awake() => Instance = this;

    private List<GameObject> stack;
    private List<Tween> tweens;

    //private float pull, swerveSpeed, translation;
    private Transform item, trailingItem;
    private GameObject collectible;

    private void Start()
    {
        stack = new List<GameObject>();
        stack.Add(stackRoot.gameObject);
        tweens = new List<Tween>();
        tweens.Add(DOVirtual.DelayedCall(1, () => { }));
    }

    private void Update()
    {
        for (int i = stack.Count - 1; i > 0; i--)
        {
            trailingItem = stack[i - 1].transform;
            item = stack[i].transform;

            item.position = new Vector3(item.position.x, item.position.y, trailingItem.position.z + offset);

            if (Abs(trailingItem.position.x - item.position.x) < 0.01)
                continue;
            tweens[i].Kill();
            tweens[i] = item.DOMoveX(trailingItem.position.x, fetchUpTime);

        }
    }
    public void StackCollectible(Collider collider)
    {
        collectible = collider.gameObject;
        stack.Add(collectible);                                     //add the collected item to the stack list
        tweens.Add(DOVirtual.DelayedCall(1, () => { }));            //add an empty tween to the tweens list
        collectible.layer = 7;                                      //switch the item's layer to the stack layer
        collider.isTrigger = false;                                 //stack items' colliders are not triggers
        collectible.GetComponent<StackedItem>().enabled = true;     //enable collecting capability
        collectible.transform.parent = transform;                   //place it under Stack object for organized hierarchy
        StartCoroutine(DoPunchWave());
    }

    private IEnumerator DoPunchWave() //performs the sequential scale punch of stack items with the specified delay
    {
        WaitForSeconds delay = new WaitForSeconds(punchScaleDelay);
        for (int i = stack.Count - 1; i > 0; i--)
        {
            stack[i].transform.DOPunchScale(Vector3.one * 0.15f, 0.3f, 6, 0.5f);
            yield return delay;
        }
    }
}
