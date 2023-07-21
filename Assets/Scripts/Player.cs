using UnityEngine;

[RequireComponent(typeof(SpriteRenderer))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody2D))]
public class Player : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _groundCheckRaycastDistance;

    private SpriteRenderer _renderer;
    private Animator _animator;
    private Rigidbody2D _rigidBody;
    private bool _isMoving;
    private bool _isGrounded = true;
    private int _speedParameterHash = Animator.StringToHash("Speed");
    private int _jumpTriggerHash = Animator.StringToHash("HasJumped");
    private int _coinsAmount = 0;

    private void Start()
    {
        _renderer = GetComponent<SpriteRenderer>();
        _animator = GetComponent<Animator>();
        _rigidBody = GetComponent<Rigidbody2D>();
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

        _animator.SetFloat(_speedParameterHash, _isMoving ?  _speed : 0);
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

    public void AddCoin()
    {
        _coinsAmount++;
    }

    private void Move(Directions direction)
    {
        float distance = _speed * Time.deltaTime;

        if (direction == Directions.Left)
            distance *= -1;

        transform.Translate(distance, 0, 0);
        _isMoving = true;
        _renderer.flipX = direction == Directions.Left;
    }

    private void Jump()
    {
        _animator.SetTrigger(_jumpTriggerHash);

        _rigidBody.AddForce(Vector2.up * _jumpForce);

        _isGrounded = false;
    }
}
