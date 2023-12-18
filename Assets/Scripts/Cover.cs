using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cover : MonoBehaviour
{
    [SerializeField] private int coverLife;
    private void OnTriggerEnter2D(Collider2D other) {
        coverLife--;
        if(coverLife == 0) this.gameObject.SetActive(false);
    }
}
