using UnityEngine;
using bronya;

public class ShadowSprite : MonoBehaviour
{
    private Transform player;
    private SpriteRenderer thisSprite;
    private SpriteRenderer playerSprite;
    private Color color;
    public float activeTime;
    public float activeStart;
    private float alpha;
    public float alphaSet;
    public float alphaMultiplier;   //smoothly change the value

    private void OnEnable()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thisSprite = GetComponent<SpriteRenderer>();
        playerSprite = player.GetComponent<SpriteRenderer>();
        alpha = alphaSet;
        //shadow��sprite�����ֱ�ӵ�����һʱ��player��sprite���
        thisSprite.sprite = playerSprite.sprite;
        //���������Ǳ�֤transform�����һ����
        transform.position = player.position;

        transform.localScale = player.localScale;
        transform.rotation = player.rotation;
        activeStart = Time.time;
    }

    private void Update()
    {
        alpha -= alphaMultiplier;       //a value which is always decreased
        color = new Color(0.5f, 0.5f, 1, alpha);
        thisSprite.color = color;
        if(Time.time>activeStart + activeTime)
        {
            //2
            ObjectPool.Instance.PushObject(gameObject);
        }
    }
}
