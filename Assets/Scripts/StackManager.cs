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
    private List<Tweening> tweens;

    private Transform item, trailingItem;
    private GameObject collectible;
    private float origScale = 0.4f;

    private void Start()
    {
        stack = new List<GameObject>();
        stack.Add(stackRoot.gameObject);
        tweens = new List<Tweening>();
        tweens.Add(new Tweening());
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
            tweens[i].MoveX.Kill();
            tweens[i].MoveX = item.DOMoveX(trailingItem.position.x, fetchUpTime);

        }
    }
    public void StackCollectible(Collider collider)
    {
        collectible = collider.gameObject;
        stack.Add(collectible);                                     //add the collected item to the stack list
        tweens.Add(new Tweening());                                 //add an empty tween to the tweens list
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
            //kill the ongoing punch tween
            tweens[i].Punch.Kill();
            //revert to the original scale before the punch tween takes the item's transform as target
            stack[i].transform.localScale = origScale * Vector3.one;
            //give the punch
            tweens[i].Punch = stack[i].transform.DOPunchScale(Vector3.one * 0.15f, 0.3f, 6, 0.5f);

            ////give the current punch and fix the scale after tween completion
            //tweens[i].Punch = stack[i].transform.DOPunchScale(Vector3.one * 0.15f, 0.3f, 6, 0.5f).OnComplete(() => tweens[i].Punch = stack[i].transform.DOScale(Vector3.one * origScale, 0.25f));

            yield return delay;
        }
    }
    private class Tweening
    {
        public Tweening()
        {
            MoveX = DOVirtual.DelayedCall(1, () => { });
            Punch = DOVirtual.DelayedCall(1, () => { });
        }
        public Tween MoveX { get; set; }
        public Tween Punch { get; set; }
    }
}
