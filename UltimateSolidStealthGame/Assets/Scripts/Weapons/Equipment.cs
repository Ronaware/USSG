using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Equipment : MonoBehaviour {

	[SerializeField]
	string equipmentType;
	[SerializeField]
	protected int count;
	[SerializeField]
	Sprite icon;

	public string EquipmentType {
		get { return equipmentType; }
	}
	public int Count {
		get { return count; }
		set { count = value; }
	}
	public Sprite Icon {
		get { return icon; }
	}

	// Use this for initialization
	void Start () {
		
	}
	
	public virtual void UseEquipment () {

	}
}
