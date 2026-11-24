using System.Collections.Generic;
using _Project.Scripts.Player.State;
using UnityEngine;

public class RewindingState : PlayerState
{
    private readonly List<GameObject> _ghosts = new();
    private int _framesSinceLastGhost;
    private float _nextRewindTime;

    public bool CanRewind() => Time.time >= _nextRewindTime;

    public RewindingState(PlayerStateMachine sm) : base(sm)
    {
    }

    public override void Enter()
    {
        Player.RevolutionController.IsRecording = false;
        Player.Rb.isKinematic = true;
        _framesSinceLastGhost = Player.RevolutionController.GhostSpawnRate;
    }

    public override void Exit()
    {
        Player.Rb.isKinematic = false;
        Player.Rb.linearVelocity = Vector3.zero;
        Player.Rb.angularVelocity = Vector3.zero;

        Player.RevolutionController.Clear();
        Player.RevolutionController.IsRecording = true;

        _nextRewindTime = Time.time + Player.RevolutionController.RewindCooldown;

        DestroyAllGhosts();
    }

    public override void LogicUpdate()
    {
    }

    public override void PhysicsUpdate()
    {
        RevolutionController recorder = Player.RevolutionController;

        if (!recorder.TryPopRewindPoint(out RecordedPoint point))
        {
            // Rewind finished -> hand control back to movement
            Player.ChangeState(Player.AirborneState);
            return;
        }

        Player.Rb.MovePosition(point.Position);
        if (recorder.EnableRewindCameraOverwrite)
        {
            Player.Rb.MoveRotation(point.Rotation);
        }

        HandleGhostSpawn(recorder);
    }

    private void HandleGhostSpawn(RevolutionController recorder)
    {
        if (!recorder.EnableGhostEffect || recorder.GhostPrefab == null) return;

        if (_ghosts.Count >= recorder.GhostSpawnCount)
        {
            Object.Destroy(_ghosts[0]);
            _ghosts.RemoveAt(0);
        }

        if (_framesSinceLastGhost >= recorder.GhostSpawnRate)
        {
            Vector3 spawnPosition = new Vector3(
                Player.Rb.position.x,
                Player.Rb.position.y + recorder.GhostSpawnYOffset,
                Player.Rb.position.z
            );

            _ghosts.Add(Object.Instantiate(recorder.GhostPrefab, spawnPosition, Player.transform.rotation));
            _framesSinceLastGhost = 0;
        }

        _framesSinceLastGhost++;
    }

    private void DestroyAllGhosts()
    {
        foreach (GameObject ghost in _ghosts)
        {
            if (ghost != null) Object.Destroy(ghost);
        }

        _ghosts.Clear();
    }
}