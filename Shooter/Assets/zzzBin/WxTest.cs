using System.Text;
using UnityEditor;
using UnityEngine;
using System.Collections;

public class WxTest : MonoBehaviour {

	// Use this for initialization
	void Start () {
	    SetMenu();
	}

    //[MenuItem("Morln/设置自定义菜单")]
    //static void SetMenu()
    //{
    //    var www = new WWW("https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid=APPID&secret=APPSECRET");
    //}

    [ContextMenu("设置自定义菜单")]
    void SetMenu()
    {
        var content = @"{
     ""button"":[
     {	
          ""type"":""click"",
          ""name"":""今日歌曲"",
          ""key"":""V1001_TODAY_MUSIC""
      },
      {
           ""type"":""click"",
           ""name"":""歌手简介"",
           ""key"":""V1001_TODAY_SINGER""
      },
      {
           ""name"":""菜单"",
           ""sub_button"":[
           {	
               ""type"":""view"",
               ""name"":""搜索"",
               ""url"":""http://www.soso.com/""
            },
            {
               ""type"":""view"",
               ""name"":""视频"",
               ""url"":""http://v.qq.com/""
            },
            {
               ""type"":""click"",
               ""name"":""赞一下我们"",
               ""key"":""V1001_GOOD""
            }]
       }]
 }";

        var www = new WWW("https://api.weixin.qq.com/cgi-bin/menu/create?access_token=5C8D63DC2569EAB9D7166857E1A6583C", Encoding.UTF8.GetBytes(content));
        StartCoroutine(Send(www));
    }

    static IEnumerator Send(WWW www)
    {
        yield return www;
        if (www.error == null)
        {
            Debug.Log(www.text);
        }
        else
        {
            Debug.LogError(www.error);
        }
    }
}
