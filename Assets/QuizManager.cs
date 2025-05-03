using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Button quizButton;  // Tombol yang ada di scene

    // URL untuk halaman quiz
    public string quizURL = "https://www.youtube.com/watch?v=KmOVNVZEP9o&list=RDKmOVNVZEP9o&start_radio=1"; 

    void Start()
    {
        // Menambahkan event listener ke tombol
        quizButton.onClick.AddListener(OnQuizButtonClicked);
    }

    // Fungsi yang dipanggil saat tombol diklik
    void OnQuizButtonClicked()
    {
        // Membuka URL quiz di browser
        Application.OpenURL(quizURL);
    }
}
