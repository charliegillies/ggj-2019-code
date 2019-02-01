using UnityEngine;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public static class Helpers {
    public static float RandNormal() {
        return UnityEngine.Random.Range(-1.0f, 1.0f);
    }
    public static Vector3 RandNormalXZ() {
        return new Vector3(RandNormal(), 0.0f, RandNormal());
    }
    public static Vector3 Range(Vector3 a, Vector3 b) {
        return new Vector3(Random.Range(a.x, b.x), Random.Range(a.y, b.y), Random.Range(a.z, b.z));
    }
    public static T RandomElement<T>(List<T> elements) {
        int i = Random.Range(0, elements.Count);
        return elements[i];
    }
    public static T RandomElement<T>(T[] elements) {
        int i = Random.Range(0, elements.Length);
        return elements[i];
    }
    public static bool ExampleMode() {
        return SceneManager.sceneCount == 1;
    }
    public static bool RandBool() {
        return Random.Range(-1.0f, 1.0f) >= 0.0f;
    }
    public static Vector3 JumpArc(Vector3 a, Vector3 b, float t, float height) {
        Vector3 pos = Vector3.Lerp(a, b, t);
        pos.y = height * Mathf.Sin(Mathf.Lerp(0.0f, Mathf.PI, t));
        return pos;
    }
}
