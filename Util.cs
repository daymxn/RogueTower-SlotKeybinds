using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;

namespace SlotKeybindsPlugin;

public static class Util
{
  public static List<GameObject> FindChildrenByName(this Transform parent, string name, bool recursive = false)
  {
    var result = new List<GameObject>();
    
    foreach (Transform child in parent)
    {
      if (child.name == name)
      {
        result.Add(child.gameObject);
      }
      
      if (recursive)
      {
        result.AddRange(FindChildrenByName(child, name, true));
      }
    }

    return result;
  }
  
  [CanBeNull]
  public static GameObject FindChildByName(this Transform parent, string name, bool recursive = false)
  {
    var maybeObject = parent.Find(name);
    if (maybeObject) return maybeObject.gameObject;

    if (!recursive) return null;
    
    foreach (Transform child in parent)
    {
      var result = FindChildByName(child, name, true);
      if (result) return result;
    }
    
    return null;
  }
}