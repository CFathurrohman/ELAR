using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroUIManager : MonoBehaviour
{
    public VideoPlayer videoPlayer;    // Video player untuk intro
    public GameObject introPanel;      // Panel intro yang berisi video dan tombol
    public GameObject uiPanel;      // Panel intro yang berisi video dan tombol
    public GameObject earthObject;     // Objek Earth yang ingin diaktifkan
    public Button startButton;         // Tombol Start untuk memulai transisi

    private void Start()
    {
        // Menampilkan intro panel dan menyembunyikan objek Earth di awal
        introPanel.SetActive(true);
        uiPanel.SetActive(false); // Pastikan intro panel aktif pada awalnya
        earthObject.SetActive(false); // Pastikan Earth tidak aktif pada awalnya

        // Menambahkan event listener untuk tombol Start
        startButton.onClick.AddListener(OnStartClicked);

        // Mulai video ketika scene dimulai
        videoPlayer.Play();
    }

    // Fungsi ini dipanggil saat tombol Start diklik
    public void OnStartClicked()
    {
        // Sembunyikan panel intro
        introPanel.SetActive(false);
        uiPanel.SetActive(true); // Pastikan intro panel aktif pada awalnya

        // Aktifkan objek Earth
        earthObject.SetActive(true);

        // Hentikan video jika tombol Start diklik
        videoPlayer.Stop();
    }
}
