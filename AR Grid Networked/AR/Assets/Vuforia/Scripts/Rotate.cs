using System.Collections;
using UnityEngine;


/// <summary>
/// 改变球体颜色
/// </summary>
public class Rotate : MonoBehaviour
{
    private Material deMaterial;         //默认材质
    public float speed = 10f;  //渐变速度
    public float rotateSpeed = 360f; //旋转速度


    void Start()
    {

    }


    void Update()
    {
        transform.Rotate(Vector3.left, rotateSpeed * Time.deltaTime); //自转 
    }


   
}