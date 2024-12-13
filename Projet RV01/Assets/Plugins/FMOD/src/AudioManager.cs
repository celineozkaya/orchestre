using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using System;

namespace CustomAudio
{
    public class AudioManager : MonoBehaviour
    {

        // Converts a speed multiplier to a pitch shift in semitones
        private static float SpeedMultiplierToPitchSemitones(float speed)
        {
            return 12f * Mathf.Log(speed, 2f);
        }

        // Converts a pitch shift in semitones to a speed multiplier
        private static float PitchSemitonesToSpeedMultiplier(float pitch)
        {
            return Mathf.Pow(2f, pitch / 12f);
        }

        [SerializeField]
        [Range(0.5f, 2f)]
        private float speed = 1f;

        private float lastSpeed = 1f;

        private const float base_tempo = 88;

        private List<FMODUnity.StudioEventEmitter> emitters;
        private int index = 0;

        // Start is called before the first frame update
        void Start()
        {
            emitters = new List<FMODUnity.StudioEventEmitter>();
            // For each GameObject with a StudioEventEmitter component
            foreach (var emitter in FindObjectsOfType<StudioEventEmitter>())
            {
                emitters.Add(emitter);
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (speed != lastSpeed)
            {
                lastSpeed = speed;
                // change the tempo
                foreach (var emitter in emitters)
                {
                    emitter.setSpeed(speed);
                }
            }
        }

        // Change the tempo of the music
        public void setTempo(float tempo)
        {
            speed = tempo / base_tempo;
        }
    }
}

