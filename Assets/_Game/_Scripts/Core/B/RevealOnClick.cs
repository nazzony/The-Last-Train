using UnityEngine;
//знаходження ключа під доками, треба натиснути на них, щоб з'явився ключ
public class RevealOnClick : MonoBehaviour
{
    [SerializeField] private GameObject toReveal;
    [SerializeField] private string revealLog = "There was something under the documents...";

    private bool revealed;
    private Collider col;

    private void Awake()
    {
        col = GetComponent<Collider>();
    }

    private void OnMouseDown()
    {
        if (revealed) return;

        revealed = true;

        if (toReveal != null)
            toReveal.SetActive(true);

        if (col != null)
            col.enabled = false; 
        Debug.Log(revealLog);
    }
}
