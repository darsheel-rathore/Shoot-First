using System;
using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PlayerMove : MonoBehaviourPun, IPunObservable
{

    Rigidbody rb;
    [SerializeField]
    Transform projectile, projectileTransform;

    [SerializeField] private float health = 100;

    public int myNumberInList = -10;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        int actorNumber = photonView.CreatorActorNr;
        Player p = null;

        foreach (var item in PhotonNetwork.PlayerList)
        {
            if (item.ActorNumber == photonView.CreatorActorNr)
                p = item;
        }

        int myIndexPosition = Array.IndexOf(PhotonNetwork.PlayerList, p);
        myNumberInList = myIndexPosition;

        SetCanvasPlayerPos(myIndexPosition);
    }

    private void SetCanvasPlayerPos(int myIndexPosition)
    {
        if (myIndexPosition == 0)
        {
            CanvasControl.Instance.p1 = this;
        }
        else if (myIndexPosition == 1)
        {
            CanvasControl.Instance.p2 = this;
        }
        else if (myIndexPosition == 2)
        {
            CanvasControl.Instance.p3 = this;
        }
    }

    private void Update()
    {
        if (!photonView.IsMine) return;

        if (Input.GetKeyDown(KeyCode.Space))
        {
            photonView.RPC("ShootSphere", RpcTarget.All);
        }

        if (Input.GetKeyDown(KeyCode.I))
            testValue -= 1;

    }

    [PunRPC]
    private void ShootSphere()
    {
        Transform t = Instantiate(projectile, projectileTransform.position, projectileTransform.rotation);
        t.GetComponent<Proj>().SetInitiateRPC(photonView.IsMine);
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;

        float inH = Input.GetAxis("Horizontal");
        float inV = Input.GetAxis("Vertical");

        Vector3 moveVector = new Vector3(inH, 0, inV);
        moveVector.Normalize();

        rb.MovePosition(transform.position + moveVector * 0.2f);

        if (moveVector.magnitude <= 0.5f) return;

        Quaternion lookRotation = Quaternion.LookRotation(moveVector);

        transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, 5);

    }

    public void TakeSphereDamage(float damageAmount)
    {
        this.photonView.RPC("_DealDamageWithSphereRPC", RpcTarget.All, damageAmount);
    }


    [PunRPC]
    public void _DealDamageWithSphereRPC(float damageAmount)
    {
        health -= damageAmount;
        if (health <= 0) health = 0f;
    }

    public int testValue = 50;

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(this.health);
        }
        else if (stream.IsReading)
        {
            this.health = (float)stream.ReceiveNext();
        }
    }

    public float GetHealth() => health;
}
