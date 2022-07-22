using UnityEngine;

public class PlayerStateMachine : MonoBehaviour
{
    //Variables
    [SerializeField] float moveSpeed;
    [SerializeField] float slashForce;

    //Components
    [HideInInspector] Animator animator;
    [HideInInspector] Rigidbody2D rb;

    [SerializeField] GameObject player;
    [SerializeField] GameObject slashPrefab;
    [SerializeField] Transform firePoint;
    [SerializeField] private PlayerAbilities abilities;
    public PlayerAbilities Abilities => abilities;
    public Animator Animator => animator;
    public float MoveSpeed => moveSpeed;
    public Rigidbody2D Rigidbody => rb;

    //Getters and Setters
    public bool AttackAnglePaused { get; set; }
    public bool IsAttacking { get; set; }

    private Camera cam;
    private PlayerState state;

    private void Awake()
    {
        animator = player.GetComponent<Animator>();
        rb = player.GetComponent<Rigidbody2D>();
        cam = Camera.main;
        SetState(new PlayerIdleState(this));
    }

    // Update is called once per frame
    void Update()
    {
        state.Update();
    }

    //Used in Animation Event
    public void AttackAnimationEnd()
    {
        animator.ResetTrigger("Attack");
        animator.ResetTrigger("Dash");
        AttackAnglePaused = false;
        SetState(new PlayerIdleState(this));
    }

    //Used in Animation Event
    public void Attack()
    {
        IsAttacking = true;
    }

    public void CreateSlash()
    {
        //Instantiate Slash prefab
        GameObject slash = Instantiate(slashPrefab, firePoint.position, firePoint.rotation);

        //Get the Rigid Body of the Slash prefab
        Rigidbody2D slashRB = slash.GetComponent<Rigidbody2D>();

        //Add Force to Slash prefab
        slashRB.AddForce(firePoint.up * slashForce, ForceMode2D.Impulse);
    }

    public bool AnyMoveKeyPressed => 
        Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D);
    public bool AttackKeyPressed => Input.GetMouseButtonDown(0);
    public bool DashKeyPressed => Input.GetKey(KeyCode.Space);
    public void SetState(PlayerState newState) => state = newState;
    public Vector2 GetPlayerMouseDifference() =>
        cam.ScreenToWorldPoint(Input.mousePosition) - transform.position;
}
