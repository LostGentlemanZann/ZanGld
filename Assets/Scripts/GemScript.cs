using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GemScript : MonoBehaviour
{
    //[Tooltip("Sound upon impact")]
    //public AudioClip ImpactAudioClip;

    //private AudioSource audioSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, 1);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag.Equals("Player"))
        {
            //audioSource.PlayOneShot(ImpactAudioClip);
            
            SceneManager.LoadScene("Game2");
        }

    }
}
