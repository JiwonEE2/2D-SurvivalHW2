using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : Item
{
	public override void Contact()
	{
		print("폭탄 습득");
		// 게임 매니저에게 모든 적들을 없애달라고 부탁하자
		GameManager.Instance.RemoveAllEnemies();
		base.Contact();		// Destroy 대신 부모의 Contact 호출
	}
}
