using System.Linq;
using UnityEngine;

namespace EditorPlus.Utils
{
    // Decompiled From EditorPlus-reborn
    public static class OffsetAnalyzer
    {
        public static float AnalyzeOffset(AudioClip clip, float minVol)
        {
            if (clip == null) return -1f;
            int frequency = clip.frequency;
            int channels = clip.channels;
            float[] array = new float[clip.samples * channels];
            if (clip.GetData(array, 0))
                return SearchOffset(array, frequency, minVol);
            return -1;
        }
        private static float SearchOffset(float[] allsamples, int freq, float minVol)
        {
            int idx = 0;
            foreach (var (item, index) in allsamples.Select((item, index) => (item, index)))
            {
                if (Mathf.Abs(item) > minVol)
                {
                    idx = index;
                    break;
                }
            }
            return (idx + 1) / 2f / freq * 1000f;
        }
    }
}
