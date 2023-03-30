using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Materials : MonoBehaviour
{
    [SerializeField] private List<Material> materials;

    Color white = Color.white;
    // Start is called before the first frame update
    void Start()
    {
        white.a = 0;
    }

    // Update is called once per frame
    void Update()
    {
    
    }

    public IEnumerator MaterialFade(float endTime)
    {
        float time = 0;

        while (time < endTime)
        {
            foreach (Material m in materials)
            {
                m.SetColor("_BaseColor", white);
            }

            time += Time.deltaTime;
            yield return null;
        } 
    }

}
