using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class WaterSimulation : MonoBehaviour
{
    [SerializeField]
    CustomRenderTexture texture;

    [SerializeField]
    int iterationPerFrame = 5;

    [SerializeField]
    float collisionDepth = 1f;

    [SerializeField]
    float displacementMultiplier = .5f;

    [SerializeField]
    float sizeMultiplier = .25f;

    [SerializeField]
    bool runUpdate = true;


    List<Vector3> hitList = new List<Vector3>();

    List<float> amplitudes = new List<float>();
    List<Vector4> directions = new List<Vector4>();

    List<WaterCollider> intersectingColliders = new List<WaterCollider>();
    List<Vector3> previousIntersectionPositions = new List<Vector3>();

    List<CustomRenderTextureUpdateZone[]> updateZonePacks = new List<CustomRenderTextureUpdateZone[]>();
    CustomRenderTextureUpdateZone defaultZone;

    void Start()
    {
        texture.Initialize();

        texture.material.SetVectorArray("_Array", new Vector4[1000]);

        defaultZone = new CustomRenderTextureUpdateZone();
		defaultZone.needSwap = true;
        defaultZone.passIndex = 0;
		defaultZone.rotation = 0f;
		defaultZone.updateZoneCenter = new Vector2(0.5f, 0.5f);
		defaultZone.updateZoneSize = new Vector2(1f, 1f);
    }

	void Update()
	{
		texture.ClearUpdateZones();

        DetectCollision();

		if (hitList.Count > 0)
		{
            CustomRenderTextureUpdateZone[] crtuz = GenerateZonePack(hitList);
			texture.SetUpdateZones(crtuz);
            texture.material.SetFloat("_ArrayLength", directions.Count);
            texture.material.SetVectorArray("_Array", directions);
			texture.Update(iterationPerFrame);
		}
		else
		{
			texture.SetUpdateZones(new CustomRenderTextureUpdateZone[] { defaultZone });
			if (runUpdate) texture.Update(iterationPerFrame);
		}
        
        
        hitList.Clear();
		amplitudes.Clear();
        directions.Clear();
	}

    CustomRenderTextureUpdateZone[] GenerateZonePack(List<Vector3> hits)
    {
        var updateZonePack = new CustomRenderTextureUpdateZone[hits.Count + 1];

        updateZonePack[0] = defaultZone; // needed for update to work properly

        for (int i = 0; i < hits.Count; i++)
        {
            updateZonePack[i + 1] = GenerateZone(hits[i].x, hits[i].y, hits[i].z * 1.5f);
        }

        return updateZonePack;
    }

    CustomRenderTextureUpdateZone GenerateZone(float x, float y, float size)
    {
        var updateZone = new CustomRenderTextureUpdateZone();
        updateZone.needSwap = true;
        updateZone.passIndex = 1;
        updateZone.rotation = 0f;
        updateZone.updateZoneCenter = new Vector2(x, 1f - y);
        updateZone.updateZoneSize = new Vector2(size, size);

        return updateZone;
    }

    private void AddHit(Vector3 movement, WaterCollider collider)
    {
        if (movement.sqrMagnitude * collider.multiplier < 1e-8f || hitList.Count > 500)
            return;
        
        Vector3 localHit = transform.InverseTransformPoint(collider.truePosition) - Vector3.one * 5f;
        Vector3 hitPoint = -new Vector3(localHit.x, localHit.z, -2f * sizeMultiplier * collider.radius);

		hitPoint *= .1f; // scale it by standard plane size

        hitList.Add(hitPoint);
            
        directions.Add(transform.InverseTransformDirection(movement).normalized);

        // Add the amplitude on the end of the direction vector
        float inverseDepth = (1f - Mathf.Clamp01(-transform.InverseTransformPoint(collider.truePosition).y / (collisionDepth + collider.radius)));

        directions[directions.Count - 1] += new Vector4(0, 0, 0, collider.multiplier) * displacementMultiplier * Mathf.Sign(transform.InverseTransformDirection(movement).y) * transform.InverseTransformDirection(movement).magnitude * inverseDepth;

        // Add the location and size information after the hit information
        float skimmingRadius = Mathf.Clamp(collider.radius - transform.InverseTransformPoint(collider.truePosition).y, 0f, collider.radius);
        directions.Add(new Vector4(hitPoint.x, hitPoint.y, skimmingRadius * sizeMultiplier / transform.localScale.x, 1f));
    }

    private void DetectCollision()
    {
        for (int i = 0; i < WaterColliderManager.Instance.GetNumColliders(); i++)
        {
            WaterCollider collider = WaterColliderManager.Instance.GetWaterCollider(i);
            if (transform.InverseTransformPoint(collider.truePosition).y <= collider.radius &&
                Mathf.Abs(transform.InverseTransformPoint(collider.truePosition).y) - collisionDepth < collider.radius * 2f)
            {
                AddHit(collider.velocity, collider);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.matrix = transform.localToWorldMatrix;
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(-Vector3.up * collisionDepth * .5f, new Vector3(10f, collisionDepth, 10f));
    }
}