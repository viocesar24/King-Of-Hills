using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameController : MonoBehaviour {

    public static GameController instance;
    public static bool GameStart; // Turn 1, when the game just started
    public static int Turn;
    public static List<GameObject> MonsterCollection;
    
    public static int GameSpeedMultiplier = 10;
    
    public GameObject PlayerFolder;
    
    public GameObject MonsterFolder;
    
    public Transform[] Waypoints;
    
    public Image TownBackground;

    [SerializeField]
    DicesController DiceController;
    
    public TownMenuController TownMenuController;

    public DialogController DialogController;
    
    public QuestionDialogController QuestionDialogController;

    [SerializeField]
    GameObject Camera;
    
    private bool AllowRoll = false;
    private CameraMover CameraMover;
    public TopPanelUpdater TopPanelUpdater;
    public BottomPanelUpdater BottomPanelUpdater;
    private List<GameObject> Players;
    private int PlayerIndex = -1;

    // Use this for initialization
    void Start ()
    {
        instance = this;
        GameStart = true;

        SetupPlayers();
        
        CameraMover = Camera.GetComponent<CameraMover>();
        CameraMover.CameraMovedToPlayer += CameraMover_CameraMovedToPlayer;
        CameraMover.CameraZoomedOut += CameraMover_CameraZoomedOut;
        DiceController.DicesRolled += DiceController_DicesRolled;

        NextPlayerTurn();
	}

    private void SetupPlayers()
    {
        MonsterCollection = new List<GameObject>();

        foreach (Transform monster in MonsterFolder.transform)
        {
            monster.gameObject.GetComponent<PlayerClass>().SetAsMonster(7, 50);
            MonsterCollection.Add(monster.gameObject);
        }

        Players = new List<GameObject>();

        foreach (Transform player in PlayerFolder.transform)
        {
            if (player.gameObject.tag == "Player")
            {
                Players.Add(player.gameObject);
            }
        }


        var Cities = new List<WaypointController>(Waypoints
            .Select(a => a.gameObject.GetComponent<WaypointController>())
            .Where(a => a.IsTown == true));
        Cities.Shuffle();
        
        int idx = 0;
        bool testBattle = false; // debug
        foreach (GameObject player in Players)
        {
            if (testBattle)
            {
                player.GetComponent<PlayerMover>().SetWayPoints(Waypoints, idx);
                player.GetComponent<PlayerMover>().PlayerMoved += GameController_PlayerMoved;
            }
            else
            {
                Cities[idx].OnVisit(player.GetComponent<PlayerClass>());
                var sibIdx = Cities[idx].SiblingIndex;
                player.GetComponent<PlayerMover>().SetWayPoints(Waypoints, sibIdx);
                player.GetComponent<PlayerClass>().RespawnPosition = Waypoints[sibIdx].position;
                player.GetComponent<PlayerClass>().initialWaypointIndex = sibIdx;
            }
            player.GetComponent<PlayerMover>().PlayerMoved += GameController_PlayerMoved;

            idx++;
        }
    }
    

    // Update is called once per frame
    void Update ()
    {
		
	}

    public void NextPlayerTurn()
    {
        PlayerIndex++;
        if (PlayerIndex > Players.Count - 1)
        {
            PlayerIndex = 0;
        }
        BottomPanelUpdater.HidePanel();
        CameraMover.ZoomToCenter();
    }
    private void CameraMover_CameraZoomedOut(object sender)
    {
        // Called once, when the game started
        if (GameStart)
        {
            GameStart = false;

            TopPanelUpdater.StartUpdater();
            TopPanelUpdater.ShowPanel();
        }

        // Flashy Turn notification

        if (PlayerIndex == 0) // new turn
        {
            if (Turn >= 200)
            {
                int highestGold = 0;
                foreach (var p in Players)
                {
                    var player = p.GetComponent<PlayerClass>();
                    if (player.Gold > highestGold)
                    {
                        StaticVariables.PostGame_WinnerPlayerName = player.Name;
                    }
                }
                
                SceneManager.LoadScene(2);
                return;
            }

            Turn++;
            TopPanelUpdater.NextTurn();
        }

        ////////
        
        BottomPanelUpdater.SetPlayer(Players[PlayerIndex].GetComponent<PlayerClass>());
        CameraMover.SetPlayer(Players[PlayerIndex]);
    }

    private void CameraMover_CameraMovedToPlayer(object sender)
    {
        AllowRoll = true;
        BottomPanelUpdater.ShowPanel();
    }

    public void RollDice()
    {
        if (AllowRoll)
        {
            DiceController.Roll();
            AllowRoll = false;
        }
    }

    private void DiceController_DicesRolled(object sender, int firstDiceValue, int secondDiceValue)
    {
        Players[PlayerIndex].GetComponent<PlayerMover>().Move(firstDiceValue + secondDiceValue);
        
    }

    private void GameController_PlayerMoved(object sender, int diceValue)
    {
        NextPlayerTurn();
    }

    public void HideBottomPanel()
    {
        BottomPanelUpdater.HidePanel();
    }

    public void ShowBottomPanel()
    {
        BottomPanelUpdater.ShowPanel();
    }

    public PlayerClass GetCurrentPlayer()
    {
        return Players[PlayerIndex].GetComponent<PlayerClass>();
    }

    public void ShutdownSession()
    {
        SceneManager.LoadScene(0);
    }
}
