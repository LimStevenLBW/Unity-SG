using UnityEngine;

/*
 * Responsible for the features of a single chunk.
 */
public class HexFeatureManager : MonoBehaviour
{
    //Referebce to prefab
    public Transform featurePrefab;

    Transform container;
    public void Clear() {
        if (container)
        {
            Destroy(container.gameObject);
        }
        container = new GameObject("Features Container").transform;
        container.SetParent(transform, false);
    }

    public void Apply() { }

    public void AddFeature(Vector3 position) {
        Transform instance = Instantiate(featurePrefab);
        position.y += instance.localScale.y * 0.5f;
        instance.localPosition = HexMetrics.Perturb(position);
        instance.SetParent(container, false);
    }

}