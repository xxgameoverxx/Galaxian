using UnityEngine;
using System.Collections;

public class Background : MonoBehaviour {

    //SpriteRenderer sr;
    //float worldScreenHeight;
    //float worldScreenWidth;

    Color original;

    void Awake()
    {
        original = renderer.material.color;
    }

    public void Flash(float flashTime = 0.1f)
    {
        StartCoroutine(FlashCoroutine(flashTime));
    }

    IEnumerator FlashCoroutine(float flashTime)
    {
        renderer.material.color = new Color(100, 100, 255);
        yield return new WaitForSeconds(flashTime);
        renderer.material.color = original;
    }
}
