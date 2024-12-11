using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrack : MonoBehaviour
{
    
    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private static float SpeedMultiplierToPitchSemitones(float speed)
    {
        return 12f * Mathf.Log(speed, 2f);
    }

    private static float PitchSemitonesToSpeedMultiplier(float pitch)
    {
        return Mathf.Pow(2f, pitch / 12f);
    }

    private void SetSpeed(float speedMultiplier)
    {
        /*float pitchSemitones = SpeedMultiplierToPitchSemitones(speedMultiplier);
        float pitchShift = 1f / speedMultiplier;
        Debug.Log($"speedMultiplier: {speedMultiplier}, pitchSemitones: {pitchSemitones}, pitchShift: {pitchShift}");
        // change the tempo
        m_MusicInstance.setPitch(speedMultiplier);
        // compensate for pitch change
        m_MusicInstance.setParameterByName("PitchShift", pitchShift);*/
    }
}
