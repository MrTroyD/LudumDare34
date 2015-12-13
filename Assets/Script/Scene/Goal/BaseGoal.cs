using UnityEngine;
using System.Collections;

public class BaseGoal : MonoBehaviour {

    protected string _description;
    public bool achieved;
       
    protected BaseScene _scene;

    public virtual string description
    {
        get { return this._description; }
    }

    public virtual void Awake()
    {

    }

    // Use this for initialization
    public virtual void Start () {
        this._scene = BaseScene.SCENE;
	}

    // Update is called once per frame
    public virtual void Update () {
	
	}
}
