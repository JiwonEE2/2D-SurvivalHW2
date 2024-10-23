using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Box : MonoBehaviour
{
	public GameObject item;
	private void Contact()
	{
		print("»óÀÚ¶û ºÎµúÈû");
		Instantiate(item, transform.position, Quaternion.identity);
		Destroy(gameObject);
	}
}
