using UnityEngine;

[RequireComponent(typeof(Animator))]
public class EndSign : MonoBehaviour
{
    [SerializeField] private AudioClip _reachSound;
    
    private Animator _animator;
    private readonly int _reachTriggerHash = Animator.StringToHash("IsReached");

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        _animator.SetTrigger(_reachTriggerHash);
        AudioSource.PlayClipAtPoint(_reachSound, transform.position);
    }
}
