using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField] private bool controleQwerty = true;
    private KeyCode right = KeyCode.D;
    private KeyCode left = KeyCode.A;
    private KeyCode up = KeyCode.W;
    private KeyCode down = KeyCode.S;

    [SerializeField] private List<GameObject> states = new List<GameObject>();

    [SerializeField] private float speed = 5;
    [SerializeField] private float distanceToMove = 1;

    private bool isMoving = false;
    private Vector3 endPosition;

    [SerializeField] private int nbMove = 0;
    [SerializeField] private int nbMoveMax = 10;
    private Vector3 initPos;

    [SerializeField] private GameObject corpPrefab;
    [SerializeField] private LayerMask wallLayer = 9;
    [SerializeField] private LayerMask switchLayer = 10;
    
    private Switch tmpSwitch;

    [SerializeField] private int idNextLevel = 0;
    [SerializeField] private Text txtNbMoveLeft;

    private void Awake()
    {
        if (!GameManager.instance.option.qwerty)
        {
            right = KeyCode.D;
            left = KeyCode.Q;
            up = KeyCode.Z;
            down = KeyCode.S;
        }

        Corp.instance = null;
    }

    void Start()
    {
        initPos = transform.position;
        endPosition = transform.position;
    }

    void FixedUpdate()
    {
        if (isMoving && !(nbMove > nbMoveMax))
        {
            transform.position = Vector3.MoveTowards(transform.position, endPosition, speed * Time.deltaTime);
            if (transform.position == endPosition)
            {
                isMoving = false;
                nbMove++;
                txtNbMoveLeft.text = (nbMoveMax - nbMove).ToString();
                foreach (GameObject item in states)
                {
                    item.SetActive(false);
                }
                if (nbMove < 10)
                {
                    states[(nbMove / 2)].SetActive(true);
                }
                else
                {
                    states[0].SetActive(true);
                }

                if (tmpSwitch != null)
                {
                    tmpSwitch.Unactive();
                }

                if (OnSwitch())
                {
                    Ray ray = new Ray(transform.position, transform.position + Vector3.down - transform.position);
                    RaycastHit hit;
                    if(Physics.Raycast(ray, out hit, 1, switchLayer))
                    {
                        tmpSwitch = hit.transform.gameObject.GetComponent<Switch>();
                        tmpSwitch.Activate();
                    }
                }
            }
        }
    }

    void Update()
    {
        if (nbMove >= nbMoveMax)
        {
            Corp tmpCorp = Instantiate(corpPrefab, transform.position, transform.rotation).GetComponent<Corp>();
            transform.position = initPos;
            endPosition = transform.position;
            nbMove = 0;
            txtNbMoveLeft.text = (nbMoveMax - nbMove).ToString();

            if (tmpSwitch != null)
            {
                tmpCorp.sitOn = tmpSwitch;
                tmpSwitch = null;
            }
        }
        else
        {
            MoveInput();
        }
    }

    private void MoveInput()
    {
        if (isMoving)
        {
            return;
        }
        if (Input.GetKey(left) && !Input.GetKey(right)) //Left
        {
            Vector3 tmpEndPos = new Vector3(endPosition.x - distanceToMove, endPosition.y, endPosition.z);
            if (!ColisionWithWall(tmpEndPos))
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, -90, 0));
                endPosition = tmpEndPos;
                isMoving = true;
            }
            
        }
        else if (Input.GetKey(right) && !Input.GetKey(left)) //Right
        {
            Vector3 tmpEndPos = new Vector3(endPosition.x + distanceToMove, endPosition.y, endPosition.z);
            if (!ColisionWithWall(tmpEndPos))
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 90, 0));
                endPosition = tmpEndPos;
                isMoving = true;
            }
        }

        if (Input.GetKey(up) && !Input.GetKey(down)) //Up
        {
            Vector3 tmpEndPos = new Vector3(endPosition.x, endPosition.y, endPosition.z + distanceToMove);
            if (!ColisionWithWall(tmpEndPos))
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
                endPosition = tmpEndPos;
                isMoving = true;
            }
        }
        else if (Input.GetKey(down) && !Input.GetKey(up)) //Down
        {
            Vector3 tmpEndPos = new Vector3(endPosition.x, endPosition.y, endPosition.z - distanceToMove);
            if (!ColisionWithWall(tmpEndPos))
            {
                transform.rotation = Quaternion.Euler(new Vector3(0, 180, 0));
                endPosition = tmpEndPos;
                isMoving = true;
            }
        }
    }

    private bool ColisionWithWall(Vector3 direct)
    {
        Ray ray = new Ray(transform.position, direct - transform.position);
        Debug.DrawLine(transform.position, direct, Color.red, 2);
        if (Physics.Raycast(ray, 1, wallLayer))
        {
            return true;
        }
        return false;
    }

    private bool OnSwitch()
    {
        Ray ray = new Ray(transform.position, transform.position + Vector3.down - transform.position);
        Debug.DrawLine(transform.position, transform.position + Vector3.down, Color.red, 2);
        if (Physics.Raycast(ray, 1, switchLayer))
        {
            return true;
        }
        return false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Gemme")
        {
            Vector3 tmpInitPos = initPos;

            nbMove = -1;

            initPos = new Vector3(other.gameObject.transform.position.x, initPos.y, other.gameObject.transform.position.z);
            other.transform.position = tmpInitPos;
        }
        else if (other.gameObject.tag == "EndStage")
        {
            MenuManager.instance.LoadScene(idNextLevel);
        }
    }
}
