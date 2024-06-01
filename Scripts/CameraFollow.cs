using UnityEngine;

[System.Serializable]       //序列化标签，是c#中的特性(attribute)
public struct MapBound      //结构体声明地图边界
{
    public float xMin;
    public float yMin;
    public float xMax;
    public float yMax;
}
public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private float smoothTime = 0.1f;
    private float _offsetZ;     //z轴偏移值
    private Vector3 _currentVelocity;
    public MapBound mapBound;
    private void Start()
    {
        if (target == null)
        {
            Debug.Log("Can't find the player, please check it!");
            return;
        }
        _offsetZ = (transform.position - target.position).z;
        //最重要的一点：_offsetZ 的值通过 (transform.position - target.position).z 计算得到，也就是摄像头的初始位置减去目标对象的初始位置的Z轴部分，表示摄像头与目标对象在Z轴上的初始偏移量。
        //我们的初始偏移量就是摄像头在target的正上方，所以只要这个_offsetZ发生变化，在下面的FixUpdate中都会被修正
    }
    public void FixedUpdate()
    {
        if (target == null) return;
        Vector3 targetPosition = target.position + Vector3.forward * _offsetZ;
        Vector3 newPosition = Vector3.SmoothDamp(transform.position, targetPosition, ref _currentVelocity, smoothTime);
        //newPosition.x = Mathf.Clamp(newPosition.x, mapBound.xMin, mapBound.xMax);
        //newPosition.y = Mathf.Clamp(newPosition.y, mapBound.yMin, mapBound.yMax);
        //这里就是对摄像机位置进行修正的，通过向量计算出准确坐标，
        transform.position = newPosition;
    }

}
