using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using AK.Wwise;

public class PlayerController : MonoBehaviour, IManager
{
    #region Fields
    [field: Header("Movement Fields")]
    [field: SerializeField] public PlayerMovementStateModule playerMovementModule { get; private set; }
    [SerializeField] private CharacterController _characterController;
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private float _walkSpeed = 2f;
    [SerializeField] private float _sprintSpeed = 4f;
    [SerializeField] private float _footStepUnitTime = 0.5f;
    [SerializeField] private BloodFootStep footStep;
    [SerializeField] private Material[] footStepMaterials;
    [field: SerializeField] public GameObject LightObject;
    [SerializeField] private Vector3 originPos;

    [Header("Wwise Audio")]
    //[SerializeField] private AK.Wwise.Event deathSoundEvent;
    [SerializeField] private AK.Wwise.Event walkEvent;
    [SerializeField] private AK.Wwise.Event runEvent;
    [SerializeField] private AK.Wwise.Event walkBloodEvent;
    [SerializeField] private AK.Wwise.Event runBloodEvent;
    #endregion

    #region Unity Methods
    private void Awake()
    {
        //DontDestroyOnLoad(this);
    }

    private void FixedUpdate()
    {
        playerMovementModule.FixedUpdate();
    }

    private void Update()
    {
        playerMovementModule.Update();
    }
    #endregion

    #region Init Methods
    public void Init()
    {
        _cameraController.Init();
        playerMovementModule = new PlayerMovementStateModule(_characterController, EPlayerMovement.Idle, new IdleState(this), 
            footStep, footStepMaterials, _walkSpeed, _sprintSpeed, _footStepUnitTime, walkEvent, runEvent, walkBloodEvent, runBloodEvent);
        playerMovementModule.TryAddState(EPlayerMovement.Walk, new WalkState(this));
        playerMovementModule.TryAddState(EPlayerMovement.Sprint, new SprintState(this));
    }
    #endregion

    public void Die()
    {
        Managers.Instance.Coroutine.StartCoroutine(CoDie());
    }

    private IEnumerator CoDie()
    {
        _cameraController.isPlayerable = false;
        float time = _cameraController.shakeDuration;

        _cameraController.originalPosition = _cameraController.transform.position;

        // ���� ���� ����
        if (Stoker.Instance != null)
        {
            Stoker.Instance.StopLoopSound();

            // Catch ���� ��� (1ȸ��)
            var stokerCatchEvent = new AK.Wwise.Event();
            stokerCatchEvent.Post(Stoker.Instance.gameObject); // Ȥ�� �ش� ������Ʈ ���� ����
        }

        AkSoundEngine.SetState("PlayerStatus", "Caught");
        // TODO: �״� ����
        Stoker.Instance.stokerCatchEvent.Post(Stoker.Instance.gameObject);

        while (time > 0f)
        {
            _cameraController.DoShakeMotion();
            time -= Time.deltaTime;
            yield return null;
        }

        time = _cameraController.shakeDuration * 0.5f;
        while (time > 0f)
        {
            _cameraController.DoDeadMotion();
            time -= Time.deltaTime;
            yield return null;
        }

        
        yield return Managers.Instance.Coroutine.GetWaitForSeconds(1f);
        // TODO: ���̵�
        GoNextScene();
    }

    private void GoNextScene()
    {
        Debug.Log("Reset");
        _cameraController.isPlayerable = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    
}
