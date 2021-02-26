using AbstractShopBusinessLogic.BindingModels;
using AbstractShopBusinessLogic.BusinessLogic;
using AbstractShopBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
namespace AbstractShopView
{
    public partial class FormCreateOrder : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        private readonly ProductLogic _logicP;
        private readonly OrderLogic _logicO;
        public FormCreateOrder(ProductLogic logicP, OrderLogic logicO)
        {
            InitializeComponent();
            _logicP = logicP;
            _logicO = logicO;
        }
        private void FormCreateOrder_Load(object sender, EventArgs e)
        {
            try
            {
                List<ProductViewModel> list = _logicP.Read(null);
                if (list != null)
                {
                    comboBoxProduct.DisplayMember = "ProductName";
                    comboBoxProduct.ValueMember = "Id";
                    comboBoxProduct.DataSource = list;
                    comboBoxProduct.SelectedItem = null;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }
        private void CalcSum()
        {
            if (comboBoxProduct.SelectedValue != null &&
           !string.IsNullOrEmpty(textBoxCount.Text))
            {
                try
                {
                    int id = Convert.ToInt32(comboBoxProduct.SelectedValue);
                    ProductViewModel product = _logicP.Read(new ProductBindingModel
                    {
                        Id
                    = id
                    })?[0];
                    int count = Convert.ToInt32(textBoxCount.Text);
                    textBoxSum.Text = (count * product?.Price ?? 0).ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
        }
        private void TextBoxCount_TextChanged(object sender, EventArgs e)
        {
            CalcSum();
        }
        private void ComboBoxProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            CalcSum();
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxCount.Text))
            {
                MessageBox.Show("Заполните поле Количество", "Ошибка",
               MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (comboBoxProduct.SelectedValue == null)
            {
                MessageBox.Show("Выберите изделие", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            try
            {
                _logicO.CreateOrder(new CreateOrderBindingModel
                {
                    ProductId = Convert.ToInt32(comboBoxProduct.SelectedValue),
                    Count = Convert.ToInt32(textBoxCount.Text),
                    Sum = Convert.ToDecimal(textBoxSum.Text)
                });
                MessageBox.Show("Сохранение прошло успешно", "Сообщение",
               MessageBoxButtons.OK, MessageBoxIcon.Information);
                DialogResult = DialogResult.OK;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }
    }
}