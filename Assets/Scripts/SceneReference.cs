using UnityEngine;

public class SceneReference : MonoBehaviour
{
    public Player player;
    public Camera camera;
    public GameUI UI;

    void Awake()
    {
        GameManager.instance.SetSceneReferences(this);
    }
}
