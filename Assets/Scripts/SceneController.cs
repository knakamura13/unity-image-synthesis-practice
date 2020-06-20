using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneController: MonoBehaviour {
    /*********************
     * Public properties *
     *********************/

    public ImageSynthesis synth;
    public GameObject[] prefabs;
    public int minObjects;
    public int maxObjects;
    public int numTrainingImages;
    public int numValidationImages;


    /*********************
     * Private properties *
     *********************/

    private ShapePool pool;
    private int frameCount = 0;
    private int savedImagesCount = 0;


    /********************
     * Built-in methods *
     ********************/

    void Start () {
        pool = ShapePool.Create(prefabs);
    }

    void Update() {
        if (savedImagesCount < numTrainingImages + numValidationImages) {
            frameCount++;

            // Only generate shapes every 30 frames (1–2 times per second)
            if (frameCount % 30 == 0 ) {
                GenerateRandomShapes();
                Debug.Log($"Frame #: {frameCount}");

                // Only save 6 images per iteration
                if (frameCount % 5 == 0) {
                    if (savedImagesCount < numTrainingImages) {
                        SaveImages(savedImagesCount, "train");
                    } else if (savedImagesCount < numTrainingImages + numValidationImages) {
                        int valFrameCount = savedImagesCount - numTrainingImages;
                        SaveImages(valFrameCount, "validate");
                    }
                }
            }
        } else {
            // Program has finished
        }
    }


    /******************
     * Custom methods *
     ******************/

    void GenerateRandomShapes() {
        // Destroy existing objects before creating new ones
        pool.ReclaimAll();

        int objectsThisTime = Random.Range(minObjects, maxObjects);

        // Create objects with random properties
        for (int i = 0; i < objectsThisTime; i++) {
            // Pick a random prefab
            int prefabIndex = Random.Range(0, prefabs.Length);

            // Set position
            float newX, newY, newZ;
            newX = Random.Range(-10.0f, 10.0f);
            newY = Random.Range(2.0f, 10.0f);
            newZ = Random.Range(-10.0f, 10.0f);
            Vector3 newPos = new Vector3(newX, newY, newZ);

            // Set rotation
            Quaternion newRot = Random.rotation;

            // Set color
            float newR, newG, newB;
            newR = Random.Range(0.0f, 1.0f);
            newG = Random.Range(0.0f, 1.0f);
            newB = Random.Range(0.0f, 1.0f);
            Color newColor = new Color(newR, newG, newB);

            // Set scale
            float s = Random.Range(0.5f, 4.0f);
            Vector3 newScale = new Vector3(s, s, s);

            // Place the prefab in the scene with its new properties
            Shape shape = pool.Get((ShapeLabel) prefabIndex);
            GameObject newObj = shape.obj;
            newObj.transform.position = newPos;
            newObj.transform.localScale = newScale;
            newObj.transform.localScale = newScale;
            newObj.GetComponent<Renderer>().material.color = newColor;
        }

        synth.OnSceneChange();
    }

    void SaveImages(int count, string relativePath) {
        string dir = $"/Users/kjnakamura/local-documents/local-development/ml-synth-captures/{relativePath}";
        string fileName = $"image_{count.ToString().PadLeft(5, '0')}";
        synth.Save(fileName, 512, 512, dir, 2);
        savedImagesCount++;
    }
}