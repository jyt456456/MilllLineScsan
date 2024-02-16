using Matrox.MatroxImagingLibrary;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;

namespace MillLineScan
{
    public partial class Form1 : Form
    {

        private List<MIL_ID> mILLBufferList = new List<MIL_ID>();
        private int Buffering_size_max = 22;
        private int Width =0;
        private int Height = 0;
        private int CurPage =1;
        private int FrameCount =0;

        private int NumberOfChannels;
        private MIL_ID ImageId;
        private Bitmap CurBitmap;

        MIL_DIG_HOOK_FUNCTION_PTR ProcessingFunctionPtr;

        MIL_ID MilApplication = MIL.M_NULL;
        MIL_ID MilSystem = MIL.M_NULL;
        MIL_ID MilDigitizer = MIL.M_NULL;
        MIL_ID MilDisplay = MIL.M_NULL;
        MIL_ID MilImageDisp = MIL.M_NULL;
        MIL_ID[] MilGrabBufferList;
        int MilGrabBufferListSize = 0;
        MIL_INT ProcessFrameCount = 0;
        double ProcessFrameRate = 0;
        private List<Bitmap> TotalBitmap;

        public class HookDataStruct
        {
            public MIL_ID MilDigitizer;
            public MIL_ID MilImageDisp;
            public int ProcessedImageCount;
        };

        // count 박스추가, x,y 값 설정하는 창 추가, 


        public Form1()
        {
            InitializeComponent();
            PrevButton.Enabled = false;
            NextButton.Enabled = false;

        }

        private void LineScan()
        {
            Buffering_size_max = int.Parse(FrameCountTextBox.Text);
            Width = int.Parse(WdthTextbox.Text);
            MilGrabBufferList = new MIL_ID[Buffering_size_max];
            HookDataStruct UserHookData = new HookDataStruct();

            // Allocate defaults.
            MIL.MappAllocDefault(MIL.M_DEFAULT, ref MilApplication, ref MilSystem, ref MilDisplay,
                                                        ref MilDigitizer, MIL.M_NULL);

            MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_PRINT_DISABLE);
            for (MilGrabBufferListSize = 0; MilGrabBufferListSize < Buffering_size_max; MilGrabBufferListSize++)
            {
                //MIL.MdigInquire(MilDigitizer, MIL.M_SIZE_X, MIL.M_NULL)
                //MIL.MdigInquire(MilDigitizer, MIL.M_SIZE_Y, MIL.M_NULL)
                MIL.MbufAlloc2d(MilSystem,
                                Width,
                                int.Parse(HeightTextbox.Text),
                                8 + MIL.M_UNSIGNED,
                                MIL.M_IMAGE + MIL.M_GRAB + MIL.M_PROC,
                                ref MilGrabBufferList[MilGrabBufferListSize]);

                if (MilGrabBufferList[MilGrabBufferListSize] != MIL.M_NULL)
                {
                    MIL.MbufClear(MilGrabBufferList[MilGrabBufferListSize], 0xFF);
                }
                else
                {
                    break;
                }
            }
            MIL.MappControl(MIL.M_DEFAULT, MIL.M_ERROR, MIL.M_PRINT_ENABLE);

            UserHookData.MilDigitizer = MilDigitizer;
            UserHookData.MilImageDisp = MilImageDisp;
            UserHookData.ProcessedImageCount = 0;

            // get a handle to the HookDataStruct object in the managed heap, we will use this 
            // handle to get the object back in the callback function
            GCHandle hUserData = GCHandle.Alloc(UserHookData);
            ProcessingFunctionPtr = new MIL_DIG_HOOK_FUNCTION_PTR(ProcessingFunction);

            // Start the processing. The processing function is called with every frame grabbed.
            MIL.MdigProcess(MilDigitizer, MilGrabBufferList, MilGrabBufferListSize, MIL.M_START, MIL.M_DEFAULT, ProcessingFunctionPtr, GCHandle.ToIntPtr(hUserData));

            // Here the main() is free to perform other tasks while the processing is executing.
            // ---------------------------------------------------------------------------------
            //MIL.Merge
            // Print a message and wait for a key press after a minimum number of frames.
            Console.WriteLine("Press <Enter> to stop.");
            Console.WriteLine();
            // Console.ReadKey();


        }

