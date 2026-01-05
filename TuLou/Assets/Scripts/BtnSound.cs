using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(AudioSource))]
public class BtnSound : MonoBehaviour
{
    [Header("Resources 下的音频路径（不含后缀）")]
    string FolderName;
    public string audioResourcePath; // 例如 Resources/Audio/myclip.mp3

    private AudioClip clip;
    private AudioSource audioSource;
    private Button button;

    private void Awake()
    {
        FolderName = transform.parent.name;
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.loop = false;

        button = GetComponent<Button>();
        button.onClick.AddListener(PlaySound);
    }

    private void OnEnable()
    {
        // 每次按钮重新显示时，确保之前的播放被终止（以防异常）
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void OnDisable()
    {
        // 当按钮所在对象被隐藏时，立即停止播放
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }

    private void PlaySound()
    {
        // 重新加载音频资源（保证每次都读取最新的）
        clip = Resources.Load<AudioClip>(FolderName+"/"+audioResourcePath);
        Debug.Log(FolderName + "/" + audioResourcePath);

        if (clip == null)
        {
            Debug.LogWarning("无法加载音频资源: " + audioResourcePath);
            return;
        }

        audioSource.clip = clip;
        audioSource.Play();
    }
}
