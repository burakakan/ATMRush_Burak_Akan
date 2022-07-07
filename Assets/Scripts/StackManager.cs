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

    private List<StackElement> stack;

    private Transform item, trailingItem;
    private GameObject collectible;

    private void Start()
    {
        stack = new List<StackElement> { new StackElement(stackRoot.gameObject) };
        stackRoot.GetComponentInParent<PlayerBehaviour>().OnMoveForward += MoveForward;
    }

    private void Update()
    {
        for (int i = stack.Count - 1; i > 0; i--)
        {
            trailingItem = stack[i - 1].GameObject.transform;
            item = stack[i].GameObject.transform;

            if (Abs(trailingItem.position.x - item.position.x) < 0.005)
                continue;
            stack[i].TweenX.Kill();
            stack[i].TweenX = item.DOMoveX(trailingItem.position.x, fetchUpTime);
        }
    }
    public void StackCollectible(Collider collider)
    {
        collectible = collider.gameObject;
        stack.Add(new StackElement(collectible));                   //add the collected item to the stack list
        collectible.layer = 7;                                      //switch the item's layer to the stack layer
        collider.isTrigger = false;                                 //stack items' colliders are not triggers
        StackedItem stackedItem = collectible.GetComponent<StackedItem>();
        stackedItem.enabled = true;                                 //enable collecting capability

        collectible.transform.position = stack[stack.Count - 2].GameObject.transform.position + offset * Vector3.forward;
        collectible.transform.parent = transform;                   //place it under Stack object for organized hierarchy

        ObjectPooler.Instance.Add(collectible, stackedItem.type);   //add the collectible to its respective pool

        StartCoroutine(DoPunchWave());
    }
    public void Replace(GameObject oldObj, GameObject newObj)
    {
        int index = stack.FindIndex(e => e.GameObject == oldObj);
        stack[index].SwitchObject(newObj);
        DoPunch(index);
    }
    public void BreakOff(GameObject element)
    {
        ObjectPooler.Instance.Kill(element);
        int index = stack.FindIndex(e => e.GameObject == element);

        List<StackElement> free = stack.GetRange(index + 1, stack.Count - index - 1);


    }
    private void DoPunch(int index)
    {
        //revert to the original scale before the punch tween takes the item's transform as target
        stack[index].GameObject.transform.localScale = stack[index].OrigScale;
        //kill the ongoing punch tween
        stack[index].TweenPunch.Kill();
        //give the punch
        stack[index].TweenPunch = stack[index].GameObject.transform.DOPunchScale(stack[index].OrigScale * 0.4f, 0.3f, 6, 0.5f);
    }
    private IEnumerator DoPunchWave() //performs the sequential scale punch of stack items with the specified delay
    {
        WaitForSeconds delay = new WaitForSeconds(punchScaleDelay);
        for (int i = stack.Count - 1; i > 0; i--)
        {
            DoPunch(i);
            yield return delay;
        }
    }
    private void MoveForward(float pace)
    {
        transform.position += pace * Time.deltaTime * Vector3.forward;
    }
    private class StackElement
    {
        public StackElement(GameObject obj)
        {
            GameObject = obj;
            TweenX = DOVirtual.DelayedCall(1, () => { });
            TweenPunch = DOVirtual.DelayedCall(1, () => { });
            OrigScale = obj.transform.localScale;
        }
        public GameObject GameObject { get; set; }
        public Tween TweenX { get; set; }
        public Tween TweenPunch { get; set; }
        public Vector3 OrigScale { get; set; }

        public void SwitchObject(GameObject newObj)
        {
            GameObject = newObj;
            OrigScale = newObj.transform.localScale;
        }
    }
}
