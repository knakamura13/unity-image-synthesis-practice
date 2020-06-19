using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController: MonoBehaviour {
    public ImageSynthesis synth;
    public GameObject[] prefabs;
    public int maxObjects;

    private GameObject[] created;

    /********************
     * Built in methods *
     ********************/

    // Start is called before the first frame update
    void Start () {
        created = new GameObject[maxObjects];
    }

    // OnRenderObject is called after all regular scene rendering is done
    void OnRenderObject() {
        GenerateRandomShapes();
        synth.OnSceneChange();
    }

    /******************
     * Custom methods *
     ******************/

    void GenerateRandomShapes() {
        // Destroy existing objects before creating new ones
        foreach (GameObject obj in created) {
            if (obj != null) {
                Destroy(obj);
            }
        }

        // Create objects with random properties
        for (int i = 0; i < maxObjects; i++) {
            // Pick a random prefab (shape)
            int prefabIndex = Random.Range(0, prefabs.Length);
            GameObject prefab = prefabs[prefabIndex];

            // Set position
            float newX, newY, newZ;
            newX = Random.Range(-10.0f, 10.0f);
            newY = Random.Range(2.0f, 10.0f);
            newZ = Random.Range(-10.0f, 10.0f);
            Vector3 newPos = new Vector3(newX, newY, newZ);

            // Set rotation
            Quaternion newRot = Random.rotation;

            // Place the prefab in the scene
            GameObject newObj = Instantiate(prefab, newPos, newRot);
            created[i] = newObj;

            // Set scale
            float s = Random.Range(0.5f, 4.0f);
            Vector3 newScale = new Vector3(s, s, s);
            newObj.transform.localScale = newScale;

            // Set color
            float newR, newG, newB;
            newR = Random.Range(0.0f, 1.0f);
            newG = Random.Range(0.0f, 1.0f);
            newB = Random.Range(0.0f, 1.0f);
            Color newColor = new Color(newR, newG, newB);
            newObj.GetComponent<Renderer>().material.color = newColor;
        }
    }
}