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
    [SerializeField] private AK.Wwise.Event stokerOneShotEvent; //  1ȸ�� ȿ����
    [SerializeField] private GameObject stokerLoopObject;       //  ���� ���尡 ���� ������Ʈ (AkAmbient ���)
    [SerializeField] private AK.Wwise.Event footstepEvent;
    [SerializeField] public AK.Wwise.Event stokerCatchEvent;

    private void Awake()
    {
        Instance = this;
        trigger.OnTrigger += () => ChangeMoveState(true);

        if (stokerLoopObject != null)
            stokerLoopObject.SetActive(false); // ó���� ���� ����
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
            // �÷��̾� ����
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
            // ���� ���� ���� ����
            AK.Wwise.Event stopEvent = new AK.Wwise.Event();
            stopEvent.Stop(stokerLoopObject); // ������ Stop �õ�

            stokerLoopObject.SetActive(false); // ������Ʈ ��Ȱ��ȭ
            isLoopPlaying = false;
            Debug.Log("Stoker ���� ���� ���� �����");
        }
    }
}
