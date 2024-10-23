using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IContactable
{
	// virtual : �θ𿡼��� �����ؾ� ��
	// abstract : �ڽĿ����� �����ؾ� ��
	public virtual void Contact()
	{
		Destroy(gameObject);
	}
}
