using Fusion;
using UnityEngine;

public class CarromGameManager : NetworkBehaviour
{
    [Header("Prefabs")]
    public NetworkPrefabRef strikerPrefab;

    [Header("Spawn Points")]
    public Transform spawn1; // assign in Inspector (Player 1 striker spawn)
    public Transform spawn2; // assign in Inspector (Player 2 striker spawn)

    [Networked] private int CurrentTurn { get; set; } = 1; // 1 = Player 1, 2 = Player 2
    private NetworkObject activeStriker;

    public override void Spawned()
    {
        if (Runner.IsServer)
        {
            SpawnStrikerForTurn();
        }
    }

    private void SpawnStrikerForTurn()
    {
        // Despawn old striker if exists
        if (activeStriker != null)
        {
            Runner.Despawn(activeStriker);
            activeStriker = null;
        }

        Transform spawnPoint = CurrentTurn == 1 ? spawn1 : spawn2;

        activeStriker = Runner.Spawn(strikerPrefab, spawnPoint.position, spawnPoint.rotation,
            inputAuthority: GetPlayerRef(CurrentTurn));
    }

    // Called when a player finishes their shot
    [Rpc(RpcSources.All, RpcTargets.StateAuthority)]
    public void Rpc_EndTurn()
    {
        CurrentTurn = (CurrentTurn == 1) ? 2 : 1;
        SpawnStrikerForTurn();
    }

    private PlayerRef GetPlayerRef(int playerIndex)
    {
        foreach (var player in Runner.ActivePlayers)
        {
            if (player.PlayerId == 0 && playerIndex == 1) // Host is always Player 1
                return player;
            if (player.PlayerId == 1 && playerIndex == 2) // First client is Player 2
                return player;
        }
        return default;
    }
}
