using UnityEngine;

public class CamaraController : MonoBehaviour
{
    [SerializeField] Transform player;

    void Update()
    {
        transform.position = new Vector3(player.position.x, player.position.y, -3f);
    }
}
