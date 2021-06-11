using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class BlockScript : MonoBehaviour
{
    [SerializeField]AudioClip breakSound;
    [SerializeField] GameObject blockVFXParticleEffect;
    [SerializeField] Sprite[] hitSprites; 
    //cache object
    Level level;
    //state object
    [SerializeField] int timesHit;
    private void Start()
    {
        CountBreakableBlocks();
    }

    private void CountBreakableBlocks()
    {
        level = FindObjectOfType<Level>();
        if (tag == "Brakeable")
        {
            level.CountBlocks();
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandleHit();
           
    }

    private void HandleHit()
    {
        if (tag == "Brakeable")
        {
            timesHit++;
            int maxHits = hitSprites.Length + 1;
            if (timesHit >= maxHits)
            {
                DestroyBlock();
            }
            else
            {
                ShowNextHitSprite();
            }
        }
        
    }

    private void ShowNextHitSprite()
    {
        int spriteIndex = timesHit - 1;
        if (hitSprites[spriteIndex] != null)
        {
            GetComponent<SpriteRenderer>().sprite = hitSprites[spriteIndex];
        }
        else {
            Debug.LogError("Block value is missing");
        }
    }

    private void DestroyBlock()
    {
        PlayBackDestroySFX();
        Destroy(gameObject);
        level.BlockDestroyed();
        TriggerSparkleVFX();
    }

    private void PlayBackDestroySFX()
    {
        FindObjectOfType<GameSession>().addToScore();
        AudioSource.PlayClipAtPoint(breakSound, Camera.main.transform.position);
    }

    private void TriggerSparkleVFX() {
        Debug.Log("vfx");
        GameObject sparkle = Instantiate(blockVFXParticleEffect,transform.position,transform.rotation);
    }

}
