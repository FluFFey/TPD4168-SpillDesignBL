using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum HERO_STATE
{
    IDLE,
    MOVING,
    DASHING,
    NO_OF_STATES
}

public class InputHandler : MonoBehaviour {

    public int moveSpeed = 1;
    public Sprite heroDashingSprite; //idle hero sprite when he is dashing
    public Sprite dashSprite; //Sprite for actual dash object
    public Sprite heroSprite;
    public float dashTimer; //dashtime in seconds
    public float dashRange; //dashrange in unity units
    public Material dasherMaterial;
    private Vector3 newVelocity;
    private int state;
    private float dt;
    // Use this for initialization
    void Start () {
        state = (int)HERO_STATE.IDLE;
	}

    void OnCollisionEnter2D(Collision2D col)
    {
        print("yo");
        moveSpeed = 0;
    }
    void OnCollisionExit2D(Collision2D col)
    {
        moveSpeed = 1;
    }

    // Update is called once per frame
    void Update()
    {
        dt = Time.deltaTime;
        switch (state)
        {
            case (int)HERO_STATE.IDLE:
                break;
            case (int)HERO_STATE.DASHING:
                break;
            case (int)HERO_STATE.MOVING:
                if (Input.GetKeyDown(KeyCode.Space)) //can't dash unless you're moving(aka:holding a direction, can change l8r if we want OBVIOUSLY!)
                {
                    StartCoroutine(ghostDash());
                }
                break;
            default:
                break;
        }
        
    }

    IEnumerator ghostDash()
    {
        state = (int)HERO_STATE.DASHING;
        gameObject.GetComponent<SpriteRenderer>().sprite = heroDashingSprite;
        
        GameObject dasher = new GameObject("dasher",typeof(SpriteRenderer),typeof(DashScript));
        dasher.transform.parent = this.transform;
        Camera.main.GetComponent<CameraScript>().target = dasher;
        transform.GetChild(0).parent = dasher.transform;
        dasher.transform.Translate(new Vector3(0, 0, 0.2f));
        dasher.transform.transform.localPosition = new Vector3(0, 0, 0);
        dasher.GetComponent<SpriteRenderer>().sprite = dashSprite;
        dasher.GetComponent<SpriteRenderer>().sortingOrder = 1;
        GetComponent<SpriteRenderer>().sortingOrder = 0;
        dasher.GetComponent<SpriteRenderer>().material = dasherMaterial;
        float smoothDelta = 0.0f;
        float oldSmoothPos = 0.0f;
        float smoothPos = 0.0f;
        for (float i = 0; i < dashTimer; i+=dt)
        {
            float pd = i / dashTimer; //pd =percent done
            //smoothPos = pd * pd * (3 - 2 * pd);
            smoothPos = 1 - (1 - pd) * (1 - pd);    
            smoothPos = Mathf.Pow(smoothPos, 4); //hype function
            smoothDelta = smoothPos - oldSmoothPos;
           // print(smoothDelta);
            oldSmoothPos = smoothPos;
            dasher.transform.Translate(newVelocity.normalized * smoothDelta*dashRange);
            //Check collision all ze time   
            yield return 0;
        }
        float failTimer = 0.07f; //never question this variable
        float rotateAngle = 10.0f;
        float randomNumber = 6.0f; //while you're at it, don't question this one either
        for (float i = 0; i < failTimer*randomNumber; i+=dt)
        {
            float phase = Mathf.Sin(i/ failTimer);
            dasher.transform.rotation = Quaternion.Euler(new Vector3(0, 0, phase * rotateAngle));
            yield return 0;
        }
        dasher.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));//make sure we are straight!
        smoothDelta = 0.0f;
        oldSmoothPos = 0.0f;
        smoothPos = 0.0f;
        for (float i = 0; i < dashTimer; i += dt)
        {
            float pd = i / dashTimer; //pd =percent done
            //smoothPos = pd * pd * (3 - 2 * pd);
            smoothPos = 1 - (1 - pd) * (1 - pd);
            smoothPos = Mathf.Pow(smoothPos, 4); //hype function

           // smoothPos = pd * pd;
            smoothDelta = smoothPos - oldSmoothPos;
            oldSmoothPos = smoothPos;
            dasher.transform.Translate(-newVelocity.normalized * smoothDelta * dashRange);

            //if (dasher.)
            yield return 0;
        }
        state = (int)HERO_STATE.IDLE;
        gameObject.GetComponent<SpriteRenderer>().sprite = heroSprite;
        Camera.main.GetComponent<CameraScript>().target = this.gameObject;
        GameObject.Destroy(dasher); //we erase our tracks
    }


    void FixedUpdate () {
        if (state != (int)HERO_STATE.DASHING)
        {
            newVelocity = new Vector3(0, 0, 0);
            float speed = 4 * Time.fixedDeltaTime;
            if (Input.GetKey(KeyCode.W) || Input.GetKeyDown(KeyCode.W))
            {
                newVelocity.y += 1;
            }
            if (Input.GetKey(KeyCode.S) || Input.GetKeyDown(KeyCode.S))
            {
                newVelocity.y -= 1;
            }
            if (Input.GetKey(KeyCode.A) || Input.GetKeyDown(KeyCode.A))
            {
                newVelocity.x -= 1;
            }
            if (Input.GetKey(KeyCode.D) || Input.GetKeyDown(KeyCode.D))
            {
                newVelocity.x += 1;
            }
            state = newVelocity.x == 0 && newVelocity.y == 0 ?
                (int)HERO_STATE.IDLE :
                (int)HERO_STATE.MOVING;
            newVelocity = newVelocity.normalized * speed;
            gameObject.transform.Translate(newVelocity);
            /*
            gameObject.GetComponent<Rigidbody2D>().AddForce(newVelocity*8);
            Vector2 oldVec = gameObject.GetComponent<Rigidbody2D>().velocity;
            if (oldVec.x >= maxSpeed)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(maxSpeed, oldVec.y);
            }
            if (oldVec.x <= -maxSpeed)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(-maxSpeed, oldVec.y);
            }

            if (oldVec.y >= maxSpeed)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(oldVec.x, maxSpeed);
            }

            if (oldVec.y <= -maxSpeed)
            {
                gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(oldVec.x, -maxSpeed);
            }
            */
        }
    }
}
