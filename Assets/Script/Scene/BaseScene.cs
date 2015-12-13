using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BaseScene : MonoBehaviour {

    public enum Difficulty
    {
        easy,
        medium,
        hard,
        impossible
    };

    public static BaseScene SCENE;
   
    public float gameSpeed = .1f;
    public int boardWidth = 12;
    public int boardHeight = 12;

    public int spawnEvery = 3;

    public GameObject dirtObject;
    public GameObject waterObject;
    public GameObject seedObject;
    public GameObject wetObject;
    public GameObject mudObject;
    public GameObject burriedObject;
    public GameObject flowerObject;

    public GameObject mushroomObject;

    public int lostSeeds = 0;
    public int lostWater = 0;
    public int lostDirt = 0;
    public int collectedFlowers = 0;
    public int createFlowers = 0;
    public Difficulty currentDifficulty;


    private bool _gameActive;
    private bool _gamePaused;
    private List<Character> _characters;

    private bool _pressedLeft;
    private bool _pressedRight;

    public bool _pressedLeftKey;
    public bool _pressedRightKey;

    public bool _pressedLeftButton;
    public bool _pressedRightButton;

    private float _movementStartTime;

    private int _spawnIndex = 0;
    private List<BaseGoal> _goals;

    public bool gameActive
    {
        get { return this._gameActive; }
    }

    public List<BaseGoal> goals
    {
        get { return this._goals; }
    }

    void Awake()
    {
        BaseScene.SCENE = this;
    }

    // Use this for initialization
    void Start() {

        Invoke("StartLevel", 2);

        this.LoadGoals();

        this._characters = new List<Character>();

        GameObject[] characters = GameObject.FindGameObjectsWithTag("MovingCharacter");

        foreach (GameObject character in characters)
        {
            if (character.GetComponent<Character>()) this._characters.Add(character.GetComponent<Character>());
        }
    }

    public void StartLevel()
    {
        print("Level Start");
        this._gameActive = true;
        Invoke("MoveCharacters", this.gameSpeed );

    }

    public void LoadGoals()
    {
        List< string > goalString = new List<string>(); 
            
        this._goals = new List<BaseGoal>();

        //DO something proper later

        CollectFlowers __collectFlowersGoal = this.gameObject.AddComponent<CollectFlowers>();
        __collectFlowersGoal.flowerGoal = 4;
        goalString.Add(__collectFlowersGoal.description);
        this._goals.Add(__collectFlowersGoal);
        
        CreateFlowers __createFlowersGoal = this.gameObject.AddComponent<CreateFlowers>();
        __createFlowersGoal.flowerGoal = 4;
        goalString.Add(__createFlowersGoal.description);
        this._goals.Add(__createFlowersGoal);

        GameObject.Find("GameUI").GetComponent<MainUI>().UpdateGoals() ;

    }

    public void MoveCharacters()
    {
        Invoke("MoveCharacters", this.gameSpeed + 1);

        this._movementStartTime = Time.time;

        if (!this._gameActive || this._gamePaused) return;
        for (int i = 0; i < this._characters.Count; i++)
        {
            Character col = this._characters[i];
            col.moveForward();
        }
        
        this._spawnIndex = (this._spawnIndex + 1) % this.spawnEvery;

        if (this._spawnIndex == 0)
        {
            SpawnObject();
        }

    }

    public void PressedCounterClockwise(bool value)
    {
        this._pressedLeftButton = value;
    }

    public void PressedClockwise(bool value)
    {
        this._pressedRightButton = value;
    }

    public void Remove(Character character, bool lost)
    {
        switch (character.type)
        {
            case Character.CollisionType.water:
                this.lostWater++;
                break;
            case Character.CollisionType.dirt:
                this.lostDirt++;
                break;
            case Character.CollisionType.seed:
                this.lostSeeds++;
                break;
            case Character.CollisionType.flower:
                this.collectedFlowers++;
                break;
        }

        Remove(character);
    }

    public void Remove(Character character)
    {
        this._characters.Remove(character);
    }

    public void SpawnObject()
    {
        for (int i = 0; i < 100; i++)
        {
            if (Random.Range(0, 2) == 2) print("It does equeals two");
        }

        Character character = null; 
        switch (Random.Range(0, 3))
        {
            case 0:
                character = Create(Character.CollisionType.dirt).GetComponent<Character>();
                break;
            case 1:
                character = Create(Character.CollisionType.seed).GetComponent<Character>();
                break;
            default:
                character = Create(Character.CollisionType.water).GetComponent<Character>();
                break;
        }

        Vector3 startPosition, endPosition;
        Character.FacingDirection direction;
        switch (Random.Range(0, 4))
        {
            case 0:
                startPosition = endPosition = new Vector3(-((this.boardWidth / 2) + 1), 0, Random.Range(-this.boardHeight - 1, this.boardHeight) * .5f);
                direction = Character.FacingDirection.east;
                break;
            case 1:
                startPosition = endPosition = new Vector3(((this.boardWidth / 2) + 1), 0, Random.Range(-this.boardHeight - 1, this.boardHeight) * .5f);
                direction = Character.FacingDirection.west;
                break;
            case 2:
                startPosition = endPosition = new Vector3(Random.Range(-this.boardWidth - 1, this.boardWidth) * .5f, 0, -((this.boardHeight / 2) + 1));
                direction = Character.FacingDirection.north;
                break;
            default:
                startPosition = endPosition = new Vector3( Random.Range(-this.boardWidth - 1, this.boardWidth) * .5f, 0, ((this.boardHeight / 2) + 1));
                direction = Character.FacingDirection.south;
                break;
        }
        character.setupValues(startPosition, endPosition, this._movementStartTime, direction);
        this._spawnIndex = 0;
    }

    public GameObject Create(Character.CollisionType type)
    {
        GameObject combined = null;
        switch (type)
        {
            case Character.CollisionType.flower:
                combined = Instantiate(flowerObject);
                createFlowers++;
                break;
            case Character.CollisionType.dirt:
                combined = Instantiate(dirtObject);
                break;
            case Character.CollisionType.seed:
                combined = Instantiate(seedObject);
                break;
            case Character.CollisionType.water:
                combined = Instantiate(waterObject);
                break;
            case Character.CollisionType.mud:
                combined = Instantiate(mudObject);
                break;
            case Character.CollisionType.burriedSeed:
                combined = Instantiate(burriedObject);
                break;
            case Character.CollisionType.wetSeed:
                combined = Instantiate(wetObject);
                break;
        }

        if (combined != null)
        {
            Character character = combined.GetComponent<Character>();
            this._characters.Add(character);
            combined.transform.parent = this.transform;
        }

        return combined;
    }

    void PauseGame()
    {
        CancelInvoke("PauseGame");
        if (!this._gameActive) return;

        this._gamePaused = true;
    }


    // Update is called once per frame
    void Update() {
        this._pressedLeftKey = (Input.GetKey(KeyCode.LeftShift));
        this._pressedRightKey = (Input.GetKey(KeyCode.RightShift));

        if (!this._pressedLeft && (this._pressedLeftButton || this._pressedLeftKey))
        {
            for (int i = 0; i < this._characters.Count; i++)
            {
                Character col = this._characters[i];
                col.rotateLeft();
            }
        }

        if (!this._pressedRight && (this._pressedRightButton || this._pressedRightKey))
        {
            for (int i = 0; i < this._characters.Count; i++)
            {
                Character col = this._characters[i];
                col.rotateRight();
            }
        }

        if ((this._pressedLeftButton || this._pressedLeftKey) && (this._pressedRightButton || this._pressedRightKey) && (!this._pressedRight || (!this._pressedLeft)))
        {
            CancelInvoke("PauseGame");
            Invoke("PauseGame", 1.5f);
            print("Double pressed");
        }

        this._pressedRight = (this._pressedRightButton || this._pressedRightKey);
        this._pressedLeft = (this._pressedLeftButton || this._pressedLeftKey);

        if ((!this._pressedLeft || !this._pressedRight) && this._gamePaused)
        {
            this._gamePaused = false;
        }
             
    }

    public void OnGoalAchieved()
    {
        int i, n;
        int succeses = 0;
        for (i = 0, n = this._goals.Count; i < n; i++)
        {
            if (this._goals[i].achieved) succeses++;
         }

        if(succeses == n)
        {
            this._gameActive = false;
        }
        GameObject.Find("GameUI").GetComponent<MainUI>().UpdateGoals();
    }
}
