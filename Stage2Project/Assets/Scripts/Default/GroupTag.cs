using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupTag : MonoBehaviour
{
    public enum Group { One, Two, Three, Four, Five, Six, Seven, Eight, Nine }

    [SerializeField]
    private Group GroupCode;

    public Group Affiliation { get { return GroupCode; } }
}
