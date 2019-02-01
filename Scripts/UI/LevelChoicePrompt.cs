using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class LevelChoicePrompt : MonoBehaviour
{
    public static Action<Environment.ID> LevelChosen;

    [SerializeField] private CanvasGroup m_dpadGroup;
    [SerializeField] private CanvasGroup m_exitGroup;
    private bool m_levelChoiceShown = false;
    private bool m_exitChoiceShown = false;

    private bool m_choiceAllowed = true;

    private void Start() {
        PlayerBox.InteractionBegins += OnBoxInteractionBegin;
        PlayerBox.InteractionEnds += OnBoxInteractionEnd;
        DoorQuitPrompt.InteractBegin += DoorQuitPrompt_InteractBegin;
        DoorQuitPrompt.InteractEnd += DoorQuitPrompt_InteractEnd;
        Bedroom.FinishedMovingUp += Bedroom_FinishedMovingUp;
    }
    private void OnDestroy() {
        PlayerBox.InteractionBegins -= OnBoxInteractionBegin;
        PlayerBox.InteractionEnds -= OnBoxInteractionEnd;
        DoorQuitPrompt.InteractBegin -= DoorQuitPrompt_InteractBegin;
        DoorQuitPrompt.InteractEnd -= DoorQuitPrompt_InteractEnd;
        Bedroom.FinishedMovingUp -= Bedroom_FinishedMovingUp;
    }
    private void Bedroom_FinishedMovingUp() {
        m_choiceAllowed = true;
    }
    private void DoorQuitPrompt_InteractEnd() {
        m_exitChoiceShown = false;
    }
    private void DoorQuitPrompt_InteractBegin() {
        m_exitChoiceShown = true;
    }
    private void OnBoxInteractionBegin() {
        m_levelChoiceShown = true;
    }
    private void OnBoxInteractionEnd() {
        m_levelChoiceShown = false;
    }

    private void LevelChoice() {
        // Fade in/out appropriately
        float targ = m_levelChoiceShown && m_choiceAllowed ? 1.0f : 0.0f;
        float offset = (m_dpadGroup.alpha < targ ? Time.deltaTime : -Time.deltaTime) * 2.0f;
        m_dpadGroup.alpha += offset;

        if (m_choiceAllowed && m_levelChoiceShown) {
            float dPadVertical = Input.GetAxis("DPadVertical");
            float dPadHorizontal = Input.GetAxis("DPadHorizontal");

            if (Mathf.Approximately(dPadVertical, 1.0f) || Input.GetKeyDown(KeyCode.UpArrow)) {
                LevelChosen?.Invoke(Environment.ID.Space);
                m_choiceAllowed = false;
            }
            else if (Mathf.Approximately(dPadHorizontal, -1.0f) || Input.GetKeyDown(KeyCode.LeftArrow)) {
                LevelChosen?.Invoke(Environment.ID.Pirate);
                m_choiceAllowed = false;
            }
            else if (Mathf.Approximately(dPadHorizontal, 1.0f) || Input.GetKeyDown(KeyCode.RightArrow)) {
                LevelChosen?.Invoke(Environment.ID.Campsite);
                m_choiceAllowed = false;
            }
        }
    }
    private void ExitChoice() {
        // Fade in/out appropriately
        float targ = m_choiceAllowed && m_exitChoiceShown ? 1.0f : 0.0f;
        float offset = (m_exitGroup.alpha < targ ? Time.deltaTime : -Time.deltaTime) * 2.0f;
        m_exitGroup.alpha += offset;

        if (m_choiceAllowed && m_exitChoiceShown) {
            if (Input.GetButtonDown("Fire1")) {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }
    }
    private void Update() {
        LevelChoice();
        ExitChoice();
    }
}
