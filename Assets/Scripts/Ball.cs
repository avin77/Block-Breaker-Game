using Mirror;
using UnityEngine;

public class Ball : NetworkBehaviour
{
    // Start is called before the first frame update
  //  [SerializeField] Paddle paddle1;
    [SerializeField] float xPush = 8f;
    [SerializeField] float yPush = 10f;
    [SerializeField] AudioClip[] ballSounds;
    [SerializeField] float randomFactor = .2f;
    //[SerializeField] private Transform ballSpawnPoint;

    Vector2 paddleToBallVector;
    bool hasStarted = false;
   // [SerializeField] private GameObject ballPrefab;
    [SyncVar(hook =nameof(HandleBallColorChange))]
    Color color;
    //cached component resources
    //AudioSource myAudioSource;
    Rigidbody2D rigidbody2D;
   // GameObject ball;
    SpriteRenderer ballColorRenderer;
    /* myAudioSource = GetComponent<AudioSource>();
         rigidbody2D = GetComponent<Rigidbody2D>();*/
    /* void Start()
     {
         paddleToBallVector = transform.position - paddle1.transform.position;
         myAudioSource = GetComponent<AudioSource>();
         rigidbody2D = GetComponent<Rigidbody2D>();
     }*/

    public override void OnStartServer()
    {
         //Debug.Log("authority" + this.hasAuthority);
         /*float yBallPos = this.transform.position.y - paddle1.transform.position.y;
         Vector2 ballPos = new Vector2(paddle1.transform.position.x, yBallPos);
         this.transform.position = ballPos;*/
        /* ball = Instantiate(ballPrefab, ballPos, Quaternion.identity);
         //ballColorRenderer = ball.GetComponent<SpriteRenderer>(); 
         NetworkServer.Spawn(ball);
         //connectionToClient.identity.AssignClientAuthority(connectionToClient);
         //connectionToClient.identity.GetComponent<>
         //myAudioSource = GetComponent<AudioSource>();
         //rigidbody2D = GetComponent<Rigidbody2D>();
         color = NetworkServer.connections.Count > 1 ? Color.blue : Color.green;*/
         //paddleToBallVector = this.transform.position - paddle1.transform.position;
        //RpcPaint();
        color = NetworkServer.connections.Count > 1 ? Color.blue : Color.green;
       
    }

   /* public void SetBallColor(Color newColor) 
    {
        oldColor = newColor;
    }*/

    public void HandleBallColorChange(Color oldColor,Color newColor) {
        this.GetComponent<SpriteRenderer>().color = newColor;
    }

    [Command]
     void CmdSetBallPosition(Vector2 paddlePos)
    {        
        //lockBallToPaddle();
        transform.position = paddlePos;
    }
/*
    [ClientRpc]
    void RpcPaint()
    {
        ball.GetComponent<SpriteRenderer>().color = color;
    }*/

    // Update is called once per frame
    [ClientCallback]
    void Update()
    {
       
        if (!hasAuthority) { return; }
        if (!hasStarted)
        {
            Paddle paddleObj = NetworkClient.connection.identity.GetComponent<Paddle>();
            Vector2 paddlePos = new Vector2(paddleObj.transform.position.x, transform.position.y);
            Physics2D.IgnoreLayerCollision(8, 8);
            Physics2D.IgnoreLayerCollision(9, 9);
            CmdSetBallPosition(paddlePos);
            LaunchBallOnMouseclick();
        }
    }

    void LaunchBallOnMouseclick()
    {
        if (Input.GetMouseButtonDown(0)&& hasAuthority)
        {
            hasStarted = true;
            Debug.Log("Inside mouse launch ball method");
            CmdAddBallForce(xPush, yPush);
        }
    }

    [Command]
    void CmdAddBallForce(float xPushVar, float yPushVar) {
        Debug.Log("Inside Add ball force");
        GetComponent<Rigidbody2D>().velocity = new Vector2(xPushVar, yPushVar); 
    }

    /* private void lockBallToPaddle(float xPushVar,float yPushVar) {
         Vector2 paddlePos = new Vector2(paddle1.transform.position.x, this.transform.position.y);
         this.transform.position = paddlePos;
     }*/

   /* [ServerCallback]
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkIdentity))
        {
            if (networkIdentity.connectionToClient == connectionToClient)
            {
                other.enabled = false;
            }
        }
    }*/

    [ServerCallback]
    private void OnCollisionEnter2D(Collision2D collision2D)
    {
       /* if (collision2D.gameObject.TryGetComponent<NetworkIdentity>(out NetworkIdentity networkIdentity)) 
        {
           if( networkIdentity.connectionToClient != connectionToClient)
            {
                Physics2D.IgnoreCollision(collision2D.gameObject.GetComponent<Collider2D>(), this.GetComponent<Collider2D>());
                }
        }*/
      /*  if (collision.collider.IsTouching(this.gameObject.GetComponent<CircleCollider2D>()))
        {*/
           // NetworkIdentity networkIdentity = collision.gameObject.GetComponent<NetworkIdentity>();
           /* if (networkIdentity != null && collision.gameObject.CompareTag("Paddle"))
            {

                NetworkIdentity playCollisionerNetworkIdentity = this.gameObject.GetComponent<NetworkIdentity>();
                Debug.Log("netid" + networkIdentity.connectionToClient.connectionId + "PlayerNetId" + playerNetworkIdentity.connectionToClient.connectionId);
                if (networkIdentity.connectionToClient.connectionId != playerNetworkIdentity.connectionToClient.connectionId)
                {
                    Debug.Log("inside network identity checker in collision");
                    //Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
                    Physics2D.IgnoreCollision(collision.gameObject.GetComponent<Collider2D>(), this.GetComponent<Collider2D>(),true);
                }
                if (collision.gameObject.CompareTag("Ball") && !hasAuthority)
                {
                    Physics.IgnoreLayerCollision(8, 8);
                }
                *//*if (collision.gameObject.GetComponent<NetworkIdentity>().connectionToClient.connectionId == NetworkConnection.LocalConnectionId)
                {
                    Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), this.GetComponent<Collider>());
                }*//*
            }*/
        //}
        Vector2 velocityTweak = new Vector2(Random.Range(0f, randomFactor), Random.Range(0f, randomFactor));
        if (hasStarted && hasAuthority)
        {
            AudioClip clip = ballSounds[UnityEngine.Random.Range(0, ballSounds.Length)];
            this.GetComponent<AudioSource>().PlayOneShot(clip);
            this.GetComponent<Rigidbody2D>().velocity += velocityTweak;
        }
    }

   /* public override void OnStopServer()
    {
        base.OnStopServer();
        if (this != null)
            NetworkServer.Destroy(this);
    }*/
}
