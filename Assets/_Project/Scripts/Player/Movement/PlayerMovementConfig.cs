using UnityEngine;

[System.Serializable]
public struct PhysicsParams
{
    public float targetMoveSpeed;
    public float movementMultiplier;
    public float idleFriction;
    public float bleedingFriction;
    public float turnaroundFriction;
    public bool appliesHardCap;
}

[System.Serializable]
public class PlayerMovementConfig : MonoBehaviour
{
    // public fields to simplify syntax - these varaibles shouldn't be modified in the code
    public PhysicsParams walkParams;

    public PhysicsParams airParams;
    public float jumpForce = 5f;

    [Header("Wall Jump Settings")]
    public int wallJumpLimit = 3;
    public float wallCheckBuffer = 0.3f;
    public LayerMask wallMask;
    public float wallJumpSideForce = 10f;
    public float wallJumpUpForce = 8f;

    public PhysicsParams slideParams;
    public float slideDuration = 0.5f;
    public float minVelocity = 10f;
    public float slidePositionYOffset = 20f;
    public float slideRotationXOffset = 0.2f;
    public Transform[] rotateOnSlideTransforms;
    public Transform[] moveOnSlideTransforms;


    [Header("Dash Settings")]
    public float dashForce = 20.0f;
    public int maxAirDashes = 1;

    [Header("GroundChecks")]
    public Transform groundCheck;

    public float groundDistance = 0.2f;
    public LayerMask groundMask;
}