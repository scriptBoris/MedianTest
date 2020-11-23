using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;

namespace WpfApp
{
    public class Filter
    {
        private int centrWindow;
        private int windowSize;
        private long step;
        private List<float> buffer = new List<float>();
        public Filter(int windowSize)
        {
            if (windowSize < 3 || windowSize > 1000001)
                throw new Exception("Ширина окна медианного фильтра должно быть в диапазоне от 3 до 1000001");

            if (windowSize % 2 == 0)
                throw new Exception("Ширина окна медианного фильтра должно быть нечетным");

            this.windowSize = windowSize;
            centrWindow = windowSize / 2;
            step = -windowSize;
        }

        public float? TryFilter(float num)
        {
            float? res = null;
            buffer.Add(num);
            if (buffer.Count > windowSize)
                buffer.RemoveAt(0);

            if (step >= 0)
            {
                float[] windowArray = new float[windowSize];
                for (int i = 0; i < windowSize; i++)
                    windowArray[i] = buffer[i];

                res = FilterWindow(windowArray);

                //float[] windowArray = new float[window];
                //int windowStep = step - centrWindow;
                //for (int i = 0; i < window; i++)
                //{
                //    if (windowStep < 0)
                //        windowArray[i] = buffer[0];
                //    else
                //        windowArray[i] = buffer[i];

                //    windowStep++;
                //}

                //return FilterWindow(windowArray);
            }

            step++;
            return res;
        }

        public float[] Finish()
        {
            var list = new List<float>();

            for (int i = 0; i < windowSize; i++)
            {
                float? res = TryFilter(buffer.LastOrDefault());
                if (res.HasValue)
                    list.Add(res.Value);
            }

            //foreach (var item in array)
            //{
            //    float? res = TryFilter(item);
            //    if (res != null)
            //        list.Add(res.Value);
            //}

            return list.ToArray();
        }

        private float FilterWindow(float[] array)
        {
            var r = array.OrderBy(x => x).ToArray();
            return r[centrWindow];
        }
    }
}
