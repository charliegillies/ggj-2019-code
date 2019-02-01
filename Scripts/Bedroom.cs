using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bedroom : MonoBehaviour
{
    public static event System.Action BeginningMoveDown;
    public static event System.Action FinishedMovingUp;

    [Header("Walls")]
    [SerializeField] private BedroomWall m_rightWall;
    [SerializeField] private BedroomWall m_backWall;

    [Header("Transition")]
    [SerializeField] private Vector3 m_dropPosition;
    [SerializeField] private float m_dropTime;
    [SerializeField] private AnimationCurve m_dropCurve;
    [SerializeField] private AnimationCurve m_upCurve;
    [SerializeField] private EnvironmentPopups m_popups;
    [SerializeField] private CharacterController m_controller;

    private Vector3 m_startPosition;
    private Environment m_environment;

    private void Awake() {
        Environment.Loaded += OnEnvironmentLoaded;
        Environment.Finished += OnEnvironmentFinished;
        LevelChoicePrompt.LevelChosen += LevelChoiceMade;
        m_startPosition = transform.position;
    }
    private void OnDestroy() {
        Environment.Loaded -= OnEnvironmentLoaded;
        Environment.Finished -= OnEnvironmentFinished;
        LevelChoicePrompt.LevelChosen -= LevelChoiceMade;
    }
    private void LevelChoiceMade(Environment.ID id) {
        m_controller.Lock();
        switch (id) {
            case Environment.ID.Pirate: LoadPirateEnv(); break;
            case Environment.ID.Campsite: LoadCamsiteEnv(); break;
            case Environment.ID.Space: LoadSpaceEnv(); break;
        }
    }
    private void OnEnvironmentLoaded(Environment env) {
        m_environment = env;
    }
    private void OnEnvironmentFinished(Environment e) {
        StartCoroutine(Do_EnvironmentUnload());
    }
    private IEnumerator Do_EnvironmentUnload() {
        JobHandle handle = m_environment.TransitionDown();
        yield return new WaitUntil(() => handle.IsComplete());

        CameraController.Instance.ZoomIn();

        m_environment.Unload();
        m_environment = null;
        yield return TransitionUp();
        m_controller.Unlock();
    }

    [ContextMenu("Pirate Environment")]
    private void LoadPirateEnv() {
        StartCoroutine(Do_LoadPirateEnv());
    }
    private IEnumerator Do_LoadPirateEnv() {
        m_environment = null;

        // Begin loading the env_pirate scene
        AsyncOperation op = SceneManager.LoadSceneAsync("Env_Pirates", LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        yield return TransitionDown();

        // Show the UI prompt, allow the player to accept before we move on
        JobHandle handle = m_popups.ShowPiratePrompt();
        yield return handle.Wait();

        op.allowSceneActivation = true;

        // Now we wait for m_environment to not be null
        // .. which means that Environment.Loaded has fired
        yield return new WaitUntil(() => m_environment != null);

        // Changes orthographic size to the minimum to allow better view
        CameraController.Instance.ZoomOut();

        // .. then we transition the environment in!
        JobHandle transition = m_environment.TransitionUp();
        yield return transition.Wait();
    }

    [ContextMenu("Campsite Environment")]
    private void LoadCamsiteEnv() {
        StartCoroutine(Do_LoadCampsite());
    }
    private IEnumerator Do_LoadCampsite() {
        m_environment = null;

        // Begin loading the env_pirate scene
        AsyncOperation op = SceneManager.LoadSceneAsync("Env_Forest", LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        yield return TransitionDown();

        // Show the UI prompt, allow the player to accept before we move on
        JobHandle handle = m_popups.ShowCampPrompt();
        yield return handle.Wait();

        op.allowSceneActivation = true;

        // Now we wait for m_environment to not be null
        // .. which means that Environment.Loaded has fired
        yield return new WaitUntil(() => m_environment != null);

        // .. then we transition the environment in!
        JobHandle transition = m_environment.TransitionUp();
        yield return transition.Wait();
    }

    [ContextMenu("Space Environment")]
    private void LoadSpaceEnv() {
        StartCoroutine(Do_LoadSpaceEnvironment());
    }
    private IEnumerator Do_LoadSpaceEnvironment() {
        m_environment = null;

        // Begin loading the env_pirate scene
        AsyncOperation op = SceneManager.LoadSceneAsync("Env_Space", LoadSceneMode.Additive);
        op.allowSceneActivation = false;

        yield return TransitionDown();

        // Show the UI prompt, allow the player to accept before we move on
        JobHandle handle = m_popups.ShowSpacePrompt();
        yield return handle.Wait();

        op.allowSceneActivation = true;

        // Now we wait for m_environment to not be null
        // .. which means that Environment.Loaded has fired
        yield return new WaitUntil(() => m_environment != null);

        // .. then we transition the environment in!
        JobHandle transition = m_environment.TransitionUp();
        yield return transition.Wait();
    }

    private IEnumerator TransitionUp() {
        float time = 0.0f;
        while (time < m_dropTime) {
            time += Time.deltaTime;
            float t = m_upCurve.Evaluate(Mathf.Clamp01(time / m_dropTime));
            transform.position = Vector3.LerpUnclamped(m_dropPosition, m_startPosition, t);
            yield return null;
        }

        JobHandle h1 = m_rightWall.FoldUp();
        JobHandle h2 = m_backWall.FoldUp();
        // Wait until the bedroom walls have finished folding down
        yield return new WaitUntil(() => h1.IsComplete() && h2.IsComplete());

        FinishedMovingUp?.Invoke();
    }
    private IEnumerator TransitionDown() {
        BeginningMoveDown?.Invoke();

        JobHandle h1 = m_rightWall.FoldDown();
        JobHandle h2 = m_backWall.FoldDown();
        // Wait until the bedroom walls have finished folding down
        yield return new WaitUntil(() => h1.IsComplete() && h2.IsComplete());

        float time = 0.0f;
        while(time < m_dropTime) {
            time += Time.deltaTime;
            float t = m_dropCurve.Evaluate(Mathf.Clamp01(time / m_dropTime));
            transform.position = Vector3.LerpUnclamped(m_startPosition, m_dropPosition, t);
            yield return null;
        }
    }

}
