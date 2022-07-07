using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RndPtTest : MonoBehaviour
{

	public GameObject obj;

	public Vector2 cellSize = Vector2.one;
	public Vector2 regionSize = Vector2.one;
	public int rejectionSamples = 30;
	public float displayRadius = 1;

	public float interval = 0.5f;

	List<Vector2> points;

    private void Start()
	{
		points = PoissonDiscSampling.GeneratePoints(cellSize, regionSize, rejectionSamples);

		StartCoroutine(Spawn(interval));
	}

	IEnumerator Spawn(float interval)
    {
		WaitForSeconds wait = new WaitForSeconds(interval);
		yield return wait;
		if (points != null)
			foreach (Vector2 point in points)
			{
				ObjectPooler.Instance.Spawn(ObjectPooler.ObjectType.Money, new Vector3(point.x - 2.2f, 0, point.y), Quaternion.identity);
				yield return wait;
			}
	}

	//void OnDrawGizmos()
	//{
	//	//Gizmos.Dra
	//	if (points != null)
	//	{
	//		foreach (Vector2 point in points)
	//		{
	//			Gizmos.DrawSphere(new Vector3(point.x - 2.2f,0,point.y), displayRadius);
	//		}
	//	}
	//}
}