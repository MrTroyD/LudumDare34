using UnityEngine;
using System.Collections;

public class CollectFlowers : BaseGoal {

    public int flowerGoal;

    public override string description
    {
        get { return (this.flowerGoal > 1) ? "Collect " + this.flowerGoal + " flowers \n" : "Create a flower"; }
    }


    public override void Start()
    {
        base.Start();

        if (this.flowerGoal < 1) this.flowerGoal = 1;
    }

    // Update is called once per frame
    public override void Update()
    {
        base.Update();

        if (this.achieved) return;

        if (this._scene.gameActive && this._scene.collectedFlowers >= flowerGoal)
        {
            print(flowerGoal + " flowers collected!");
            this.achieved = true;

            this._scene.OnGoalAchieved();
        }
    }
}
