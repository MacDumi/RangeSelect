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
        [Description("Minimum value of the slider.")]
        public int Min
        {
            get { return min; }
            set { min = value; Invalidate(); }
        }
        int min = 0;
        /// <summary>
        /// Maximum value of the slider.
        /// </summary>
        [Description("Maximum value of the slider.")]
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
        SolidBrush brush = new SolidBrush(Color.FromArgb(255, Color.Green));
        /// <summary>
        /// Fired when the selected range changes.
        /// </summary>
        [Description("Fired when the selceted range changes.")]
        public event EventHandler SelectionChanged;
  

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

        void SelectionRangeSlider_MouseDown(object sender, MouseEventArgs e)
        {
            //check where the user clicked so we can decide which thumb to move
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

        void SelectionRangeSlider_MouseMove(object sender, MouseEventArgs e)
        {
            //if the left button is pushed, move the selected thumb
            if (e.Button != MouseButtons.Left)
                return;
            int pointedValue = Min + e.X * (Max - Min) / Width;
            if (movingMode == MovingMode.MovingMin)
            {
                SelectedMin = pointedValue;
                labelMin.Text = SelectedMin.ToString();
            }
            else if(movingMode == MovingMode.MovingMax)
            {
                SelectedMax = pointedValue;
                labelMax.Text = SelectedMax.ToString();
            }

        }

        /// <summary>
        /// Enumeration for the slider edge
        /// </summary>
        enum MovingMode {MovingMin, MovingMax }
        MovingMode movingMode;
    }
}