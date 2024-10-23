using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IContactable
{
	// interface는 함수 가질 필요 없다.
	public void Contact();
	// IContactable을 가졌다면 Contact함수를 재정의하라
}
