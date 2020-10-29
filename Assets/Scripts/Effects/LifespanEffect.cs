﻿using UnityEngine;

public class LifespanEffect : MonoBehaviour
{
    [SerializeField]
    private float lifespan = 3f;

    private void Update()
    {
        lifespan -= Time.deltaTime;

        if (lifespan <= 0f)
        {
            Destroy(gameObject);
        }
    }
}