        private MIL_INT ProcessingFunction(MIL_INT HookType, MIL_ID HookId, IntPtr HookDataPtr)
        {
            MIL_ID ModifiedBufferId = MIL.M_NULL;

            // this is how to check if the user data is null, the IntPtr class
            // contains a member, Zero, which exists solely for this purpose
            if (!IntPtr.Zero.Equals(HookDataPtr))
            {
                // get the handle to the DigHookUserData object back from the IntPtr
                GCHandle hUserData = GCHandle.FromIntPtr(HookDataPtr);

                // get a reference to the DigHookUserData object
                HookDataStruct UserData = hUserData.Target as HookDataStruct;

                // Retrieve the MIL_ID of the grabbed buffer.
                MIL.MdigGetHookInfo(HookId, MIL.M_MODIFIED_BUFFER + MIL.M_BUFFER_ID, ref ModifiedBufferId);
                //ModifiedBufferI

                if (mILLBufferList.Count < Buffering_size_max)
                {
                    mILLBufferList.Add(ModifiedBufferId);
                }
                else
                {
                    MIL.MdigProcess(MilDigitizer, MilGrabBufferList, MilGrabBufferListSize, MIL.M_STOP, MIL.M_DEFAULT, ProcessingFunctionPtr, GCHandle.ToIntPtr(hUserData));


                    // Stop the processing.
                    //

                    // Free the GCHandle when no longer used
                    hUserData.Free();

                    // Print statistics.
                    MIL.MdigInquire(MilDigitizer, MIL.M_PROCESS_FRAME_COUNT, ref ProcessFrameCount);
                    MIL.MdigInquire(MilDigitizer, MIL.M_PROCESS_FRAME_RATE, ref ProcessFrameRate);
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("{0} frames grabbed at {1:0.0} frames/sec ({2:0.0} ms/frame).", ProcessFrameCount, ProcessFrameRate, 1000.0 / ProcessFrameRate);
                    Console.WriteLine("Press <Enter> to end.");
                    Console.WriteLine();
                    //Console.ReadKey();

                    // Free the grab buffers.
                    while (MilGrabBufferListSize > 0)
                    {
                        MIL.MbufFree(MilGrabBufferList[--MilGrabBufferListSize]);
                    }

                    // Free display buffer.
                    MIL.MbufFree(MilImageDisp);

                    // Release defaults.
                    MIL.MappFreeDefault(MilApplication, MilSystem, MilDisplay, MilDigitizer, MIL.M_NULL);
                }

                // Increment the frame counter.
                UserData.ProcessedImageCount++;

                // Print and draw the frame count (remove to reduce CPU usage).
                Console.Write("Processing frame #{0}.\r", UserData.ProcessedImageCount);
                //MIL.MgraText(MIL.M_DEFAULT, ModifiedBufferId, STRING_POS_X, STRING_POS_Y, String.Format("{0}", UserData.ProcessedImageCount));

                // Execute the processing and update the display.
                //MIL.MimArith(ModifiedBufferId, MIL.M_NULL, UserData.MilImageDisp, MIL.M_NOT);
            }

            return 0;
        }

        private void PrevButton_Click(object sender, EventArgs e)
        {
            --CurPage;
            PageLabel.Text = CurPage.ToString();

            if (CurPage == 2)
            {
                PrevButton.Enabled = false;
            }
            else
            {
                if(CurPage == mILLBufferList.Count -1)
                {
                   NextButton.Enabled = true;
                }
            }
            
            ChangeImage(mILLBufferList[CurPage - 1]);
        }

        private void NextButton_Click(object sender, EventArgs e)
        {          
            ++CurPage;
            PageLabel.Text = CurPage.ToString();

            if (CurPage == 2)
            {
                PrevButton.Enabled = true;
            }
            else if (CurPage == mILLBufferList.Count)
            {
                NextButton.Enabled = false;
            }

            ChangeImage(mILLBufferList[CurPage - 1]);
        }

        private void PlayButton_Click(object sender, EventArgs e)
        {
            mILLBufferList.Clear();

            LineScan();

        }

        private void SaveButton_Click(object sender, EventArgs e)
        {

            for (int i = 0; i < mILLBufferList.Count; ++i)
            {
                MIL.MbufSave($"C:\\temp\\{i}.bmp", mILLBufferList[i]);
                TotalBitmap.Add(ToBitmap(mILLBufferList[i]));
            }

            Bitmap result =  CombineImagesVertically();
            result.Save($"C:\\temp\\result.bmp");

        }

