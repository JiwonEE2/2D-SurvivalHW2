using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	public int level = 0;   // ����
	public int exp = 0;     // ����ġ

	// �������� ���ߵǴ� ��κ��� ������ exp���� ���� ����
	// ��� exp�� �����ϴ� ��ſ� ���� exp�� ������ ȯ���ϸ� �� ������ �ش��ϴ� �� ���

	private int[] levelupSteps = { 100, 200, 300, 400 };  // �ִ� ���� 5������ ����ġ �ܰ�
	private int currentMaxExp;  // ���� �������� ������ �ϱ���� �ʿ��� ����ġ ��

	private float maxHp;
	public float hp = 100f; //ü��
	public float damage = 5f; //���ݷ�
	public float moveSpeed = 5f; //�̵��ӵ�

	public Projectile projectilePrefab; //����ü ������

	public float HpAmount { get => hp / maxHp; }    // ���� ü�� ����

	public int killCount = 0;
	public Text killCountText;
	public Image hpBarImage;
	public Text levelText;
	public Text expText;

	private Transform moveDir;
	private Transform fireDir;

	private Rigidbody2D rb;
	public Animator tailfireAnimCtrl;

	private void Awake()
	{
		moveDir = transform.Find("MoveDir");
		fireDir = transform.Find("FireDir");
		rb = GetComponent<Rigidbody2D>();
	}

	void Start()
	{
		maxHp = hp;   // �ִ� ü�� ����
		currentMaxExp = levelupSteps[0];  // �ִ� ����ġ

		levelText.text = (level + 1).ToString();
		expText.text = exp.ToString();
		GameManager.Instance.player = this;

		// ������ �ִ� �Լ��� ȣ���� ��, ������ ������� �ʴ´ٸ�
		// �ƿ� ��ȯ�� ���� �޸𸮸� �������� �ʰ� �Լ��� ȣ��
		_ = StartCoroutine(FireCoroutine());
	}

	void Update()
	{
		float x = Input.GetAxis("Horizontal");
		float y = Input.GetAxis("Vertical");

		Vector2 moveDir = new Vector2(x, y);

		//this.moveDir.gameObject.SetActive(moveDir != Vector2.zero);

		tailfireAnimCtrl.SetBool("IsMoving", moveDir.magnitude > 0.1f);

		// ���콺 ��ġ�� ��� ������ ���ؾ� �� ��
		//Vector2 mousePos = Input.mousePosition;
		//Vector2 mouseScreenPos = Camera.main.ScreenToWorldPoint(mousePos);
		//Vector2 fireDir = mouseScreenPos - (Vector2)transform.position;
		//Vector3 -> Vector2�� ĳ���� �� �� : z���� ����

		// ���� ����� ���� Ž���Ͽ� ��� ������ ���� ��
		Enemy targetEnemy = null;   // ������� ������ ��
		float targetDistance = float.MaxValue;    // ������ �Ÿ�

		if (GameManager.Instance.enemies.Count == 0)
		{
			// �߻� ������ ����
			isFiring = false;
		}
		else
		{
			isFiring = true;
		}

		foreach (Enemy enemy in GameManager.Instance.enemies)
		{
			float distance = Vector3.Distance(enemy.transform.position, transform.position);
			if (distance < targetDistance)    // ������ ���� ������ ������
			{
				targetDistance = distance;
				targetEnemy = enemy;
			}

		}

		Vector2 fireDir = Vector2.zero;
		if (targetEnemy != null)
		{
			fireDir = targetEnemy.transform.position - transform.position;
		}

		Move(moveDir);

		// ���콺 ��Ŭ�� �Ǵ� ���� ctrl Ű�� �߻�
		//if (Input.GetButtonDown("Fire1"))
		//{
		//	Fire(fireDir);
		//}

		killCountText.text = killCount.ToString();
		hpBarImage.fillAmount = HpAmount;

		// tail �����·� ���ư� �ڿ� ������� ���� �ذ��� ���� 
		if (moveDir.magnitude > 0.1f)
		{
			// transform.up/right/forward�� ���� ���͸� ������ ���� ���� ������ magnitude�� ���� 1�� �������� �ʾƵ� �ȴ�
			this.moveDir.up = moveDir;
		}
		this.fireDir.up = fireDir;

		//print(this.moveDir.up);		// normalized�Ǿ� magnitude�� 1�� ������ ���� ���Ͱ� ��ȯ��
	}

	/// <summary>
	/// Transform�� ���� ���� ������Ʈ�� �����̴� �Լ�.
	/// </summary>
	/// <param name="dir">�̵� ����</param>
	public void Move(Vector2 dir)
	{
		//transform.Translate(dir * moveSpeed * Time.deltaTime);
		Vector2 movePos = rb.position + (dir * moveSpeed * Time.fixedDeltaTime);
		rb.MovePosition(movePos);
	}

	/// <summary>
	/// ����ü�� �߻�.
	/// </summary>
	public void Fire()
	{
		Projectile projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);

		projectile.transform.up = fireDir.up;
		projectile.damage = damage;
	}

	public float fireInterval;
	public bool isFiring;

	// �ڵ����� ����ü �߻� �ڷ�ƾ
	private IEnumerator FireCoroutine()
	{
		while (true)
		{
			yield return new WaitForSeconds(fireInterval);
			if (isFiring)
			{
				Fire();
			}
		}
	}

	public void TakeHeal(float heal)
	{
		hp += heal;
		if (hp > maxHp)
		{
			hp = maxHp;
		}
	}

	public void TakeDamage(float damage)
	{
		//print($"�ƾ�! : {damage}");
		if (damage < 0)
		{
			//TakeHeal(-damage);	// ��� �� �ϵ��� ó��
			damage = 0;
		}
		hp -= damage;
		if (hp <= 0)
		{
			// ���� ���� ó��
			hp = 0;
		}
	}

	// ����ġ ���渶�� ȣ��
	public void GainExp(int exp)
	{
		this.exp += exp;    // ���� ����ġ ����
		if (level < levelupSteps.Length && this.exp >= currentMaxExp)  // ����ġ ���� �� �������� ���� ����ġ�� �����ϸ�
		{
			// ������
			level++;
			this.exp -= currentMaxExp;
			if (level < levelupSteps.Length)
			{
				currentMaxExp -= levelupSteps[level];
			}

			// �������ϸ� ������ ����Ʈ, UI, ��Ե� ��ų��
			//DoLevelUp();
		}

		levelText.text = level.ToString();
		expText.text = this.exp.ToString();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		// �������̶� ��ȣ�ۿ� �� �ǵ�... �������� bomb�� �ְ� heal�� �־ ���� �� ��ü ���� �ൿ�� �����ߴ��� �ҽ��ڵ嵵 �������..
		//if (collision.TryGetComponent<Bomb>(out Bomb bomb))   // ���� ��ȣ�ۿ��� Ʈ���ſ� Bomb ������Ʈ�� ���� ���
		//{
		//	bomb.Contact();
		//}
		//if(collision.TryGetComponent<Heal>(out Heal heal))
		//{
		//	heal.Contact();
		//}

		// �̷� �� �����ڰ� "������"�� �����Ͽ�
		// �ҽ��ڵ带 ȿ�������� �ۼ��� �� �ִ� ��� 3����
		// 1. �θ� Ŭ������ ���
		// 2. �������̽��� ����
		// 3. ����Ƽ�� SendMessage ���

		// 1. �θ� Ŭ������ ������� ���
		//if(collision.TryGetComponent<Item>(out Item item))
		//{
		//	item.Contact();
		//	// �ε��� ��ü�� ��Ȯ�� � Ÿ���� ���� �𸣰����� Item�̶�� Ŭ������ ����� ���� Ȯ���ϰ� �׷��ٸ� Contact() �Լ��� ������ �����Ƿ� ȣ���� �� �ִ�.
		//}

		// 2. ���� Ư�� Ŭ������ ������� �ʰ�, �������� ���� ���� ��ü���� ��쿡 ���� ���� �ൿ�� �ؾ��� ���, Interface�� ����� �� �ִ�.
		//if (collision.TryGetComponent<IContactable>(out var contact))
		//{
		//	contact.Contact();
		//	// �ε��� ��ü�� Enemy���� Item�������� �𸣰����� ��·�� IContactable �������̽��� �����ߴٸ� Contact() �Լ��� ������ �����Ƿ� ȣ���� �� �ִ�.
		//}

		// 3. ���ӿ�����Ʈ�� ��� SendMessage�� ���� ������ �ִ� ������Ʈ�� Ư�� �̸��� ���� �Լ��� ȣ���ϵ��� ����� ����. UnityEngine�� ���� ���
		collision.SendMessage("Contact", /*this, */SendMessageOptions.DontRequireReceiver);
		// ���ڿ� �����̱� ������ �̸� ���� �� ������ ���� �� ������, ���� ���ʹ��� Contact�� item�� Contact�� ������ ���� ������ �� item�� Contact�� �̸��� �ٲٴ��� enemy������ �ȹٲ��.

		// SendMessage ������
		// 1. ���ڿ��� �Լ��� ȣ���ϹǷ� �Լ� �̸� ���� �Ǵ� ��Ÿ �߻� �� ���� ã�� �����.
		// 2. �ش� ��ü�� �ִ� ��� ������Ʈ���� Contact��� �Լ��� ������ �ִ� �� Ž���� �����ϱ� ������ �����ս��� ȿ�����̶�� ���� �����.
		// 3. ȣ���� �Լ��� �Ķ���ʹ� 0�� �Ǵ� 1���� ���ѵ�.
		// ���� ���߰� ������Ÿ���ο��� ����ϱ�� ������, ���������� ���� ����� �ƴϹǷ� ������ ���� �������̳� ���� �Ը� �̻��� ��������� ���� �ʴ� ��
		// ������ �Ķ���ʹ� null �϶� ������ ���� ������ ������ �� �ִ�. �׷��� �̷��� �ִ� 3���� �Ķ���͸� ���� �� �ִ�.
	}
}
