using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChangeScene : MonoBehaviour
{
    Animator animator;
    public GameObject gm;
    // Indice della scena che deve essere caricato

    public int sceneIndex;
    private void Start()
    {

        animator = gm.GetComponent<Animator>();

        animator.SetTrigger("StartTransition");
        // ANIMAZIONE ENTRATA
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {

        // Controlla se l'oggetto entrante ha il tag "Player"
        if (collision.collider.CompareTag("Player"))
        {
            Scene currentScene = SceneManager.GetActiveScene(); int currentSceneIndex = currentScene.buildIndex;
            if (currentSceneIndex == 3 || currentSceneIndex == 4)
            {
                PlayerMovement playerMovement = FindAnyObjectByType<PlayerMovement>();
                if (playerMovement != null)
                {
                    List<int> lista = playerMovement.visitedScenes;
                    if (lista.Count > 0)
                    {
                        int sceneIndex = lista[lista.Count - 1];
                        Debug.Log("Ultimo indice della scena visitata: " + sceneIndex);
                    }
                    else
                    {
                        Debug.Log("La lista delle scene visitate è vuota.");
                    }
                }
                else
                {
                    Debug.Log("PlayerMovement non trovato.");
                }
            }
            Debug.Log("Player collided with the object. Loading scene index: " + sceneIndex);
            // ANIMAZIONE
            animator.SetTrigger("EndingTransition");
            StartCoroutine(WaitForAnimationToEnd());
            // QUANDO LANIMAZIONE FINISCE
        }
    }


    private IEnumerator WaitForAnimationToEnd()
    {
        // Attende che l'animazione "EndTransition" finisca
        while (animator.GetCurrentAnimatorStateInfo(0).IsName("EndTransition") &&
               animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            yield return null;
        }

        // Carica la scena una volta completata l'animazione
        SceneManager.LoadScene(sceneIndex);
    }
}
