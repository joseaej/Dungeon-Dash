using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float pointspercoin = 10;
    [SerializeField] private Points pointsScrip;
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            audioSource.Play();
            pointsScrip.GetPoint(pointspercoin);
            gameObject.SetActive(false);
            Destroy(gameObject, audioSource.clip.length);
        }
    }
}