using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel; // Add this using directive
using System.Collections.Generic; // Add this using directive for dictionary

namespace Chip_Inventory_System
{
    public class CustomFlagColumn : DataGridViewImageColumn
    {
        public CustomFlagColumn()
        {
            this.CellTemplate = new CustomFlagCell();
        }
    }

    public class CustomFlagCell : DataGridViewImageCell
    {
        // Create a dictionary to cache the loaded flag images
        private readonly Dictionary<string, Image> flagImageCache = new Dictionary<string, Image>();

        protected override object GetFormattedValue(object value, int rowIndex, ref DataGridViewCellStyle cellStyle, TypeConverter valueTypeConverter, TypeConverter formattedValueTypeConverter, DataGridViewDataErrorContexts context)
        {
            // Replace these conditions with your logic to determine the symbol and color
            string pickedValue = this.DataGridView.Rows[rowIndex].Cells["Picked"].Value?.ToString();
            string lotValue = this.DataGridView.Rows[rowIndex].Cells["Lot"].Value?.ToString();
            string lotStatusValue = this.DataGridView.Rows[rowIndex].Cells["Lot status"].Value?.ToString();

            // Get the flagKey based on your logic
            string flagKey = GetFlagKey(pickedValue, lotValue, lotStatusValue);

            // Load the flag image from cache or resources
            Image flagImage = LoadFlagImage(flagKey);

            return flagImage;
        }

        // Define a method to return the corresponding image for the flag
        public Image LoadFlagImage(string flagKey)
        {
            // Check if the image is already in the cache
            if (flagImageCache.ContainsKey(flagKey))
            {
                return flagImageCache[flagKey];
            }

            // Load the image from resources or use Font Awesome or other sources
            Image flagImage = null;
            if (flagImageCache.ContainsKey(flagKey))
            {
                flagImage = flagImageCache[flagKey];
            }
            else
            {
                switch (flagKey)
                {
                    case "greenFlag":
                        Icon greenFlagIcon = Properties.Resources.greenFlag;
                        flagImage = greenFlagIcon.ToBitmap(); // Convert the icon to an image
                        break;
                    case "orangeFlag":
                        Icon orangeFlagIcon = Properties.Resources.orangeFlag;
                        flagImage = orangeFlagIcon.ToBitmap(); // Convert the icon to an image
                        break;
                    case "redFlag":
                        Icon redFlagIcon = Properties.Resources.redFlag;
                        flagImage = redFlagIcon.ToBitmap(); // Convert the icon to an image
                        break;
                    case "blueFlag":
                        Icon blueFlagIcon = Properties.Resources.blueFlag;
                        flagImage = blueFlagIcon.ToBitmap(); // Convert the icon to an image
                        break;
                    default:
                        break;
                }

                // Cache the loaded image
                if (flagImage != null)
                {
                    flagImageCache[flagKey] = flagImage;
                }
            }

            return flagImage;

        }

        // Define a method to determine the flag key based on your logic
        public string GetFlagKey(string pickedValue, string lotValue, string lotStatusValue)
        {
            // Replace these conditions with your logic to determine the symbol and color
            string flagKey = "greenFlag"; // Default to greenFlag

            if (string.IsNullOrWhiteSpace(pickedValue) || string.IsNullOrWhiteSpace(lotValue))
            {
                flagKey = "orangeFlag";
            }
            if (string.IsNullOrWhiteSpace(pickedValue) && string.IsNullOrWhiteSpace(lotValue))
            {
                flagKey = "greenFlag";
            }
            if (!string.IsNullOrWhiteSpace(pickedValue) && !string.IsNullOrWhiteSpace(lotValue))
            {
                flagKey = "redFlag";
            }
            if (lotStatusValue == "Closed")
            {
                flagKey = "blueFlag";
            }
            return flagKey;
        }
    }
}
