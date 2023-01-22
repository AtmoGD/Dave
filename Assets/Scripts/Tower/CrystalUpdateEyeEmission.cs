using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalUpdateEyeEmission : MonoBehaviour
{
    [SerializeField] private AnimationCurve eyeEmissionCurve = null;
    [SerializeField] private List<ParticleSystem> eyeParticles = new List<ParticleSystem>();

    private void Update()
    {

        float emissionAmount = 0f;

        foreach (CollectedRessource CollectedRessource in LevelManager.Instance.GatheredRessources)
            emissionAmount += CollectedRessource.amount;

        emissionAmount = eyeEmissionCurve.Evaluate(emissionAmount);

        foreach (ParticleSystem particle in eyeParticles)
        {
            var emission = particle.emission;
            emission.rateOverTime = emissionAmount;
        }
    }
}