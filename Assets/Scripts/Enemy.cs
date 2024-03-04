using System;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Health))]
[RequireComponent(typeof(CircleCollider2D))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private MovementPath _path;
    [SerializeField] private float _damage;

    private Waypoint[] _waypoints;
    private int _currentWaypointIndex = 0;
    private SpriteRenderer _renderer;
    private Transform _currentMovementTarget;
    private Health _health;

    private void Start()
    {
        _waypoints = _path.GetComponentsInChildren<Waypoint>();
        _renderer = GetComponent<SpriteRenderer>();
        _health = GetComponent<Health>();
        
        _currentMovementTarget = _waypoints[0].transform;
    }

    private void Update()
    {
        if (_waypoints is null || _waypoints.Length == 0)
            return;

        Vector3 currentPosition = transform.position;
        Vector3 targetPosition = _currentMovementTarget.position;
        currentPosition = Vector3.MoveTowards(currentPosition, targetPosition, _speed * Time.deltaTime);

        _renderer.flipX = targetPosition.x < currentPosition.x;

        if (currentPosition == targetPosition)
        {
            if (_currentMovementTarget.TryGetComponent<Waypoint>(out _))
            {
                _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;
                _currentMovementTarget = _waypoints[_currentWaypointIndex].transform;
            }
            else if (_currentMovementTarget.TryGetComponent<Player>(out var player))
            {
                player.Health.ApplyDamage(_damage);
            }
        }

        transform.position = currentPosition;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player) == false)
            return;

        _currentMovementTarget = player.transform;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player) == false)
            return;

        _currentMovementTarget = _waypoints[_currentWaypointIndex].transform;
    }

    public void ApplyDamage(float damage)
    {
        if (damage < 0)
            throw new ArgumentOutOfRangeException(nameof(damage), "Applied damage can't be less than 0");
        
        _health.ApplyDamage(damage);
        
        if(_health.IsAlive == false)
            Destroy(gameObject);
    }
}
