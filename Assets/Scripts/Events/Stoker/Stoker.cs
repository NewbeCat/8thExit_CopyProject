using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using AK.Wwise;

public class Stoker : MonoBehaviour
{
    public static Stoker Instance;
    private bool isLoopPlaying = false;

    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private BoxEventTrigger trigger;
    [SerializeField] private Animator animator;

    [SerializeField] private bool isMove = false;

    [Header("Wwise Events")]
    [SerializeField] private AK.Wwise.Event stokerOneShotEvent; //  1회성 효과음
    [SerializeField] private GameObject stokerLoopObject;       //  루프 사운드가 붙은 오브젝트 (AkAmbient 사용)
    [SerializeField] private AK.Wwise.Event footstepEvent;
    [SerializeField] public AK.Wwise.Event stokerCatchEvent;

    private void Awake()
    {
        Instance = this;
        trigger.OnTrigger += () => ChangeMoveState(true);

        if (stokerLoopObject != null)
            stokerLoopObject.SetActive(false); // 처음엔 꺼진 상태
    }

    private void ChangeMoveState(bool isMove)
    {
        this.isMove = isMove;
        animator.SetBool("IsMove", isMove);

        if (isMove)
        {
            stokerOneShotEvent.Post(gameObject);

            if (stokerLoopObject != null && !isLoopPlaying)
            {
                stokerLoopObject.SetActive(true);
                isLoopPlaying = true;
            }
        }
    }

    private void Update()
    {
        if (isMove)
        {
            navMeshAgent.SetDestination(Managers.Instance.Player.transform.position);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            // 플레이어 죽음
            Debug.Log("Die");

            other.GetComponent<PlayerController>().Die();
            ChangeMoveState(false);
        }
    }

    private void FootStep()
    {
        footstepEvent.Post(gameObject);
    }

    public void StopLoopSound()
    {
        if (stokerLoopObject != null)
        {
            // 루프 사운드 강제 중지
            AK.Wwise.Event stopEvent = new AK.Wwise.Event();
            stopEvent.Stop(stokerLoopObject); // 강제로 Stop 시도

            stokerLoopObject.SetActive(false); // 오브젝트 비활성화
            isLoopPlaying = false;
            Debug.Log("Stoker 루프 사운드 강제 종료됨");
        }
    }
}
