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
        //shadow的sprite组件就直接等于这一时刻player的sprite组件
        thisSprite.sprite = playerSprite.sprite;
        //这三步则是保证transform组件的一致性
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
