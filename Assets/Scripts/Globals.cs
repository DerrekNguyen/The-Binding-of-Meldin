using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Globals
{
    public class BulletConfiguration
    {
        // Bullet size scale
        private float _scale;
        public event Action<float> OnScaleChanged;
        public float Scale {
            get => _scale; 
            set {
                _scale = value;
                OnScaleChanged?.Invoke(_scale);
            }
        }

        public BulletConfiguration(float scale = 0.3f)
        {
            _scale = scale;
        }
    }
}
