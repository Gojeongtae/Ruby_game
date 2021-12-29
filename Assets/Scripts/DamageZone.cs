using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageZone : MonoBehaviour
{
    void OnTriggerStay2D(Collider2D collision)
    {
        PubyController controller = collision.GetComponent<PubyController>();

        if (controller != null)
        {
            controller.ChangeHealth(-1);
        }
    }
}
