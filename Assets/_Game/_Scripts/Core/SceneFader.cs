using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneFader : MonoBehaviour
{
    public static SceneFader instance;
    [SerializeField] private float _fadeRate = 1.7f;
    [SerializeField] private CanvasGroup _group;
    public void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else
        {
            Destroy(gameObject);
        }
    }
    
    IEnumerator FadeLoad(int sceneId)
    {
        _group.blocksRaycasts = true;

        while (_group.alpha < 1)
        {
            _group.alpha += Time.deltaTime * _fadeRate;
            yield return null;
        }

        SceneManager.LoadScene(sceneId);

        while (_group.alpha > 0)
        {
            _group.alpha -= Time.deltaTime * _fadeRate;
            yield return null;
        }

        _group.blocksRaycasts = false;
    }
    public void LoadScene(int sceneId)
    {
        StartCoroutine(FadeLoad(sceneId));
    }

}
