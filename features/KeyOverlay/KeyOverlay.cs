using HarmonyLib;
using UnityEngine;
using UnityEngine.UI;

namespace SlotKeybindsPlugin.features.KeyOverlay;

public class KeyOverlay : Feature
{
  protected static string KeyCodeToString(KeyCode code)
  {
    return code switch
    {
      KeyCode.Alpha0 => "0",
      KeyCode.Alpha1 => "1",
      KeyCode.Alpha2 => "2",
      KeyCode.Alpha3 => "3",
      KeyCode.Alpha4 => "4",
      KeyCode.Alpha5 => "5",
      KeyCode.Alpha6 => "6",
      KeyCode.Alpha7 => "7",
      KeyCode.Alpha8 => "8",
      KeyCode.Alpha9 => "9",
      _ => code.ToString()
    };
  }

  protected static void ConfigureTextObject(Transform clonedTransform, string text, Color? color = null, int? fontSize = null)
  {
    clonedTransform.name = "KeyOverlay";
    clonedTransform.localPosition = new Vector3(0, 0, 0);
    
    var textObj = clonedTransform.GetComponent<Text>();
    textObj.fontStyle = FontStyle.Bold;
    textObj.color = color ?? textObj.color;
    textObj.fontSize = fontSize ?? textObj.fontSize;
    textObj.text = $"[ {text} ]";

    clonedTransform.transform.SetAsLastSibling();
  }
}
