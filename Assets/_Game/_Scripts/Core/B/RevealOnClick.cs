using UnityEngine;
//знаходження ключа під доками, треба натиснути на них, щоб з'явився ключ
public class RevealOnClick : MonoBehaviour
{
    [SerializeField] private GameObject toReveal;
    [SerializeField] private string revealLog = "There was something under the documents...";
    [SerializeField] private string revealId = "docs_01"; // для збереження стану

    private bool revealed;
    private Collider2D col;

    private string SaveKey => $"reveal_{revealId}";

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void Start()
    {
        revealed = PlayerPrefs.GetInt(SaveKey, 0) == 1;

        if (revealed)
        {
            if (toReveal != null)
                toReveal.SetActive(true);

            if (col != null)
                col.enabled = false;
        }
    }

    private void OnMouseDown()
    {
        if (revealed) return;

        revealed = true;

        if (toReveal != null)
            toReveal.SetActive(true);

        if (col != null)
            col.enabled = false;

        PlayerPrefs.SetInt(SaveKey, 1);
        PlayerPrefs.Save();

        Debug.Log(revealLog);
    }
}