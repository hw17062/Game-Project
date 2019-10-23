
using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
/// <summary>
/// 实现触屏旋转物体的功能，建立一个Image 设为透明的 代码绑定到Image上，并指定旋转的对象即可。
/// </summary>
public class RotateObject : MonoBehaviour,IDragHandler {


    public Transform target;
    private float speed = 0.3f;


    public void OnDrag(PointerEventData eventData)
    {
        Vector3 Vec3rote = new Vector3(eventData.delta.y,-eventData.delta.x);
        target.Rotate(Vec3rote*speed,Space.World);
        //自身轴旋转
        //Vector3 Vec3rote = new Vector3(0, -eventData.delta.x);
        //target.Rotate(Vec3rote * speed, Space.Self);
    }
}
