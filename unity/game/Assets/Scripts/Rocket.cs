using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip winLevel;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip correctAnswer;

    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem winLevelParticle;
    [SerializeField] ParticleSystem deathParticle;

    public GameObject completeLevelUI;


    //GameObject[] ansObstacles;

    public GameObject[] answers;
    Checker checker;

    enum State { Alive, Dying, WinStage}
    State state = State.Alive;

    Rigidbody rigidBody;
    AudioSource audioSource;


    
    string[] level1Εquations = { "8 + 5 * 2 =", "15 + 16 / 2 =", "12 * 4 - 3 =", "11 * 12 - 20 =", "6 * 5 =", "7 * 4 =", "8 * 8 =" };
    string[] level1Passwords = { "18", "23", "45", "112", "30", "28", "64" };

    string[] level2Εquations = { "8 * (8 - 6 / 3) = ?", "10 * 3 / (4 + 1 * 2) = ?", "(144 / (3 * 6 - 6)", "9 * (48 / 2 / 2)" };
    string[] level2Passwords = { "48", "5", "12", "108" };

    string[] level3Εquations = { "(8 * 7 - 1) / (1 + (15 * 4 / 6)) = ?", "(5^2 + 5) / ( 3 * ( 5 * 4 / 10 / 2 )) = ?", "(9 * 7) * ( 2^3 / (8 - 2 * 2)) = ?" };
    string[] level3Passwords = { "5", "10", "126" };

    Dictionary<string, string> selectedEquations;

    string password;
    int counter;
    public Text equation;

    
    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
        //checker = GetComponent<Checker>();
        //checker = GameObject.FindGameObjectWithTag("Num").GetComponent<Checker>();
        selectedEquations = new Dictionary<string, string>();
        SetAnswers();
    }
	
	// Update is called once per frame
	void Update () {
        if (state == State.Alive){
            Thrust();
            Rotate();
        }
    }

   
    private void OnCollisionEnter(Collision collision)
    {
        if (state != State.Alive){ return; }

        switch (collision.gameObject.tag)
        {
            case "Friendly":
                break;
            case "Finish":
                state = State.WinStage;
                audioSource.Stop();
                audioSource.PlayOneShot(winLevel);
                winLevelParticle.Play();
                // Update Score
                int total = GameManager.instance.getScore() + GameManager.instance.GetTimeLeft();
                Debug.Log("Final Score = " + total);

                Invoke( "CompleteLevel", 1f);
                break;
            case "Num":
                if (collision.gameObject.GetComponent<Checker>().number.text == selectedEquations.ElementAt(counter).Value)
                {
                    collision.gameObject.GetComponent<Checker>().GetComponent<BoxCollider>().enabled = false;
                    collision.gameObject.GetComponent<Checker>().GetComponent<BoxCollider>().isTrigger = true;

                    if (counter < selectedEquations.Count - 1) { 
                        counter++;
                        equation.text = selectedEquations.ElementAt(counter).Key;
                    }
                    else{
                        equation.text = "";
                    }

                    // Play a sound for correct answer and increase scorex
                    audioSource.PlayOneShot(correctAnswer);
                    GameManager.instance.UpdateScore();
                   

                }
                else
                {
                    state = State.Dying;
                    audioSource.Stop();
                    audioSource.PlayOneShot(death);
                    deathParticle.Play();
                    mainEngineParticle.Stop();
                    Invoke("LoadNextScene", 1f);
                }

                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                deathParticle.Play();
                mainEngineParticle.Stop();
                Invoke("LoadNextScene", 1f);
                break;
        }
    }

    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
    }

    private void LoadNextScene()
    {
        if (state == State.WinStage) {
            if (SceneManager.GetActiveScene().buildIndex == SceneManager.sceneCountInBuildSettings) {
                SceneManager.LoadScene(0);
            }
            else {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }

        }
        else if (state == State.Dying){    
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);   // I add * Time.deltaTime
            if (!audioSource.isPlaying){
                audioSource.PlayOneShot(mainEngine);
            }
            mainEngineParticle.Play();
        }
        else{
            audioSource.Stop();
            mainEngineParticle.Stop();
        }
    }

    private void Rotate()
    {
        rigidBody.freezeRotation = true;

        float rotationThisFrame = rcsThrust * Time.deltaTime;

        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        
        /*
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Rotate(Vector3.forward * rotationThisFrame);
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Rotate(-Vector3.forward * rotationThisFrame);
        }
        */

        rigidBody.freezeRotation = false;
    }

    private void SetAnswers()
    {
        /*
        answers[0].GetComponent<Checker>().number.text = "100";
        Debug.Log(answers.Length);

        collision.gameObject.GetComponent<Checker>().GetComponent<BoxCollider>().enabled = true;
        collision.gameObject.GetComponent<Checker>().GetComponent<BoxCollider>().isTrigger = false;
        */

        int index = -1;
        int i =0;

        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        for (i=0; i< answers.Length; i++)
        {
            checker = answers[i].GetComponent<Checker>();
            checker.number.text = Random.Range(0, 100).ToString();
            Debug.Log(answers[i].GetComponent<Checker>().number.text);
        }
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");

        

        for (i = 0; i < answers.Length; i=i+3)
        {
            // Pick a randomly the equation
            index = Random.Range(0, level1Passwords.Length);
            while (selectedEquations.ContainsKey(level1Εquations[index])){                
                index = Random.Range(0, level1Passwords.Length);
            }

            selectedEquations.Add(level1Εquations[index], level1Passwords[index]);
            

            int temp = Random.Range(i, i+3);
            checker = answers[temp].GetComponent<Checker>();
            checker.number.text = level1Passwords[index];
            answers[temp].GetComponent<BoxCollider>().enabled = true;
            answers[temp].GetComponent<BoxCollider>().isTrigger = false;


        }
        

        Debug.Log("---------------------------------------------------------");
        for (i = 0; i < selectedEquations.Count(); i++)
        {
            Debug.Log("Key: " + selectedEquations.ElementAt(i).Key + " Value: " + selectedEquations.ElementAt(i).Value);
        }
        Debug.Log("---------------------------------------------------------");

        //counter = selectedEquations.Count -1;
        counter = 0;
        Debug.Log(counter);
        equation.text = selectedEquations.ElementAt(counter).Key;


    }


}
