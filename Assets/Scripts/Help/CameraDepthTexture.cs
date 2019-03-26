/* Скрипт, отвечающий за включение у кмеры                      */
/* свойство DepthTextureMode                                    */
/* алгоритма Depth-first Seach                                  */
/* Создан Максимом WhiteCoyote 30.05.2018                       */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class CameraDepthTexture : MonoBehaviour {

    private Camera cam;

	// Use this for initialization
	void Start () 
    {
        cam = GetComponent<Camera>();
        cam.depthTextureMode = DepthTextureMode.Depth;
	}
}
