using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletManager : MonoBehaviour
{
    public int id;
    public GameObject explosionPrefab;

    #region Interpolation
    public float InterpolationTickRate = 0.08f;
    private Vector3 fromPos = Vector3.zero;
    private Vector3 toPos = Vector3.zero;
    private float lastTimePos;
    private Quaternion fromRot = Quaternion.identity;
    private Quaternion toRot = Quaternion.identity;
    private float lastTimeRot;
    #endregion
    public Quaternion explosionRotation;

    public void Initialize(int _id)
    {
        id = _id;
    }

    private void Start()
    {
        StartCoroutine(DisappearAfterTime());
    }
    public void Explode(Vector3 _position)
    {
        transform.position = _position;
        explosionRotation = Random.rotation;
        //Instantiate(explosionPrefab, transform.position, Quaternion.identity);
        Instantiate(explosionPrefab, transform.position, explosionRotation);
        GameManager.bullets.Remove(id);
        Destroy(gameObject);
    }

    public void Explode()
    {
       
        GameManager.bullets.Remove(id);
        Destroy(gameObject);
    }
    private IEnumerator DisappearAfterTime()
    {
        yield return new WaitForSeconds(6f);

        Explode();
    }

    public void SetPosition(Vector3 _position)
    {
        fromPos = toPos;
        toPos = _position;
        lastTimePos = Time.time;
    }

    private void Update()
    {
        transform.position = Vector3.Lerp(fromPos, toPos, (Time.time - lastTimePos) / InterpolationTickRate);

    }

}

