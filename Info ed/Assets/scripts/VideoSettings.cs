using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VideoSettings : MonoBehaviour
{
    [Header("UI Elements")]
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] resolutions;

    void Start()
    {
        // 🔥 FULLSCREEN
        fullscreenToggle.isOn = Screen.fullScreen;
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);

        // 🔥 REZOLUȚII
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        int currentResolutionIndex = 0;
        var options = new System.Collections.Generic.List<string>();

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int index)
    {
        Resolution res = resolutions[index];
        Screen.SetResolution(res.width, res.height, Screen.fullScreen);
    }
}
