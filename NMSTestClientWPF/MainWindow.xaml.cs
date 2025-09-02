using System;
using System.Collections.Generic;
using System.ServiceModel.Channels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using FTN.Common;
using FTN.Services.NetworkModelService.TestClient;
using TelventDMS.Services.NetworkModelService.TestClient.Tests;

namespace NMSTestClientWPF
{
    public partial class MainWindow : Window
    {
        private TestGda testGda;
        private Dictionary<long, string> modelCodeDescriptions;
        private Dictionary<short, string> dmsTypeDescriptions;

        private static readonly Dictionary<string, Dictionary<string, ModelCode>> ModelCodeAttributeMap =
    new Dictionary<string, Dictionary<string, ModelCode>>
    {
        ["0x1200000000010000"] = new Dictionary<string, ModelCode> // PID
    {
        { "B", ModelCode.PID_B },
        { "R", ModelCode.PID_R },
        { "SEQUENCENUMBER", ModelCode.PID_SEQUENCENUMBER },
        { "X", ModelCode.PID_X },
        { "PHASEIMPEDANCE", ModelCode.PID_PHASEIMPEDANCE }
    },
        ["0x1120000000020000"] = new Dictionary<string, ModelCode> // PLSI
    {
        { "B0CH", ModelCode.PLSI_B0CH },
        { "BCH", ModelCode.PLSI_BCH },
        { "G0CH", ModelCode.PLSI_G0CH },
        { "GCH", ModelCode.PLSI_GCH },
        { "R", ModelCode.PLSI_R },
        { "R0", ModelCode.PLSI_R0 },
        { "X", ModelCode.PLSI_X },
        { "X0", ModelCode.PLSI_X0 }
    },
        ["0x1311120000050000"] = new Dictionary<string, ModelCode> // ACLS
    {
        { "B0CH", ModelCode.ACLS_B0CH },
        { "BCH", ModelCode.ACLS_BCH },
        { "G0CH", ModelCode.ACLS_G0CH },
        { "GCH", ModelCode.ACLS_GCH },
        { "R", ModelCode.ACLS_R },
        { "R0", ModelCode.ACLS_R0 },
        { "X", ModelCode.ACLS_X },
        { "X0", ModelCode.ACLS_X0 },
        { "PERLENGTHIMPEDANCE", ModelCode.ACLS_PERLENGTHIMPEDANCE }
    },
        ["0x1311200000040000"] = new Dictionary<string, ModelCode> // SC
    {
        { "R", ModelCode.SC_R },
        { "R0", ModelCode.SC_R0 },
        { "X", ModelCode.SC_X },
        { "X0", ModelCode.SC_X0 }
    },
        ["0x1400000000060000"] = new Dictionary<string, ModelCode> // TERM
    {
        { "CONDUCTINGEQUIPMENT", ModelCode.TERM_CONDUCTINGEQUIPMENT }
    },
        ["0x1110000000070000"] = new Dictionary<string, ModelCode> // PLPI
    {
        { "CONDUCTORCOUNT", ModelCode.PLPI_CONDUCTORCOUNT },
        { "PHASEIMPEDANCEDATAS", ModelCode.PLPI_PHASEIMPEDANCEDATAS }
    }
    };

        public MainWindow()
        {
            InitializeComponent();
        }

        //GET VALUES
        private static long InputGlobalId(string strId)
        {
            CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Entering globalId started.");

            try
            {
                if (strId.StartsWith("0x", StringComparison.Ordinal))
                {
                    strId = strId.Remove(0, 2);
                    CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Entering globalId successfully ended.");

                    return Convert.ToInt64(Int64.Parse(strId, System.Globalization.NumberStyles.HexNumber));
                }
                else
                {
                    CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Entering globalId successfully ended.");
                    return Convert.ToInt64(strId);
                }
            }
            catch (FormatException ex)
            {
                string message = "Entering entity id failed. Please use hex (0x) or decimal format.";
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                throw ex;
            }
        }


        private void GetValues_Click(object sender, RoutedEventArgs e)
        {
            if (EntityTypeComboBox.SelectedItem == null)
            {
                GetValuesStatusLabel.Content = "Please select an entity type";
                GetValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Orange);
                return;
            }
            if (string.IsNullOrEmpty(PositionInputTextBox.Text))
            {
                GetValuesStatusLabel.Content = "Please enter a position";
                GetValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Orange);
                return;
            }
            TestGda tgda = new TestGda();
            try
            {
                string entityTypeTag = ((ComboBoxItem)EntityTypeComboBox.SelectedItem).Tag.ToString();
                string entityTypeName = ((ComboBoxItem)EntityTypeComboBox.SelectedItem).Content.ToString();
                string position = PositionInputTextBox.Text.Trim().PadLeft(2, '0'); // Pad position to 2 chars

                // Remove 0x prefix and add position
                string baseHex = entityTypeTag.Substring(2); // Remove "0x"
                string fullGidHex = $"0x{baseHex}{position}";

                Console.WriteLine("FULL GID: " + fullGidHex);
                tgda.GetValues(InputGlobalId(fullGidHex));
                GetValuesStatusLabel.Content = $"GetValues successful for {entityTypeName}";
                GetValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                GetValuesStatusLabel.Content = $"Error: {ex.Message}";
                GetValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
        }


