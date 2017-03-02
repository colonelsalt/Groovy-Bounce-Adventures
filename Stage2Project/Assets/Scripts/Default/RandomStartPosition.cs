using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomStartPosition : MonoBehaviour
{
	void Start()
    {
        float hw = (Arena.Width - (VerticalWall.Width * 2)) * 0.5f;
        float hh = (Arena.Height - (HorizontalWall.Height * 2)) * 0.5f;
        float x = Random.Range( -hw, hw );
        float z = Random.Range(-hh, hh);
        transform.position = new Vector3(x, 3f, z);
    }
}
