using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Fade : MonoBehaviour
{
    Image bg;

    // Start is called before the first frame update
    public void Init()
    {
        bg = GetComponentInChildren<Image>(true);
        bg.gameObject.SetActive(false);
        DontDestroyOnLoad(gameObject);
    }

    public void FadeIn()
    {
        StartCoroutine(IEFade(Color.black, new Color(0,0,0,0), 2f, false));
    }
    public void FadeOut()
    {
        StartCoroutine(IEFade(new Color(0, 0, 0, 0), Color.black, 1f, true));
    }
    IEnumerator IEFade(Color startColor, Color endColor, float delayTime, bool state)
    {
        bg.gameObject.SetActive(true);
        bg.color = startColor;
        float elapsed = 0;
        while(true)
        {
            elapsed += Time.deltaTime / delayTime;
            bg.color = Color.Lerp(startColor, endColor, elapsed);
            if(elapsed >= 1.0f)
            {
                bg.gameObject.SetActive(state);
                break;
            }
            yield return null;
        }
        yield return null;
    }

}
