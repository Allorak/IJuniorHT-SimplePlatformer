using System;
using UnityEngine;

[RequireComponent(typeof(Health))]
[RequireComponent(typeof(CircleCollider2D))]
public class Enemy : MonoBehaviour
{
    private const float ReachDistance = 0.15f;
    
    [SerializeField] private float _speed;
    [SerializeField] private MovementPath _path;
    [SerializeField] private float _damage;

    private Waypoint[] _waypoints;
    private int _currentWaypointIndex = 0;
    private Player _currentPlayerTarget = null;
    
    public Health Health { get; private set; }

    private void Awake()
    {
        Health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        Health.Died += OnDied;
    }

    private void Start()
    {
        _waypoints = _path.GetComponentsInChildren<Waypoint>();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out var player))
            _currentPlayerTarget = player;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.TryGetComponent<Player>(out _))
            _currentPlayerTarget = null;
    }

    private void Update()
    {
        if (_waypoints is null || _waypoints.Length == 0)
            return;

        var currentPosition = transform.position;
        
        var targetPosition = GetTargetPosition();

        var movementDirection = (targetPosition - currentPosition).normalized;

        currentPosition.x = Mathf.MoveTowards(currentPosition.x, targetPosition.x, _speed * Time.deltaTime);
        
        transform.position = currentPosition;

        if (Mathf.Abs(currentPosition.x - targetPosition.x) <= ReachDistance)
        {
            if (_currentPlayerTarget is null)
                _currentWaypointIndex = ++_currentWaypointIndex % _waypoints.Length;
            else
                _currentPlayerTarget.Health.ApplyDamage(_damage);
        }
        else
        {
            transform.rotation = movementDirection.x switch
            {
                < 0 => Quaternion.Euler(0, -180, 0),
                > 0 => Quaternion.Euler(0, 0, 0),
                _ => transform.rotation
            };
        }
    }

    private void OnDisable()
    {
        Health.Died -= OnDied;
    }

    private void OnDied()
    {
        Destroy(gameObject);
    }

    private Vector3 GetTargetPosition()
    {
        return _currentPlayerTarget is null 
            ? _waypoints[_currentWaypointIndex].transform.position 
            : _currentPlayerTarget.transform.position;
    }
}
