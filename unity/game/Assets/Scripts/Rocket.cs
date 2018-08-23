using System;
using System.Collections;
using System.Collections.Generic;
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
   

    enum State { Alive, Dying, WinStage}
    State state = State.Alive;

    Rigidbody rigidBody;
    AudioSource audioSource;

    // Use this for initialization
    void Start () {
        rigidBody = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
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

}
