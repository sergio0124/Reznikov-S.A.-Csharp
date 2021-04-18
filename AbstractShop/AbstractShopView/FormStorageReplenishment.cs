using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.BusinessLogic;
using LawFirmBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Unity;

namespace LawFirmView
{
    public partial class FormStorageReplenishment : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }

        public int BlankId
        {
            get
            {
                return Convert.ToInt32(comboBoxBlank.SelectedValue);
            }
            set
            {
                comboBoxBlank.SelectedValue = value;
            }
        }

        public int Storage
        {
            get
            {
                return Convert.ToInt32(comboBoxBlank.SelectedValue);
            }
            set
            {
                comboBoxBlank.SelectedValue = value;
            }
        }

        public int Count
        {
            get
            {
                return Convert.ToInt32(textBoxCount.Text);
            }
            set
            {
                textBoxCount.Text = value.ToString();
            }
        }

        private readonly StorageLogic _storageLogic;

        public FormStorageReplenishment(BlankLogic blankLogic, StorageLogic storageLogic)
        {
            InitializeComponent();

            _storageLogic = storageLogic;

            List<BlankViewModel> listBlanks = blankLogic.Read(null);

            if (listBlanks != null)
            {
                comboBoxBlank.DisplayMember = "BlankName";
                comboBoxBlank.ValueMember = "Id";
                comboBoxBlank.DataSource = listBlanks;
                comboBoxBlank.SelectedItem = null;
            }

            List<StorageViewModel> listStorages = storageLogic.Read(null);

            if (listStorages != null)
            {
                comboBoxName.DisplayMember = "StorageName";
                comboBoxName.ValueMember = "Id";
                comboBoxName.DataSource = listStorages;
                comboBoxName.SelectedItem = null;
            }
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (comboBoxName.SelectedValue == null)
            {
                MessageBox.Show("Выберите склад", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (comboBoxBlank.SelectedValue == null)
            {
                MessageBox.Show("Выберите бланк", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Неизвестное количество", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _storageLogic.Replenishment(new ReplenishStorageBindingModel
            {
                StorageId = Convert.ToInt32(comboBoxName.SelectedValue),
                BlankId = Convert.ToInt32(comboBoxBlank.SelectedValue),
                Count = Convert.ToInt32(textBoxCount.Text)
            });

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
