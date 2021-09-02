using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

namespace RangeSelect
{
    public partial class SelectionRangeSlider : UserControl
    {
        /// <summary>
        /// Enumeration for the slider edge
        /// </summary>
        enum MovingMode { MovingMin, MovingMax }
        MovingMode movingMode;

        #region Properties
        /// <summary>
        /// Background Image.
        /// </summary>
        [Description("Background image of the range selector.")]
        public Image Image
        {
            get { return image; }
            set { image = value; Invalidate();}
        }
        Image image = null;

        /// <summary>
        /// Minimum value of the slider.
        /// </summary>
        [Description("Minimum value of the range selector.")]
        public int Min
        {
            get { return min; }
            set { min = value; Invalidate(); }
        }
        int min = 0;

        /// <summary>
        /// Maximum value of the slider.
        /// </summary>
        [Description("Maximum value of the range selector.")]
        public int Max
        {
            get { return max; }
            set { max = value; Invalidate(); }
        }
        int max = 100;

        /// <summary>
        /// Minimum value of the selection range.
        /// </summary>
        [Description("Minimum value of the selection range.")]
        public int SelectedMin
        {
            get { return selectedMin; }
            set
            {
                selectedMin = value;
                if (SelectionChanged != null)
                    SelectionChanged(this, null);
                labelMin.Text = value.ToString();
                Invalidate();
            }
        }
        int selectedMin = 0;

        /// <summary>
        /// Maximum value of the selection range.
        /// </summary>
        [Description("Maximum value of the selection range.")]
        public int SelectedMax
        {
            get { return selectedMax; }
            set
            {
                selectedMax = value;
                if (SelectionChanged != null)
                    SelectionChanged(this, null);
                labelMax.Text = value.ToString();
                Invalidate();
            }
        }
        int selectedMax = 100;
        /// <summary>
        /// Step.
        /// </summary>
        [Description("Step.")]
        public int Step
        {
            get { return step; }
            set
            {
                step = value;
                Invalidate();
            }
        }
        int step = 5;
        /// <summary>
        /// Label / Description of the range selector.
        /// </summary>
        [Description("Label / Description of the range selector.")]
        public string Label
        {
            get { return label; }
            set
            {
                label = value;
                labelDescription.Text = value;
                Invalidate();
            }
        }
        string label = "";
        /// <summary>
        /// Color of the background.
        /// </summary>
        [Description("Background color.")]
        public Color BackgroundColor
        {
            get { return bgBrush.Color; }
            set { bgBrush.Color = value; Invalidate(); }
        }
        SolidBrush bgBrush = new SolidBrush(Color.White);

        /// <summary>
        /// Color of the selected range.
        /// </summary>
        [Description("Selected range color.")]
        public Color SelectionColor
        {
            get { return brush.Color; }
            set { brush.Color = value; Invalidate(); }
        }
        SolidBrush brush = new SolidBrush(Color.FromArgb(255, Color.Red));

        /// <summary>
        /// Fired when the selected range changes.
        /// </summary>
        [Description("Fired when the selected range changes.")]
        public event EventHandler SelectionChanged;

        #endregion


        public SelectionRangeSlider()
        {
            InitializeComponent();
            //avoid flickering
            typeof(Panel).InvokeMember("DoubleBuffered",
            BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
             null, panelBG, new object[] { true });
            panelBG.Paint += new PaintEventHandler(SelectionRangeSlider_Paint);
            panelBG.MouseDown += new MouseEventHandler(SelectionRangeSlider_MouseDown);
            panelBG.MouseMove += new MouseEventHandler(SelectionRangeSlider_MouseMove);

            //change the text on labels
            labelMin.Text = min.ToString();
            labelMax.Text = max.ToString();
            labelDescription.Text = label;
        }

     
        #region Events

        // Paint event
        void SelectionRangeSlider_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;

            if (image != null)
            {
                //draw the image
                Bitmap bm = new Bitmap(panel.Width, panel.Height);
                using (Graphics gr = Graphics.FromImage(bm))
                {
                    gr.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    Rectangle dest_rect = new Rectangle(0, 0, panel.Width, panel.Height);
                    Rectangle source_rect = new Rectangle(0, 0, image.Width, image.Height);
                    e.Graphics.DrawImage(image, dest_rect, source_rect, GraphicsUnit.Pixel);
                }
                bm.Dispose();

            }
            else
            {
                //paint background in white
                e.Graphics.FillRectangle(bgBrush, ClientRectangle);
            }
            
            //paint selection range in blue
            Rectangle selectionRect = new Rectangle(
                (selectedMin - Min) * panel.Width / (Max - Min),
                0,
                (selectedMax - selectedMin) * panel.Width / (Max - Min),
                panel.Height);
            e.Graphics.FillRectangle(brush, selectionRect);           
        }

        // Mouse down event
        void SelectionRangeSlider_MouseDown(object sender, MouseEventArgs e)
        {
            //check where the user clicked so we can decide which side to move
            int pointedValue = Min + e.X * (Max - Min) / Width;
            int distMin = Math.Abs(pointedValue - SelectedMin);
            int distMax = Math.Abs(pointedValue - SelectedMax);
            int minDist = Math.Min(distMin, distMax);
            if (minDist == distMin)
                movingMode = MovingMode.MovingMin;
            else
                movingMode = MovingMode.MovingMax;
            //call this to refresh the position of the selected slider
            SelectionRangeSlider_MouseMove(sender, e);
        }

        // Mouse drag event
        void SelectionRangeSlider_MouseMove(object sender, MouseEventArgs e)
        {
            //if the left mouse button was pushed, move the selected slider
            if (e.Button != MouseButtons.Left)
                return;
            int pointedValue = Min + e.X * (Max - Min) / Width;
            int dStep = pointedValue % step;
            pointedValue = step * (pointedValue / step);
            if (dStep > step / 2) pointedValue += step;
            if (movingMode == MovingMode.MovingMin)
            {
                if (pointedValue >= SelectedMax)
                {
                    // we don't want a region of 0
                    pointedValue = SelectedMax - 1;
                    movingMode = MovingMode.MovingMax;
                }
                if (pointedValue < min) pointedValue = min;
                SelectedMin = pointedValue;
                labelMin.Text = SelectedMin.ToString();
            }
            else if(movingMode == MovingMode.MovingMax)
            {
                if (pointedValue <= SelectedMin)
                {
                    // we don't want a region of 0
                    pointedValue = SelectedMin + 1;
                    movingMode = MovingMode.MovingMin;
                }
                if (pointedValue > max) pointedValue = max;
                SelectedMax = pointedValue;
                labelMax.Text = SelectedMax.ToString();
            }

        }

        #endregion
    }
}