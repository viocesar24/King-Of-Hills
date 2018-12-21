using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMover : MonoBehaviour {
    
    private Transform[] Waypoints;
    private Stack<Vector3> MicroMoves;
    private int MoveRemaining = 0;

    public delegate void PlayerMovedHandler(object sender, int diceValue);
    public event PlayerMovedHandler PlayerMoved;

    private bool pauseMove = false;
    private bool haltMove = false;
    private bool flipped = false;
    private Quaternion defaultRotation;
    private int WaypointIndex = 0;

	// Use this for initialization
	void Start ()
    {
        defaultRotation = transform.rotation;
        MicroMoves = new Stack<Vector3>();
    }
	
	// Update is called once per frame
	void Update ()
    {

	}

    public string GetWaypointName()
    {
        return Waypoints[WaypointIndex].GetComponent<WaypointController>().Name;
    }

    public int GetMoveRemaining()
    {
        return MoveRemaining;
    }

    public void SetWayPoints(Transform[] waypoints, int waypointIndex = 0)
    {
        this.Waypoints = waypoints;
        WaypointIndex = waypointIndex;
        transform.position = Waypoints[WaypointIndex].position;
    }

    public void SetWayPoint(int idx)
    {
        WaypointIndex = idx;
        transform.position = Waypoints[WaypointIndex].position;
    }

    public void Move(int diceValue)
    {
        MoveRemaining += diceValue;
        StartCoroutine(CoroutineMove());
    }

    public void PauseMove(bool pause)
    {
        pauseMove = pause;
    }

    public void HaltMove()
    {
        haltMove = true;
    }

    public void MicroMove(Vector3 addition, bool FlipX = false, Action completed = null, float actionDelaySeconds = 0.5f)
    {
        StartCoroutine(CoroutineMicroMove(addition, FlipX, completed, actionDelaySeconds));
    }

    public void MicroMoveUndo(Action completed = null, float actionDelaySeconds = 0.5f)
    {
        var counterAddition = new Vector3(MicroMoves.Peek().x*-1, MicroMoves.Peek().y*-1, MicroMoves.Pop().z*-1);
        StartCoroutine(CoroutineMicroMove(counterAddition, flipped, completed, actionDelaySeconds));
    }

    public void AttackMove(Vector3 waypointPosition, Action AttackCompleted = null)
    {
        StartCoroutine(CoroutineAttack(waypointPosition, AttackCompleted));
    }

    IEnumerator CoroutineMove()
    {
        int totalMove = MoveRemaining;
        yield return new WaitForSeconds(0.5f);
        
        float speed = GameController.GameSpeedMultiplier;
        while (MoveRemaining > 0)
        {
            Waypoints[WaypointIndex].gameObject.GetComponent<WaypointController>().OnPast(gameObject.GetComponent<PlayerClass>());

            WaypointIndex++;
            if (WaypointIndex > Waypoints.Length - 1)
            {
                WaypointIndex = 0;
            }

            Waypoints[WaypointIndex].gameObject.GetComponent<WaypointController>().OnTheWay(gameObject.GetComponent<PlayerClass>());

            float t = 0;
            float elapsed = 0;
            Vector3 currentPos = transform.position;
            var destination = Waypoints[WaypointIndex].transform.position;
            Quaternion currentRotation = transform.rotation;
            while (t < 1)
            {
                if (haltMove)
                {
                    break;
                }

                elapsed += Time.deltaTime;
                t = Easing.Cubic.Out(Mathf.Clamp01(elapsed / (2.5f/speed)));
                transform.position = Vector3.Lerp(currentPos, destination, t);
                transform.rotation = Quaternion.Lerp(currentRotation, defaultRotation, t);

                yield return new WaitForEndOfFrame();
            }

            if (haltMove)
            {
                haltMove = false;
                break;
            }

            flipped = false;

            yield return new WaitForSeconds(1f/speed);
            
            Waypoints[WaypointIndex].gameObject.GetComponent<WaypointController>().OnVisit(gameObject.GetComponent<PlayerClass>());

            while (pauseMove)
            {
                yield return null;
            }

            MoveRemaining--;
        }

        PlayerMoved(this, totalMove);
    }

    IEnumerator CoroutineAttack(Vector3 waypointPosition, Action AttackCompleted)
    {
        float t = 0;
        float elapsed = 0;
        Vector3 currentPos = transform.position;

        while (t < 0.7)
        {
            elapsed += Time.deltaTime;
            t = Easing.Cubic.Out(Mathf.Clamp01(elapsed / 0.3f));
            transform.position = Vector3.Lerp(currentPos, waypointPosition, t);

            yield return null;
        }

        yield return new WaitForSeconds(.5f);

        t = 0;
        elapsed = 0;

        while (t < 0.94)
        {
            elapsed += Time.deltaTime;
            t = Easing.Cubic.Out(Mathf.Clamp01(elapsed / 0.3f));
            transform.position = Vector3.Lerp(waypointPosition, currentPos, t);

            yield return null;
        }

        if (AttackCompleted != null)
        {
            AttackCompleted();
        }
    }

    IEnumerator CoroutineMicroMove(Vector3 addition, bool flipX, Action completed, float actionDelaySeconds)
    {
        Vector3 dest = transform.position + addition;
        float t = 0;
        float elapsed = 0;
        Vector3 currentPos = transform.position;
        Quaternion currentRotation = transform.rotation;
        Transform tnew = transform;
        tnew.Rotate(0, (flipX ? 180 : 0), 0);
        Quaternion newRotation = tnew.rotation;

        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Cubic.Out(Mathf.Clamp01(elapsed / 0.3f));
            transform.position = Vector3.Lerp(currentPos, dest, t);
            transform.rotation = Quaternion.Lerp(currentRotation, newRotation, t);

            yield return null;
        }
        flipped = flipX;

        MicroMoves.Push(addition);

        if (completed != null)
        {
            yield return new WaitForSeconds(actionDelaySeconds);
            completed();
        }
    }
}
