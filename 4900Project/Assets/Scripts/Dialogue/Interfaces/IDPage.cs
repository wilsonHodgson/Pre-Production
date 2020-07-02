﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dialogue
{
    /// <summary>
    /// The Dialogue Page interface. Every Dialogue page has text and an array of buttons to be shown.
    /// </summary>
    public interface IDPage
    {
        /// <summary>
        /// The text that the page should display.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// The list of buttons to show with the page.
        /// </summary>
        IEnumerable<IDButton> Buttons { get; set; }

        IDButton GetButton(int buttonIndex);
    }
}