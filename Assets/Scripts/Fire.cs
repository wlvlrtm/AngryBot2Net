using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Fire : MonoBehaviour {
    public Transform firePos;
    public GameObject bulletPrefab;

    private ParticleSystem muzzleFlash;
    private PhotonView pv;
    private bool isMouseClick => Input.GetMouseButtonDown(0);


    private void Start() {
        pv = GetComponent<PhotonView>();
        muzzleFlash = firePos.Find("MuzzleFlash").GetComponent<ParticleSystem>();
    }

    private void Update() {
        if (pv.IsMine && isMouseClick) {
            FireBullet(pv.Owner.ActorNumber);
            pv.RPC("FireBullet", RpcTarget.Others, pv.Owner.ActorNumber);
        }
    }


    [PunRPC] 
    public void FireBullet(int actorNumber) {
        if (!muzzleFlash.isPlaying) {
            muzzleFlash.Play(true);
            GameObject bullet = Instantiate(bulletPrefab, firePos.position, firePos.rotation);
            bullet.GetComponent<Bullet>().actorNumber = actorNumber;
        }
    }
}
