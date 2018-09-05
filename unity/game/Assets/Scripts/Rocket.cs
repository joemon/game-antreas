using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class Rocket : MonoBehaviour {
    [SerializeField] float rcsThrust = 100f;
    [SerializeField] float mainThrust = 100f;

    // Variables for the audioclips
    [SerializeField] AudioClip mainEngine;
    [SerializeField] AudioClip winLevel;
    [SerializeField] AudioClip death;
    [SerializeField] AudioClip correctAnswer;
    [SerializeField] AudioClip wrongAnswer;

    // Variable for the particles of the rocket for engine fire and death explotion
    [SerializeField] ParticleSystem mainEngineParticle;
    [SerializeField] ParticleSystem deathParticle;

    enum State { Alive, Dying, WinStage }
    State state = State.Alive;

    public GameObject[] answers;
    Checker checker;
    
    Rigidbody rigidBody;
    AudioSource audioSource;

    Dictionary<string, string> allTheEquations;
    Dictionary<string, string> selectedEquations;

    public Text equation;
    int counter;

    public void Construct(Dictionary<string, string> allEquations)
    {
        allTheEquations = allEquations;
    }

    // Use this for initialization
    void Start () {
        //PlayerPrefs.DeleteAll();

        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();

        allTheEquations = new Dictionary<string, string>();
        selectedEquations = new Dictionary<string, string>();

        // Load text file
        TextAsset textAssets = (TextAsset)Resources.Load("equations");
        LoadEquations(textAssets);

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
                Invoke("CompleteLevel", 1f);
                break;
            default:
                state = State.Dying;
                audioSource.Stop();
                audioSource.PlayOneShot(death);
                deathParticle.Play();
                mainEngineParticle.Stop();
                Invoke("ReloadLevel", 1f);
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

    // If the player manage to finish a level
    public void CompleteLevel()
    {
        GameManager.instance.CompleteLevel();
    }

    // If the player hit on an obstacle and lose
    private void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    
    private void Thrust()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            GameManager.instance.StartTimer();
            rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);   
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

  
 
    public bool LoadEquations(TextAsset textAssets)
    {
        // Handle any problems that might arise when reading the text
        try
        {
            var equationsList = EquationsToList(textAssets);

            string line;

            for (int i = 0; i < equationsList.Count; i++)
            {
                line = equationsList[i];
                if (line != null)
                {
                    // I split it into equation and answers based on '=' 
                    // and then I save them into a dictionary as key and value.
                    string[] entries = line.Split('=');
                    if ((entries.Length > 0) && (entries.Length == 2))
                    {
                        allTheEquations.Add(entries[0] + "= ?", entries[1]);
                    }
                }
            }
            
            return true;
        }
        catch (System.Exception e)      
        {
            // If something goes wrong, throw an exception and return false.
            Debug.Log(e.Message);
            return false;
        }
        
    }


    private List<string> EquationsToList(TextAsset ta)
    {
        var listToReturn = new List<string>();
        var arrayString = ta.text.Split('\n');

        for(int i=0; i<arrayString.Length; i++)
        {
            listToReturn.Add(arrayString[i]);
        }

        return listToReturn;
    }


    private void SetAnswers()
    {

        int index = -1;
        List<int> ansPositions = new List<int>();
        List<string> ansValues = new List<string>();

        
        for (int i = 0; i < answers.Length; i = i + 3)
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

            // Save the position and the value of the correct answers. 
            ansPositions.Add(temp);
            string content = Regex.Replace(allTheEquations.ElementAt(index).Value, @"\s+", string.Empty);
            ansValues.Add(content);
        }

        
        for (int i = 0; i < answers.Length; i++)
        {
            if (ansPositions.Contains(i))
            {
                continue;
            }
            else
            { 
                string rand = Random.Range(0, 110).ToString();
                while (ansValues.Contains(rand))
                {
                    rand = Random.Range(0, 110).ToString();
                }

                checker = answers[i].GetComponent<Checker>();
                checker.number.text = rand;
            }
        }
        

        counter = 0;
        equation.text = selectedEquations.ElementAt(counter).Key;

    }


}
