using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMover : MonoBehaviour
{
    public delegate void CameraMovedToPlayerHandler(object sender);
    public event CameraMovedToPlayerHandler CameraMovedToPlayer;
    public delegate void CameraZoomOutHandler(object sender);
    public event CameraZoomOutHandler CameraZoomedOut;

    private Camera ThisCamera;
    private Vector2 CenterPoint;
    private Vector2 CameraOffset;
    public static float ActionDuration = 1.7f;

    private GameObject Player;
	private bool IsPlayerMoving = false;
    

    // Use this for initialization
	void Start ()
    {
        ThisCamera = GetComponent<Camera>();
        CenterPoint = new Vector2(-1.08f, -0.12f);
        CameraOffset = new Vector2(0, -0.2f);
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void LateUpdate()
    {
        if (Player != null && IsPlayerMoving)
        {
            transform.position = Player.transform.position + new Vector3(CameraOffset.x, CameraOffset.y, 0);
        }
    }


    public void FollowPlayer()
    {
        IsPlayerMoving = true;
    }

    public void SetPlayer(GameObject player)
    {
        StartCoroutine(CoroutineCameraZoomToPlayer(player));
        Player = player;
    }

    public void ZoomToCenter()
    {
        IsPlayerMoving = false;
        StartCoroutine(CoroutineCameraZoomToCenter());
        
    }

    IEnumerator CoroutineCameraZoomToCenter()
    {
        float elapsed = 0;
        float t = 0;
        float currentZoom = ThisCamera.orthographicSize;

        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Cubic.Out(Mathf.Clamp01(elapsed / (GameController.GameStart ? 5f : ActionDuration)));
            ThisCamera.orthographicSize = Mathf.Lerp(currentZoom, 5.6f, t);
            
            transform.position = Vector2.Lerp(transform.position, CenterPoint, t);

            yield return null;
        }

        yield return new WaitForSeconds(GameController.GameStart ? 3f : 0.4f);
        
        CameraZoomedOut(this);
    }

    IEnumerator CoroutineCameraZoomToPlayer(GameObject player)
    {
        float elapsed = 0;
        float t = 0;
        float currentZoom = ThisCamera.orthographicSize;

        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Cubic.InOut(Mathf.Clamp01(elapsed / ActionDuration));
            ThisCamera.orthographicSize = Mathf.Lerp(currentZoom, 2f, t);
            transform.position = Vector2.Lerp(transform.position, player.transform.position + new Vector3(CameraOffset.x, CameraOffset.y, 0), t);

            yield return null;
        }

        IsPlayerMoving = true;
        CameraMovedToPlayer(this);
    }
}
