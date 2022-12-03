using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Day8_SpaceImageFormat
{
    class Program
    {
        static void Main(string[] args)
        {
            IEnumerable<int> data = GetDataFromFile();

            int width = 25;
            int height = 6;
            var layers = ProcessDataIntoLayers(data, width, height);

            IEnumerable<(int Count, int Index)> layer0Counts = layers.Select((l, index) => (l.Count(d => d == 0), index));
            int leastZeros = layer0Counts.Min(l => l.Count);

            int layerWithLeastZeros = layer0Counts.First(l => l.Count == leastZeros).Index;

            int ones = layers[layerWithLeastZeros].Count(d => d == 1);
            int twos = layers[layerWithLeastZeros].Count(d => d == 2);

            Console.WriteLine(ones * twos);

            RenderImage(layers, width, height);
        }

        private static List<List<int>> ProcessDataIntoLayers(IEnumerable<int> data, int width, int height)
        {
            var layers = new List<List<int>>();
            layers.Add(new List<int>());

            int currentLayer = 0;

            bool LayerIsNotFull(int layerIndex) => layers[layerIndex].Count < width * height;
            foreach (int num in data)
            {
                if (LayerIsNotFull(currentLayer))
                {
                    layers[currentLayer].Add(num);
                }
                else
                {
                    layers.Add(new List<int> { num });
                    currentLayer++;
                }
            }

            return layers;
        }

        private static IEnumerable<int> GetDataFromFile()
        {
            string dataText = File.ReadAllText("input.txt").Trim();
            
            IEnumerable<int> data = dataText.Select(c => int.Parse(c.ToString()));
            return data;
        }

        private static void RenderImage(List<List<int>> data, int width, int height)
        {
            int layerCount = data.Count;
            int pixelCount = width * height;

            var finalData = new int[width * height];

            for(int p = 0; p < pixelCount; p++)
            {
                int pixelValue = data.Select(l => l[p]).First(v => v != 2);

                finalData[p] = pixelValue;
            }

            for(int p = 0; p < pixelCount; p++)
            {
                char renderValue = finalData[p] == 0 ? ' ' : 'H';
                Console.Write(renderValue);

                if (p % width == width - 1)
                {
                    Console.Write(Environment.NewLine);
                }
            }
        }
    }
}
