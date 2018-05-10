using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTarget : MonoBehaviour {

	[SerializeField]
	int location;

	LaserPointer pointer;

	public int Location {
		get { return location; }
		set { location = value; }
	}
	public LaserPointer Pointer {
		set { pointer = value; }
	}
		
	void Start () {
	}

	void OnDestroy() {
		if (pointer) {
			pointer.LaserInvestigated ();
		}
	}
}
