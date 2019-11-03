using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public GameManager gameManager;
    public Animator animator;
    public SplashScreenUtils splashScreenUtils;
    public Image image;
    private int scene;
    void Awake()
    {
        // splashScreenUtils = FindObjectOfType<SplashScreenUtils>();
    }
    public void FadeIn()
    {
        animator.SetTrigger("FadeIn");
    }
    public void FadeOut(int _scene)
    {
        scene = _scene;
        animator.SetTrigger("FadeOut");
    }
    public void OnBlackFrame() // end of fade out
    {
        SceneManager.LoadScene(scene);
    }
    public void OnClearFrame() // end of fade in
    {
        if (gameManager != null)
        {
            gameManager.StartLevel();
        }
    }
}
