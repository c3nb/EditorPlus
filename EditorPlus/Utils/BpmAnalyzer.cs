using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EditorPlus.Utils
{
    // Decompiled From EditorPlus-reborn
    public static class BpmAnalyzer
    {
        public static int AnalyzeBpm(AudioClip clip)
        {
            if (clip == null) return -1;
            Array.Clear(bpmMatchDatas, 0, MAX_BPM);
            int frequency = clip.frequency;
            int channels = clip.channels;
            int frameUnit = (int)Math.Floor(frequency / BASE_FREQUENCY * (channels / 2) * BASE_SPLIT_SAMPLE_SIZE);
            float[] array = new float[clip.samples * channels];
            if (clip.GetData(array, 0))
                return SearchBpm(CreateVolumeArray(array, frameUnit), frequency, frameUnit);
            return -1;
        }
        private static double[] CreateVolumeArray(float[] allSamples, int splitFrameSize)
        {
            double[] volumes = new double[(int)Math.Ceiling(allSamples.Length / (double)splitFrameSize)];
            int totalIndex = 0;
            for (int i = 0; i < allSamples.Length; i += splitFrameSize)
            {
                double num2 = 0f;
                int index = i;
                while (index < i + splitFrameSize && allSamples.Length > index)
                {
                    double num4 = Mathf.Abs(allSamples[index]);
                    if (num4 <= 1f) num2 += num4 * num4;
                    index++;
                }
                volumes[totalIndex++] = Math.Sqrt(num2 / splitFrameSize);
            }
            double maxVolume = volumes.Max();
            for (int j = 0; j < volumes.Length; j++)
                volumes[j] /= maxVolume;
            return volumes;
        }

        private static int SearchBpm(double[] volumeArr, int frequency, int splitFrameSize)
        {
            List<double> list = new List<double>();
            for (int i = 1; i < volumeArr.Length; i++)
                list.Add(Math.Max(volumeArr[i] - volumeArr[i - 1], 0f));
            int num = 0;
            double num2 = frequency / splitFrameSize;
            for (int j = MIN_BPM; j <= MAX_BPM; j++)
            {
                double num3 = 0f;
                double num4 = 0f;
                double num5 = j / 60;
                if (list.Count > 0)
                {
                    for (int k = 0; k < list.Count; k++)
                    {
                        num3 += list[k] * Math.Cos(k * BASE_CHANNELS * Math.PI * num5 / num2);
                        num4 += list[k] * Math.Sin(k * BASE_CHANNELS * Math.PI * num5 / num2);
                    }
                    num3 *= MIN_BPM / list.Count;
                    num4 *= MIN_BPM / list.Count;
                }
                double num6 = Math.Sqrt(num3 * num3 + num4 * num4);
                bpmMatchDatas[num].bpm = j;
                bpmMatchDatas[num].match = num6;
                num++;
            }
            int num7 = Array.FindIndex(bpmMatchDatas, x => x.match == bpmMatchDatas.Max(y => y.match));
            return bpmMatchDatas[num7].bpm;
        }
        private const int MIN_BPM = 1;
        private const int MAX_BPM = 600;
        private const double BASE_FREQUENCY = 44100;
        private const int BASE_CHANNELS = 2;
        private const int BASE_SPLIT_SAMPLE_SIZE = 2205;
        static BpmMatchData[] bpmMatchDatas = new BpmMatchData[MAX_BPM];
        public struct BpmMatchData
        {
            public int bpm;
            public double match;
        }
    }
}
