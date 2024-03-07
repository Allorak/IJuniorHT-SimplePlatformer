using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Health))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundCheckRaycastDistance;
    [SerializeField] private float _damage;
    [SerializeField] private float _attackRange;
    [SerializeField] private SpriteRenderer _swordSpriteRenderer;

    public Health Health { get; private set; }

    private SpriteRenderer _renderer;
    private Animator _animator;
    private Rigidbody2D _rigidBody;
    private bool _isMoving;
    private bool _isGrounded = true;
    private int _speedParameterHash = Animator.StringToHash("Speed");
    private int _jumpTriggerHash = Animator.StringToHash("HasJumped");
    private int _coinsAmount = 0;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
        Health = GetComponent<Health>();
    }

    private void OnEnable()
    {
        Health.HealthChanged += OnHealthChanged;
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        if (_isGrounded)
            return;

        if (collision.collider.TryGetComponent(out Ground _) == false)
            return;

        RaycastHit2D[] hits = Physics2D.RaycastAll(transform.position, Vector2.down, _groundCheckRaycastDistance);

        if (hits.Length <= 1 || hits[1].collider.TryGetComponent(out Ground _) == false)
            return;

        _isGrounded = true;
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.TryGetComponent(out Ground _))
            _isGrounded = false;
    }
    
    private void Update()
    {
        if (Input.GetKey(KeyCode.D))
            Move(Directions.Right);
        else if (Input.GetKey(KeyCode.A))
            Move(Directions.Left);
        else
            _isMoving = false;

        if (Input.GetKey(KeyCode.Space) && _isGrounded)
            Jump();

        if (Input.GetKey(KeyCode.Mouse0))
            Attack();

        _animator.SetFloat(_speedParameterHash, _isMoving ?  _speed : 0);
    }
    
    private void OnDisable()
    {
        Health.HealthChanged -= OnHealthChanged;
    }

    public void AddCoin()
    {
        _coinsAmount++;
    }

    private void Move(Directions direction)
    {
        var rotation = transform.rotation;
        
        if (direction == Directions.Left)
            rotation.y = -180;
        else
            rotation.y = 0;
        
        transform.rotation = rotation;
        
        float distance = _speed * Time.deltaTime;
        transform.Translate(distance, 0, 0);
        _isMoving = true;
    }

    private void OnHealthChanged(float health)
    {
        if(health <= 0)
            Destroy(gameObject);
    }

    private void Jump()
    {
        _animator.SetTrigger(_jumpTriggerHash);

        _rigidBody.AddForce(Vector2.up * _jumpForce);

        _isGrounded = false;
    }

    private void Attack()
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
        yield return new WaitForSeconds(1);
        _swordSpriteRenderer.enabled = false;
    }
}
