using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour {
    public GameObject effect;

    private void Start() {
        GetComponent<Rigidbody>().AddRelativeForce(Vector3.forward * 1000.0f);
        Destroy(this.gameObject, 3.0f);
    }

    private void OnCollisionEnter(Collision other) {
        var contact = other.GetContact(0);
        var obj = Instantiate(effect, contact.point, Quaternion.LookRotation(-contact.normal));

        Destroy(obj, 2.0f);
        Destroy(this.gameObject);
    }
}