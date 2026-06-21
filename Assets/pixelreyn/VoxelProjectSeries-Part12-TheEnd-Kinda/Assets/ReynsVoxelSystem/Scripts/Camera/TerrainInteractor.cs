using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class TerrainInteractor : MonoBehaviour
{
    public bool ReplaceBlockInPlace = false;
    public ToolMode toolMode;
    public ToolType toolType;
    public int radiusToAffect = 2;
    public byte voxelIDToPlace = 4;

    public bool dig_state;
    public int range;
    public List<Transform> dig_points;
    public Transform current_dig_point;
    public float delay_between_actions;


    public Animator animator;
    public AudioSource  audioSource;
    public AudioClip[] clips;

    private void Start()
    {
        StartCoroutine(dig_loop());
    }

    void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            ReplaceBlockInPlace = !ReplaceBlockInPlace;
        }
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            int indexOf = (int)toolMode;
            toolMode = indexOf == 1 ? ToolMode.Single : ToolMode.Continuous;
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            toolMode = ToolMode.Continuous;
            toolType = ToolType.SingleBlock;
            voxelIDToPlace = 240;
        }*/

        /*if (toolMode == ToolMode.Single ? Input.GetMouseButtonDown(1) : Input.GetMouseButton(1))
        {
            //Spawn water as demo
            if (GetBlockCoordAtRay(out Vector3 chunkPos, out Vector3 blockPos))
            {
                if (blockPos.y < 0)
                    return;

                byte v = (byte)(voxelIDToPlace == 240 ? 100 : 0);
                if (toolType == ToolType.SingleBlock)
                {
                    World.Instance.SetVoxelAtCoord(chunkPos, math.round(blockPos),
                        new Voxel() { ID = voxelIDToPlace, ActiveValue = v });
                }
                else
                {
                    for (int x = -radiusToAffect; x < radiusToAffect; x++)
                        for (int y = -radiusToAffect; y < radiusToAffect; y++)
                            for (int z = -radiusToAffect; z < radiusToAffect; z++)
                            {
                                Vector3 modPos = math.round(blockPos + new Vector3(x, y, z));
                                if ((modPos.y < 0 && voxelIDToPlace != 0) || (modPos.y < 1 && voxelIDToPlace == 0))
                                    continue;

                                World.Instance.SetVoxelAtCoord(chunkPos, modPos, new Voxel() { ID = voxelIDToPlace, ActiveValue = v });
                            }
                }
            }

        }*/
    }

    
    

    public IEnumerator dig_loop()
    {
        while (this.enabled == true)
        {
            if (dig_state == true)
            {
                foreach( Transform digpoint in dig_points)
                {
                    current_dig_point = digpoint;
                    dig();
                }

            }

            yield return new WaitForSeconds(delay_between_actions);
        }

        if (dig_state == false)
        {
            yield break;
        }
    }

    public void dig_pulse()
    {

        foreach (Transform digpoint in dig_points)
        {
            current_dig_point = digpoint;
            dig();
        }

    }

    public void dig()
    {
        //Debug.Log("trying to dig");
        
        //if (toolMode == ToolMode.Single)// ? Input.GetMouseButtonDown(1) : Input.GetMouseButton(1))
        //{
            //Spawn water as demo
            if (GetBlockCoordAtRay(out Vector3 chunkPos, out Vector3 blockPos))
            {
                if (blockPos.y < 0)
                    return;

                byte v = (byte)(voxelIDToPlace == 240 ? 100 : 0);
                if (toolType == ToolType.SingleBlock)
                {
                    World.Instance.SetVoxelAtCoord(chunkPos, math.round(blockPos),
                        new Voxel() { ID = voxelIDToPlace, ActiveValue = v });
                }
                else
                {
                    for (int x = -radiusToAffect; x < radiusToAffect; x++)
                    for (int y = -radiusToAffect; y < radiusToAffect; y++)
                    for (int z = -radiusToAffect; z < radiusToAffect; z++)
                    {
                        Vector3 modPos = math.round(blockPos + new Vector3(x, y, z));
                        if ((modPos.y < 0 && voxelIDToPlace != 0) || (modPos.y < 1 && voxelIDToPlace == 0))
                            continue;

                        World.Instance.SetVoxelAtCoord(chunkPos, modPos, new Voxel() { ID = voxelIDToPlace, ActiveValue = v });
                    }
                }
            }

        //}
        
    }

    bool GetBlockCoordAtRay(out Vector3 ChunkPos, out Vector3 blockPos)
    {
        if (Physics.Raycast(new Ray(current_dig_point.position, current_dig_point.forward), out var hitInfo))
        {
            if (hitInfo.distance <= range)
            {
                if (hitInfo.collider.transform.GetComponent<Chunk>() != null)
                {
                    ChunkPos = hitInfo.collider.transform.GetComponent<Chunk>().chunkPosition;
                    blockPos = math.floor(hitInfo.point - ChunkPos);
                    if (hitInfo.normal.x != 0)
                    {
                        blockPos.x = math.round(blockPos.x);
                    }
                    else
                    {
                        blockPos.x = math.floor(blockPos.x);
                    }

                    if (hitInfo.normal.y != 0)
                    {
                        blockPos.y = math.round(blockPos.y);
                    }
                    else
                    {
                        blockPos.y = math.floor(blockPos.y);
                    }

                    if (hitInfo.normal.z != 0)
                    {
                        blockPos.z = math.round(blockPos.z);
                    }
                    else
                    {
                        blockPos.z = math.floor(blockPos.z);
                    }

                    if (ReplaceBlockInPlace)
                    {
                        if (hitInfo.normal.x > 0 || hitInfo.normal.y > 0 || hitInfo.normal.z > 0)
                            blockPos -= hitInfo.normal;
                    }
                    else
                    {

                        if (hitInfo.normal.x < 0 || hitInfo.normal.y < 0 || hitInfo.normal.z < 0)
                            blockPos += hitInfo.normal;
                    }

                    return true;
                }
            }
        }

        ChunkPos = Vector3.zero;
        blockPos = Vector3.zero;
        return false;
    }

    public enum ToolMode
    {
        Single,
        Continuous
    }
    public enum ToolType
    {
        SingleBlock,
        Radius
    }

    public void OnDrawGizmos()
    {
        Gizmos.DrawRay(transform.position, transform.forward);
    }

    public void play_shovel_dig()
    {
        if (this.gameObject.activeSelf == true)
        {
            animator.Play("dig_trowel");
            sound_shovel_dig();
        }
    }

    public void sound_shovel_dig()
    {
        PlayRandomClip(audioSource,clips);
    }
    
    //this code was copied from the FirstPersonAudio script
    public void PlayRandomClip(AudioSource audio, AudioClip[] clips)
    {
        if (!audio || clips.Length <= 0)
            return;

        // Get a random clip. If possible, make sure that it's not the same as the clip that is already on the audiosource.
        AudioClip clip = clips[Random.Range(0, clips.Length)];
        if (clips.Length > 1)
            while (clip == audio.clip)
                clip = clips[Random.Range(0, clips.Length)];

        // Play the clip.
        audio.clip = clip;
        audio.Play();
        Debug.Log(clip.name);
    }
}