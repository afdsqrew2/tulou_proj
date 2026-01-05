using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class VideoPlayerController : MonoBehaviour
{
    public string videoPathInResources = "video/test1"; // Resources/video/test1
    public RawImage videoDisplay;
    public GameObject pauseIcon;
    public Slider videoSlider;

    private VideoPlayer videoPlayer;
    private bool isPaused = false;
    private bool isDragging = false;
    private bool hasReachedEnd = false;

    void Awake()
    {
        // 添加并配置 VideoPlayer
        videoPlayer = gameObject.AddComponent<VideoPlayer>();
        videoPlayer.renderMode = VideoRenderMode.RenderTexture;
        videoPlayer.targetTexture = new RenderTexture(1920, 1080, 0);
        videoPlayer.audioOutputMode = VideoAudioOutputMode.AudioSource;

        videoDisplay.texture = videoPlayer.targetTexture;

        videoPlayer.isLooping = false;
        videoPlayer.playOnAwake = false;
        videoPlayer.prepareCompleted += OnVideoPrepared;

        // 播放/暂停
        Button button = videoDisplay.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(TogglePlayPause);
        }

        // 进度条
        if (videoSlider != null)
            videoSlider.onValueChanged.AddListener(OnSliderValueChanged);
    }

    void OnEnable()
    {
        // 每次重新启用时都重新播放
        PlayVideoFromStart();
    }

    void OnDisable()
    {
        if (videoPlayer != null)
        {
            videoPlayer.Stop();
            videoPlayer.clip = null;
        }

        if (videoSlider != null)
            videoSlider.value = 0f;

        hasReachedEnd = false;
        isPaused = false;
    }


    public void PlayVideoFromStart()
    {
        hasReachedEnd = false;
        isPaused = false;

        // 强制清空旧 clip（这一步关键）
        videoPlayer.Stop();
        videoPlayer.clip = null;

        // 重新加载 clip
        VideoClip clip = Resources.Load<VideoClip>(videoPathInResources);
        if (clip == null)
        {
            Debug.LogError("Video not found at Resources/" + videoPathInResources);
            return;
        }

        videoPlayer.clip = clip;

        // 重置时间
        videoPlayer.time = 0;
        videoPlayer.frame = 0;

        // 准备播放
        videoPlayer.Prepare();

        if (pauseIcon != null)
            pauseIcon.SetActive(false);

        if (videoSlider != null)
            videoSlider.value = 0f;
    }



    void OnVideoPrepared(VideoPlayer vp)
    {
        Debug.Log("Video is prepared. Starting playback from the beginning.");
        vp.Play();
        hasReachedEnd = false;
        isPaused = false;

        if (pauseIcon != null)
            pauseIcon.SetActive(false);
    }

    void Update()
    {
        if (videoPlayer.clip == null || !videoPlayer.isPrepared)
            return;

        if (!isDragging && videoPlayer.isPlaying)
        {
            float progress = (float)(videoPlayer.time / videoPlayer.length);
            videoSlider.value = progress;
        }

        // 播放结束处理
        if (!hasReachedEnd && videoPlayer.frame >= (long)(videoPlayer.frameCount - 1))
        {
            hasReachedEnd = true;
            isPaused = true;
            videoPlayer.Pause();

            if (pauseIcon != null)
                pauseIcon.SetActive(true);

            if (videoSlider != null)
            {
                videoSlider.value = 1f;
                videoSlider.normalizedValue = 1f;
            }
        }
    }

    public void TogglePlayPause()
    {
        if (!videoPlayer.isPrepared) return;

        if (isPaused)
        {
            // 如果视频已经播放到结尾，重播
            if (hasReachedEnd)
            {
                videoPlayer.frame = 0;
                hasReachedEnd = false;
            }

            videoPlayer.Play();
            isPaused = false;

            if (pauseIcon != null)
                pauseIcon.SetActive(false);
        }
        else
        {
            videoPlayer.Pause();
            isPaused = true;

            if (pauseIcon != null)
                pauseIcon.SetActive(true);
        }
    }

    void OnSliderValueChanged(float value)
    {
        if (!videoPlayer.isPrepared || videoPlayer.length <= 0) return;
        if (!isDragging) return;

        double targetTime = videoPlayer.length * value;
        videoPlayer.time = targetTime;
        hasReachedEnd = false;
    }

    public void OnBeginDrag()
    {
        isDragging = true;
    }

    public void OnEndDrag()
    {
        isDragging = false;
        OnSliderValueChanged(videoSlider.value);
    }
}
