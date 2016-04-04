using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System;

public class GameManager : MonoBehaviour {

    public static GameManager instance;
    public enum InputType
    {
        MOVING,
        CHOOSEEMPTY, CHOOSEPLANT, CHOOSETAKECARE, CHOOSEHARVEST, CHOOSEPLOW,
        ACTIONPLANTCARROT, ACTIONPLANTLETTUCE, ACTIONPLANTPOTATOES,
        ACTIONWATER, ACTIONHARVEST
    }

    public InputType inputType = InputType.MOVING;
    public HexTerrain terrain;

    public Transform emptyWheel;
    public Transform plantWheel;
    public Transform takeCareWheel;
    public Transform harvestWheel;
    public Transform plowWheel;

    public List<TimedKey> actionBuffer = new List<TimedKey>();
    public ActionBuffer actionBufferUI;
    public Image dayFill;
    public float dayDuration = 60f;
    private float dayClock;
    public bool allowAction = true;
    public bool timePass = false;

    private int harvestClickCount = 0;
    private float inputClock = 0f;
    [HideInInspector]
    public ScoreManager scoreManager;
    public CanvasGroup titlescreen;
    public GameObject pointsUI;
    public GameObject dayUI;

    public void Awake()
    {
        if (instance != null)
        {
            DestroyImmediate(gameObject);
        }
        else
        {
            instance = this;
        }
        scoreManager = GetComponent<ScoreManager>();
    }

    public void StartGame()
    {
        StartCoroutine(Coroutines.Fade(titlescreen, 1f, 0f, 0.25f));
        StartCoroutine(Coroutines.Destroy(titlescreen.gameObject, 0.33f));
        terrain.Build();
        GetComponent<TutorialManager>().StartTutorial();
        pointsUI.SetActive(true);
        dayUI.SetActive(true);
        dayFill.fillAmount = 0f;
    }

    public void ShowWheel(Transform wheel)
    {
        StartCoroutine(ShowWheelCoroutine(wheel.transform, 0.1f));
    }

    public void HideWheel(Transform wheel)
    {
        StartCoroutine(HideWheelCoroutine(wheel, 0.1f));
    }

    public IEnumerator SetMoving()
    {
        yield return null;
        inputType = InputType.MOVING;
        terrain.focusedTile.GetComponent<Tile>().beingUsed = false;
    }

    public IEnumerator ShowWheelCoroutine(Transform wheel, float duration)
    {
        Vector3 start = wheel.localScale;
        for (float clock = 0f; clock < duration; clock += Time.deltaTime)
        {
            wheel.localScale = Vector3.Lerp(start, Vector3.one, clock / duration);
            yield return null;
        }
        wheel.localScale = Vector3.one;
    }

    public IEnumerator HideWheelCoroutine(Transform wheel, float duration)
    {
        Vector3 start = wheel.localScale;
        for (float clock = 0f; clock < duration; clock += Time.deltaTime)
        {
            wheel.localScale = Vector3.Lerp(start, Vector3.zero, clock / duration);
            yield return null;
        }
        wheel.localScale = Vector3.zero;
    }

    public CanvasGroup gameOver;
    public Text gameOverPointsText;
    private bool gameOverShown;

    public void ShowGameOver()
    {
        allowAction = false;
        gameOver.gameObject.SetActive(true);
        StartCoroutine(Coroutines.Fade(gameOver, 0f, 1f, 0.33f));
        gameOverPointsText.text = "Tes Points LEGUM : " + GetComponent<ScoreManager>().totalPoints;
    }

    public void RestartDay()
    {
        StartCoroutine(Coroutines.Fade(gameOver, 1f, 0f, 0.33f));
        StartCoroutine(Coroutines.Disable(gameOver.gameObject, 0.33f));
        dayClock = 0f;
        terrain.Cleanup();
        terrain.NextLevel();
        terrain.Build();
        ScoreManager sm = GetComponent<ScoreManager>();
        sm.totalPoints = 0;
        sm.currentPoints = 0;
        sm.RefreshPoints();
        allowAction = true;
        gameOverShown = false;
        inputType = InputType.MOVING;
        actionBufferUI.Cleanup();
        actionBuffer.Clear();
    }

