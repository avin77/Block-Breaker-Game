using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Paddle : NetworkBehaviour
{
    [SerializeField] float ScreenWidth=16f;
    [SerializeField] float minScreenSize = 1f;
    [SerializeField] float maxScreenSize = 15f;

    [Command]
    private void CmdMove(Vector2 paddlePos)
    {
        transform.position = paddlePos;
    }

    // Update is called once per frame
    [ClientCallback]
    void Update()
    {
        if (hasAuthority)
        {
            float mousePosInUnits = (Input.mousePosition.x) / Screen.width * ScreenWidth;
            Vector2 paddlePos = new Vector2(transform.position.x, transform.position.y);
            paddlePos.x = Mathf.Clamp(mousePosInUnits, minScreenSize, maxScreenSize);
/*            Color color = NetworkServer.connections.Count > 1 ? Color.blue : Color.green;
            this.GetComponent<SpriteRenderer>().color = color;*/
            CmdMove(paddlePos);
            //transform.position = paddlePos;
        }
    }
}
