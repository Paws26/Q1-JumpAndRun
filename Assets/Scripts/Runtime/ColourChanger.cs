using UnityEngine;

public class ColourChanger : MonoBehaviour
{
    [SerializeField] private Color colour;
    private MaterialPropertyBlock mpb;

    private MeshRenderer mr;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        this.mpb = new MaterialPropertyBlock();
        this.mr = this.GetComponent<MeshRenderer>();
        this.mr.SetPropertyBlock(this.mpb);
    }

    // Update is called once per frame
    void Update()
    {
        this.mpb.SetColor("_BaseColor", this.colour);
        this.mr.SetPropertyBlock(this.mpb);
    }
}
