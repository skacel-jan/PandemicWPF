using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Pandemic.Views.CustomPanels
{
    public class OverlapCanvas : Canvas
    {
        public static readonly DependencyProperty OrientationProperty =
          DependencyProperty.Register("Orientation", typeof(Orientation),
          typeof(OverlapCanvas), new FrameworkPropertyMetadata(Orientation.Horizontal,
          FrameworkPropertyMetadataOptions.AffectsArrange));

        public static readonly DependencyProperty SpacingProperty =
          DependencyProperty.Register("Spacing", typeof(double),
          typeof(OverlapCanvas), new FrameworkPropertyMetadata(10d,
          FrameworkPropertyMetadataOptions.AffectsArrange));


        public static readonly DependencyProperty ReverseZIdnexProperty =
          DependencyProperty.Register("ReverseZIdnex", typeof(bool),
          typeof(OverlapCanvas), new FrameworkPropertyMetadata(false,
          FrameworkPropertyMetadataOptions.AffectsArrange));

        public Orientation Orientation
        {
            get { return (Orientation)GetValue(OrientationProperty); }
            set { SetValue(OrientationProperty, value); }
        }

        public double Spacing
        {
            get { return (double)GetValue(SpacingProperty); }
            set { SetValue(SpacingProperty, value); }
        }

        public bool ReverseZIdnex
        {
            get { return (bool)GetValue(ReverseZIdnexProperty); }
            set { SetValue(ReverseZIdnexProperty, value); }
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            foreach (UIElement child in this.Children)
            {
                // Give each child all the space it wants
                if (child != null)
                    child.Measure(new Size(Double.PositiveInfinity,
                                           Double.PositiveInfinity));
            }

            // The SimpleCanvas itself needs no space
            return new Size(0, 0);
        }


        protected override Size ArrangeOverride(Size finalSize)
        {
            // Center the children
            Point location = new Point(0, 0);

            int zIndex = 0;
            if (ReverseZIdnex)
            {
                zIndex = Children.Count;
            }

            foreach (UIElement child in this.Children)
            {
                if (child != null)
                {
                    // Give the child its desired size
                    child.Arrange(new Rect(location, child.DesiredSize));

                    // Update the offset and angle for the next child
                    if (Orientation == Orientation.Vertical)
                        location.Y += Spacing;
                    else
                        location.X += Spacing;

                    if (ReverseZIdnex)
                    {
                        SetZIndex(child, zIndex--);
                    }
                }
            }

            // Fill all the space given
            return finalSize;
        }
    }
}