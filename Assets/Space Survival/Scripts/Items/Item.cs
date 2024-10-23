using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IContactable
{
	// virtual : 부모에서도 구현해야 함
	// abstract : 자식에서만 구현해야 함
	public virtual void Contact()
	{
		Destroy(gameObject);
	}
}