    public void Update()
    {
        if(timePass)
        {
            dayClock += Time.deltaTime;
            dayFill.fillAmount = dayClock / dayDuration;
        }
        if (!gameOverShown && dayClock / dayDuration >= 1f)
        {
            gameOverShown = true;
            ShowGameOver();
        }

        if (!allowAction)
            return;

        KeyCode[] validInputs = new KeyCode[0];
        switch (inputType)
        {
            case InputType.MOVING:
                if (Input.GetKeyDown(KeyCode.Space))
                {
                    switch (terrain.focusedTile.GetComponent<Tile>().status)
                    {
                        case Tile.Status.EMPTY:
                            ShowWheel(emptyWheel.transform);
                            inputType = InputType.CHOOSEEMPTY;
                            terrain.focusedTile.GetComponent<Tile>().beingUsed = true;
                            break;
                        case Tile.Status.PLANTED:
                            if (terrain.focusedTile.GetComponent<Tile>().needsWater)
                            {
                                ShowWheel(takeCareWheel.transform);
                                inputType = InputType.CHOOSETAKECARE;
                                terrain.focusedTile.GetComponent<Tile>().beingUsed = true;
                            }
                            break;
                        case Tile.Status.GROWN:
                            ShowWheel(harvestWheel.transform);
                            inputType = InputType.CHOOSEHARVEST;
                            terrain.focusedTile.GetComponent<Tile>().beingUsed = true;
                            break;
                        case Tile.Status.HARVESTED:
                            break;
                    }
                }
                break;

            case InputType.CHOOSEEMPTY:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    HideWheel(emptyWheel.transform);
                    ShowWheel(plantWheel.transform);
                    inputType = InputType.CHOOSEPLANT;
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    HideWheel(emptyWheel.transform);
                    StartCoroutine(SetMoving());
                }
                break;

            case InputType.CHOOSEPLANT:
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    HideWheel(plantWheel.transform);
                    inputType = InputType.ACTIONPLANTCARROT;
                    actionBuffer.Clear();
                    actionBuffer.AddRange(new TimedKey[] { KeyCode.S, KeyCode.D, KeyCode.S, KeyCode.D });
                    actionBufferUI.SetActionBuffer(actionBuffer);
                }
                if (Input.GetKeyDown(KeyCode.UpArrow))
                {
                    HideWheel(plantWheel.transform);
                    inputType = InputType.ACTIONPLANTLETTUCE;
                    actionBuffer.Clear();
                    actionBuffer.AddRange(new TimedKey[] { KeyCode.S, KeyCode.S, KeyCode.D, KeyCode.S, KeyCode.D, KeyCode.D });
                    actionBufferUI.SetActionBuffer(actionBuffer);
                }
                if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    HideWheel(plantWheel.transform);
                    inputType = InputType.ACTIONPLANTPOTATOES;
                    actionBuffer.Clear();
                    actionBuffer.AddRange(new TimedKey[] { KeyCode.S, KeyCode.D, KeyCode.S, KeyCode.D, KeyCode.J, KeyCode.K, KeyCode.S, KeyCode.D });
                    actionBufferUI.SetActionBuffer(actionBuffer);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    HideWheel(plantWheel.transform);
                    StartCoroutine(SetMoving());
                }
                break;

            case InputType.CHOOSETAKECARE:

                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    HideWheel(takeCareWheel.transform);
                    inputType = InputType.ACTIONWATER;
                    actionBuffer.Clear();
                    actionBuffer.AddRange(new TimedKey[] {  new TimedKey(KeyCode.LeftArrow, 0.33f), new TimedKey(KeyCode.RightArrow, 0.33f),
                                                            new TimedKey(KeyCode.LeftArrow, 0.33f), new TimedKey(KeyCode.RightArrow, 0.33f) });
                    actionBufferUI.SetActionBuffer(actionBuffer);
                }
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    HideWheel(takeCareWheel.transform);
                    StartCoroutine(SetMoving());
                }
                break;

            case InputType.CHOOSEHARVEST:
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    HideWheel(harvestWheel.transform);
                    StartCoroutine(SetMoving());
                }
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    HideWheel(harvestWheel.transform);
                    inputType = InputType.ACTIONHARVEST;
                    actionBufferUI.SetRepeatKey(KeyCode.H);
                    harvestClickCount = 0;
                }
                break;

            case InputType.CHOOSEPLOW:
                if (Input.GetKeyDown(KeyCode.DownArrow))
                {
                    HideWheel(plowWheel.transform);
                    StartCoroutine(SetMoving());
                }
                break;

            case InputType.ACTIONPLANTCARROT:
            case InputType.ACTIONPLANTLETTUCE:
            case InputType.ACTIONPLANTPOTATOES:
                validInputs = new KeyCode[] { KeyCode.S, KeyCode.D, KeyCode.J, KeyCode.K };
                foreach(var validInput in validInputs)
                {
                    if (Input.GetKeyDown(validInput))
                    {
                        if (actionBuffer[actionBufferUI.index].key == validInput)
                        {
                            actionBufferUI.forward();
                            ShakeCamera();

                            if (actionBufferUI.index == actionBuffer.Count)
                            {
                                switch (inputType)
                                {
                                    case InputType.ACTIONPLANTCARROT: terrain.focusedTile.GetComponent<Tile>().AddCarrot(); break;
                                    case InputType.ACTIONPLANTLETTUCE: terrain.focusedTile.GetComponent<Tile>().AddLettuce(); break;
                                    case InputType.ACTIONPLANTPOTATOES: terrain.focusedTile.GetComponent<Tile>().AddPotatoes(); break;
                                }
                                actionBufferUI.SetActionBuffer(null);
                                inputType = InputType.MOVING;
                                terrain.focusedTile.GetComponent<Tile>().beingUsed = false;
                            }
                        }
                        else {
                            actionBufferUI.backward();
                        }
                    }
                }
                
                break;

            case InputType.ACTIONWATER:
                if (actionBufferUI.index < actionBuffer.Count)
                {
                    if (actionBuffer[actionBufferUI.index].key == KeyCode.LeftArrow && Input.GetKey(KeyCode.LeftArrow) && !Input.GetKey(KeyCode.RightArrow) ||
                        (actionBuffer[actionBufferUI.index].key == KeyCode.RightArrow && Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow)))
                    {
                        inputClock += Time.deltaTime;
                        actionBufferUI.SetFill(inputClock / actionBuffer[actionBufferUI.index].time);
                        if (inputClock > actionBuffer[actionBufferUI.index].time)
                        {
                            actionBufferUI.forward();
                            ShakeCamera();
                            inputClock = 0f;
                            break;
                        }
                    }
                }
                    

                if (actionBufferUI.index == actionBuffer.Count)
                {
                    terrain.focusedTile.GetComponent<Tile>().Water();
                    actionBufferUI.SetActionBuffer(null);
                    inputType = InputType.MOVING;
                    terrain.focusedTile.GetComponent<Tile>().beingUsed = false;
                }
                break;

            case InputType.ACTIONHARVEST:
                if (Input.GetKeyDown(KeyCode.H))
                {
                    harvestClickCount++;
                    if(harvestClickCount == 10)
                    {
                        terrain.focusedTile.GetComponent<Tile>().Harvest();
                        actionBufferUI.RemoveRepeatKey();
                        inputType = InputType.MOVING;
                        terrain.focusedTile.GetComponent<Tile>().beingUsed = false;
                    }
                }
                break;
        }
    }

    public static void ShakeCamera()
    {
        Camera.main.transform.parent.GetComponent<CameraShake>().Shake();
    }
}
