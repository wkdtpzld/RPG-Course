using System.Collections;
using UnityEngine;

public class EntityFX : MonoBehaviour
{
    private SpriteRenderer sr;

    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration;
    private Material originalMat;
    private bool isBlinking = false;

    private void Start()
    {
        sr = GetComponentInChildren<SpriteRenderer>();

        originalMat = sr.material;
    }

    public IEnumerator FlashFX()
    {
        sr.material = hitMat;

        yield return new WaitForSeconds(flashDuration);

        sr.material = originalMat;
    }

    public IEnumerator RedColorBlink()
    {
        isBlinking = true;
        while (isBlinking)
        {
            if (sr.color != Color.white)
            {
                sr.color = Color.white;
            }
            else
            {
                sr.color = Color.red;
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void StopBlink()
    {
        isBlinking = false;
        sr.color = Color.white;
        sr.material = originalMat;
    }
}
