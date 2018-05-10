using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
	base class of all Equipment types
*/
public abstract class Equipment : MonoBehaviour {

	/*
		name of type of equipment
	*/
	[SerializeField]
	string equipmentType;
	/*
		initial number of Equipment
	*/
	[SerializeField]
	protected int count;
	/*
		icon used to display in weapon wheel
	*/
	[SerializeField]
	Sprite icon;

	/*
		reference to MeshRenderer component
	*/
	protected MeshRenderer render;

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

	public virtual void Awake() {
		render = GetComponent<MeshRenderer> ();
	}

	public abstract void UseEquipment ();

	/*
		enables or disables rendering of Equipment object
		@param enable - whether to enable or disable rendering
	*/
	public void EquipmentRender(bool enable) {
		if (render) {
			render.enabled = enable;
		}
	}
}
