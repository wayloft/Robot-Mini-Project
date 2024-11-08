using UnityEngine;
using UnityEngine.SceneManagement;

public class ToyReloader : MonoBehaviour
{
    [SerializeField] GameObject toyPrefab;
    private GameObject currentToy;

    private void Awake()
    {
        if(toyPrefab == null)
        {
            Debug.LogWarning($"You have no Toy Prefab Set in the {this}");
            return;
        }

        CreateToy();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            if (currentToy != null)
                Destroy(currentToy);

            CreateToy();
        }
    }

    void CreateToy()
    {
        currentToy = Instantiate(toyPrefab, Vector3.zero, Quaternion.identity);
    }
}
