using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//����ü
public class Projectile : MonoBehaviour
{
	public float damage = 10;//������
	public float moveSpeed = 5;//�̵��ӵ�
	public float duration = 3;//���ӽð�

	// �ǰ� ��ƼŬ ����
	public ParticleSystem shotParticle;

	private void Start()
	{
		Destroy(gameObject, duration); //3�� �Ŀ� ������Ʈ ����
	}

	private void Update()
	{
		Move(Vector2.up);
		//Physics2D.OverlapCircle();
	}

	public void Move(Vector2 dir)
	{
		transform.Translate(dir * moveSpeed * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.TryGetComponent<Enemy>(out Enemy enemy))
		{
			// �ǰ� ��ƼŬ ��� �Ǳ�
			// ���� ��ġ�� ��ƼŬ �����ǵ��� �Ͽ��µ�, GetContact(0).point�ʹ� ���� �����ϱ�?
			var particle = Instantiate(shotParticle, other.transform.position, Quaternion.identity);
			particle.Play();
			Destroy(particle.gameObject, 2f);

			enemy.TakeDamage(damage);
			Destroy(gameObject);
		}
	}

}
