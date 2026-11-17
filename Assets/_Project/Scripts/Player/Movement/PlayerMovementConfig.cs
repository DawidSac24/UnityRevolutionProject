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
    [Header("State Physics Profiles")]
    public PhysicsParams walkParams;
    public PhysicsParams airParams;
    public PhysicsParams slideParams;

    // [Header("Walking")]
    // public float walkSpeed = 15.0f;
    // public float idleFriction = 20.0f;
    // public float bleedingFriction = 3.0f;
    // public float turnaroundFriction = 25.0f;
    //
    // [Header("Airborne")]
    // public float airborneSpeed = 15.0f;
    // public float airborneMultiplier = 0.4f;
    // public float jumpForce = 5.0f;
    // public float airborneFriction = 0.0f;

    [Header("Slide Settings")]
    public float slideDuration = 0.5f;
    public float slideRotationXOffset = 15f;

    [Header("Dash Settings")]
    public float dashForce = 20.0f;

    [Header("GroundChecks")]
    public Transform groundCheck;

    public float groundDistance = 0.2f;
    public LayerMask groundMask;
}