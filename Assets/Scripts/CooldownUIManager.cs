using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CooldownUIManager : MonoBehaviour
{
    public static CooldownUIManager instance;
    public Image cooldownLBImage;
    public Image cooldownRBImage;
    public TextMeshProUGUI textLBCD;
    public TextMeshProUGUI textRBCD;
    public float fromLBCd;
    public float toLBCd;
    public float lastLBTimeCd;
    public float fromRBCd;
    public float toRBCd;
    public float lastRBTimeCd;


    // Start is called before the first frame update
    //void Start()
    //{
    //    cooldownLBImage = GameObject.Find("CooldownLB").GetComponent<Image>();
    //    textLBCD = GameObject.Find("Text_CooldownLB").GetComponent<TextMeshProUGUI>();
    //    cooldownRBImage = GameObject.Find("CooldownRB").GetComponent<Image>();
    //    textRBCD = GameObject.Find("Text_CooldownRB").GetComponent<TextMeshProUGUI>();

    //}
    private void Awake()
    {
        instance = this;
    }


    public IEnumerator ActiveLBCooldown(int _cooldownTime)
    {
        
        if (_cooldownTime == 0)
        {
            textLBCD.text = "";
            fromLBCd = toLBCd;
            toLBCd = _cooldownTime;
            lastLBTimeCd = Time.time;
            yield break;
        }
        textLBCD.text = _cooldownTime.ToString();
        fromLBCd = toLBCd;
        toLBCd = _cooldownTime;
        lastLBTimeCd = Time.time;
        //cooldownLBImage.fillAmount = (6f -_cooldownTime)/6f;        
        yield return new WaitForSeconds(1);
        instance.StartCoroutine(instance.ActiveLBCooldown(_cooldownTime- 1));
    }

    public IEnumerator ActiveRBCooldown(int _cooldownTime)
    {
        
        if (_cooldownTime == 0)
        {
            textRBCD.text = "";
            fromRBCd = toRBCd;
            toRBCd = _cooldownTime;
            lastRBTimeCd = Time.time;
            yield break;
        }
        textRBCD.text = _cooldownTime.ToString();
        fromRBCd = toRBCd;
        toRBCd = _cooldownTime;
        lastRBTimeCd = Time.time;
        //cooldownRBImage.fillAmount = (6f - _cooldownTime) / 6f;        
        yield return new WaitForSeconds(1);
        instance.StartCoroutine(instance.ActiveRBCooldown(_cooldownTime- 1));
    }

    private void Update()
    {
        if (toLBCd != 5)
        {
            cooldownLBImage.fillAmount = Mathf.Lerp((5f - fromLBCd) / 5f, (5f - toLBCd) / 5f, (Time.time - lastLBTimeCd) / 1f);
        }
        if (toRBCd != 5)
        {
            cooldownRBImage.fillAmount = Mathf.Lerp((5f - fromRBCd) / 5f, (5f - toRBCd) / 5f, (Time.time - lastRBTimeCd) / 1f);
        }
        
    }

}
