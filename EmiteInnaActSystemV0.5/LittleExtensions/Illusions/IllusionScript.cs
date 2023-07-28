using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace EmiteInnaACT
{
    [RequireComponent(typeof(Animator))]
    public class IllusionScript : MonoBehaviour
    {
        private void Awake()
        {
            MeshRenderer meshR = GetComponent<MeshRenderer>();
            Material mat = meshR.sharedMaterial;
            mat = Instantiate(mat);
            meshR.sharedMaterial = mat;
        }

    }
}
