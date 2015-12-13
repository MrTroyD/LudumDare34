using UnityEngine;
using System.Collections;

public class CreateFlowers : BaseGoal {

    public int flowerGoal;

    public override string description
    {
        get { return (this.flowerGoal > 1) ? "Create " + this.flowerGoal + " flowers \n" : "Create a flower"; }
    }

    // Use this for initialization
    public override void Awake()
    {
        base.Awake();

    }

	public override void Start () {
        base.Start();
        
        if (this.flowerGoal < 1) this.flowerGoal = 1;
	}

    // Update is called once per frame
    public override void Update () {
        base.Update();

        if (this.achieved) return;

	    if (this._scene.gameActive && this._scene.createFlowers >= flowerGoal)
        {
            print(flowerGoal + " flowers created!");
            this.achieved = true;

            this._scene.OnGoalAchieved();
        }
	}
}
