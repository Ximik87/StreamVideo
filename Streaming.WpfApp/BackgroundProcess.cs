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
using Streaming.Core;

namespace Streaming.WpfApp
{
    class BackgroundProcess
    {
        private Stream _empty;
        private Stream _frame1;
        private Stream _frame2;
        private readonly ObservableCollection<CameraData> _cameras;
        private List<ISeparateCameraProcess> _consumers;
        private ILinkContainer _linkContainer;

        public BackgroundProcess(ObservableCollection<CameraData> cameras, ILinkContainer linkContainer)
        {
            _cameras = cameras;
            _linkContainer = linkContainer;
            _consumers = new List<ISeparateCameraProcess>();
            Init();
            Test();
        }

        private void Init()
        {
            // todo implement beautiful
            _empty = StreamHelper.GetStream(@"d:\empty.jpg");
            _frame1 = StreamHelper.GetStream(@"d:\qqqq.jpg");
            _frame2 = StreamHelper.GetStream(@"d:\qqqq2.jpg");

            _cameras.Add(new CameraData { Name = "firstCamera", Image = _empty });
            _cameras.Add(new CameraData { Name = "secondCamera", Image = _empty });
            _cameras.Add(new CameraData { Name = "thirdCamera", Image = _empty });
            _cameras.Add(new CameraData { Name = "4Camera", Image = _empty });

            //Task.Factory.StartNew(() => DowWork(), TaskCreationOptions.LongRunning);
        }

        private void Test()
        {
            foreach (var item in _cameras)
            {              
                var stub = new VideoConsumerStub(item.Url);
                var consumer = new SeparateCameraProcess(stub, item);
                consumer.Start();
                _consumers.Add(consumer);
            }
        }
    }
}
