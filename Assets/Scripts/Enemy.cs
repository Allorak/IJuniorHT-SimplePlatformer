using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private MovementPath _path;

    private Waypoint[] _waypoints;
    private int _currentWaypointIndex = 0;
    private SpriteRenderer _renderer;

    private void Start()
    {
        _waypoints = _path.GetComponentsInChildren<Waypoint>();
        _renderer = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (_waypoints is null || _waypoints.Length == 0)
            return;

        Vector3 targetPosition = _waypoints[_currentWaypointIndex].transform.position;
        Vector3 currentPosition = transform.position;
        currentPosition = Vector3.MoveTowards(currentPosition, targetPosition, _speed * Time.deltaTime);

        _renderer.flipX = targetPosition.x < currentPosition.x;

        if (currentPosition == targetPosition)
            _currentWaypointIndex = (_currentWaypointIndex + 1) % _waypoints.Length;

        transform.position = currentPosition;
    }
}
