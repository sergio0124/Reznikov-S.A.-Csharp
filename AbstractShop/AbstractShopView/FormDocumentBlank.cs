using LawFirmBusinessLogic.BusinessLogic;
using LawFirmBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
namespace LawFirmView
{
    public partial class FormDocumentBlank : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int Id
        {
            get { return Convert.ToInt32(comboBoxBlank.SelectedValue); }
            set { comboBoxBlank.SelectedValue = value; }
        }
        public string BlankName { get { return comboBoxBlank.Text; } }
        public int Count
        {
            get { return Convert.ToInt32(textBoxCount.Text); }
            set
            {
                textBoxCount.Text = value.ToString();
            }
        }
        public FormDocumentBlank(BlankLogic logic)
        {
            InitializeComponent();
            List<BlankViewModel> list = logic.Read(null);
            if (list != null)
            {
                comboBoxBlank.DisplayMember = "BlankName";
                comboBoxBlank.ValueMember = "Id";
                comboBoxBlank.DataSource = list;
                comboBoxBlank.SelectedItem = null;
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxBlank.SelectedValue == null)
            {
                MessageBox.Show("Выберите бланк", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
                
            }
            DialogResult = DialogResult.OK;
            Close();
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}