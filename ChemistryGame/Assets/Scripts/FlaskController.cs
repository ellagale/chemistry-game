using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.Events;
using Assets.Scripts;

public class FlaskController : MonoBehaviour
{
    // Handles flask behaviour

    // Initializes the hotplate mantle socket
    [SerializeField] private XRSocketInteractor hotplateSocket;

    // Initializes the flask socket (condenser attachment)
    [SerializeField] private XRSocketInteractor flaskSocket;

    // Ensures that the Unity Events are triggered only once
    private bool triggered = false;

    private SolidHolder granuleHolder;
    private NewLiquidContainer flask;
    public WhiteboardMessageController whiteboard;

    // Start is called before the first frame update
    private void Start()
    {
        // Initialize script
        flask = GetComponent<NewLiquidContainer>();
        granuleHolder = GetComponentInChildren<SolidHolder>();
    }

    // Update is called once per frame
    private void Update()
    {
        HandleTriggers();
    }

    // Returns a bool value which indicates that the flask is in its correct state for reflux success
    public bool IsReady()
    {
        return flask.liquidAmount >= 0.4 && hotplateSocket.hasSelection &&
               flaskSocket.hasSelection && HasAntiBumpingGranules();
    }

    // Monitors state of flask for event triggers
    private void HandleTriggers()
    {
        // If the condenser has been attached when no anti-bumping granules are contained in the flask, triggers error event
        if (!triggered && !HasAntiBumpingGranules() && flaskSocket.hasSelection)
        {
            whiteboard.RaiseMessage(WhiteboardMessage.GRANULE_ERROR);
            triggered = true;
        }

        // Triggers correction event when anti-bump granules are then added
        else if (triggered && HasAntiBumpingGranules())
        {
            whiteboard.RemoveMessage(WhiteboardMessage.GRANULE_ERROR);
            triggered = false;
        }
    }
    public bool HasAntiBumpingGranules()
    {
        // As this is a simple experiment, the only solid _is_ bumping granules.
        return !granuleHolder.IsEmpty();
    }
}
