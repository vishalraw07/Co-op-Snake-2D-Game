using System.Collections;
using UnityEngine;
public enum Player
{
    Player1, Player2
}
public class PlayerMovement : MonoBehaviour
{
    Vector3 newDirection;
    Vector3 headPos;
    [SerializeField] float timeStep = 0.5f;
    [SerializeField] float step = 0.5f;
    [SerializeField] GameObject bodyPrefab;
    [SerializeField] Transform[] bodies;
    [SerializeField] public Player player;
    int length;
    float newX, newY;
    [SerializeField] float minX;
    [SerializeField] float maxX;
    [SerializeField] float minY;
    [SerializeField] float maxY;
    void Awake()
    {
        length = -1;
    }
    void Start()
    {
        if (player == Player.Player1)
            newDirection = Vector3.right;
        if (player == Player.Player2)
            newDirection = Vector3.left;
        bodies = GetComponentsInChildren<Transform>();
        length = bodies.Length;
        if (length < 1)
        {
            Debug.LogError("No body segments found");
            return;
        }
        StartCoroutine(MoveHead());
    }
    IEnumerator MoveHead()
    {
        while (true)
        {
            Vector3[] oldPositions = new Vector3[length];
            for (int i = 0; i < length; i++)
            {
                oldPositions[i] = bodies[i].position;
            }
            headPos = oldPositions[1];
            newX = step * newDirection.x;
            newY = step * newDirection.y;
            headPos += new Vector3(newX * WrapSnakeX(newX + headPos.x), newY * WrapSnakeY(newY + headPos.y), 0);
            bodies[1].position = headPos;
            for (int i = 2; i < length; i++)
            {
                if (bodies[i] != null && oldPositions[i - 1] != null)
                    MoveBody(i, oldPositions[i - 1]);
            }
            yield return new WaitForSeconds(timeStep);
        }
    }
    float WrapSnakeX(float newX)
    {
        if (newX > maxX)
        {
            return -2 * 2 * maxX;
        }
        if (newX < minX)
        {
            return 2 * 2 * minX;
        }
        return 1;
    }
    float WrapSnakeY(float newY)
    {
        if (newY > maxY)
        {
            return (-2 * 2 * maxY) - 2;
        }
        if (newY < minY)
        {
            return (2 * 2 * minY) + 2;
        }
        return 1;
    }
    void MoveBody(int index, Vector3 prevPos)
    {
        bodies[index].transform.position = prevPos;
    }
    public int GetPlayerLength()
    {
        return length;
    }
    void Update()
    {
        if (player == Player.Player1)
        {
            if (Input.GetKeyDown(KeyCode.W) && newDirection != Vector3.down)
                newDirection = Vector3.up;
            else if (Input.GetKeyDown(KeyCode.S) && newDirection != Vector3.up)
                newDirection = Vector3.down;
            else if (Input.GetKeyDown(KeyCode.A) && newDirection != Vector3.right)
                newDirection = Vector3.left;
            else if (Input.GetKeyDown(KeyCode.D) && newDirection != Vector3.left)
                newDirection = Vector3.right;
        }
        if (player == Player.Player2)
        {
            if (Input.GetKeyDown(KeyCode.UpArrow) && newDirection != Vector3.down)
                newDirection = Vector3.up;
            else if (Input.GetKeyDown(KeyCode.DownArrow) && newDirection != Vector3.up)
                newDirection = Vector3.down;
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && newDirection != Vector3.right)
                newDirection = Vector3.left;
            else if (Input.GetKeyDown(KeyCode.RightArrow) && newDirection != Vector3.left)
                newDirection = Vector3.right;
        }
    }
    public void SpeedUp()
    {
        timeStep = timeStep / 2;
    }
    public Transform[] GetBodiesTransform()
    {
        bodies = GetComponentsInChildren<Transform>();
        return bodies;
    }
    [ContextMenu("Add Body")]
    public void AddBody()
    {
        StartCoroutine(AddBodyWaiter());
    }
    IEnumerator AddBodyWaiter()
    {
        Vector3 currentHeadPos = bodies[1].position;
        yield return new WaitForSeconds(timeStep * (length - 1));
        GameObject newBody = Instantiate(bodyPrefab, currentHeadPos, Quaternion.identity);
        newBody.transform.parent = this.transform;
        newBody.transform.SetAsLastSibling();
        length++;
        bodies = GetComponentsInChildren<Transform>();
    }
    public void RemoveBody()
    {
        Destroy(bodies[length - 1].gameObject);
        length--;
        bodies = GetComponentsInChildren<Transform>();
    }
    public void PowerUp(PowerUpType powerUpType, int timer)
    {
        switch (powerUpType)
        {
            case PowerUpType.SpeedBoost:
                StartCoroutine(SpeedBoost(timer));
                break;
            case PowerUpType.Shield:
                StartCoroutine(ShieldEnable(timer));
                break;
            default: break;
        }
    }
    IEnumerator ShieldEnable(int timer)
    {
        MPBody headBody = bodies[1].GetComponent<MPBody>();
        headBody.TriggerShield();
        yield return new WaitForSeconds(timer);
        headBody.TriggerShield();
    }
    IEnumerator SpeedBoost(int timer)
    {
        float oldTimeStep = timeStep;
        timeStep = timeStep / 2;
        yield return new WaitForSeconds(timer);
        timeStep = oldTimeStep;
    }
}
