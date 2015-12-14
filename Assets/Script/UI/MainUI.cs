using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class MainUI : MonoBehaviour {

    private Text _lostSeed;
    private Text _lostWater;
    private Text _lostDirt;
    private Text _collectedFlower;

    private Text _goal;

    private BaseScene _gameScene;

	// Use this for initialization
	void Start ()
    {
        this._lostSeed = this.transform.FindChild("Lost Seeds").GetComponent<Text>();
        this._lostWater = this.transform.FindChild("Lost Waters").GetComponent<Text>();
        this._lostDirt = this.transform.FindChild("Lost Dirt").GetComponent<Text>();
        this._collectedFlower = this.transform.FindChild("Collected Flowers").GetComponent<Text>();

        this._goal = this.transform.FindChild("Goal").GetComponent<Text>();

        this._gameScene = BaseScene.SCENE;
    }

    public void UpdateGoals()
    {
        if (!this._gameScene)
            this._gameScene = BaseScene.SCENE;
        // this.transform.FindChild("Goal").GetComponent<Text>().text = description;
        this._goal = this.transform.FindChild("Goal").GetComponent<Text>();
        this._goal.text = "<b>Goal</b>\n";
        print(this._goal + " " + this.transform.FindChild("Goal").GetComponent<Text>() + "? "+ this._gameScene);
        foreach (BaseGoal goal in this._gameScene.goals)
       {
            this._goal.text += goal.achieved ? "  <b>" + goal.description + "</b>" : "  <i><color=\"#999999\">"+goal.description+"</color></i>";
       }
    }

    // Update is called once per frame
    void Update ()
    {
        this._lostSeed.text = "Lost Seed\n" + this._gameScene.lostSeeds;
        this._lostWater.text = "Lost Water\n" + this._gameScene.lostWater;
        this._lostDirt.text = "Lost Dirt\n" + this._gameScene.lostDirt;

        this._collectedFlower.text = "Flowers\n" + this._gameScene.collectedFlowers +" of "+this._gameScene.createFlowers; 
    }

    public void HideReady()
    {
        GameObject.Find("ReadyText").GetComponent<Text>().text = "GO!";
        Invoke("HideGo", 1);
    }

    public void HideGo()
    {
        GameObject.Find("ReadyText").GetComponent<Text>().text = "";

    }

    public void OnWin()
    {
        GameObject.Find("ReadyText").GetComponent<Text>().text = "You did it!";
    }
}
