using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Track
{
    None,
    Title,
    Shop,
    Level,
    Boss,
    FinalBoss,
    GameOver,
    Victory
}
public class BGMLooper : MonoBehaviour
{
    public Track firstTrack, currentTrack;
    public AudioClip titleTheme, levelTheme, shopTheme, bossTheme, finalBossTheme, gameOverTheme, victoryTheme;
    public float titleLoopPoint = 57.6f, shopLoopPoint = 36, levelLoopPoint = 72.0f, bossLoopPoint = 64.0f, finalBossLoopPoint = 76.8f;
    AudioSource sourceA, sourceB;
    bool onB;
    bool firstFrame = true;
    bool secondFrame = false;
    public float loopTimer;

    // Start is called before the first frame update
    void Start()
    {
        sourceA = gameObject.GetComponents<AudioSource>()[0];
        //sourceA.volume = 0.3f;
        sourceB = gameObject.GetComponents<AudioSource>()[1];
        //sourceB.volume = 0.3f;
        PlayTrack(firstTrack);
    }

    // Update is called once per frame
    void Update()
    {
        //if(countInFrames == 0) { 
        //    PlayTrack(firstTrack);
        //    loopTimer += Time.timeSinceLevelLoad;
        //    countInFrames = -1;
        //    return;
        //}

        //bool reinitted = false;
        //if (firstFrame)
        //{
        //    firstFrame = false;
        //    secondFrame = true;
        //}
        //else if(secondFrame)
        //{
        //    switch (currentTrack)
        //    {
        //        case Track.Title:
        //            loopTimer = titleLoopPoint;
        //            break;
        //        case Track.Level:
        //            loopTimer = levelLoopPoint;
        //            break;
        //        case Track.Boss:
        //            loopTimer = bossLoopPoint;
        //            break;
        //        case Track.GameOver:
        //        case Track.None:
        //            loopTimer = -1;
        //            break;
        //    }
        //    secondFrame = false;
        //    reinitted= true;
        //}
        if (loopTimer == -1) return;
        if (loopTimer < (!onB?sourceA.time:sourceB.time))
        {
            onB = !onB;
            PlayTrack(currentTrack);
        }
        //else if (!reinitted && (sourceA.isPlaying || sourceB.isPlaying))
        //{
        //    loopTimer -= Time.unscaledDeltaTime;
        //}

    }
    public void StopTrack()
    {
        sourceA.Stop();
        sourceB.Stop();
        onB = false;
    }
    public void PlayTrack(Track track)
    {
        currentTrack = track;
        if(onB)
        {
            switch (track)
            {
                case Track.Title:
                    sourceB.clip = titleTheme;
                    break;
                case Track.Shop:
                    sourceB.clip = shopTheme;
                    break;
                case Track.Level:
                    sourceB.clip = levelTheme;
                    break;
                case Track.Boss:
                    sourceB.clip = bossTheme;
                    break;
                case Track.FinalBoss:
                    sourceB.clip = finalBossTheme;
                    break;
                case Track.GameOver:
                    sourceB.clip = gameOverTheme;
                    break;
                case Track.Victory:
                    sourceB.clip = victoryTheme;
                    break;
            }
            sourceB.Play();
        }
        else
        {
            switch (track)
            {
                case Track.Title:
                    sourceA.clip = titleTheme;
                    break;
                case Track.Shop:
                    sourceA.clip = shopTheme;
                    break;
                case Track.Level:
                    sourceA.clip = levelTheme;
                    break;
                case Track.Boss:
                    sourceA.clip = bossTheme;
                    break;
                case Track.FinalBoss:
                    sourceA.clip = finalBossTheme;
                    break;
                case Track.GameOver:
                    sourceA.clip = gameOverTheme;
                    break;
                case Track.Victory:
                    sourceA.clip = victoryTheme;
                    break;
            }
            sourceA.Play();
        }
        switch(track)
        {
            case Track.Title:
                loopTimer = titleLoopPoint;
                break;
                case Track.Shop:
                loopTimer = shopLoopPoint;
                break;
                case Track.Level:
                loopTimer = levelLoopPoint;
                break;
                case Track.Boss:
                loopTimer = bossLoopPoint;
                break;
                case Track.FinalBoss:
                loopTimer = finalBossLoopPoint;
                break;
            case Track.GameOver:
            case Track.Victory:
            case Track.None:
                loopTimer = -1;
                break;
        }
    }
    public void FadeOut(float time)
    {

    }
    public void FadeIn(float time)
    {

    }
}
