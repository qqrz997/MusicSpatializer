namespace MusicSpatializer.Components;

// todo: this code might not actually be necessary and needs testing again

//this fixes a bug in the base game where the song AudioSource gets stopped by unity because it runs out of virtual channels due to too many hitsounds
// public class MainSongAudioSourceRestarterBugFix : MonoBehaviour
// {
//     private AudioTimeSyncController audioTimeSyncController;
//     private AudioSource source;
//     // Start is called before the first frame update
//     void Start()
//     {
//         audioTimeSyncController = GetComponent<AudioTimeSyncController>();
//         source = GetComponent<AudioSource>();
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         if (source != null && audioTimeSyncController != null)
//         {
//             if (audioTimeSyncController.state == AudioTimeSyncController.State.Playing && audioTimeSyncController.songTime > 0 && audioTimeSyncController.songTime < audioTimeSyncController.songEndTime - 0.5f && source is { isPlaying: false, time: 0 })
//             {
//                 Plugin.log.Info("Song Time: " + audioTimeSyncController.songTime + " Song End Time: " + audioTimeSyncController.songEndTime);
//                 source.time = audioTimeSyncController.songTime;
//                 source.Play();
//                 if (Plugin.log != null)
//                 {
//                     Plugin.log.Error("Main song AudioSource had to be restarted. This is a bug in the base game.");
//                 }
//             }
//         }
//     }
// }