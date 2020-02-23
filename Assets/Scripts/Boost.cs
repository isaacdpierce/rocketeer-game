using UnityEngine;
using UnityEngine.SceneManagement;

public class Boost : MonoBehaviour
{
  [SerializeField] float rcsThrust = 100f;
  [SerializeField] float mainThrust = 100f;
  [SerializeField] float levelLoadDelay = 2f;
  [SerializeField] float effectsLoadDelay = 1f;
  [SerializeField] AudioClip mainEngine;
  [SerializeField] AudioClip mainFinish;
  [SerializeField] AudioClip mainDeath;
  [SerializeField] AudioClip explode;
  [SerializeField] AudioClip mainStart;

  [SerializeField] ParticleSystem jetsParticles;
  [SerializeField] ParticleSystem explodeParticles;
  [SerializeField] ParticleSystem finishParticles;
  Rigidbody rigidBody;
  AudioSource audioSource;

  enum State { Alive, Dying, Transcending }
  State state = State.Alive;

  bool collisionsDisabled = false;

  // Start is called before the first frame update
  void Start()
  {
    rigidBody = GetComponent<Rigidbody>();
    audioSource = GetComponent<AudioSource>();

  }

  // Update is called once per frame
  void Update()
  {
    if (state == State.Alive)
    {
      RespondToThrustInput();
      RespondToRotateInput();
    }

    if (Debug.isDebugBuild)
    {
      RespondToDebugKeys();
    }


  }
  void RespondToDebugKeys()
  {
    if (Input.GetKeyDown(KeyCode.L))
    {
      LoadNextLevel();
    }
    if (Input.GetKeyDown(KeyCode.C))
    {
      collisionsDisabled = !collisionsDisabled;
    }
  }

  void OnCollisionEnter(Collision collision)
  {
    if (state != State.Alive || collisionsDisabled) { return; }

    switch (collision.gameObject.tag)
    {

      case "Friendly":
        break;
      case "Finish":
        StartEndSequence();
        break;
      default:
        StartDeathSequence();
        break;
    }
  }

  private void StartEndSequence()
  {
    state = State.Transcending;
    audioSource.Stop();
    audioSource.PlayOneShot(mainFinish);
    finishParticles.Play();
    Invoke("LoadNextLevel", levelLoadDelay);
  }


  private void StartDeathSequence()
  {
    state = State.Dying;
    audioSource.Stop();
    audioSource.PlayOneShot(mainDeath);
    Invoke("PlayExplode", effectsLoadDelay);
    Invoke("LoadFirstLevel", levelLoadDelay);
  }

  private void PlayExplode()
  {
    audioSource.PlayOneShot(explode);
    explodeParticles.Play();

  }

  private void RespondToThrustInput()
  {
    if (Input.GetKey(KeyCode.Space))
    {
      ApplyThrust();
    }
    else
    {
      audioSource.Stop();
    }
  }

  private void ApplyThrust()
  {
    rigidBody.AddRelativeForce(Vector3.up * mainThrust * Time.deltaTime);
    if (!audioSource.isPlaying)
    {
      audioSource.PlayOneShot(mainEngine);
      jetsParticles.Play();
    }
  }

  private void RespondToRotateInput()
  {
    rigidBody.freezeRotation = true;
    float rotationThisFrame = rcsThrust * Time.deltaTime;

    if (Input.GetKey(KeyCode.A))
    {
      transform.Rotate(Vector3.forward * rotationThisFrame);
    }
    else if (Input.GetKey(KeyCode.D))
    {
      transform.Rotate(-Vector3.forward * rotationThisFrame);
    }
    rigidBody.freezeRotation = false;
  }

  private void LoadNextLevel()
  {
    SceneManager.LoadScene(1);
  }

  private void LoadFirstLevel()
  {
    SceneManager.LoadScene(0);
    audioSource.PlayOneShot(mainStart);
  }

}