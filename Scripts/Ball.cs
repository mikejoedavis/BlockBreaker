using UnityEngine;

public class Ball : MonoBehaviour
{

    // configuration parameters
    [SerializeField] Paddle paddle1 = default;
    [SerializeField] float xPush = 2f;
    [SerializeField] float yPush = 15f;
    [SerializeField] AudioClip[] ballsounds;
    [SerializeField] float randomFactor = 0.2f;


    // state
    Vector2 paddleToBallVector;
    bool hasStarted = false;

    //cached component references
    AudioSource myAudioSource;
    Rigidbody2D myRigidBody2D;


    // Start is called before the first frame update
    void Start()
    {
       paddleToBallVector = transform.position - paddle1.transform.position;
       myAudioSource = GetComponent<AudioSource>();
       myRigidBody2D = GetComponent<Rigidbody2D>();
       myRigidBody2D.simulated = false; //added because was getting an error where if you didnt launch the ball quickly it thought it was hitting the lose collider after a few seconds. so turning off simulation until ball is launched
    }

    // Update is called once per frame
    void Update()
    {
        if (!hasStarted)
        {
            LockBallToPaddle();
            LaunchOnMouseClick();
        }
    }

    private void LaunchOnMouseClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            hasStarted = true;
            myRigidBody2D.velocity = new Vector2(xPush, yPush);
            myRigidBody2D.simulated = true; //added because was getting an error where if you didnt launch the ball quickly it thought it was hitting the lose collider after a few seconds. so turning off simulation until ball is launched
        }

    }

    private void LockBallToPaddle()
    {
        Vector2 paddlePos = new Vector2(paddle1.transform.position.x, paddle1.transform.position.y);
        transform.position = paddlePos + paddleToBallVector;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Vector2 velocityTweak = new Vector2(Random.Range(0f, randomFactor), Random.Range(0f, randomFactor));
        if (hasStarted)
        {
            AudioClip clip = ballsounds[UnityEngine.Random.Range(0, ballsounds.Length)];
            myAudioSource.PlayOneShot(clip);
            myRigidBody2D.velocity += velocityTweak;

        }
    }

}
