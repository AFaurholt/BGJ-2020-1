using UnityEngine;

public class WormholePositionProvider : PositionProvider
{
    [SerializeField] private InfiniteWormholeGenerator wormhole;
    [SerializeField] private float distance = 1000;
    [SerializeField] private float spawnRadius = 10;

    public override Vector3 Get()
    {
        var ring = wormhole.FindNext(distance);
        ring.DrawFor(5f);

        Vector3 offset = Random.insideUnitSphere * spawnRadius;

        return ring.GlobalPos + offset;
    }
}