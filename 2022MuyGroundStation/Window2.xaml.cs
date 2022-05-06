using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Input;
using GMap.NET;
using GMap.NET.MapProviders;
using System.Windows.Forms.DataVisualization.Charting;
using System.Drawing;
using AForge.Video;
using AForge.Video.DirectShow;
using System.Windows.Media.Media3D;
using HelixToolkit.Wpf;

namespace _2022MuyGroundStation
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        bool isConnectedToPayload = false;
        List<String> timeList;
        private FilterInfoCollection VideoCaptureDevices;
        private VideoCaptureDevice FinalVideo;
        private Color myRed = Color.FromArgb(186, 29, 20);
        private Color myYellow = Color.FromArgb(227, 192, 16);
        private Color myGreen = Color.FromArgb(95, 201, 14);
        private Color myBlue = Color.FromArgb(5, 79, 135);
        private Color myPink = Color.FromArgb(173, 10, 157);
        
        
        public Window2()
        {

            InitializeComponent();
            
            VideoCaptureDevices = new FilterInfoCollection(FilterCategory.VideoInputDevice);
            startWebcam();

            timeList = new List<String>();
            for (int i = 0; i < 10; i++)
            {
                timeList.Add(DateTime.Now.ToString("hh:mm:ss"));
            }
        }

        private void startWebcam()
        {
            FinalVideo = new VideoCaptureDevice(VideoCaptureDevices[0].MonikerString);
            FinalVideo.NewFrame += new NewFrameEventHandler(FinalVideo_NewFrame);
            FinalVideo.Start();
        }

        #region Graphs

        private void pressure1GraphInitalized(object sender, EventArgs e)
        {
            Console.WriteLine("Görev yükü basınç grafiği başlatıldı");
            List<double> pressure1List = new List<double>();
            for (double i = 3.6; i > 2.6; i = i - 0.1)
            {
                pressure1List.Add(i);
            }

            pressure1Chart.Titles.Add("Taşıyıcı Basınç");
            TitleCollection t = pressure1Chart.Titles;
            t[0].ForeColor = Color.White;

            pressure1Area.BackColor = Color.FromArgb(31, 30, 29);
            pressure1Area.BorderWidth = 1;

            pressure1Points.Color = myYellow;
            pressure1Points.ChartType = SeriesChartType.FastLine;
            pressure1Points.BorderWidth = 3;

            pressure1Area.AxisX.Title = "Zaman";
            pressure1Area.AxisX.TitleForeColor = Color.White;
            pressure1Area.AxisX.MajorGrid.Enabled = false;
            pressure1Area.AxisX.LineColor = Color.White;
            pressure1Area.AxisX.LabelStyle.ForeColor = Color.White;
            pressure1Area.AxisX.MajorTickMark.LineWidth = 2;
            pressure1Area.AxisX.MajorTickMark.LineColor = Color.White;

            pressure1Area.AxisY.Title = "Metre";
            pressure1Area.AxisY.TitleForeColor = Color.White;
            pressure1Area.AxisY.MajorGrid.Enabled = false;
            pressure1Area.AxisY.LabelStyle.ForeColor = Color.White;
            pressure1Area.AxisY.LineColor = Color.White;
            pressure1Area.AxisY.LabelStyle.ForeColor = Color.White;
            pressure1Area.AxisY.MajorTickMark.LineWidth = 2;
            pressure1Area.AxisY.MajorTickMark.LineColor = Color.White;


            pressure1Area.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None;
            pressure1Area.AxisY.Minimum = 0;
            pressure1Area.AxisY.Maximum = 4;
            pressure1Area.AxisX.Interval = 1.0;


            pressure1Points.Points.DataBindXY(timeList, pressure1List);
        }

        private void pressure2GraphInitalized(object sender, EventArgs e)
        {
            Console.WriteLine("Taşıyıcı basınç grafiği başlatıldı");
            List<double> voltList = new List<double>();
            for (double i = 3.6; i > 2.6; i = i - 0.1)
            {
                voltList.Add(i);
            }

            pressure2Chart.Titles.Add("Görev Yükü Basınç");
            TitleCollection t = pressure2Chart.Titles;
            t[0].ForeColor = Color.White;

            pressure2Area.BackColor = Color.FromArgb(31, 30, 29);
            pressure2Area.BorderWidth = 1;

            pressure2Points.Color = myBlue;
            pressure2Points.ChartType = SeriesChartType.FastLine;
            pressure2Points.BorderWidth = 3;

            pressure2Area.AxisX.Title = "Zaman";
            pressure2Area.AxisX.TitleForeColor = Color.White;
            pressure2Area.AxisX.MajorGrid.Enabled = false;
            pressure2Area.AxisX.LineColor = Color.White;
            pressure2Area.AxisX.LabelStyle.ForeColor = Color.White;
            pressure2Area.AxisX.MajorTickMark.LineWidth = 2;
            pressure2Area.AxisX.MajorTickMark.LineColor = Color.White;

            pressure2Area.AxisY.Title = "Pascal(p)";
            pressure2Area.AxisY.TitleForeColor = Color.White;
            pressure2Area.AxisY.MajorGrid.Enabled = false;
            pressure2Area.AxisY.LabelStyle.ForeColor = Color.White;
            pressure2Area.AxisY.LineColor = Color.White;
            pressure2Area.AxisY.LabelStyle.ForeColor = Color.White;
            pressure2Area.AxisY.MajorTickMark.LineWidth = 2;
            pressure2Area.AxisY.MajorTickMark.LineColor = Color.White;


            pressure2Area.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None;
            pressure2Area.AxisY.Minimum = 0;
            pressure2Area.AxisY.Maximum = 4;
            pressure2Area.AxisX.Interval = 1.0;

            pressure2Points.Points.DataBindXY(timeList, voltList);
        }

        private void heightGraphInitilazed(object sender, EventArgs e)
        {
            Console.WriteLine("Görev yükü yükseklik grafiği başlatıldı");
            List<double> containerHeightList = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                containerHeightList.Add(i);
            }

            List<double> payloadHeightList = new List<double>();
            for (int i = 10; i > 0; i--)
            {
                payloadHeightList.Add(i);
            }
            
            heightChart.Titles.Add("Görev Yükü ve Taşıyıcı Yükseklik");
            TitleCollection t = heightChart.Titles;
            t[0].ForeColor = Color.White;

            #region Height Area Setup
            heightArea.BackColor = Color.FromArgb(31, 30, 29);
            heightArea.BorderWidth = 1;

            heightArea.AxisX.Title = "Zaman";
            heightArea.AxisX.TitleForeColor = Color.White;
            heightArea.AxisX.MajorGrid.Enabled = false;
            heightArea.AxisX.LineColor = Color.White;
            heightArea.AxisX.LabelStyle.ForeColor = Color.White;
            heightArea.AxisX.MajorTickMark.LineWidth = 2;
            heightArea.AxisX.MajorTickMark.LineColor = Color.White;

            heightArea.AxisY.Title = "Metre";
            heightArea.AxisY.TitleForeColor = Color.White;
            heightArea.AxisY.MajorGrid.Enabled = false;
            heightArea.AxisY.LabelStyle.ForeColor = Color.White;
            heightArea.AxisY.LineColor = Color.White;
            heightArea.AxisY.LabelStyle.ForeColor = Color.White;
            heightArea.AxisY.MajorTickMark.LineWidth = 2;
            heightArea.AxisY.MajorTickMark.LineColor = Color.White;
            
            heightArea.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None;
            heightArea.AxisY.Minimum = -2;
            heightArea.AxisY.Maximum = 10;
            heightArea.AxisX.Interval = 1.0;
            #endregion

            #region Height Series Setup
            Series containerHeight = new Series();
            Series payloadHeight = new Series();
            heightChart.Series.Add(containerHeight);
            heightChart.Series.Add(payloadHeight);

            containerHeight.Color = myRed;
            containerHeight.ChartType = SeriesChartType.FastLine;
            containerHeight.BorderWidth = 3;

            payloadHeight.Color = myYellow;
            payloadHeight.ChartType = SeriesChartType.FastLine;
            payloadHeight.BorderWidth = 3;
            #endregion

            containerHeight.Points.DataBindXY(timeList, containerHeightList);
            payloadHeight.Points.DataBindXY(timeList, payloadHeightList);
        }

        private void tempGraphInitilazed(object sender, EventArgs e)
        {
            Console.WriteLine("Görev yükü sıckalık grafiği başlatıldı");
            List<double> voltList = new List<double>();
            for (double i = 3.6; i > 2.6; i = i - 0.1)
            {
                voltList.Add(i);
            }

            tempChart.Titles.Add("Görev Yükü Sıcaklık");
            TitleCollection t = tempChart.Titles;
            t[0].ForeColor = Color.White;

            tempArea.BackColor = Color.FromArgb(31, 30, 29);
            tempArea.BorderWidth = 1;

            tempPoints.Color = myYellow;
            tempPoints.ChartType = SeriesChartType.FastLine;
            tempPoints.BorderWidth = 3;

            tempArea.AxisX.Title = "Zaman";
            tempArea.AxisX.TitleForeColor = Color.White;
            tempArea.AxisX.MajorGrid.Enabled = false;
            tempArea.AxisX.LineColor = Color.White;
            tempArea.AxisX.LabelStyle.ForeColor = Color.White;
            tempArea.AxisX.MajorTickMark.LineWidth = 2;
            tempArea.AxisX.MajorTickMark.LineColor = Color.White;

            tempArea.AxisY.Title = "Celsius(C°)";
            tempArea.AxisY.TitleForeColor = Color.White;
            tempArea.AxisY.MajorGrid.Enabled = false;
            tempArea.AxisY.LabelStyle.ForeColor = Color.White;
            tempArea.AxisY.LineColor = Color.White;
            tempArea.AxisY.LabelStyle.ForeColor = Color.White;
            tempArea.AxisY.MajorTickMark.LineWidth = 2;
            tempArea.AxisY.MajorTickMark.LineColor = Color.White;


            tempArea.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None;
            tempArea.AxisY.Minimum = 0;
            tempArea.AxisY.Maximum = 4;
            tempArea.AxisX.Interval = 1.0;

            tempPoints.Points.DataBindXY(timeList, voltList);
        }

        private void speedGraphInitilazed(object sender, EventArgs e)
        {
            Console.WriteLine("Görev yükü hız grafiği başlatıldı");
            List<double> voltList = new List<double>();
            for (double i = 3.6; i > 2.6; i = i - 0.1)
            {
                voltList.Add(i);
            }

            speedChart.Titles.Add("Görev Yükü Düşey Hız");
            TitleCollection t = speedChart.Titles;
            t[0].ForeColor = Color.White;

            speedArea.BackColor = Color.FromArgb(31, 30, 29);
            speedArea.BorderWidth = 1;

            speedPoints.Color = myBlue;
            speedPoints.ChartType = SeriesChartType.FastLine;
            speedPoints.BorderWidth = 3;

            speedArea.AxisX.Title = "Zaman";
            speedArea.AxisX.TitleForeColor = Color.White;
            speedArea.AxisX.MajorGrid.Enabled = false;
            speedArea.AxisX.LineColor = Color.White;
            speedArea.AxisX.LabelStyle.ForeColor = Color.White;
            speedArea.AxisX.MajorTickMark.LineWidth = 2;
            speedArea.AxisX.MajorTickMark.LineColor = Color.White;

            speedArea.AxisY.Title = "Metre/Saniye(m/s)";
            speedArea.AxisY.TitleForeColor = Color.White;
            speedArea.AxisY.MajorGrid.Enabled = false;
            speedArea.AxisY.LabelStyle.ForeColor = Color.White;
            speedArea.AxisY.LineColor = Color.White;
            speedArea.AxisY.LabelStyle.ForeColor = Color.White;
            speedArea.AxisY.MajorTickMark.LineWidth = 2;
            speedArea.AxisY.MajorTickMark.LineColor = Color.White;


            speedArea.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None;
            speedArea.AxisY.Minimum = 0;
            speedArea.AxisY.Maximum = 4;
            speedArea.AxisX.Interval = 1.0;


            speedPoints.Points.DataBindXY(timeList, voltList);
        }

        private void voltGraphInitlazed(object sender, EventArgs e)
        {
            Console.WriteLine("Görev yükü güç grafiği başlatıldı");
            List<double> voltList = new List<double>();
            for (double i = 3.6; i > 2.6; i=i-0.1)
            {
                voltList.Add(i);
            }


            voltChart.ForeColor = Color.White;
            voltChart.Titles.Add("Görev Yükü Batarya Seviyesi");
            TitleCollection t = voltChart.Titles;
            t[0].ForeColor = Color.White;

            voltArea.BackColor = Color.FromArgb(31, 30, 29);
            voltArea.BorderWidth = 1;

            voltPoints.Color = myPink;
            voltPoints.ChartType = SeriesChartType.FastLine;
            voltPoints.BorderWidth = 3;

            voltArea.AxisX.Title = "Zaman";
            voltArea.AxisX.TitleForeColor = Color.White;
            voltArea.AxisX.MajorGrid.Enabled = false;
            voltArea.AxisX.LineColor = Color.White;
            voltArea.AxisX.LabelStyle.ForeColor = Color.White;
            voltArea.AxisX.MajorTickMark.LineWidth = 2;
            voltArea.AxisX.MajorTickMark.LineColor = Color.White;

            voltArea.AxisY.Title = "Volt(V)";
            voltArea.AxisY.TitleForeColor = Color.White;
            voltArea.AxisY.MajorGrid.Enabled = false;
            voltArea.AxisY.LabelStyle.ForeColor = Color.White;
            voltArea.AxisY.LineColor = Color.White;
            voltArea.AxisY.LabelStyle.ForeColor = Color.White;
            voltArea.AxisY.MajorTickMark.LineWidth = 2;
            voltArea.AxisY.MajorTickMark.LineColor = Color.White;


            voltArea.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None;
            voltArea.AxisY.Minimum = 0;
            voltArea.AxisY.Maximum = 4;
            voltArea.AxisX.Interval = 1.0;


            voltPoints.Points.DataBindXY(timeList, voltList);
        }


        private void motionGraphInitilazed(object sender, EventArgs e)
        {
            Console.WriteLine("Görev yükü motion grafiği başlatıldı");
            List<double> rollList = new List<double>();
            for (int i = 0; i < 10; i++)
            {
                rollList.Add(i*0.1);
            }

            List<double> yawList = new List<double>();
            for (int i = 10; i > 0; i--)
            {
                yawList.Add(i*0.2);
            }

            List<double> pitchList = new List<double>();
            for (int i = 10; i > 0; i--)
            {
                pitchList.Add(i * 0.3);
            }

            motionChart.Titles.Add("Yön Bilgileri");
            TitleCollection t = motionChart.Titles;
            t[0].ForeColor = Color.White;

            #region Height Area Setup
            motionArea.BackColor = Color.FromArgb(31, 30, 29);
            motionArea.BorderWidth = 1;

            motionArea.AxisX.Title = "Zaman";
            motionArea.AxisX.TitleForeColor = Color.White;
            motionArea.AxisX.MajorGrid.Enabled = false;
            motionArea.AxisX.LineColor = Color.White;
            motionArea.AxisX.LabelStyle.ForeColor = Color.White;
            motionArea.AxisX.LabelAutoFitMaxFontSize = 5;
            motionArea.AxisX.MajorTickMark.LineWidth = 2;
            motionArea.AxisX.MajorTickMark.LineColor = Color.White;

            motionArea.AxisY.Title = "Derece";
            motionArea.AxisY.TitleForeColor = Color.White;
            motionArea.AxisY.MajorGrid.Enabled = false;
            motionArea.AxisY.LabelAutoFitMaxFontSize = 5;
            motionArea.AxisY.LabelStyle.ForeColor = Color.White;
            motionArea.AxisY.LineColor = Color.White;
            motionArea.AxisY.LabelStyle.ForeColor = Color.White;
            motionArea.AxisY.MajorTickMark.LineWidth = 2;
            motionArea.AxisY.MajorTickMark.LineColor = Color.White;

            motionArea.AxisY.LabelAutoFitStyle = LabelAutoFitStyles.None;
            motionArea.AxisY.Minimum = -20;
            motionArea.AxisY.Maximum = 20;
            motionArea.AxisY.Interval = 15;
            motionArea.AxisX.Interval = 2.0;
            #endregion

            #region Height Series Setup
            Series yaw = new Series();
            Series pitch = new Series();
            Series roll = new Series();
            motionChart.Series.Add(yaw);
            motionChart.Series.Add(pitch);
            motionChart.Series.Add(roll);

            yaw.Color = myBlue;
            yaw.ChartType = SeriesChartType.FastLine;
            yaw.BorderWidth = 3;

            roll.Color = myYellow;
            roll.ChartType = SeriesChartType.FastLine;
            roll.BorderWidth = 3;

            pitch.Color = myPink;
            pitch.ChartType = SeriesChartType.FastLine;
            pitch.BorderWidth = 3;
            #endregion

            yaw.Points.DataBindXY(timeList, yawList);
            pitch.Points.DataBindXY(timeList, pitchList);
            roll.Points.DataBindXY(timeList, rollList);
        }
        #endregion

        private void startConnection_Click(object sender, RoutedEventArgs e)
        {
            if (!isConnectedToPayload)
            {
                Console.WriteLine("Bağlantı başlatıldı");
                isConnectedToPayload = true;
            }
            
        }

        private void stopConnection_Click(object sender, RoutedEventArgs e)
        {
            if (isConnectedToPayload)
            {
                Console.WriteLine("Bağlantı durduruldu");
                isConnectedToPayload = false;
            }
        }

        private void sendVideo_Click(object sender, RoutedEventArgs e)
        {
            

        }

        void FinalVideo_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap video = (Bitmap)eventArgs.Frame.Clone();
            pictureBox.Image = video;

        }

        private void startVideo()
        {
            Console.WriteLine("[INFO]: KAMERA BAŞLATILDI");
            try
            {
                string video_ip = "http://192.168.0.20:8000/camera/mjpeg";
                JPEGStream stream = new JPEGStream(video_ip);
                stream.NewFrame += new NewFrameEventHandler(video_NewFrame);
                //videoPlayer.SignalToStop();
                //videoPlayer.WaitForStop();
                //videoPlayer.VideoSource = stream;
                //videoPlayer.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("[ERROR : ]"+ex.Message);
            }
            

            
        }

        private void video_NewFrame(object sender, NewFrameEventArgs eventArgs)
        {
            Bitmap bitmap = eventArgs.Frame;
            if (bitmap != null)
            {
                try
                {
                    Console.WriteLine("Görüntü işle");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message + " TO SAVE IMAGE FRAME");

                }

            }
        }

        private void VideoPlayer_NewFrame(object sender, ref System.Drawing.Bitmap image)
        {
           
        }

        private void seperatePayload_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Ayrılma komutu gönderildi!!");
        }

        private void Map_Loaded(object sender, EventArgs e)
        {
            Console.WriteLine("Harita dolduruldu!");
        }

        private void DropVideo(object sender, MouseButtonEventArgs e)
        {

        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {

        }

        private void DropVideoToMedia(object sender, DragEventArgs e)
        {

        }

        private void ProgramClosed(object sender, EventArgs e)
        {

        }

        private void MapInitilazed(object sender, EventArgs e)
        {
            Console.WriteLine("MAP HAS INITILAZED!");
            GMapProviders.GoogleMap.ApiKey = "AIzaSyBVGE_WK-JzMB3-i5ntWH1bXqJ0TIGCHK4";
            mapView.MapProvider = GMapProviders.GoogleMap;
            GMaps.Instance.Mode = AccessMode.ServerAndCache;

            mapView.MaxZoom = 70;
            mapView.MinZoom = 5;
            mapView.Zoom = 15;
            mapView.ShowCenter = true;
            mapView.DragButton = MouseButton.Left;
            mapView.Position = new PointLatLng(41.022181615692155, 29.008275402666367);
            //setGroundMarker(38.39818, 33.71133);
        }

        private void ModelInitilazed(object sender, EventArgs e)
        {
            ModelVisual3D device3D = new ModelVisual3D();
            device3D.Content = Display3d(@"C:\Users\faruk\source\repos\2022MuyGroundStation\2022MuyGroundStation\3Dmodels\simualtion_model.obj");
            modelSatellite3D.Children.Add(device3D);
            CameraController control = new CameraController();
            //c_viewPort.ZoomExtents(-1000);
            modelSatellite3D.ZoomExtentsWhenLoaded = true;
        }

        private Model3D Display3d(string model)
        {
            Model3D device = null;
            try
            {
                Console.WriteLine("Hello!!!");
                modelSatellite3D.RotateGesture = new MouseGesture(MouseAction.LeftClick);
                ModelImporter import = new ModelImporter();
                Material material = new DiffuseMaterial(new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.Blue));
                import.DefaultMaterial = material;
                device = import.Load(model);

            }
            catch (Exception e)
            {
                MessageBox.Show("Exception Error : " + e.StackTrace);
            }
            return device;
        }

    }
}
