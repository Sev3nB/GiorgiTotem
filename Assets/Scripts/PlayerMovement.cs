using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    public List<int> visitedScenes = new List<int>();

    public float speed = 5f;
    private Vector2 movement;

    private Rigidbody2D rb;
    public Animator animator;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Ottieni l'indice della scena corrente
        int currentSceneIndex = scene.buildIndex;

        // Aggiungi l'indice della scena alla lista, evitando duplicati consecutivi
        if (visitedScenes.Count == 0 || visitedScenes[visitedScenes.Count - 1] != currentSceneIndex)
        {
            visitedScenes.Add(currentSceneIndex);
        }
    }
    public List<int> GetVisitedSceneIndices()
    {
        return new List<int>(visitedScenes);
    }

    void Update()
    {
        // Limit movement to horizontal and vertical only (no diagonal)
        float moveX = SimpleInput.GetAxis("Horizontal");
        float moveY = SimpleInput.GetAxis("Vertical");

        if (Mathf.Abs(moveX) > 0)
        {
            movement.x = moveX * speed;
            movement.y = 0; // Ensure only horizontal movement
        }
        else if (Mathf.Abs(moveY) > 0)
        {
            movement.y = moveY * speed;
            movement.x = 0; // Ensure only vertical movement
        }
        else
        {
            movement.x = 0;
            movement.y = 0;
        }

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("Speed", movement.sqrMagnitude);
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
    }
}
