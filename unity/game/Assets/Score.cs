using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour {

    
    public Transform rocket;
    public Text scoreText;

    
    //private float startTime = Time.time;
    private float timer;
    
    private float score;

    // Update is called once per frame
    void Update () {

        score = rocket.position.x + 317;
        scoreText.text = score.ToString("0");
        
        
        /*
        timer = Time.time;
        score = 10000 / timer;
        scoreText.text = score.ToString("0");
        */

        /*
        timer = Time.time + startTime;
        scoreText.text = timer.ToString("0");
        */



    }

    public float getScore()
    {
        return (this.score);
    } 


}
