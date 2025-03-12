using UnityEngine;

public class CamaraController : MonoBehaviour
{

    [SerializeField] Transform player;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, new Vector3(player.position.x, player.position.y, -5f), 10f * Time.deltaTime);
    }
}
