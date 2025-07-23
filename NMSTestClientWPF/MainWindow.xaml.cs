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
            TestGda tgda = new TestGda();
            string gidInput = GidInputTextBox.Text;
            try
            {
                tgda.GetValues(InputGlobalId(gidInput));
                GetValuesStatusLabel.Content = $"GetValues was successful for {gidInput}";
                GetValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Green);
            }
            catch (Exception ex)
            {
                GetValuesStatusLabel.Content = $"Error: {ex.Message}";
                GetValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Red);
            }
        }


        //Get extent values
        private void LoadAttributes_Click(object sender, RoutedEventArgs e)
        {
            string modelCode = ModelCodeTextBox.Text.Trim();
            List<string> attributes = new List<string>();
            switch (modelCode)
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
            }
            else
            {
                if (modelCode == "0x1311110000030000") // DCLS
                {
                    AttributesMessageLabel.Content = "Model has no attributes.";
                }
                else
                {
                    AttributesMessageLabel.Content = "Invalid model code.";
                }

                AttributesMessageLabel.Visibility = Visibility.Visible;
            }
        }
        private void GetExtentValues_Click(object sender, RoutedEventArgs e)
        {
            TestGda tgda = new TestGda();
            string modelCodeString = ModelCodeTextBox.Text.Trim();
            string position = PositionTextBox.Text.Trim();

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
                Console.WriteLine("Invalid model code.");
                return;
            }

            if (!Enum.TryParse<ModelCode>(modelCodeString, true, out ModelCode modelCode))
            {
                Console.WriteLine($"Model code {modelCodeString} is not a valid ModelCode enum.");
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

            var result = tgda.GetExtentValues(modelCode, selectedModelCodes);
        }
    }
}