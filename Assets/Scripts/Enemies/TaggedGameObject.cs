using System.Collections.Generic;
using UnityEngine;

public enum Tags
{
    Wall, CameraWall
}

public class TaggedMonobehaviour : MonoBehaviour
{
    [SerializeField]
    public List<Tags> collisionTags;
}
public class TaggedGameObject : TaggedMonobehaviour
{

}

public static class TaggedMonoBehaviourUtils
{

}