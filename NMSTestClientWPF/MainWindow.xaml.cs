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
                string position = PositionInputTextBox.Text.Trim().PadLeft(2, '0'); 

                string baseHex = entityTypeTag.Substring(2); 
                string fullGidHex = $"0x{baseHex}{position}";

                var (rd, xmlContent) = tgda.GetValues(InputGlobalId(fullGidHex));
                tgda.GetValues(InputGlobalId(fullGidHex));
                GetValuesStatusLabel.Content = $"GetValues successful for {entityTypeName}";
                GetValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Green);

                XmlContentTextBlock.Text = xmlContent;
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
                var (result, xmlContent) = tgda.GetExtentValues(InputModelCode(modelCodeString), selectedModelCodes, 0);
                ExtendedValuesStatusLabel.Content = $"GetExtentValues successful for {entityTypeName}";
                ExtendedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Green);
                ExtentXmlContentTextBlock.Text = xmlContent;
            }
            catch (Exception ex)
            {
                ExtendedValuesStatusLabel.Content = $"Error: {ex.Message}";
                ExtendedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Red);
                ExtentXmlContentTextBlock.Text = $"Error occurred: {ex.Message}";
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
        private void SourceEntityTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RelatedPropertyComboBox.Items.Clear();
            RelatedTargetTypeComboBox.Items.Clear();
            RelatedPropertyComboBox.IsEnabled = false;
            RelatedTargetTypeComboBox.IsEnabled = false;
            GetRelatedValuesButton.IsEnabled = false;

            if (SourceEntityTypeComboBox.SelectedItem == null) return;

            string entityTypeName = ((ComboBoxItem)SourceEntityTypeComboBox.SelectedItem).Content.ToString();

            // Based on your diagram, populate available properties for each entity type
            switch (entityTypeName)
            {
                case "Terminal":
                    RelatedPropertyComboBox.Items.Add(new ComboBoxItem
                    {
                        Content = "ConductingEquipment",
                        Tag = "0x1400000000060109"
                    });
                    break;

                case "ACLineSegment":
                    RelatedPropertyComboBox.Items.Add(new ComboBoxItem
                    {
                        Content = "PerLengthImpedance",
                        Tag = "0x1311120000050909"
                    });
                    break;

                case "PerLengthPhaseImpedance":
                    RelatedPropertyComboBox.Items.Add(new ComboBoxItem
                    {
                        Content = "PhaseImpedanceDatas",
                        Tag = "0x1110000000070219"
                    });
                    break;

                case "PhaseImpedanceData":
                    RelatedPropertyComboBox.Items.Add(new ComboBoxItem
                    {
                        Content = "PerLengthPhaseImpedance",
                        Tag = "0x1200000000010509"
                    });
                    break;
            }

            RelatedPropertyComboBox.IsEnabled = RelatedPropertyComboBox.Items.Count > 0;
            if (RelatedPropertyComboBox.Items.Count > 0)
            {
                RelatedPropertyComboBox.SelectedIndex = 0;
            }
        }

        private void RelatedPropertyComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RelatedTargetTypeComboBox.Items.Clear();
            RelatedTargetTypeComboBox.IsEnabled = false;
            GetRelatedValuesButton.IsEnabled = false;

            if (RelatedPropertyComboBox.SelectedItem == null) return;

            string propertyContent = ((ComboBoxItem)RelatedPropertyComboBox.SelectedItem).Content.ToString();
            string sourceEntityType = ((ComboBoxItem)SourceEntityTypeComboBox.SelectedItem).Content.ToString();

            switch (propertyContent)
            {
                case "ConductingEquipment": 
                    RelatedTargetTypeComboBox.Items.Add(new ComboBoxItem { Content = "ACLineSegment", Tag = "0x1311120000050000" });
                    RelatedTargetTypeComboBox.Items.Add(new ComboBoxItem { Content = "SeriesCompensator", Tag = "0x1311200000040000" });
                    RelatedTargetTypeComboBox.Items.Add(new ComboBoxItem { Content = "DCLineSegment", Tag = "0x1311110000030000" });
                    break;

                case "PerLengthImpedance": 
                    RelatedTargetTypeComboBox.Items.Add(new ComboBoxItem { Content = "PerLengthPhaseImpedance", Tag = "0x1110000000070000" });
                    RelatedTargetTypeComboBox.Items.Add(new ComboBoxItem { Content = "PerLengthSequenceImpedance", Tag = "0x1111000000070000" }); // You may need to update this tag
                    break;

                case "PhaseImpedanceDatas": 
                    RelatedTargetTypeComboBox.Items.Add(new ComboBoxItem { Content = "PhaseImpedanceData", Tag = "0x1200000000010000" });
                    break;

                case "PerLengthPhaseImpedance": 
                    RelatedTargetTypeComboBox.Items.Add(new ComboBoxItem { Content = "PerLengthPhaseImpedance", Tag = "0x1110000000070000" });
                    break;
            }

            RelatedTargetTypeComboBox.IsEnabled = RelatedTargetTypeComboBox.Items.Count > 0;
            GetRelatedValuesButton.IsEnabled = true;

            if (RelatedTargetTypeComboBox.Items.Count > 0)
            {
                RelatedTargetTypeComboBox.SelectedIndex = 0;
            }
        }

        private void GetRelatedValues_Click(object sender, RoutedEventArgs e)
        {
            if (SourceEntityTypeComboBox.SelectedItem == null)
            {
                RelatedValuesStatusLabel.Content = "Please select a source entity type";
                RelatedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Orange);
                RelatedXmlContentTextBlock.Text = "Error: Please select a source entity type";
                return;
            }
            if (string.IsNullOrEmpty(RelatedPositionTextBox.Text))
            {
                RelatedValuesStatusLabel.Content = "Please enter a position";
                RelatedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Orange);
                RelatedXmlContentTextBlock.Text = "Error: Please enter a position";
                return;
            }
            if (RelatedPropertyComboBox.SelectedItem == null)
            {
                RelatedValuesStatusLabel.Content = "Please select a property";
                RelatedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Orange);
                RelatedXmlContentTextBlock.Text = "Error: Please select a property";
                return;
            }

            TestGda tgda = new TestGda();
            try
            {
                // Build GID from entity type + position
                string entityTypeTag = ((ComboBoxItem)SourceEntityTypeComboBox.SelectedItem).Tag.ToString();
                string entityTypeName = ((ComboBoxItem)SourceEntityTypeComboBox.SelectedItem).Content.ToString();
                string position = RelatedPositionTextBox.Text.Trim().PadLeft(2, '0');
                string baseHex = entityTypeTag.Substring(2);
                string fullGidHex = $"0x{baseHex}{position}";
                long sourceGid = InputGlobalId(fullGidHex);

                // Get property ID
                string propertyIdString = ((ComboBoxItem)RelatedPropertyComboBox.SelectedItem).Tag.ToString();
                long propertyIdLong = Convert.ToInt64(propertyIdString.Substring(2), 16);
                ModelCode propertyId = (ModelCode)propertyIdLong;

                // Get target type
                ModelCode targetType = 0;
                if (RelatedTargetTypeComboBox.SelectedItem != null)
                {
                    string targetTypeString = ((ComboBoxItem)RelatedTargetTypeComboBox.SelectedItem).Tag.ToString();
                    long targetTypeLong = Convert.ToInt64(targetTypeString.Substring(2), 16);
                    targetType = (ModelCode)targetTypeLong;
                }

                // Create Association (determine if inverse based on relationship direction)
                bool isInverse = DetermineIfInverse(entityTypeName, ((ComboBoxItem)RelatedPropertyComboBox.SelectedItem).Content.ToString());
                Association association = new Association()
                {
                    PropertyId = propertyId,
                    Type = targetType,
                    Inverse = isInverse
                };

                // Call the modified GetRelatedValues method that returns both IDs and XML content
                var (result, xmlContent) = tgda.GetRelatedValues(sourceGid, association);

                // Update the UI with success status
                RelatedValuesStatusLabel.Content = $"Found {result.Count} related resources for {entityTypeName} position {position}";
                RelatedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Green);

                // Display the XML content in the ScrollViewer
                RelatedXmlContentTextBlock.Text = xmlContent;
            }
            catch (Exception ex)
            {
                RelatedValuesStatusLabel.Content = $"Error: {ex.Message}";
                RelatedValuesStatusLabel.Foreground = new SolidColorBrush(Colors.Red);

                // Show error message in the XML content area as well
                RelatedXmlContentTextBlock.Text = $"Error occurred: {ex.Message}";
            }
        }

        private bool DetermineIfInverse(string sourceEntityType, string propertyName)
        {
            switch (sourceEntityType)
            {
                case "Terminal":
                    return propertyName == "ConductingEquipment" ? false : true;
                case "ACLineSegment":
                    return propertyName == "PerLengthImpedance" ? false : true;
                case "PerLengthPhaseImpedance":
                    return propertyName == "PhaseImpedanceDatas" ? false : true;
                case "PhaseImpedanceData":
                    return propertyName == "PerLengthPhaseImpedance" ? false : true;
                default:
                    return false;
            }
        }
    }
}