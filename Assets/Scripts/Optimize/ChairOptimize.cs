using System;
using System.Linq;
using UnityEngine;

public class ChairOptimize : MonoBehaviour
{
    [SerializeField] private Material sharedMat;

    [SerializeField] private GameObject[] chairs;

    private void Awake()
    {
        foreach (var chair in chairs)
        {
            var renderer = chair.GetComponent<Renderer>();
            MaterialPropertyBlock block = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(block);
            Debug.Log($"[{name}] MaterialPropertyBlock empty? {block.isEmpty}");
            Debug.Log($"[{name}] SharedMaterial: {renderer.sharedMaterial.name}");
            Debug.Log($"[{name}] Is instanced material? {renderer.material != renderer.sharedMaterial}");
        }

        //var mats = chairs.Select(c => c.GetComponent<Renderer>().sharedMaterial).Distinct().ToList();
        //Debug.Log($"Material instance count: {mats.Count}");

        //foreach (Transform trans in transform)
        //{
        //    if (trans.TryGetComponent(out Renderer charRenderer))
        //    {
        //        charRenderer.sharedMaterial = sharedMat;
        //    }
        //}
    }
}
