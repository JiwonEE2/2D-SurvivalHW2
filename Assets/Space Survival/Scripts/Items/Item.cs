using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Item : MonoBehaviour, IContactable
{
	// virtual : 부모에서도 구현해야 함
	// abstract : 자식에서만 구현해야 함

	public ParticleSystem contactParticle;

	public virtual void Contact()
	{
		// 파티클 구현하기
		var particle = Instantiate(contactParticle, transform.position, Quaternion.identity);
		particle.Play();
		Destroy(particle.gameObject, 2f);
		Destroy(gameObject);
	}
}
