using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class WaypointController : MonoBehaviour {
    
    public string Name = "undefined";
    
    public int SiblingIndex { get; set; }

    public bool IsTown { get; set; }
    public PlayerClass HomelandOfPlayer { get; set; }

    private List<PlayerClass> PlayersOnVisit;
    private List<Vector3> PlayersPositions;
    private PlayerClass ActivePlayer;
    private bool visitCity = false;

    private System.Random rnd;
    private int monsterSpawnProbability = 10; //percent

	// Use this for initialization
	void Start () {
        SiblingIndex = transform.GetSiblingIndex();
        HomelandOfPlayer = null;
        PlayersOnVisit = new List<PlayerClass>();
        IsTown = gameObject.CompareTag("Waypoint - City");
        
        rnd = new System.Random(SiblingIndex);

        PlayersPositions = new List<Vector3>();
        PlayersPositions.Add(new Vector3(-0.41f, 0));
        PlayersPositions.Add(new Vector3(-0.45f, -0.31f));
        PlayersPositions.Add(new Vector3(0, -0.4f));
        PlayersPositions.Add(new Vector3(0.48f, -0.31f));
        PlayersPositions.Add(new Vector3(0.4f, 0));
        PlayersPositions.Add(new Vector3(0.48f, 0.41f));
        PlayersPositions.Add(new Vector3(0f, 0.54f));
        PlayersPositions.Add(new Vector3(-0.45f, 0.41f));
    }

    private void EnqueueText (string speaker, string text)
    {
        GameController.instance.DialogController.EnqueueText(speaker, text);
    }

    private void StartDialog(System.Action completed)
    {
        GameController.instance.DialogController.StartDialog(completed);
    }
    
	
	private void SpawnMonster()
    {
        bool spawn = rnd.Next(101) >= 100 - monsterSpawnProbability;

        if (spawn)
        {
            var monster = GameController.MonsterCollection[rnd.Next(GameController.MonsterCollection.Count)];
            monster.transform.position = transform.position;
            monster.SetActive(true);
            EnqueueText("World", monster.GetComponent<PlayerClass>().Name + " has appeared!");
            PlayersOnVisit.Add(monster.GetComponent<PlayerClass>());
        }
    }

    public void OnTheWay(PlayerClass player)
    {
        ActivePlayer = player;

    }

    public void OnVisit(PlayerClass player)
    {
        ActivePlayer = player;
        PlayersOnVisit.Add(player);

        player.gameObject.GetComponent<PlayerMover>().PauseMove(true);

        if (IsTown)
        {
            if (GameController.GameStart)
            {
                HomelandOfPlayer = player;
                return;
            }

            bool moveEnded = ActivePlayer.gameObject.GetComponent<PlayerMover>().GetMoveRemaining() == 0;
            GameController.instance.QuestionDialogController.ShowDialog(Name + " village is near. Enter?", "Sounds fun", moveEnded ? "nah, I'm broke" : "nah, keep going",
                (answer, optionText) => {
                    if (answer)
                    {
                        visitCity = true;
                        GameController.instance.HideBottomPanel();
                    }
                    else
                    {
                        visitCity = false;
                    }

                    RepositionPlayers(true);
                });
        }
        else
        {
            SpawnMonster();
            RepositionPlayers(true);
        }
    }

    public void OnPast(PlayerClass player)
    {
        PlayersOnVisit.Remove(player);
        RepositionPlayers(false);

        ActivePlayer = null;
    }

    private void RepositionPlayers(bool addition)
    {
        switch (PlayersOnVisit.Count)
        {
            case 2:
                if (addition)
                {
                    PlayersOnVisit[0].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[0], !IsTown);
                    PlayersOnVisit[1].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[4], false, RepositionCompleted); // always put handler on last
                }
                else
                {
                    PlayersOnVisit[0].gameObject.GetComponent<PlayerMover>().MicroMoveUndo();
                    PlayersOnVisit[1].gameObject.GetComponent<PlayerMover>().MicroMoveUndo(RepositionCompleted); // always put handler on last
                }
                break;
            case 3:
                if (addition)
                {
                    PlayersOnVisit[0].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[0], !IsTown);
                    PlayersOnVisit[1].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[5]);
                    PlayersOnVisit[2].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[3], false, RepositionCompleted);
                }
                else
                {
                    PlayersOnVisit[0].gameObject.GetComponent<PlayerMover>().MicroMoveUndo();
                    PlayersOnVisit[1].gameObject.GetComponent<PlayerMover>().MicroMoveUndo();
                    PlayersOnVisit[2].gameObject.GetComponent<PlayerMover>().MicroMoveUndo(RepositionCompleted);
                }
                break;
            case 4:
                if (addition)
                {
                    PlayersOnVisit[0].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[7], !IsTown);
                    PlayersOnVisit[1].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[1], !IsTown);
                    PlayersOnVisit[2].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[3]);
                    PlayersOnVisit[3].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[5], false, RepositionCompleted);
                }
                else
                {
                    PlayersOnVisit[0].gameObject.GetComponent<PlayerMover>().MicroMoveUndo();
                    PlayersOnVisit[1].gameObject.GetComponent<PlayerMover>().MicroMoveUndo();
                    PlayersOnVisit[2].gameObject.GetComponent<PlayerMover>().MicroMoveUndo();
                    PlayersOnVisit[3].gameObject.GetComponent<PlayerMover>().MicroMoveUndo(RepositionCompleted);
                }
                break;
            case 5:
                if (addition)
                {
                    PlayersOnVisit[0].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[7], !IsTown);
                    PlayersOnVisit[1].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[1], !IsTown);
                    PlayersOnVisit[2].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[3]);
                    PlayersOnVisit[3].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[5]);
                    PlayersOnVisit[4].gameObject.GetComponent<PlayerMover>().MicroMove(PlayersPositions[0], false, RepositionCompleted);
                }
                break;
            case 1:
                if (addition)
                {
                    PlayersOnVisit[0].gameObject.GetComponent<PlayerMover>().MicroMove(new Vector3(0, 0, 0), false, RepositionCompleted, 0f);
                }
                else
                {
                    PlayersOnVisit[0].gameObject.GetComponent<PlayerMover>().MicroMoveUndo(RepositionCompleted, 0f);
                }
                break;
            default:
                RepositionCompleted();
                break;
        }
    }

    private void RepositionCompleted()
    {
        // Events, battles, whatever...
        if (!IsTown && PlayersOnVisit.Count > 1)
        {
            GameController.instance.HideBottomPanel();
            StartDialog(DialogPreBattleCompleted);
        }
        else if (IsTown && visitCity)
        {
            visitCity = false;

            GameController.instance.HideBottomPanel();
            StartCoroutine(CoroutineShowTownDialog());
        }
        ////////////////////////////////////
        else
        {
            //When done, resume move
            if (ActivePlayer != null)
            {
                ActivePlayer.gameObject.GetComponent<PlayerMover>().PauseMove(false);
            }
        }
        
    }
    
    private void DialogPreBattleCompleted()
    {
        int idx = 0;
        var done = new List<PlayerClass>();
        foreach (var player in PlayersOnVisit)
        {
            for (int i = 0; i < PlayersOnVisit.Count; i++)
            {
                if (i == idx || done.Contains(PlayersOnVisit[i]) || PlayersOnVisit[i].Lives <= 0)
                {
                    continue;
                }

                var cur_atk = player.ATK;
                var tar_atk = PlayersOnVisit[i].ATK;

                if (cur_atk > tar_atk)
                {
                    PlayersOnVisit[i].Lives--;
                    EnqueueText("World", PlayersOnVisit[i].Name + (PlayersOnVisit[i].Lives == 0 ? " was killed by " + player.Name :
                                                                                                  " took damage from " + player.Name + ". Lives remaining: " + PlayersOnVisit[i].Lives));
                    if (PlayersOnVisit[i].Lives == 0)
                    {
                        PlayersOnVisit[i].Defeated(player);
                    }
                }
                else if (tar_atk > cur_atk)
                {
                    player.Lives--;
                    EnqueueText("World", player.Name + (player.Lives == 0 ? " was killed by " + PlayersOnVisit[i].Name :
                                                                            " took damage from " + PlayersOnVisit[i].Name + ". Lives remaining: " + player.Lives));
                    if (player.Lives == 0)
                    {
                        player.Defeated(PlayersOnVisit[i]);
                    }
                }
                else
                {
                    // draw...
                    EnqueueText("World", "Both party was strong enough that they made a Ceasefire agreement!");
                }
            }

            done.Add(player);
            idx++;
        }

        for (int i = 0; i < PlayersOnVisit.Count - 1; i++)
        {
            PlayersOnVisit[i].gameObject.GetComponent<PlayerMover>().AttackMove(transform.position);
        }
        PlayersOnVisit[PlayersOnVisit.Count-1].gameObject.GetComponent<PlayerMover>().AttackMove(transform.position, AttackCompleted);
    }

    private void AttackCompleted()
    {
        var toBeRemoved = new List<PlayerClass>();
        foreach (var player in PlayersOnVisit.Where(a => a.Lives <= 0))
        {

            if (player.IsMonster)
            {
                player.Lives = 1;
            }
            else
            {
                player.Lives = 5;
            }

            if (player.IsMonster)
            {
                player.gameObject.GetComponent<PlayerMover>().transform.position = player.RespawnPosition; // respawn outside view
            }
            else
            {
                player.gameObject.GetComponent<PlayerMover>().SetWayPoint(player.initialWaypointIndex); // respawn on homeland city
            }

            toBeRemoved.Add(player);
        }
        foreach (var player in toBeRemoved)
        {
            PlayersOnVisit.Remove(player);
        }

        StartDialog(DialogPostBattleCompleted);
    }

    private void DialogPostBattleCompleted()
    {
        //When done, resume move
        if (ActivePlayer != null)
        {
            GameController.instance.ShowBottomPanel();
            if (ActivePlayer.Lives > 0)
            {
                ActivePlayer.gameObject.GetComponent<PlayerMover>().PauseMove(false);
            }
            else
            {
                ActivePlayer.gameObject.GetComponent<PlayerMover>().HaltMove();
            }
        }
    }

    IEnumerator CoroutineShowTownDialog()
    {
        float elapsed = 0;
        float t = 0;
        Color curColor = GameController.instance.TownBackground.color;
        var currentAlpha = curColor.a;

        while (t < 1)
        {
            elapsed += Time.deltaTime;
            t = Easing.Linear(Mathf.Clamp01(elapsed / 0.3f));
            curColor.a = Mathf.Lerp(currentAlpha, 1f, t);
            GameController.instance.TownBackground.color = curColor;

            yield return null;
        }

        GameController.instance.TownMenuController.OpenMenu(ActivePlayer);
    }

}
