using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] GameObject characterModel;
    bool isDead = false;
    [SerializeField] AudioSource dieSound;

    private void Update()
    {
        if (transform.position.y < -1f && !isDead)
        {
            Die();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy Body"))
        {
            characterModel.SetActive(false);
            GetComponent<Rigidbody>().isKinematic = true;
            GetComponent<PlayerMovement>().enabled = false;
            Die();
        }

    }

    void Die()
    {
        Invoke(nameof(ReloadLevel), 1.3f);
        isDead = true;
        dieSound.Play();
    }

    void ReloadLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
