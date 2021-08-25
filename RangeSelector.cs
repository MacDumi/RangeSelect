using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Reflection;

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
            set { image = value; Invalidate(); }
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
                Invalidate();
            }
        }
        int selectedMax = 100;

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
        }

        #region Events
        // Paint event
        void SelectionRangeSlider_Paint(object sender, PaintEventArgs e)
        {
            var panel = sender as Panel;

            if (image != null)
            {
                //draw the image
                e.Graphics.DrawImage(image, ClientRectangle);
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
            //call this to refreh the position of the selected thumb
            SelectionRangeSlider_MouseMove(sender, e);
        }

        // Mouse drag event
        void SelectionRangeSlider_MouseMove(object sender, MouseEventArgs e)
        {
            //if the left mouse button was pushed, move the selected slider
            if (e.Button != MouseButtons.Left)
                return;
            int pointedValue = Min + e.X * (Max - Min) / Width;
            if (movingMode == MovingMode.MovingMin)
            {
                if (pointedValue >= SelectedMax)
                {
                    // we don't want a region of 0
                    pointedValue = SelectedMax - 1;
                    movingMode = MovingMode.MovingMax;
                }
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
                SelectedMax = pointedValue;
                labelMax.Text = SelectedMax.ToString();
            }

        }

        #endregion
    }
}