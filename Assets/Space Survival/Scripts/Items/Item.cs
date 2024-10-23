using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IContactable
{
	// virtual : �θ𿡼��� �����ؾ� ��
	// abstract : �ڽĿ����� �����ؾ� ��

	public ParticleSystem contactParticle;

	public virtual void Contact()
	{
		// ��ƼŬ �����ϱ�
		var particle = Instantiate(contactParticle, transform.position, Quaternion.identity);
		particle.Play();
		Destroy(particle.gameObject, 2f);
		Destroy(gameObject);
	}
}
