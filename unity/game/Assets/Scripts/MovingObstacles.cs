using UnityEngine;

[DisallowMultipleComponent]
public class MovingObstacles : MonoBehaviour
{

    [SerializeField] Vector3 movementVector = new Vector3(12f, 0f, 0f);
    [SerializeField] float period = 3f;

    float movementFactor;   // 0 for not moved, 1 for fully moved.

    Vector3 startingPos;

    // Use this for initialization
    void Start()
    {
        startingPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (period <= Mathf.Epsilon) { return; }

        // grows as the game goes, from 0
        // float cycles = Time.time / period;

        //const float tau = Mathf.PI * 2f;

        makeTransition();
    }

    public void makeTransition()
    {
        float rawSinWave = Mathf.Sin((Time.time / period) * (Mathf.PI * 2f));     // goes from -1 to +1

        movementFactor = rawSinWave / 2f + 0.5f;
        Vector3 offset = movementFactor * movementVector;
        transform.position = startingPos + offset;
    }
}
