using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StarFieldGenerator : MonoBehaviour
{
    [SerializeField] private Star m_starPrefab;

    [Header("Star Generation")]
    [SerializeField] private int m_numStars = 12;
    [SerializeField] private float m_radius = 5.0f;
    [SerializeField] private Star[] m_stars;

    [ContextMenu("Generate")]
    private void GenerateField() {
        m_stars = new Star[m_numStars];

        // evenly space the stars out into the grid
        float radius = 1f;
        for (int i = 0; i < m_numStars; i++) {
            m_stars[i] = Instantiate(m_starPrefab, transform);
            float angle = i * Mathf.PI * 2f / m_numStars;
            Vector3 vec = new Vector3(Mathf.Cos(angle) * radius, 0.0f, Mathf.Sin(angle) * radius);
            m_stars[i].transform.localPosition = vec * m_radius;
        }

        for (int i = 0; i < m_numStars; i++) {
            int left = (i - 1) < 0 ? m_numStars - 1 : i - 1;
            int right = (i + 1) >= m_numStars ? 0 : i + 1;
            m_stars[i].Neighbours = new List<Star>() {
                m_stars[left], m_stars[right]
            };
        }
    }

    public Star[] PickSequence(int num) {
        // Enters all possible indices into a list
        var indices = new List<int>();
        for (int i = 0; i < m_stars.Length; i++)
            indices.Add(i);

        // Picks from the indices, removes index so it's never picked twice
        Star[] sequence = new Star[num];
        for(int i = 0; i < num; i++) {
            int rindex = UnityEngine.Random.Range(0, indices.Count);
            sequence[i] = m_stars[indices[rindex]];
            indices.RemoveAt(rindex);
        }
        return sequence;
    }
}
