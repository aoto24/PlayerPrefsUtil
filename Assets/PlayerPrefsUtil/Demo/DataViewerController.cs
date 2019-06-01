using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class DataViewerController : MonoBehaviour
{
  public MyDataClass SaveMydata;

  public MyDataClass LoadMydata;

  private string saveKey;

  private bool compressTrigger = false;

  public void Awake()
  {
    saveKey = "PlayerPrefsUtilDemo";
  }

  public void SetObject()
  {
    PlayerPrefsUtil.SetObj<MyDataClass>(saveKey, SaveMydata, compressTrigger);
  }

  public void GetObject()
  {
    LoadMydata = PlayerPrefsUtil.GetObj<MyDataClass>(saveKey);
  }

  public void SetKey(Text str)
  {
    saveKey = str.text;
  }

  public void CompressChenge(Text childtext)
  {
    compressTrigger = !compressTrigger;
    childtext.text = compressTrigger.ToString();
  }

  public void DebugKey()
  {
    Debug.Log("\n\n\n\n\n\n\n" + PlayerPrefs.GetString(saveKey));
  }
}

public class ButtonValues : UnityEvent<string,bool>
{

}