using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class ColorChange : MonoBehaviourPun
{
    private Material deMaterial;         //默认材质
    public  float    speed       = 10f;  //渐变速度
    public  float    rotateSpeed = 360f; //旋转速度
    // Start is called before the first frame update
    void Start()
    {
        if (photonView.IsMine)
        {
            deMaterial = GetComponent<MeshRenderer>().material;
            InvokeRepeating("ChangeColor", 1, 1);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            transform.Rotate(Vector3.up * 5); //自转
        }
    }

    /// <summary>
    /// 随机颜色
    /// </summary>
    /// <returns> Color </returns>
    private Color RandomColor()
    {
        float r     = Random.Range(0f, 1f);
        float g     = Random.Range(0f, 1f);
        float b     = Random.Range(0f, 1f);
        Color color = new Color(r, g, b);
        return color;
    }


    /// <summary>
    /// 改变颜色
    /// </summary>
    private void ChangeColor()
    {
        StopAllCoroutines();
        Color temColor = RandomColor();
        StartCoroutine(ColorEnumerator(temColor));
    }


    /// <summary>
    /// 开启协程 —— 循环颜色
    /// </summary>
    /// <returns></returns>
    IEnumerator ColorEnumerator(Color temColor)
    {
        while (true) //死循环
        {
            deMaterial.color = Color.Lerp(deMaterial.color, temColor, speed * Time.deltaTime); //插值
            yield return 10;                                                                   //每次暂停10帧
        }
    }
}
