using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    private const float SwordVisibilityDuration = 0.5f;
    
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundCheckRaycastDistance;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackRange;
    [SerializeField] private SpriteRenderer _swordSpriteRenderer;

    private readonly int _speedParameterHash = Animator.StringToHash("Speed");
    private readonly int _jumpTriggerHash = Animator.StringToHash("HasJumped");
    private Animator _animator;
    private Rigidbody2D _rigidBody;
    private bool _isMoving;
    private bool _isGrounded = true;
    private bool _shouldJump;
    private int _coinsAmount = 0;
    
    public Health Health { get; private set; }

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        Health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        Health.Died += OnDied;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (_isGrounded)
            return;

        if (collision.collider.TryGetComponent(out Ground _) == false)
            return;

        var end = (Vector3)(Vector2.down * _groundCheckRaycastDistance);
        Debug.DrawLine(transform.position, transform.position + end);
        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, _groundCheckRaycastDistance);

        if (hits.Length == 0 || hits[0].collider.TryGetComponent(out Ground _) == false)
            return;

        _isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Ground _))
            _isGrounded = false;
    }

    private void FixedUpdate()
    {
        if (_shouldJump == false)
            return;
        
        _rigidBody.AddForce(Vector2.up * _jumpForce);
        _shouldJump = false;
    }

    private void OnDisable()
    {
        Health.Died -= OnDied;
    }

    public void AddCoin()
    {
        _coinsAmount++;
    }

    public void Move(Directions direction)
    {
        var rotation = transform.rotation;
        rotation.y = direction == Directions.Right ? 0 : -180;
        transform.rotation = rotation;
        
        float distance = _speed * Time.deltaTime;
        transform.Translate(distance, 0, 0);
        _animator.SetFloat(_speedParameterHash, _speed);
        _isMoving = true;
    }

    public void Stop()
    {
        if(_isMoving == false)
            return;
        
        _isMoving = false;
        _animator.SetFloat(_speedParameterHash, 0);
    }

    public void Jump()
    {
        if (_isGrounded == false)
            return;
        
        _animator.SetTrigger(_jumpTriggerHash);

        _shouldJump = true;

        _isGrounded = false;
    }

    public void Attack()
    {
        var playerPosition = transform.position;

        StartCoroutine(nameof(ShowSword));
        
        var attackLineEnd = playerPosition + transform.forward * _attackRange;
        RaycastHit2D[] hits = Physics2D.LinecastAll(playerPosition, attackLineEnd);

        if (hits.Length <= 1)
            return;

        foreach (var hit in hits)
            if (hit.transform.TryGetComponent<Enemy>(out var enemy))
                enemy.Health.ApplyDamage(_damage);
    }

    private IEnumerator ShowSword()
    {
        _swordSpriteRenderer.enabled = true;
        yield return new WaitForSeconds(SwordVisibilityDuration);
        _swordSpriteRenderer.enabled = false;
    }
    
    private void OnDied()
    {
        Destroy(gameObject);
    }
}
