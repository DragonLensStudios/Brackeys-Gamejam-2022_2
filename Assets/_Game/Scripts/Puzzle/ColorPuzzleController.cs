using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

public class ColorPuzzleController : MonoBehaviour
{
    [SerializeField]
    private string id;
    [SerializeField]
    private float solveTimeBeforeReset = 10f;
    [SerializeField]
    List<ColorPuzzleSwitch> switches = new List<ColorPuzzleSwitch>();
    [SerializeField]
    private bool isActivated;
    [SerializeField]
    private UnityEvent activatedSwitchesCallback;

    private bool isPuzzleStarted, isPuzzleComplete;
    private float currentTimer;
    private List<ColorPuzzleSwitch> activatedSwitches = new List<ColorPuzzleSwitch>();

    public bool IsActivated { get => isActivated; set => isActivated = value; }

    private void OnEnable()
    {
        EventManager.onColorSwitchActivate += EventManager_onColorSwitchActivate;
    }

    private void OnDisable()
    {
        EventManager.onColorSwitchActivate -= EventManager_onColorSwitchActivate;
    }

    private void Awake()
    {
        if(switches.Count <= 0 && transform.childCount > 0)
        {
            switches = GetComponentsInChildren<ColorPuzzleSwitch>().ToList();
        }
    }

    private void Update()
    {
        if (isPuzzleStarted && !isPuzzleComplete)
        {
            currentTimer += Time.deltaTime;
            if(currentTimer >= solveTimeBeforeReset)
            {
                ResetPuzzle();
            }
        }
    }

    public void Activate()
    {
        isActivated = true;
        isPuzzleComplete = true;
        activatedSwitchesCallback.Invoke();
    }

    public void ResetPuzzle()
    {
        for (int i = 0; i < activatedSwitches.Count; i++)
        {
            activatedSwitches[i].Deactivate(activatedSwitches[i].CandleColor);
        }
        activatedSwitches.Clear();
        isPuzzleStarted = false;
        currentTimer = 0;
    }

    private void EventManager_onColorSwitchActivate(string name, CandleColor color)
    {
        var colorSwitch = switches.Find(x => x.SwitchName == name);
        if(colorSwitch != null)
        {
            if (colorSwitch.IsActivated)
            {
                isPuzzleStarted = true;
                if (!activatedSwitches.Contains(colorSwitch))
                {
                    activatedSwitches.Add(colorSwitch);
                }
            }
        }
        if(activatedSwitches.Count >= switches.Count)
        {
            if(activatedSwitches.All(x=> x.IsActivated))
            {
                Activate();
            }
        }
    }

}
