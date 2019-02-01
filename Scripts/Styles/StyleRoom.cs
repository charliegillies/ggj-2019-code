using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StyleRoom : MonoBehaviour
{
    [SerializeField] private MeshRenderer m_bed, m_rug, m_poster;
    [SerializeField] private RoomStyle m_generic, m_pirates, m_forest, m_space;

    [SerializeField] private GameObject m_forestRightWall, m_forestLeftWall;
    [SerializeField] private GameObject m_piratesRightWall, m_piratesLeftWall;
    [SerializeField] private GameObject m_genericRightWall, m_genericLeftWall;
    [SerializeField] private GameObject m_spaceRightWall, m_spaceLeftWall;

    private void OnEnable() {
        Environment.Finished += RestyleRoomByEnvironment;
    }
    private void OnDisable() {
        Environment.Finished -= RestyleRoomByEnvironment;
    }
    private void DeactivateWalls() {
        m_forestLeftWall.SetActive(false); m_forestRightWall.SetActive(false);
        m_piratesLeftWall.SetActive(false); m_piratesRightWall.SetActive(false);
        m_genericLeftWall.SetActive(false); m_genericLeftWall.SetActive(false);
        m_spaceLeftWall.SetActive(false); m_spaceRightWall.SetActive(false);
    }
    private void RestyleRoomByEnvironment(Environment e) {
        switch(e.GetID()) {
            case Environment.ID.Pirate: Pirates();  break;
            case Environment.ID.Campsite: Forest(); break;
            case Environment.ID.Space: Space();  break;
        }
    }

    [ContextMenu("Generic")]
    public void Generic() {
        DeactivateWalls();
        ApplyStyle(m_generic);
        m_genericLeftWall.SetActive(true); m_genericLeftWall.SetActive(true);
    }
    [ContextMenu("Pirates")]
    public void Pirates() {
        DeactivateWalls();
        ApplyStyle(m_pirates);
        m_piratesLeftWall.SetActive(true); m_piratesRightWall.SetActive(true);
    }
    [ContextMenu("Forest")]
    public void Forest() {
        DeactivateWalls();
        ApplyStyle(m_forest);
        m_forestLeftWall.SetActive(true); m_forestRightWall.SetActive(true);
    }
    [ContextMenu("Space")]
    public void Space() {
        DeactivateWalls();
        ApplyStyle(m_space);
        m_spaceLeftWall.SetActive(true); m_spaceRightWall.SetActive(true);
    }
    public void ApplyStyle(RoomStyle style) {
        m_bed.material = style.Bed;
        m_rug.material = style.Rug;
        m_poster.material = style.Poster;
    }
}
