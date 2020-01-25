using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinPickup : MonoBehaviour
{
    [SerializeField] AudioClip CoinPickupSfx;

    Collider2D myCollider;

    private void Start()
    {
        myCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (myCollider.IsTouchingLayers(LayerMask.GetMask("Player")))
        {
            AudioSource.PlayClipAtPoint(CoinPickupSfx, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}
