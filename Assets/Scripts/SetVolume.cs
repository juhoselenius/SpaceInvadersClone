using Newtonsoft.Json.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/*
 * @Author: Juho Selenius
 * With the help of "Separate Volumes for Music & Sound Effects! - Unity Tutorial"
 * by Ricky Dev (https://www.youtube.com/watch?v=LfU5xotjbPw).
 */

public class SetVolume : MonoBehaviour
{
    public static float musicVolume { get; private set; }
    public static float soundEffectsVolume { get; private set; }

    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider soundEffectsSlider;
    [SerializeField] private Text musicSliderText;
    [SerializeField] private Text soundEffectsSliderText;

    private void Awake()
    {
        musicSlider = GameObject.FindGameObjectWithTag("MusicSlider").GetComponent<Slider>();
        soundEffectsSlider = GameObject.FindGameObjectWithTag("SoundEffectsSlider").GetComponent<Slider>();
        musicSliderText = GameObject.FindGameObjectWithTag("MusicValueText").GetComponent<Text>();
        soundEffectsSliderText = GameObject.FindGameObjectWithTag("SFXValueText").GetComponent<Text>();

        musicSlider.value = AudioManager.aManager.GetMusicValue();
        musicSliderText.text = ((int)(AudioManager.aManager.GetMusicValue() * 100)).ToString() + " %";
        soundEffectsSlider.value = AudioManager.aManager.GetSoundEffectsValue();
        soundEffectsSliderText.text = ((int)(AudioManager.aManager.GetSoundEffectsValue() * 100)).ToString() + " %";
    }

    public void OnMusicSliderValueChanged(float value)
    {
        musicVolume = value;   
        musicSliderText.text = ((int)(value * 100)).ToString() + " %";
        AudioManager.aManager.UpdateMixerVolume();
    }

    public void OnSoundEffectsSliderValueChanged(float value)
    {
        soundEffectsVolume = value;
        soundEffectsSliderText.text = ((int)(value * 100)).ToString() + " %";
        AudioManager.aManager.UpdateMixerVolume();
    }
}
