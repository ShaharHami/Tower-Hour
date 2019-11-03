using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSFX : MonoBehaviour
{
    [SerializeField] AudioSource source;
    [SerializeField] AudioClip hoverAudio;
    [SerializeField] AudioClip clickAudio;
    [SerializeField] AudioClip deniedAudio;
    [SerializeField] UpgradeMenu upgradeMenu;
    private void Start()
    {
        source = GetComponent<AudioSource>();
        source.ignoreListenerPause = true;
    }
    public void OnButtonHover()
    {
        if (source != null && hoverAudio != null)
        {
            source.PlayOneShot(hoverAudio);
        }
    }
    public void OnButtonClick()
    {
        if (source != null && hoverAudio != null)
        {
            source.PlayOneShot(clickAudio);
        }
    }
    public void OnUpgrade()
    {
        if (upgradeMenu.upgradable)
        {
            OnButtonClick();
        }
        else
        {
            OnDenied();
        }
    }
    public void OnDenied()
    {
        if (deniedAudio != null)
        {
            source.PlayOneShot(deniedAudio);
        }
    }
}
