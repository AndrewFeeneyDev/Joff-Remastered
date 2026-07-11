using UnityEngine;

public class ClickParticles : MonoBehaviour
{
    [SerializeField] private ParticleSystem clickEffect;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private float clickEffectInterval;

    private float nextEffectTime;

    private void Update()
    {
        if (Input.GetMouseButton(0) && Time.time >= nextEffectTime)
        {
            nextEffectTime = Time.time + clickEffectInterval;

            Vector3 pos = Input.mousePosition;
            pos.z = 10f;

            Vector3 worldPos = mainCamera.ScreenToWorldPoint(pos);

            ParticleSystem effect = Instantiate(clickEffect, worldPos, Quaternion.identity);

            effect.Play();

            Destroy(effect.gameObject, effect.main.duration + effect.main.startLifetime.constantMax);
        }
    }
}
