using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace Streaming.WpfApp
{
    class BackgroundProcess
    {
        private Stream _empty;
        private Stream _frame1;
        private Stream _frame2;
        private ObservableCollection<CameraData> _cameras;

        public BackgroundProcess(ObservableCollection<CameraData> cameras)
        {
            _cameras = cameras;
            Init();
        }

        private async Task DowWork()
        {
            int counter = 0;
            while (true)
            {
                foreach (var item in _cameras)
                {
                    if (counter % 3 == 0)
                    {
                        item.Image = _frame1;
                    }
                    else
                    {
                        item.Image = _frame2;
                    }
                    counter++;

                    await Task.Delay(TimeSpan.FromMilliseconds(50));
                }
            }
        }

        private void Init()
        {
            //var stream = File.OpenRead(@"d:\qqqq.jpg");
            //_frame1 = new byte[stream.Length];
            //stream.Read(_frame1, 0, (int)stream.Length);

            //stream = File.OpenRead(@"d:\qqqq2.jpg");
            //_frame2 = new byte[stream.Length];
            //stream.Read(_frame2, 0, (int)stream.Length);

            //stream.Dispose();
            // todo сделать красиво
            _empty = new MemoryStream();
            File.OpenRead(@"d:\empty.jpg").CopyTo(_empty);

            _frame1 = new MemoryStream();
            File.OpenRead(@"d:\qqqq.jpg").CopyTo(_frame1);

            _frame2 = new MemoryStream();
            File.OpenRead(@"d:\qqqq2.jpg").CopyTo(_frame2);


            //_cameras = new ObservableCollection<CameraData>
            //{
            //    new CameraData { Name = "firstCamera"  },
            //    new CameraData { Name = "secondCamera" },
            //    new CameraData { Name = "thirdCamera"  },
            //    new CameraData { Name = "4Camera"  },
            //};
            _cameras.Add(new CameraData { Name = "firstCamera", Image = _empty });
            _cameras.Add(new CameraData { Name = "secondCamera", Image = _empty });
            _cameras.Add(new CameraData { Name = "thirdCamera", Image = _empty });
            _cameras.Add(new CameraData { Name = "4Camera", Image = _empty });

            Task.Factory.StartNew(() => DowWork(), TaskCreationOptions.LongRunning);
        }
    }
}
