//using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip winLevel;
    [SerializeField] AudioClip death;

    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem winLevelParticle;
    [SerializeField] ParticleSystem deathParticle;

    public GameObject completeLevelUI;

    GameObject[] answers;
    Checker checker;

    enum State { Alive, Dying, WinStage}
    State state = State.Alive;

    Rigidbody rigidBody;
    AudioSource audioSource;


    string password;
    int counter;

    string[] level1Εquations = { "8 + 5 * 2 = ?", "15 + 16 / 2 = ?", "12 * 4 - 3 = ?", "11 * 12 - 20 = ?" };
    string[] level1Passwords = { "18", "23", "45", "112" };

    string[] level2Εquations = { "8 * (8 - 6 / 3) = ?", "10 * 3 / (4 + 1 * 2) = ?", "(144 / (3 * 6 - 6)", "9 * (48 / 2 / 2)" };
    string[] level2Passwords = { "48", "5", "12", "108" };

    string[] level3Εquations = { "(8 * 7 - 1) / (1 + (15 * 4 / 6)) = ?", "(5^2 + 5) / ( 3 * ( 5 * 4 / 10 / 2 )) = ?", "(9 * 7) * ( 2^3 / (8 - 2 * 2)) = ?" };
    string[] level3Passwords = { "5", "10", "126" };




    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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
                Invoke( "CompleteLevel", 1f);
                break;
            case "Num":
                if (collision.gameObject.GetComponent<Checker>().number.text == password)
                {
                    //Play a sound
                    return;
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

        rigidBody.freezeRotation = false;
    }

    private void SetAnswers()
    {
        
        if (answers == null)
            answers = GameObject.FindGameObjectsWithTag("Num");

        checker = GameObject.FindGameObjectWithTag("Num").GetComponent<Checker>();
        
        answers[0].GetComponent<Checker>().number.text = "100";
        answers[1].GetComponent<Checker>().number.text = "100";
        answers[2].GetComponent<Checker>().number.text = "100";
        Debug.Log(answers.Length);

        

        int index = -1;
        int i =0;
        for (i=0; i< answers.Length; i++)
        {
            checker = answers[i].GetComponent<Checker>();
            checker.number.text = Random.Range(0, 100).ToString();

        }

        for (i = 2; i < answers.Length; i=i+3)
        {
            index = Random.Range(0, level1Passwords.Length);
            password = level1Passwords[index];
            //equation.text = level1Εquations[index];

            int temp = Random.Range(i - 2, i);
            checker = answers[temp].GetComponent<Checker>();
            answers[temp].GetComponent<BoxCollider>().enabled = false;
            answers[temp].GetComponent<BoxCollider>().isTrigger = true;
            checker.number.text = level1Passwords[index];
            


            Debug.Log(checker.number.ToString());
            Debug.Log(password);

        }
        
        
    }


}
