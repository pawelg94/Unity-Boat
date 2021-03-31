using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CompassIndicator : MonoBehaviour
{

    public static CompassIndicator instance { get; private set; }
    [SerializeField] Transform playerOnScene;
    [SerializeField] RectTransform playerOnCompass;
    [SerializeField] RectTransform playerOnCompassParent;
    [SerializeField] float scale = 0.1f;
    [SerializeField] bool relativeDirection;

    public List<Transform> players = new List<Transform>();
    public List<RectTransform> playersOnMap = new List<RectTransform>();
    public List<GameObject> tempsOnCompass = new List<GameObject>();

    // Start is called before the first frame update

    private void Awake()
    {
        instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        for (int i = 0; i < players.Count; i++)
        {
            AlignPosition(i);
        }

    }


    public void RegisterPlayer(PlayerManager player)
    {
        players.Add(player.transform);
        GameObject temp = Instantiate(playerOnCompass.gameObject, playerOnCompassParent);
        tempsOnCompass.Add(temp);
        temp.GetComponent<Image>().color = player.color;
        temp.transform.localScale = Vector3.one * player.scale;
        playersOnMap.Add(temp.GetComponent<RectTransform>());
    }

    void AlignPosition(int i)
    {
        var player = players[i];
        var indicator = playersOnMap[i];

        if (player != null && indicator != null)
        {
            Vector3 relativePos;
            if (relativeDirection)
                relativePos = playerOnScene.InverseTransformPoint(player.position);
            else
                relativePos = player.position - playerOnScene.position;

            indicator.anchoredPosition = new Vector2(relativePos.x, relativePos.z) * scale;
        }
    }

    public void ClearCompass()
    {
        foreach (GameObject _temp in tempsOnCompass)
        {
            Destroy(_temp);
        }
    }
}
