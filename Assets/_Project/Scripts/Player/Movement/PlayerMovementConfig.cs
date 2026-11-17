using UnityEngine;

public class PlayerMovementConfig : MonoBehaviour
{
    [System.Serializable]
    public class WalkingConfig
    {
        [SerializeField] private float walkSpeed = 15.0f;
        [SerializeField] private float idleFriction = 20.0f;
        [SerializeField] private float bleedingFriction = 3.0f;
        [SerializeField] private float turnaroundFriction = 25.0f;
    }

    [System.Serializable]
    public class AirborneConfig
    {
    }

    [System.Serializable]
    public class SlidingConfig
    {
        [SerializeField] private float slidingFriction = 0.0f;
        [SerializeField] private float slideDuration = 0.5f;
        [SerializeField] private Transform[] rotateOnSlideTransforms;
        [SerializeField] private Transform[] moveOnSlideTransforms;
        [SerializeField] private float slidePositionYOffset = -0.5f;
        [SerializeField] private float slideRotationXOffset = 15f;
    }

    [System.Serializable]
    public class DashingConfig
    {
        [SerializeField] private float dashForce = 20.0f;
    }
}