        //Get extent values
        private static ModelCode InputModelCode(string userModelCode)
        {
            CommonTrace.WriteTrace(CommonTrace.TraceVerbose, "Entering Model Code started.");

            try
            {
                Console.Write("Enter Model Code: ");
                ModelCode modelCode = 0;

                if (!ModelCodeHelper.GetModelCodeFromString(userModelCode, out modelCode))
                {
                    if (userModelCode.StartsWith("0x", StringComparison.Ordinal))
                    {
                        modelCode = (ModelCode)long.Parse(userModelCode.Substring(2), System.Globalization.NumberStyles.HexNumber);
                    }
                    else
                    {
                        modelCode = (ModelCode)long.Parse(userModelCode);
                    }
                }

                return modelCode;
            }
            catch (Exception ex)
            {
                string message = string.Format("Entering Model Code failed. {0}", ex);
                CommonTrace.WriteTrace(CommonTrace.TraceError, message);
                Console.WriteLine(message);
                throw ex;
            }
        }
        private void LoadAttributes_Click(object sender, RoutedEventArgs e)
        {
            if (ExtentEntityTypeComboBox.SelectedItem == null) return;

            string modelCodeTag = ((ComboBoxItem)ExtentEntityTypeComboBox.SelectedItem).Tag.ToString();
            List<string> attributes = new List<string>();

            switch (modelCodeTag)
            {
                case "0x1200000000010000": // PID
                    attributes.AddRange(new[] { "B", "R", "SEQUENCENUMBER", "X", "PHASEIMPEDANCE" });
                    break;

                case "0x1120000000020000": // PLSI
                    attributes.AddRange(new[] { "B0CH", "BCH", "G0CH", "GCH", "R", "R0", "X", "X0" });
                    break;

                case "0x1311120000050000": // ACLS
                    attributes.AddRange(new[] { "B0CH", "BCH", "G0CH", "GCH", "R", "R0", "X", "X0", "PERLENGTHIMPEDANCE" });
                    break;

                case "0x1311200000040000": // SC
                    attributes.AddRange(new[] { "R", "R0", "X", "X0" });
                    break;

                case "0x1400000000060000": // TERM
                    attributes.Add("CONDUCTINGEQUIPMENT");
                    break;

                case "0x1110000000070000": // PLPI
                    attributes.AddRange(new[] { "CONDUCTORCOUNT", "PHASEIMPEDANCEDATAS" });
                    break;

                case "0x1311110000030000": // DCLS
                                           // No attributes
                    break;

                default:
                    // Wrong input
                    break;
            }

            DynamicCheckboxesPanel.Items.Clear();
            AttributesMessageLabel.Visibility = Visibility.Collapsed;

            if (attributes.Count > 0)
            {
                foreach (var attr in attributes)
                {
                    var checkbox = new CheckBox
                    {
                        Content = attr,
                        Margin = new Thickness(0, 5, 0, 0)
                    };
                    DynamicCheckboxesPanel.Items.Add(checkbox);
                }

                // Enable Get Extended Values button after attributes are loaded
                GetExtendedValuesButton.IsEnabled = true;
            }
            else
            {
                if (modelCodeTag == "0x1311110000030000") // DCLS
                {
                    AttributesMessageLabel.Content = "Model has no attributes.";
                    GetExtendedValuesButton.IsEnabled = true; // Still enable since it's valid
                }
                else
                {
                    AttributesMessageLabel.Content = "Invalid model code.";
                    GetExtendedValuesButton.IsEnabled = false;
                }

                AttributesMessageLabel.Visibility = Visibility.Visible;
            }
        }
        private void GetExtentValues_Click(object sender, RoutedEventArgs e)
        {
            TestGda tgda = new TestGda();
            string modelCodeString = ((ComboBoxItem)ExtentEntityTypeComboBox.SelectedItem).Tag.ToString();
            string entityTypeName = ((ComboBoxItem)ExtentEntityTypeComboBox.SelectedItem).Content.ToString();

            // Get selected attributes
            List<string> selectedAttributes = new List<string>();

            foreach (var item in DynamicCheckboxesPanel.Items)
            {
                if (item is CheckBox checkbox && checkbox.IsChecked == true)
                {
                    selectedAttributes.Add(checkbox.Content.ToString());
                }
            }

            if (!ModelCodeAttributeMap.ContainsKey(modelCodeString))
            {
                ExtendedValuesStatusLabel.Content = "Invalid model code.";
                ExtendedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            var attributesMap = ModelCodeAttributeMap[modelCodeString];
            var selectedModelCodes = new List<ModelCode>();

            foreach (string attr in selectedAttributes)
            {
                if (attributesMap.TryGetValue(attr, out ModelCode propCode))
                {
                    selectedModelCodes.Add(propCode);
                }
                else
                {
                    Console.WriteLine($"Unknown attribute: {attr}");
                }
            }

            try
            {
                var result = tgda.GetExtentValues(InputModelCode(modelCodeString), selectedModelCodes, 0); // Always position 0 (all)
                ExtendedValuesStatusLabel.Content = $"GetExtentValues successful for {entityTypeName}";
                ExtendedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                ExtendedValuesStatusLabel.Content = $"Error: {ex.Message}";
                ExtendedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
        }

        private void ExtentEntityTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Clear any existing attributes
            DynamicCheckboxesPanel.Items.Clear();
            AttributesMessageLabel.Visibility = Visibility.Collapsed;

            // Enable Load Attributes button when entity type is selected
            LoadAttributesButton.IsEnabled = ExtentEntityTypeComboBox.SelectedItem != null;

            // Disable Get Extended Values button until attributes are loaded
            GetExtendedValuesButton.IsEnabled = false;

            // Clear status
            ExtendedValuesStatusLabel.Content = "";
        }

        // GET RELATED VALUES
        private void PropertyIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This can be used to provide context-specific help or validation
            if (PropertyIdComboBox.SelectedItem is ComboBoxItem selectedItem)
            {
                string tag = selectedItem.Tag?.ToString();
                // You can add logic here to show relevant target types based on the property selected
            }
        }

