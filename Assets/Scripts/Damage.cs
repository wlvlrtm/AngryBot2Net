using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour {
    private Renderer[] renderers;
    private int initHp = 100;
    public int currHp = 100;

    private Animator anim;
    private CharacterController cc;

    private readonly int hashDie = Animator.StringToHash("Die");
    private readonly int hashRespawn = Animator.StringToHash("Respawn");


    private void Awake() {
        renderers = GetComponentsInChildren<Renderer>();
        anim = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();

        currHp = initHp;
    }

    private void OnCollisionEnter(Collision other) {
        if (currHp > 0 && other.collider.CompareTag("BULLET")) {
            currHp -= 20;

            if (currHp <= 0) {
                StartCoroutine(PlayerDie());
            }
        }
    }

    IEnumerator PlayerDie() {
        cc.enabled = false;
        anim.SetBool(hashRespawn, false);
        anim.SetTrigger(hashDie);

        yield return new WaitForSeconds(3.0f);

        anim.SetBool(hashRespawn, true);
        SetPlayerVisible(false);

        yield return new WaitForSeconds(1.5f);

        Transform[] points = GameObject.Find("SpawnPointGroup").GetComponentsInChildren<Transform>();
        int idx = Random.Range(1, points.Length);
        transform.position = points[idx].position;

        currHp = 100;
        SetPlayerVisible(true);
        cc.enabled = true;
    }

    public void SetPlayerVisible(bool isVisible) {
        for (int i = 0; i < renderers.Length; i++) {
            renderers[i].enabled = isVisible;
        }
    }
}