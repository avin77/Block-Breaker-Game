using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RPCNetworkManager : NetworkManager
{
    // Start is called before the first frame update
    public Transform leftRacketSpawn;
 //   public Transform ballSpawn;
    public Transform rightRacketSpawn;
    public Transform ballSpawnPosition;
    GameObject ball;
    List<NetworkIdentity> networkIdentities = new List<NetworkIdentity>();
   // List<GameObject> listOfPlayers = new List<GameObject>();
    public override void OnServerAddPlayer(NetworkConnection conn)
    {
        // add player at correct spawn position
        Transform start = numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
        GameObject player = Instantiate(playerPrefab, start.position, start.rotation);
        NetworkServer.AddPlayerForConnection(conn, player);
//        listOfPlayers.Add(player);
        NetworkManager networkManager = NetworkManager.singleton;
       

        // spawn ball if two players
        /* if (numPlayers == 2)
         {*/
        /*ballSpawn =numPlayers == 0 ? leftRacketSpawn : rightRacketSpawn;
        Vector2 ballPosition = ballSpawn.position;
        ballPosition.y= ballPosition.y+3;
        ballSpawn.position = ballPosition;*/
        GameObject ballObject = spawnPrefabs.Find(prefab => prefab.name == "Ball");
        //float yBallPos = ballObject.transform.position.y - player.transform.position.y;
        //Vector2 ballPos = new Vector2(player.transform.position.x, yBallPos);
        //ballObject.transform.position = ballPos;
        //ball = Instantiate(spawnPrefabs.Find(prefab => prefab.name == "Ball"));
        ball = Instantiate(ballObject, ballSpawnPosition.position, ballSpawnPosition.rotation);
        NetworkServer.Spawn(ball, conn);
        /* float yBallPos = ball.transform.position.y - player.transform.position.y;
         Vector2 ballPos = new Vector2(player.transform.position.x, yBallPos);
         ball.transform.position = ballPos;*/
        /* }*/
        if(numPlayers == 2)
        {
            foreach (NetworkConnection connection in NetworkServer.connections.Values)
            {
                if (connection != conn)
                {
                    HashSet<NetworkIdentity> enumerator = connection.clientOwnedObjects;
                    foreach (NetworkIdentity networkPlayerObj in enumerator)
                    {
                        networkIdentities.Add(networkPlayerObj);
                    }
                }
            }
            Physics2D.IgnoreCollision(networkIdentities[0].gameObject.GetComponent<Collider2D>(), ball.GetComponent<Collider2D>());
            Physics2D.IgnoreCollision(conn.identity.gameObject.GetComponent<Collider2D>(), networkIdentities[1].gameObject.GetComponent<Collider2D>());
        }
    }
    }

    /*public override void OnServerDisconnect(NetworkConnection conn)
    {
        // destroy ball
        if (ball != null)
            NetworkServer.Destroy(ball);

        // call base functionality (actually destroys the player)
        base.OnServerDisconnect(conn);
    }*/

  /*  private Transform lockBallToPaddle(GameObject player)
    {
        Vector2 paddlePos = new Vector2(player.transform.position.x, player.transform.position.y);
        Vector2 paddleToBallVector = transform.position - player.transform.position;
        transform.position = paddlePos + paddleToBallVector;
        return transform;
    }*/