        private void GetRelatedValues_Click(object sender, RoutedEventArgs e)
        {
            TestGda tgda = new TestGda();
            string sourceGidText = SourceGidTextBox.Text.Trim();

            if (string.IsNullOrEmpty(sourceGidText))
            {
                RelatedValuesStatusLabel.Content = "Please enter a source GID.";
                RelatedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            if (PropertyIdComboBox.SelectedItem == null)
            {
                RelatedValuesStatusLabel.Content = "Please select a Property ID.";
                RelatedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Red);
                return;
            }

            try
            {
                long sourceGid = InputGlobalId(sourceGidText);

                // Get the selected property ID string and convert using the correct hex values
                string propertyIdString = ((ComboBoxItem)PropertyIdComboBox.SelectedItem).Tag.ToString();

                // Parse the hex string to long first, then cast to ModelCode
                long propertyIdLong;
                if (propertyIdString.StartsWith("0x"))
                {
                    propertyIdLong = Convert.ToInt64(propertyIdString.Substring(2), 16);
                }
                else
                {
                    propertyIdLong = Convert.ToInt64(propertyIdString);
                }
                ModelCode propertyId = (ModelCode)propertyIdLong;

                // Get the selected target type
                ModelCode targetType = 0; // Default to base type
                if (TargetTypeComboBox.SelectedItem != null)
                {
                    string targetTypeString = ((ComboBoxItem)TargetTypeComboBox.SelectedItem).Tag.ToString();
                    if (targetTypeString != "0x0000000000000000") // Not "All Types"
                    {
                        long targetTypeLong;
                        if (targetTypeString.StartsWith("0x"))
                        {
                            targetTypeLong = Convert.ToInt64(targetTypeString.Substring(2), 16);
                        }
                        else
                        {
                            targetTypeLong = Convert.ToInt64(targetTypeString);
                        }
                        targetType = (ModelCode)targetTypeLong;
                    }
                }

                // Create Association
                Association association = new Association()
                {
                    PropertyId = propertyId,
                    Type = targetType,
                    Inverse = InverseCheckBox.IsChecked ?? false
                };

                // Debug output to verify values
                Console.WriteLine($"Source GID: {sourceGid}");
                Console.WriteLine($"Property ID: {propertyId} ({(long)propertyId})");
                Console.WriteLine($"Target Type: {targetType} ({(long)targetType})");
                Console.WriteLine($"Inverse: {association.Inverse}");
                var result = tgda.GetRelatedValues(sourceGid, association);
                RelatedValuesStatusLabel.Content = $"GetRelatedValues successful. Found {result.Count} related resources.";
                RelatedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                RelatedValuesStatusLabel.Content = $"Error: {ex.Message}";
                RelatedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Red);

                // Additional debug info
                Console.WriteLine($"Full exception: {ex}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"Inner exception: {ex.InnerException.Message}");
                }
            }
        }
    }
}