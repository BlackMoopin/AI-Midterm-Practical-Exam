using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Summary: The script will be the core of the civilian Finite State Machine 
//          every frame it senses how close the villain is, drives the Animator parameters, 
//          and updates the state label above the head.
// State names, timers and fear logic are the engine - do not edit those parts.
// -------- DO NOT TOUCH THIS SCRIPT --------

// Instructions: Setup necessary variables in the inspector.


public class CivilianBrain : MonoBehaviour {

    public Transform threat;
    public string threatTag = "Player";
    public float detectionRadius = 8f;
    public float panicRadius = 4f;
    public float wanderSpeed = 2f;
    public float panicSpeed = 5f;
    public Transform[] coverSpots;
    public Transform[] waypoints;
    public SpriteRenderer spriteRenderer;
    public TextMeshPro stateLabel;

    private static readonly string[] TRIGGER_NAMES = { "tStartleDone", "tFleeDone", "tHideDone", "tFaintDone", "tRecoverDone" };

    private int fearLevel;
    private Animator animator;
    private float stateTimer;
    private string currentStateTrigger;
    private bool triggerFired;
    private string currentStateName;
    private bool warnedUnmatchedState;

    private void Awake() {
        animator = GetComponent<Animator>();
        if (panicRadius > detectionRadius) {
            panicRadius = detectionRadius;
        }
        ValidateConfig();
    }

    private void Start() {
        if (threat == null) {
            GameObject found = GameObject.FindGameObjectWithTag(threatTag);
            if (found == null) {
                Debug.LogError($"[CivilianBrain] No GameObject tagged '{threatTag}' found. Tag the threat object '{threatTag}' or assign threat in the Inspector, otherwise threat detection will not work");
            } else {
                threat = found.transform;
            }
        }
        OnStateEntered(GetCurrentStateName());
    }

    private void Update() {
        UpdateThreatParams();

        string stateName = GetCurrentStateName();
        if (stateName != currentStateName) {
            OnStateEntered(stateName);
        }

        if (stateTimer > 0f && !triggerFired) {
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0f) {
                if (!string.IsNullOrEmpty(currentStateTrigger)) {
                    animator.SetTrigger(currentStateTrigger);
                }
                triggerFired = true;
            }
        }
    }

    private void UpdateThreatParams() {
        if (threat == null) {
            animator.SetBool("isThreatNear", false);
            animator.SetBool("isThreatClose", false);
            return;
        }

        float dist = Vector2.Distance(transform.position, threat.position);
        animator.SetBool("isThreatNear", dist <= detectionRadius);
        animator.SetBool("isThreatClose", dist <= panicRadius);
    }

    private string GetCurrentStateName() {
        AnimatorStateInfo info = animator.GetCurrentAnimatorStateInfo(0);
        if (info.IsName("Wander")) return "Wander";
        if (info.IsName("Startle")) return "Startle";
        if (info.IsName("Flee")) return "Flee";
        if (info.IsName("Hide")) return "Hide";
        if (info.IsName("Faint")) return "Faint";
        if (info.IsName("Recover")) return "Recover";
        if (!warnedUnmatchedState) {
            Debug.LogError("[CivilianBrain] Current animator state is not one of Wander, Startle, Flee, Hide, Faint, Recover - label and timer will not update for it. Check the state names in the Animator.");
            warnedUnmatchedState = true;
        }
        return currentStateName;
    }

    private void OnStateEntered(string stateName) {
        ResetAllTriggers();
        currentStateName = stateName;
        triggerFired = false;
        ApplyStateConfig(stateName);

        if (stateLabel != null) {
            stateLabel.text = stateName;
            stateLabel.color = GetStateColor(stateName);
        }

        fearLevel = Mathf.Clamp(fearLevel + GetFearDelta(stateName), 0, 3);
        animator.SetInteger("fearLevel", fearLevel);
    }

    private void ApplyStateConfig(string stateName) {
        currentStateTrigger = null;
        stateTimer = 0f;

        if (stateName == "Startle") {
            stateTimer = 0.3f;
            currentStateTrigger = "tStartleDone";
        } else if (stateName == "Flee") {
            stateTimer = 1.5f;
            currentStateTrigger = "tFleeDone";
        } else if (stateName == "Hide") {
            stateTimer = 2f;
            currentStateTrigger = "tHideDone";
        } else if (stateName == "Faint") {
            stateTimer = 3f;
            currentStateTrigger = "tFaintDone";
        } else if (stateName == "Recover") {
            stateTimer = 1f;
            currentStateTrigger = "tRecoverDone";
        }
    }

    private Color GetStateColor(string stateName) {
        if (stateName == "Startle") {
            return Color.yellow;
        }
        if (stateName == "Flee") {
            return Color.red;
        }
        if (stateName == "Hide") {
            return Color.grey;
        }
        if (stateName == "Faint") {
            return new Color(0.5f, 0f, 0.5f);
        }
        if (stateName == "Recover") {
            return Color.green;
        }
        return Color.white;
    }

    private int GetFearDelta(string stateName) {
        if (stateName == "Startle") {
            return 1;
        }
        if (stateName == "Faint") {
            return -10;
        }
        return 0;
    }

    private void ResetAllTriggers() {
        for (int i = 0; i < TRIGGER_NAMES.Length; i++) {
            animator.ResetTrigger(TRIGGER_NAMES[i]);
        }
    }

    private void ValidateConfig() {
        if (animator.runtimeAnimatorController == null) {
            Debug.LogError("[CivilianBrain] No Animator Controller assigned - FSM cannot run");
            return;
        }

        List<string> required = new List<string> { "isThreatNear", "isThreatClose", "fearLevel", "tStartleDone", "tFleeDone", "tHideDone", "tFaintDone", "tRecoverDone" };
        AnimatorControllerParameter[] paramList = animator.parameters;
        for (int i = 0; i < required.Count; i++) {
            bool found = false;
            for (int j = 0; j < paramList.Length; j++) {
                if (paramList[j].name == required[i]) {
                    found = true;
                    break;
                }
            }
            if (!found) {
                Debug.LogError($"[CivilianBrain] Animator parameter '{required[i]}' is missing - Unity will silently ignore calls to it");
            }
        }
    }
}
