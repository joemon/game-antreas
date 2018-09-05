using UnityEngine;

[DisallowMultipleComponent]
public class MovingObstacles : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(12f, 0f, 0f);
    [SerializeField] float period = 3f;

    private float movement;   // 0 for not moved, 1 for fully moved.
    private Vector3 startingPos;

    private bool result;


    /// <summary>
    /// Acts like a contsructor
    /// </summary>
    /// <param name="moveVector"> A Vector3 for the range of movement</param>
    /// <param name="p">A float for the period of the moving obstacle</param>
    public void Construct(Vector3 moveVector, float p)
    {
        movementVector = moveVector;
        period = p;
        startingPos = transform.position;
    }

    // Use this for initialization
    void Start()
    {
        startingPos = transform.position;
        result = false;
    }

    // Update is called once per frame
    void Update()
    {
        result = MakeTransition();

        if (result == false)
        {
            throw new System.Exception("Argument for period cannot be negative");
        }

    }

    /// <summary>
    ///  Method for implementing moving obstacles.
    ///  MovementVector indicates the axes and the range of the movement
    ///  and period indicates the speed of the moving obstacle
    /// </summary>
    /// <returns>
    /// False if the period is zero or negative
    /// True otherwise
    /// </returns>
    public bool MakeTransition()
    {
        if (period <= Mathf.Epsilon) { return false; }

        float sinWave = Mathf.Sin((Time.time / period) * (Mathf.PI * 2f));     

        movement = sinWave / 2f + 0.5f;
        Vector3 offset = movement * movementVector;

        transform.position = startingPos + offset;

        return true;
    }

}
