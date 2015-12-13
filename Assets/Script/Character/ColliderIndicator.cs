using UnityEngine;
using System.Collections;

[RequireComponent (typeof (Rigidbody))]
public class ColliderIndicator : MonoBehaviour {

    private Character _character;
	// Use this for initialization
	void Start () {
        this._character = this.transform.parent.GetComponent<Character>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        //print("Oh yeah here we go!" + this.transform.parent + " "+col.transform);
        if (col.tag == "MovingIndicator")
        {
            if ((col.transform.parent.tag =="MovingCharacter" &&  col.transform.parent.GetComponent<Character>().type == this._character.type))
            {
                this._character.rotateRight();
            }
        }
    }
}
