using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth;
    public int team;
    public Image healthBarImage;
    public Text usernameText;

    #region Compass settings
    public Color color;
    public float scale;
    #endregion

    public Material teamMatematerial;
    public Material enemyMaterial;


    #region Interpolation
    public float InterpolationTickRate = 0.08f;
    private Vector3 fromPos = Vector3.zero;
    private Vector3 toPos = Vector3.zero;
    private float lastTimePos;
    private Quaternion fromRot = Quaternion.identity;
    private Quaternion toRot = Quaternion.identity;
    private float lastTimeRot;
    private float distance;
    private bool packetsLost = false;
    public Vector3 oldPos = Vector3.zero;
    public Vector3 nextPos = Vector3.zero;
    public Vector3 diffPos = Vector3.zero;
    public Vector3 tempPos = Vector3.zero;
    private Quaternion oldRot = Quaternion.identity;
    private Quaternion nextRot = Quaternion.identity;
    private Quaternion difftRot = Quaternion.identity;
    private Quaternion tempRot = Quaternion.identity;
    public Vector3 localdir = Vector3.zero;
    private float lostConnectionTime;
    #endregion

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        usernameText.text = username;
        health = maxHealth;
    }

    public void SetHealth(float _health)
    {
        health = _health;
        healthBarImage.fillAmount = health / 100f;

        if (health <= 0f)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log($"I died");
    }

    public void Respawn()
    {
        SetHealth(maxHealth);
    }

    public void SetPosition(Vector3 _position)
    {
        fromPos = toPos;
        toPos = _position;
        lastTimePos = Time.time;
        diffPos = toPos - fromPos;
        var direction = toPos - fromPos;
        localdir = transform.InverseTransformDirection(direction);
    }

    public void SetRotation(Quaternion _rotation)
    {
        fromRot = toRot;
        toRot = _rotation;
        lastTimeRot = Time.time;
        difftRot = toRot * Quaternion.Inverse(fromRot);
    }

    public void SetMaterial(int _material)
    {
        if (_material == 1)
        {
            GetComponentInChildren<MeshRenderer>().material.color = Color.green;
            color = new Color(0f, 1f, 0f, 1f);
        }

        if (_material == 2)
        {
            GetComponentInChildren<MeshRenderer>().material.color = Color.red;
            color = new Color(1f, 0f, 0f, 1f);
        }
    }

    public void Extrapolate()
    {
        nextPos = oldPos + diffPos;
        nextRot = oldRot * difftRot;
        transform.position = Vector3.Lerp(oldPos, nextPos, (Time.time - lastTimePos) / InterpolationTickRate);
        transform.rotation = Quaternion.Lerp(oldRot, nextRot, (Time.time - lastTimeRot) / InterpolationTickRate);
    }
    public void RegisterToCompass()
    {
        CompassIndicator.instance.RegisterPlayer(this);
    }

    private void FixedUpdate()
    {
        if (packetsLost)
        {
            lastTimePos = Time.time;
            lastTimeRot = Time.time;
            oldPos = nextPos;
            oldRot = nextRot;
        }
    }
    private void Update()
    {
        if (!packetsLost)
        {
            transform.position = Vector3.Lerp(fromPos, toPos, (Time.time - lastTimePos) / InterpolationTickRate);
            transform.rotation = Quaternion.Lerp(fromRot, toRot, (Time.time - lastTimeRot) / InterpolationTickRate);
        }        
        if (transform.position == toPos)
        {
            packetsLost = true;
            oldPos = toPos;
            tempPos = toPos;
            oldRot = toRot;
            lostConnectionTime = Time.time;
        }
        if (tempPos != toPos)
        {
            packetsLost = false;
        }
        if (packetsLost)
        {
            Extrapolate();
        }
        if (Time.time > lostConnectionTime + 2.0)
        {
            packetsLost = false;
        }
    }

}
