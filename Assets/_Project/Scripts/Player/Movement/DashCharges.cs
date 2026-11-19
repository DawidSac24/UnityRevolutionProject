using System;
using UnityEngine;

public class DashCharges : MonoBehaviour
{
    // Pass the new charge count (or expose ChargeCount as public) so UI can read it
    public static event Action<int> OnChargesChanged;

    [SerializeField] private int maxDashCharges = 3;
    [SerializeField] private int maxAirDashes = 1;
    [SerializeField] private float dashCooldownInSeconds = 3f;

    private int _chargeCount;
    private int _airDashesRemaining;
    private float _rechargeTimer;

    public int ChargeCount
    {
        get => _chargeCount;
        private set
        {
            int clamped = Mathf.Clamp(value, 0, maxDashCharges);
            if (_chargeCount == clamped) return; // Prevents redundant UI updates

            _chargeCount = clamped;
            OnChargesChanged?.Invoke(_chargeCount);
        }
    }

    private void Start()
    {
        _airDashesRemaining = maxAirDashes;
        ChargeCount = maxDashCharges;
    }

    public void TickRecharge(float deltaTime)
    {
        if (ChargeCount >= maxDashCharges)
        {
            _rechargeTimer = 0f;
            return;
        }

        _rechargeTimer += deltaTime;
        if (_rechargeTimer >= dashCooldownInSeconds)
        {
            _rechargeTimer = 0f;
            ChargeCount++;
        }
    }

    public bool TryGroundDash()
    {
        if (!CanDash()) return false;

        ChargeCount--;
        return true;
    }

    public bool TryAirDash()
    {
        if (!CanDash() || _airDashesRemaining <= 0) return false;

        ChargeCount--;
        _airDashesRemaining--;
        return true;
    }

    public void ClearAirDash()
    {
        _airDashesRemaining = maxAirDashes;
        ChargeCount++;
    }

    private bool CanDash() => ChargeCount > 0;
}