using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MathChecker : MonoBehaviour {
    string password;

    string[] level1Εquations = { "8 + 5 * 2 = ?", "15 + 16 / 2 = ?", "12 * 4 - 3 = ?", "11 * 12 - 20 = ?", "6 * 5 = ?", "7 * 4 = ?", "8 * 8 = ?" };
    string[] level1Passwords = { "18", "23", "45", "112", "30", "28", "64" };

    string[] level2Εquations = { "8 * (8 - 6 / 3) = ?", "10 * 3 / (4 + 1 * 2) = ?", "(144 / (3 * 6 - 6)", "9 * (48 / 2 / 2)" };
    string[] level2Passwords = { "48", "5", "12", "108" };

    string[] level3Εquations = { "(8 * 7 - 1) / (1 + (15 * 4 / 6)) = ?", "(5^2 + 5) / ( 3 * ( 5 * 4 / 10 / 2 )) = ?", "(9 * 7) * ( 2^3 / (8 - 2 * 2)) = ?" };
    string[] level3Passwords = { "5", "10", "126" };
    
    /*
    public Text message;
    public Text equation;
    public InputField inputField;
    

    // Use this for initialization
    void Start () {
        inputField.Select();
        inputField.ActivateInputField();
        inputField.onEndEdit.AddListener(delegate { ValueChangeCheck(); });
        SelectEquation();
    }


    private void ValueChangeCheck()
    {
        if (inputField.text == "menu")
        {
            // ShowMainMenu();
        }
        else if (inputField.text == "quit" || inputField.text == "close" || inputField.text == "exit")
        {
            inputField.text = "";
            equation.text = "";
            message.text = "If you want to exit just close the tab.";
            Application.Quit();
        }
        else
        {
            CheckPassword(inputField.text);
        }
    }

    private void SelectEquation()
    {
        int index = -1;
        if (SceneManager.GetActiveScene().buildIndex < 2)
        {
            index = Random.Range(0, level1Passwords.Length);
            password = level1Passwords[index];
            equation.text = level1Εquations[index];
        }
        else if (SceneManager.GetActiveScene().buildIndex < 4)
        {
            index = Random.Range(0, level2Passwords.Length);
            password = level2Passwords[index];
            equation.text = level2Εquations[index];
        }
        else
        {
            index = Random.Range(0, level3Passwords.Length);
            password = level3Passwords[index];
            equation.text = level3Εquations[index];
        }

    }

    void CheckPassword(string input)
    {
        if (input == password)
        {
            LoadNextLevel();
        }
        else
        {
            inputField.text = "";
            message.text = "Wrong answer. Please try again.";
            Start();
        }
    }

    private void LoadNextLevel()
    {
        if (SceneManager.GetActiveScene().buildIndex + 1 == SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("AAAAAAAAAAA");
            SceneManager.LoadScene(0);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
    */
}
