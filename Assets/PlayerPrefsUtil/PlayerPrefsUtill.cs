using UnityEngine;
using System.IO;
using System.Xml.Serialization;
using System.IO.Compression;

/// <summary>
/// PlayerPrefs拡張
/// </summary>
public class PlayerPrefsUtil : PlayerPrefs
{
  /// <summary>
  /// ジェネリックで指定した型で保存する(オプションで圧縮)
  /// </summary>
  public static void SetObj<T>(string key, T obj, bool compression = false)
  {
    //バイナリデータに
    var mssr = new MemoryStream();
    new XmlSerializer(typeof(T)).Serialize(mssr, obj);

    if (compression)
    {
      //バイナリデータを圧縮
      MemoryStream mscm = new MemoryStream();
      DeflateStream cmpstr = new DeflateStream(mscm, CompressionMode.Compress, true);

      cmpstr.Write(mssr.ToArray(), 0, mssr.ToArray().Length);

      mssr.Close();
      cmpstr.Close();

      //圧縮されたバイナリデータをBase64形式のstringに変えて保存
      PlayerPrefs.SetString(key, System.Convert.ToBase64String(mscm.ToArray()));

      mscm.Close();
    }
    else
    {
      //そのまま保存
      PlayerPrefs.SetString(key, System.Text.Encoding.GetEncoding("UTF-8").GetString(mssr.ToArray()));
      mssr.Close();
    }
  }

  /// <summary>
  /// ジェネリックで指定した型でキーに保存されているデータを返す
  /// </summary>
  public static T GetObj<T>(string key)
  {
    string datastr = PlayerPrefs.GetString(key);
    if (datastr.StartsWith("<"))
    {
      //デシリアライズ返しておわり
      return (T)new XmlSerializer(typeof(T)).Deserialize(new StringReader(datastr));
    }
    else
    {
      //展開用変数
      MemoryStream otdcms = new MemoryStream();
      DeflateStream dcmpms = new DeflateStream(new MemoryStream(System.Convert.FromBase64String(datastr)), CompressionMode.Decompress, true);

      int n;
      byte[] temp = new byte[128];

      //展開処理
      while ((n = dcmpms.Read(temp, 0, temp.Length)) > 0)
      {
        otdcms.Write(temp, 0, n);
      }

      dcmpms.Close();

      //デシリアライズ、ストリームを閉じ、値を返して終わり
      otdcms.Position = 0;
      T rtnobj = (T)new XmlSerializer(typeof(T)).Deserialize(otdcms);
      otdcms.Close();
      return rtnobj;
    }
  }
}