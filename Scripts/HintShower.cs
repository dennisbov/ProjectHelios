using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class HintShower : MonoBehaviour
{
     [TextArea][SerializeField] private string text;

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out TextController player))
        {
            player.SetText(text);
            gameObject.SetActive(false);
        }
    }
}
