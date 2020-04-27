using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VRM;
using UnityEngine;

public class GameCharacterController : MonoBehaviour
{
    private Collider attachedCollider = null;
    private Rigidbody attachedRigidbody = null;

    [Header("Required Setting")]
    public Vector3 Center = new Vector3(0, 0, 0);
    public float MinDelta = 0.05f;
    [Range(0.0f, 2.0f)]
    public float GroundHitDistence = 0.575f;
    //與環境的地面摩擦力
    [Range(0.0f, 1.0f)]
    public float friction = 0.5f;
    //與環境的空氣摩擦力
    [Range(0.0f, 1.0f)]
    public float airFriction = 0.5f;



    [Header("Information")]
    //每次畫面更新前的位移量
    public Vector3 moveVector;
    //每次畫面更新前速度量
    public Vector3 moveSpeed;
    //移動方向純量
    public float moveMagnitude = 0;
    //重力速度
    public float gravityVelocity = 0.0f;
    //落地計時器
    public float unGroundedTimer = 0;
    //是否啟用重力
    public bool useGravity = true;

    //是否落地(請使用屬性設定)
    public bool _IsGrounded = false;
    public bool IsGrounded
    {
        get
        {
            return !(unGroundedTimer > 0.15f);
        }
        set
        {
            _IsGrounded = true;
            unGroundedTimer = 0;
        }
    }
    public Vector3 CenterPosition
    {
        get
        {
            return transform.position + Center;
        }
    }
    void CheckRequireds()
    {
        try
        {
            attachedCollider = GetComponent<Collider>();
            attachedRigidbody = GetComponent<Rigidbody>();
        }
        catch
        {
            Debug.LogError("GameCharacterController Missing Some Component");
        }
    }

    // Use this for initialization
    void Start()
    {
        CheckRequireds();
        unGroundedTimer = 0;
        moveVector = new Vector3(0, 0, 0);
        moveSpeed = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (this.attachedRigidbody.velocity.magnitude < MinDelta)
        {
            this.attachedRigidbody.velocity = new Vector3(0, 0, 0);
        }
    }

    //確認落地狀態
    private void UpdateIsGround()
    {
        if (!_IsGrounded)
            unGroundedTimer += Time.deltaTime;
        else
            unGroundedTimer = 0;
        RaycastHit rhf = new RaycastHit();
        Physics.SphereCast(CenterPosition, 0.3f, Physics.gravity.normalized, out rhf, GroundHitDistence);
        if (rhf.collider != null)
            _IsGrounded = true;
        else
            _IsGrounded = false;

    }

    //處理速度摩擦力
    private void ProcessSpeedFriction()
    {
        if (IsGrounded)
        {
            moveSpeed = moveSpeed * (1 - (friction / 10));
        }
        else
        {
            moveSpeed = moveSpeed * (1 - (airFriction / 10));
        }
    }

    public void ProcessGravity(Omector3 gravity)
    {
        //增加重力
        if (useGravity && IsGrounded == false)
            gravityVelocity += gravity.Power * 0.2f;
        else
        {
            if (gravityVelocity > 0 && moveSpeed.y > 0)
                moveSpeed.y = 0;
            gravityVelocity = 0;
        }
        if (useGravity)
            moveVector += (Vector3)gravity.DirectionOmector * gravityVelocity;
    }

    public void Move(Omector3 gravity,float Fix)
    {
        //不使用Rigidbody的速度
        attachedRigidbody.velocity = new Vector3(0, 0, 0);
        UpdateIsGround();
        ProcessSpeedFriction();
        ProcessGravity(gravity);
        moveVector += moveSpeed;
        attachedRigidbody.transform.position +=(moveVector * Time.deltaTime/ Fix);
        moveVector = new Vector3(0, 0, 0);
    }
}
