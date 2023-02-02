using UnityEngine;
using UnityEngine.UI;

public class OptionsMenuView : View
{
    [SerializeField] private Button returnButton;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;

    private AudioTrack[] tracks;
    
    public override void Initialize()
    {
        tracks = AudioController.Instance.tracks;
        returnButton.onClick.AddListener(() => ViewManager.ShowLast());
    }

    public override void Show()
    {
        base.Show();
        GetCurrentVolume();
    }

    private void GetCurrentVolume()
    {
        musicVolumeSlider.value = tracks[0].Source.volume;
        sfxVolumeSlider.value = tracks[1].Source.volume;
    }
    
    void OnEnable()
    {
        musicVolumeSlider.onValueChanged.AddListener(delegate { ChangeMusicVolume(musicVolumeSlider.value); });
        sfxVolumeSlider.onValueChanged.AddListener(delegate { ChangeSfxVolume(sfxVolumeSlider.value); });
    }
    
    void ChangeMusicVolume(float sliderValue)
    {
        tracks[0].Source.volume = sliderValue;
    }
    
    void ChangeSfxVolume(float sliderValue)
    {
        tracks[1].Source.volume = sliderValue;
    }

    void OnDisable()
    {
        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider.onValueChanged.RemoveAllListeners();
    }
}