        private void HeightTextbox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void FrameCountTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ChangeImage(MIL_ID milId)
        {
            //Image Change
           // ImageId = 
            CurBitmap = ToBitmap(milId);
            panel1.BackgroundImage = CurBitmap;
            panel1.BackgroundImageLayout = ImageLayout.Zoom;
        }

        public Bitmap ToBitmap(MIL_ID milId)
        {
            MIL_INT numBand = 0;
            MIL.MbufInquire(milId, MIL.M_SIZE_BAND, ref numBand);

            NumberOfChannels = (int)numBand;
            //return (int)numBand;


            byte[] data = new byte[Width * Height * NumberOfChannels];
            Get(data);

            return CreateBitmap(Width, Height, Width, NumberOfChannels, data);
        }

        public void Put(byte[] userArrayPtr, bool isPacked = false)
        {
            if (ImageId == MIL.M_NULL)
            {
                throw new InvalidOperationException("[MilImage.Get] Image is null");
            }

            if (isPacked && NumberOfChannels == 3)
                MIL.MbufPutColor2d(ImageId, MIL.M_PACKED + MIL.M_RGB24, MIL.M_ALL_BANDS, 0, 0, Width, Height, userArrayPtr); // RGBRGBRGB.........
            else
                MIL.MbufPut2d(ImageId, 0, 0, Width, Height, userArrayPtr);
        }

        public void Get(byte[] userArrayPtr, bool isPacked = false)
        {
            if (ImageId == MIL.M_NULL)
            {
                throw new InvalidOperationException("[MilImage.Get] Image is null");
            }

            if (isPacked && NumberOfChannels == 3)
                MIL.MbufGetColor2d(ImageId, MIL.M_PACKED + MIL.M_RGB24, MIL.M_ALL_BANDS, 0, 0, Width, Height, userArrayPtr); // RGBRGBRGB.........
            else
                MIL.MbufGet2d(ImageId, 0, 0, Width, Height, userArrayPtr); // RRR...GGG...BBB...
        }

        public Bitmap CreateBitmap(int width, int height, int pitch, int numBand, byte[] imageData)
        {
            Debug.Assert(imageData != null);

            Color[] pallet = null;
            PixelFormat pixelFormat;
            if (numBand == 4)
            {
                pixelFormat = PixelFormat.Format32bppPArgb;
            }
            else if (numBand == 3)
            {
                pixelFormat = System.Drawing.Imaging.PixelFormat.Format24bppRgb;
            }
            else if (numBand == 2)
            {
                pixelFormat = System.Drawing.Imaging.PixelFormat.Format16bppRgb565;
            }
            else
            {
                pixelFormat = System.Drawing.Imaging.PixelFormat.Format8bppIndexed;

                pallet = new Color[256];
                for (int i = 0; i < 256; i++)
                {
                    pallet[i] = Color.FromArgb(i, i, i);
                }
            }

            var bitmap = new Bitmap(width, height, pixelFormat);
            if (pallet != null)
            {
                ColorPalette cp = bitmap.Palette;
                Array.Copy(pallet, cp.Entries, Math.Min(pallet.Length, bitmap.Palette.Entries.Length));
                bitmap.Palette = cp;
            }

            BitmapData bmpData = bitmap.LockBits(new Rectangle(0, 0, width, height), ImageLockMode.ReadOnly, pixelFormat);
            for (int y = 0; y < height; y++)
            {
                Marshal.Copy(imageData, y * pitch, new IntPtr(bmpData.Scan0.ToInt64() + y * bmpData.Stride), bmpData.Stride);
            }

            bitmap.UnlockBits(bmpData);
            return bitmap;
        }

        public Bitmap CombineImagesVertically()
        {
            //Bitmap[] images = new Bitmap[mILLBufferList.Count];
            
            // 이미지를 세로로 합치기 위한 총 높이를 계산합니다.
            int totalHeight = 0;
            foreach (Bitmap image in TotalBitmap)
            {
                totalHeight += image.Height;
            }

            // 새로운 비트맵 이미지를 생성합니다.
            Bitmap combinedImage = new Bitmap(TotalBitmap[0].Width, totalHeight);

            using (Graphics g = Graphics.FromImage(combinedImage))
            {
                int y = 0;
                foreach (Bitmap image in TotalBitmap)
                {
                    // 각 이미지를 세로로 합칩니다.
                    g.DrawImage(image, new Rectangle(0, y, image.Width, image.Height));
                    y += image.Height;
                }
            }

            return combinedImage;
        }
    }
}
