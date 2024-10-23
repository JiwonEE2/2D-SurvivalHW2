using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Heal : Item
{
	public float healAmount;		// Èú·®
	public override void Contact()
	{
		print("È¸º¹ ½Àµæ");
		GameManager.Instance.player.TakeHeal(healAmount);
		base.Contact();
	}
}
