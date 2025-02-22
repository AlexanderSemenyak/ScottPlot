﻿using System;

namespace ScottPlot.Plottable
{
    /// <summary>
    /// The scatter plot renders X/Y pairs as points and/or connected lines.
    /// Scatter plots can be extremely slow for large datasets, so use Signal plots in these situations.
    /// </summary>
    public class ScatterPlotDraggable : ScatterPlot, IDraggable
    {
        public new double[] Xs { get; private set; }
        public new double[] Ys { get; private set; }
        public int CurrentIndex { get; set; } = 0;

        /// <summary>
        /// Indicates whether scatter points are draggable in user controls.
        /// </summary>
        public bool DragEnabled { get; set; } = false;

        /// <summary>
        /// Indicates whether scatter points are horizontally draggable in user controls.
        /// </summary>
        public bool DragEnabledX { get; set; } = true;

        /// <summary>
        /// Indicates whether scatter points are vertically draggable in user controls.
        /// </summary>
        public bool DragEnabledY { get; set; } = true;

        /// <summary>
        /// Cursor to display while hovering over the scatter points if dragging is enabled.
        /// </summary>
        public ScottPlot.Cursor DragCursor { get; set; } = ScottPlot.Cursor.Crosshair;

        /// <summary>
        /// If dragging is enabled the points cannot be dragged more negative than this position
        /// </summary>
        public double DragXLimitMin = double.NegativeInfinity;

        /// <summary>
        /// If dragging is enabled the points cannot be dragged more positive than this position
        /// </summary>
        public double DragXLimitMax = double.PositiveInfinity;

        /// <summary>
        /// If dragging is enabled the points cannot be dragged more negative than this position
        /// </summary>
        public double DragYLimitMin = double.NegativeInfinity;

        /// <summary>
        /// If dragging is enabled the points cannot be dragged more positive than this position
        /// </summary>
        public double DragYLimitMax = double.PositiveInfinity;

        /// <summary>
        /// This event is invoked after the plot is dragged
        /// </summary>
        public event EventHandler Dragged = delegate { };

        /// <summary>
        /// Move a scatter point to a new coordinate in plot space.
        /// </summary>
        /// <param name="coordinateX">new X position</param>
        /// <param name="coordinateY">new Y position</param>
        /// <param name="fixedSize">This argument is ignored</param>
        public void DragTo(double coordinateX, double coordinateY, bool fixedSize)
        {
            if (!DragEnabled)
                return;

            if (coordinateX < DragXLimitMin) coordinateX = DragXLimitMin;
            if (coordinateX > DragXLimitMax) coordinateX = DragXLimitMax;
            if (coordinateX < DragYLimitMin) coordinateY = DragYLimitMin;
            if (coordinateX > DragYLimitMax) coordinateY = DragYLimitMax;

            if (DragEnabledX) Xs[CurrentIndex] = coordinateX;
            if (DragEnabledY) Ys[CurrentIndex] = coordinateY;

            Dragged(this, EventArgs.Empty);
        }

        /// <summary>
        /// Return True if a scatter point is within a certain number of pixels (snap) to the mouse
        /// </summary>
        /// <param name="coordinateX">mouse position (coordinate space)</param>
        /// <param name="coordinateY">mouse position (coordinate space)</param>
        /// <param name="snapX">snap distance (pixels)</param>
        /// <param name="snapY">snap distance (pixels)</param>
        /// <returns></returns>
        public bool IsUnderMouse(double coordinateX, double coordinateY, double snapX, double snapY)
        {
            bool test = false;
            for (int i = 0; i < PointCount; i++)
            {
                test = Math.Abs(Ys[i] - coordinateY) <= snapY && Math.Abs(Xs[i] - coordinateX) <= snapX;
                if (test)
                {
                    CurrentIndex = i;
                    return test;
                }
            }
            return test;
        }

        public ScatterPlotDraggable(double[] xs, double[] ys, double[] errorX = null, double[] errorY = null) : base(xs, ys, errorX, errorY)
        {
            this.Xs = xs;
            this.Ys = ys;
            this.XError = errorX;
            this.YError = errorY;
        }
    }
}
