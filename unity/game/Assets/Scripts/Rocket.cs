using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
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
    [SerializeField] AudioClip wrongAnswer;

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


    Dictionary<string, string> allTheEquations;

    Dictionary<string, string> selectedEquations;

    string password;
    int counter;
    public Text equation;

    
    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        allTheEquations = new Dictionary<string, string>();
        selectedEquations = new Dictionary<string, string>();

        LoadEquations();

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

    

    void OnTriggerEnter(Collider collider)
    {
        if (state != State.Alive) { return; }

        switch (collider.gameObject.tag)
        {
            case "Num":
                if (collider.gameObject.GetComponent<Checker>().number.text == selectedEquations.ElementAt(counter).Value)
                {
                    // Play a sound for correct answer and increase the score by 100 points
                    audioSource.PlayOneShot(correctAnswer);
                    GameManager.instance.UpdateScore(100);
                }
                else
                {
                    // Play a sound for wrong answer and reduce the score by 50 points
                    audioSource.PlayOneShot(wrongAnswer, 0.3f);
                    GameManager.instance.UpdateScore(-50);
                }
                

                // If there is next equation then display it on the screen otherwise dont show anything.
                if (counter < selectedEquations.Count - 1)
                {
                    counter++;
                    equation.text = selectedEquations.ElementAt(counter).Key;
                }
                else
                {
                    equation.text = "";
                }

                // For each equation I have to remove all the three possible answers just after the selection of one of them. 
                Destroy(collider.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.GetChild(4).gameObject);
                Destroy(collider.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.GetChild(5).gameObject);
                Destroy(collider.gameObject.transform.parent.gameObject.transform.parent.gameObject.transform.GetChild(6).gameObject);

                break;
            default:
                break;
        }
    }


    public void CompleteLevel()
    {
        completeLevelUI.SetActive(true);
        
        int total = GameManager.instance.getScore() + GameManager.instance.GetTimeLeft();

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

    
  
 
    private bool LoadEquations()
    {
        // Handle any problems that might arise when reading the text
        try
        {
            string path = "Assets/Equations/equations.txt";
            string line;
                
            // Create a new StreamReader, tell it which file to read and the encoding of the file
            StreamReader theReader = new StreamReader(path, Encoding.Default);

            using (theReader)
            {
                // While there's lines left in the text file, do this:
                do
                {
                    line = theReader.ReadLine();

                    if (line != null)
                    {
                        // I split it into equation and answers based on '=' 
                        // and then I save them into a dictionary as key and value.
                        string[] entries = line.Split('=');
                        if ((entries.Length > 0) && (entries.Length == 2))
                        {
                            allTheEquations.Add(entries[0] + "=" , entries[1]);
                        }
                    }
                }
                while (line != null);
                // Done reading, close the reader and return true 

                Debug.Log("***********************************************************");
                int i;
                for (i = 0; i < allTheEquations.Count(); i++)
                {
                    Debug.Log("Key: " + allTheEquations.ElementAt(i).Key + " Value: " + allTheEquations.ElementAt(i).Value);
                }
                Debug.Log("***********************************************************");

            theReader.Close();
                return true;
            }
        }
        // If something goes wrong, we throw an exception with some information
        catch (System.Exception e)
        {
            System.Console.WriteLine("{0}\n", e.Message);
            return false;
        }
    }
        



    
    private void SetAnswers()
    {
        
        int index = -1;
        int i =0;

        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        for (i=0; i< answers.Length; i++)
        {
            checker = answers[i].GetComponent<Checker>();
            checker.number.text = Random.Range(0, 110).ToString();
            Debug.Log(answers[i].GetComponent<Checker>().number.text);
        }
        Debug.Log("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");


        /*
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
        */

        for (i = 0; i < answers.Length; i = i + 3)
        {
            // Pick the equation Randomly.
            index = Random.Range(0, allTheEquations.Count);
            // If that equation has been allready selected, pick another one.
            while (selectedEquations.ContainsKey(allTheEquations.ElementAt(index).Key))
            {
                index = Random.Range(0, allTheEquations.Count);
            }

            selectedEquations.Add(allTheEquations.ElementAt(index).Key, allTheEquations.ElementAt(index).Value);

            int temp = Random.Range(i, i + 3);
            checker = answers[temp].GetComponent<Checker>();
            checker.number.text = allTheEquations.ElementAt(index).Value;
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
