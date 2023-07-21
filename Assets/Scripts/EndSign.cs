using UnityEngine;
using UnityEngine.Events;

public class EndSign : MonoBehaviour
{
    [SerializeField] private UnityEvent _isReached;

    private void OnTriggerEnter2D(Collider2D col)
    {
        _isReached?.Invoke();
    }
}
