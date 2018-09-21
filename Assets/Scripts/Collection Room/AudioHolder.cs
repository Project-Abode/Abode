using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioHolder : Holder
{
    private const float BASE_SCALE = 0.025f;

    public GameObject recordPlayer;
    public GameObject teller;
    public GameObject colorObj;
    public GameObject textObj;

    private AudioSource audioSrc;
    private RecordPlayer rp;
    private Renderer colorRend;
    private TextMesh text3d;

    protected override void Start()
    {
        base.Start();
        audioSrc = GetComponent<AudioSource>();
        rp = recordPlayer.GetComponent<RecordPlayer>();
        colorRend = colorObj.GetComponent<Renderer>();
        text3d = textObj.GetComponent<TextMesh>();
        SetText("BASS");
    }

    public override void Apply(GameObject obj)
    {
        Record rec = obj.GetComponent<Record>();
        if (rec != null)
        {
            base.Apply(obj);
            audioSrc.clip = rec.GetAudio();
            audioSrc.Play();
            teller.SetActive(true);
            colorRend.material.color = rec.GetColor();
            SetText(rec.GetText());
            rp.recordPlayerActive = true;
        }
    }

    private void SetText(string text)
    {
        text3d.text = text;
        float scale = BASE_SCALE / text.Length;
        textObj.transform.localScale = new Vector3(scale, scale, 1);
    }

    protected override void Remove()
    {
        base.Remove();

        audioSrc.Stop();
        audioSrc.clip = null;
        rp.recordPlayerActive = false;
        teller.SetActive(false);
    }
}
