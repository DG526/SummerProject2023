using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Track
{
    None,
    Title,
    Level,
    Boss,
    GameOver
}
public class BGMLooper : MonoBehaviour
{
    public Track firstTrack, currentTrack;
    public AudioClip titleTheme, levelTheme, bossTheme, gameOverTheme;
    public float titleLoopPoint = 57.6f, levelLoopPoint = 72.0f, bossLoopPoint = 64.0f;
    AudioSource sourceA, sourceB;
    bool onB;
    bool firstFrame = true;
    bool secondFrame = false;
    public float loopTimer;

    // Start is called before the first frame update
    void Start()
    {
        sourceA = gameObject.AddComponent<AudioSource>();
        sourceB = gameObject.AddComponent<AudioSource>();
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
                case Track.Level:
                    sourceB.clip = levelTheme;
                    break;
                case Track.Boss:
                    sourceB.clip = bossTheme;
                    break;
                case Track.GameOver:
                    sourceB.clip = gameOverTheme;
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
                case Track.Level:
                    sourceA.clip = levelTheme;
                    break;
                case Track.Boss:
                    sourceA.clip = bossTheme;
                    break;
                case Track.GameOver:
                    sourceA.clip = gameOverTheme;
                    break;
            }
            sourceA.Play();
        }
        switch(track)
        {
            case Track.Title:
                loopTimer = titleLoopPoint;
                break;
                case Track.Level:
                loopTimer = levelLoopPoint;
                break;
                case Track.Boss:
                loopTimer = bossLoopPoint;
                break;
            case Track.GameOver:
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
