using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Stoker : MonoBehaviour
{
    [SerializeField] private NavMeshAgent navMeshAgent;
    [SerializeField] private BoxEventTrigger trigger;
    [SerializeField] private Animator animator;

    [SerializeField] private bool isMove = false;
    [SerializeField] private AudioSource source;

    private void Awake()
    {
        trigger.OnTrigger += () => ChangeMoveState(true);
    }

    private void ChangeMoveState(bool isMove)
    {
        if (isMove)
        {
            Managers.Instance.Sound.PlaySFX(ESoundClip.StokerOneShot);
            source.clip = Managers.Instance.Sound.GetAudioClip(ESoundClip.StokerLoop);
            source.Play();
        }

        this.isMove = isMove;
        
        animator.SetBool("IsMove", isMove);
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
        Managers.Instance.Sound.PlaySFX(ESoundClip.Run);
    }

}
