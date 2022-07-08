using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PressAnyKey : MonoBehaviour
{
    [SerializeField] GameObject MainPanel;
    [SerializeField] GameObject TitlePanel;
    [SerializeField] TextMeshProUGUI PressAnyKeyText;
    [SerializeField] TextMeshProUGUI GameTwoKeyText;

    bool FadeEnded = true;

    // Update is called once per frame
    void Update()
    {
        if(Input.anyKey)
        {
            MainPanel.SetActive(true);
            TitlePanel.SetActive(true);
            this.gameObject.SetActive(false);
        }

        if(FadeEnded)
            StartCoroutine(FadeOut());

    }

    private IEnumerator FadeOut()
    {
        FadeEnded = false;
        PressAnyKeyText.CrossFadeAlpha(0.0f, 3f, false);
        GameTwoKeyText.CrossFadeAlpha(0.0f, 3f, false);
        yield return new WaitForSeconds(2.5f);
        PressAnyKeyText.CrossFadeAlpha(1.0f, 3f, false);
        GameTwoKeyText.CrossFadeAlpha(1.0f, 3f, false);
        yield return new WaitForSeconds(4f);
        FadeEnded = true;
    }
}
