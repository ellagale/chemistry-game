using UnityEngine;
using UnityEngine.Events;
using Assets.Scripts;

public class HeatDialController : MonoBehaviour
{
    private const float MAX_TEMP_PROPORTION = 0.6f;
    private const float MIN_TEMP_PROPORTION = 0.2f;
    // The HeatDialController class handles functions related to the dial on the hotplate

    // Initialize Unity Events, which communicate when a mistake has been made and when it has been corrected
    [SerializeField] private UnityEvent correctTrigger;
    [SerializeField] private UnityEvent incorrectTrigger;
    public WhiteboardMessageController whiteboard;

    // Ensures that the Unity Events are triggered only once
    private bool eventHasBeenTriggered = false;

    private DialInteractable dial;

    // Start is called before the first frame update
    void Start()
    {
        // Initialize script
        dial = GetComponent<DialInteractable>();
        
    }

    // Update is called once per frame
    void Update()
    {
        HandleTriggers();
    }

    // Monitors state of dial for events
    private void HandleTriggers()
    {
        // If the heat is too high, triggers an error event.
        if (!eventHasBeenTriggered && dial.Value >= MAX_TEMP_PROPORTION)
        {
            whiteboard.RaiseMessage(WhiteboardMessage.HEAT_ERROR);
            eventHasBeenTriggered = true;
        } 

        // Triggers correction event when heat is lowered
        else if (eventHasBeenTriggered && dial.Value < MAX_TEMP_PROPORTION)
        {
            whiteboard.RemoveMessage(WhiteboardMessage.HEAT_ERROR);
            eventHasBeenTriggered = false;
        }
    }

    public bool IsTooHot()
    {
        return dial.Angle >= MAX_TEMP_PROPORTION;
    }

    public bool IsReady()
    {
        return dial.Angle >= MIN_TEMP_PROPORTION && dial.Angle < MAX_TEMP_PROPORTION;
    }
}
