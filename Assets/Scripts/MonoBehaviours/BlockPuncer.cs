﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockPuncer : MonoBehaviour
{
    public WorldManager worldManager;
    public int GroundLayer;
    private int groundLayerMask;
    // Start is called before the first frame update
    void Start()
    {
        groundLayerMask = 1 << GroundLayer;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3Int dest = GetRayDest();
            worldManager.world.setBlockAndMesh(dest, BlockType.empty);
            //MeshGenerator.remeshChunk(worldManager.world, worldManager.world.loadedChunks[worldManager.world.WorldToChunkCoords(dest)]);
        }
        if (Input.GetMouseButtonDown(1))
        {
            Vector3Int dest = GetRayDest() + Vector3Int.up;
            BlockData dat = worldManager.world.getBlock(dest);
            if (dat.interactable)
            {
                dat.interact(dest, worldManager.world.WorldToChunkCoords(dest), worldManager.world.loadedChunks[worldManager.world.WorldToChunkCoords(dest)], worldManager.world);
            }
            else
            {
                worldManager.world.setBlockAndMesh(dest, BlockType.tnt);
            }

        }
    }

    public Vector3Int GetRayDest()
    {
        RaycastHit hit;
        Physics.Raycast(transform.position, transform.forward, out hit, 10, groundLayerMask);
        Vector3 blockPoint = hit.point + new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 roundedBlockPoint = new Vector3(Mathf.Round(blockPoint.x), Mathf.Round(blockPoint.y), Mathf.Round(blockPoint.z));
        int hitx = (int)hit.point.x;
        int hity = (int)hit.point.y;
        int hitz = (int)hit.point.z;
        if (transform.position.y < hit.point.y && Mathf.Abs(blockPoint.y - roundedBlockPoint.y) < 0.01f)
        {
            //hitting the y face of the block from the -y direction
            hity += 1;
        }
        if (transform.position.x < hit.point.x && Mathf.Abs(blockPoint.x - roundedBlockPoint.x) < 0.01f)
        {
            //hitting the x face of the block from the -x direction
            hitx += 1;
        }
        if (transform.position.z < hit.point.z && Mathf.Abs(blockPoint.z - roundedBlockPoint.z) < 0.01f)
        {
            //hitting the z face of the block from the -z direction.
            hitz += 1;
        }
        return new Vector3Int(hitx, hity, hitz);
    }
}
