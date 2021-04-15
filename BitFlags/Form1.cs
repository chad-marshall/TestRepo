using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Linq;


namespace BitFlags
{
    public partial class Form1 : Form
    {
        private BitFlagConfiguration _config = null;
        private Dictionary<int, string> _statusCodes = new Dictionary<int, string>();
        private BitFlagGroup _currentGroup = null;
        private List<DataSetItem> _gridItems = new List<DataSetItem>();
        public Form1()
        {
            InitializeComponent();

            try
            {
                _config = ConfigurationManager.GetSection("BitFlagConfiguration") as BitFlagConfiguration;
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Configuration file is not valid.  Program will now exit.\n\nDetails:\n\n{0}", ex.Message));
                Environment.Exit(99);
            }

            //Load configured groups to the drop down
            ddlBitFlagGroups.Items.AddRange(_config.BitFlagGroups.OfType<BitFlagGroup>().Select(g => g.Name).ToArray());
            dataGrid.AutoGenerateColumns = false;
        }


        /// <summary>
        /// If the text is changed, filter to only numbers and x and enable the process button.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtValue_TextChanged(object sender, EventArgs e)
        {
            string text = new String(txtValue.Text.Where(c => c=='x' || (c >= 'A' && c <= 'F') ||( c >= '0' && c <= '9')).ToArray());

            btnProcess.Enabled = !string.IsNullOrEmpty(text);

            if (!text.Equals(txtValue.Text))
            {
                txtValue.Text = text;
                txtValue.SelectionStart = txtValue.TextLength;
            }
        }

        /// <summary>
        /// If a valid selection was chosen then set the currently selected group and enable the value entry text box.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ddlBitFlagGroups_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtValue.Enabled = (sender as ComboBox).SelectedIndex != -1;
            if (txtValue.Enabled)
            {
                _currentGroup = _config.BitFlagGroups[ddlBitFlagGroups.Text];
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            Int64 testValue = 0;
            //Global try/catch for generic exceptions.
            try
            {
                //Now try parsing the 3 different ways and catch format errors.
                try
                {
                    //Parse as decimal: 10
                    testValue = Int64.Parse(txtValue.Text);
                }
                catch (FormatException)
                {
                    try
                    {
                        //Parse as HEX without specifier: A
                        testValue = Int64.Parse(txtValue.Text, System.Globalization.NumberStyles.AllowHexSpecifier);
                    }
                    catch (FormatException)
                    {
                        //Parse as HEX with specifier: 0xA
                        testValue = (Int64)new System.ComponentModel.Int64Converter().ConvertFromString(txtValue.Text);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Value {0} was incorrect.  Please try again.\n\nDetails:\n\n{1}", txtValue.Text, ex.Message));
            }


            var usedFlags = _currentGroup.Flags.OfType<BitFlag>().Where(f => (f.Value & testValue) == f.Value);
            _gridItems.Clear();
            dataGrid.DataSource = null;
            Int64 totalVal = 0;

            if (usedFlags.Count() > 0)
            {
                usedFlags.ToList().ForEach(uf =>
                    {
                        _gridItems.Add(new DataSetItem(uf.Value, uf.Name, uf.Description));
                        totalVal += uf.Value;
                    });
            }

            if (totalVal != testValue)
            {
                _gridItems.Add(new DataSetItem(testValue - totalVal, "Unused", "This part of the input did not match any configured flag."));
            }
            dataGrid.DataSource = _gridItems;
        }

        private void dataGrid_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (e.ColumnIndex == 0 && e.Value != null)
            {
                Int64 myVal = Int64.Parse(e.Value.ToString());
                e.Value = "0x" + myVal.ToString("X8");
                e.FormattingApplied = true;
            }
        }
    }
}
