using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {

    public enum CollisionType
    {
        water,
        dirt,
        seed,
        mud,
        burriedSeed,
        wetSeed,
        flower

    };

    public enum FacingDirection
    {
        north,
        south,
        east,
        west
    }

    public CollisionType type;
    public FacingDirection currentDirection;
    public bool moving;
    public bool playerControlled;

    private Vector3 _startPosition;
    private Vector3 _endPosition;
    private float _startTime;
    private BaseScene _scene;

    private bool _hasMoved; //Prevents player from accidently despawning it as soon as it spawns.

    void Awake()
    {
        this._startPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);
        this._endPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);
    }

	// Use this for initialization
	void Start () {
        this._scene = GameObject.Find("GameScene").GetComponent<BaseScene>();

        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Cube);

        go.transform.parent = this.transform.FindChild("Direction");
        go.transform.localScale = new Vector3(.25f, .25f, .25f);
        go.transform.localPosition = new Vector3(0, 0, 0);
        Destroy(go.GetComponent<BoxCollider>());

        this._hasMoved = false;

        updateDirection();
    }
	
	// Update is called once per frame
	void Update () {

        if (this._startPosition != null && this._endPosition != null) transform.localPosition = Vector3.Lerp(this._startPosition, this._endPosition, (Time.time - this._startTime) *2);
        moving = ((Time.time - this._startTime) * 2 < 1);

        if (!moving)
        {
            if (this.transform.localPosition.x <= -((this._scene.boardWidth / 2) + 1) && this.currentDirection != FacingDirection.east)
            {
                this._scene.Remove(this, true);
                Destroy(gameObject);
            }

            if (this.transform.localPosition.x >= ((this._scene.boardWidth / 2) + 1) && this.currentDirection != FacingDirection.west)
            {
                this._scene.Remove(this, true);
                Destroy(gameObject);
            }

            if (this.transform.localPosition.z <= -((this._scene.boardHeight / 2) + 1) && this.currentDirection != FacingDirection.north)
            {
                this._scene.Remove(this, true);
                Destroy(gameObject);
            }

            if (this.transform.localPosition.z >= ((this._scene.boardHeight / 2) + 1) && this.currentDirection != FacingDirection.south)
            {
                this._scene.Remove(this, true);
                Destroy(gameObject);
            }
        }
    }

    public void moveForward()
    {
        this._hasMoved = true;
        this._startTime = Time.time;
        this._startPosition = new Vector3(this.transform.localPosition.x, this.transform.localPosition.y, this.transform.localPosition.z);
        this._endPosition = new Vector3(this.transform.localPosition.x + (this.transform.forward.x * .5f), this.transform.localPosition.y + (this.transform.forward.y * .5f), this.transform.localPosition.z + (this.transform.forward.z * .5f));
    }

    public void rotateRight()
    {
        /* if (!this._hasMoved || (this.transform.localPosition.x < -((this._scene.boardWidth / 2) + 1)) || (this.transform.localPosition.x > ((this._scene.boardWidth / 2) + 1)) || (this.transform.localPosition.z < -((this._scene.boardHeight / 2) + 1)) || (this.transform.localPosition.z > ((this._scene.boardHeight / 2) + 1)))
         {
             return;
         }*/
        if (!this._hasMoved || !this._scene.gameActive) return;

        switch (currentDirection)
        {
            case FacingDirection.east:
                currentDirection = FacingDirection.south;
                break;
            case FacingDirection.west:
                currentDirection = FacingDirection.north;
                break;
            case FacingDirection.north:
                currentDirection = FacingDirection.east;
                break;
            case FacingDirection.south:
                currentDirection = FacingDirection.west;
                break;
        }

        updateDirection();
    }

    public void rotateLeft()
    {
        /*if (!this._hasMoved || (this.transform.localPosition.x < -((this._scene.boardWidth / 2) + 1)) || (this.transform.localPosition.x > ((this._scene.boardWidth / 2)) + 1) || (this.transform.localPosition.z < -(this._scene.boardHeight / 2)) || (this.transform.localPosition.z > (this._scene.boardHeight / 2)))
        {
            return;
        }*/

        if (!this._hasMoved || !this._scene.gameActive) return;

        switch (currentDirection)
        {
            case FacingDirection.east:
                currentDirection = FacingDirection.north;
                break;
            case FacingDirection.west:
                currentDirection = FacingDirection.south;
                break;
            case FacingDirection.north:
                currentDirection = FacingDirection.west;
                break;
            case FacingDirection.south:
                currentDirection = FacingDirection.east;
                break;
        }

        updateDirection();
    }

    void updateDirection()
    {
        switch (currentDirection)
        {
            case FacingDirection.east:
                this.transform.localRotation = Quaternion.Euler(0, 90, 0);
                break;
            case FacingDirection.west:
                this.transform.localRotation = Quaternion.Euler(0, -90, 0);
                break;
            case FacingDirection.north:
                this.transform.localRotation = Quaternion.Euler(0, 0, 0);
                break;
            case FacingDirection.south:
                this.transform.localRotation = Quaternion.Euler(0, -180, 0);
                break;
        }
    }

    public void setupValues(Vector3 oldStart, Vector3 oldEnd, float oldStartTime, FacingDirection oldDirection)
    {
        this._startPosition = oldStart;
        this._endPosition = oldEnd;
        this._startTime = oldStartTime;
        this.currentDirection = oldDirection;
    }

    void OnTriggerEnter(Collider col)
    {
        switch (col.tag)
        {
            case "MovingCharacter":

                Character characterA = col.GetComponent<Character>();
                GameObject combined = null;

                if (characterA.type == CollisionType.flower && this.type != CollisionType.flower)
                {
                    this._scene.Remove(this, true);
                    Destroy(gameObject);
                }

                if ((characterA.type == CollisionType.seed && this.type == CollisionType.mud) ||
                    (characterA.type == CollisionType.dirt && this.type == CollisionType.wetSeed) || 
                    (characterA.type == CollisionType.water && this.type == CollisionType.burriedSeed) ||
                    (characterA.type == CollisionType.mud && this.type == CollisionType.burriedSeed) ||
                    (characterA.type == CollisionType.burriedSeed && this.type == CollisionType.wetSeed) ||
                    (characterA.type == CollisionType.wetSeed && this.type == CollisionType.mud))
                {
                    this._scene.Remove(this);
                    combined = this._scene.Create(CollisionType.flower);
                    Destroy(gameObject);

                    this._scene.Remove(characterA);
                    Destroy(characterA.gameObject);
                }

                if ((characterA.type == CollisionType.dirt && this.type == CollisionType.water))
                {
                    this._scene.Remove(this);
                    combined = this._scene.Create(CollisionType.mud);
                    Destroy(gameObject);

                    this._scene.Remove(characterA);
                    Destroy(characterA.gameObject);
                }

                if ((characterA.type == CollisionType.water && this.type == CollisionType.seed))
                {
                    this._scene.Remove(this);
                    combined = this._scene.Create(CollisionType.wetSeed);
                    Destroy(gameObject);

                    this._scene.Remove(characterA);
                    Destroy(characterA.gameObject);
                }

                if ((characterA.type == CollisionType.seed && this.type == CollisionType.dirt))
                {
                    this._scene.Remove(this);
                    combined = this._scene.Create(CollisionType.burriedSeed);
                    Destroy(gameObject);

                    this._scene.Remove(characterA);
                    Destroy(characterA.gameObject);
                }

                if (combined != null)
                {
                    combined.GetComponent<Character>().setupValues(this._startPosition, this._endPosition, this._startTime, this.currentDirection);
                }
                break;

        }
    }
    
}
