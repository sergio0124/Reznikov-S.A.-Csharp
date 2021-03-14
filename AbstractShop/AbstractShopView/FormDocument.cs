using LawFirmBusinessLogic.BindingModels;
using LawFirmBusinessLogic.BusinessLogic;
using LawFirmBusinessLogic.ViewModels;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using Unity;
namespace LawFirmView
{
    public partial class FormDocument : Form
    {
        [Dependency]
        public new IUnityContainer Container { get; set; }
        public int Id { set { id = value; } }
        private readonly DocumentLogic logic;
        private int? id;
        private Dictionary<int, (string, int)> documentBlanks;
        public FormDocument(DocumentLogic service)
        {
            InitializeComponent();
            this.logic = service;
        }
        private void FormProduct_Load(object sender, EventArgs e)
        {
            if (id.HasValue)
            {
                try
                {

                    DocumentViewModel view = logic.Read(new DocumentBindingModel
                    {
                        Id =
    id.Value
                    })?[0];
                    if (view != null)
                    {
                        textBoxName.Text = view.DocumentName;
                        textBoxPrice.Text = view.Price.ToString();
                        documentBlanks = view.DocumentBlanks;
                        LoadData();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                   MessageBoxIcon.Error);
                }
            }
            else
            {
                documentBlanks = new Dictionary<int, (string, int)>();
            }
        }
        private void LoadData()
        {
            try
            {
                if (documentBlanks != null)
                {

                    dataGridView.Rows.Clear();
                    foreach (var db in documentBlanks)
                    {
                        dataGridView.Rows.Add(new object[] { db.Key, db.Value.Item1,
db.Value.Item2 });
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
            }
        }
        private void buttonAdd_Click(object sender, EventArgs e)
        {
            var form = Container.Resolve<FormDocumentBlank>();
            if (form.ShowDialog() == DialogResult.OK)
            {
                if (documentBlanks.ContainsKey(form.Id))
                {
                    documentBlanks[form.Id] = (form.BlankName, form.Count);
                }
                else
                {
                    documentBlanks.Add(form.Id, (form.BlankName, form.Count));
                }
                LoadData();
            }
        }
        private void buttonUpd_Click(object sender, EventArgs e)
        {
            LoadData();
        }
        private void buttonDel_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {
                if (MessageBox.Show("Удалить запись", "Вопрос", MessageBoxButtons.YesNo,
               MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {

                        documentBlanks.Remove(Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value));
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message, "Ошибка", MessageBoxButtons.OK,
                       MessageBoxIcon.Error);
                    }
                    LoadData();
                }
            }
        }
        private void buttonRef_Click(object sender, EventArgs e)
        {
            if (dataGridView.SelectedRows.Count == 1)
            {

                var form = Container.Resolve<FormDocumentBlank>();
                int id = Convert.ToInt32(dataGridView.SelectedRows[0].Cells[0].Value);
                form.Id = id;
                form.Count = documentBlanks[id].Item2;
                if (form.ShowDialog() == DialogResult.OK)
                {
                    documentBlanks[form.Id] = (form.BlankName, form.Count);
                    LoadData();
                }
            }
        }
        private void buttonSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBoxName.Text))
            {
                MessageBox.Show("Заполните название", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (string.IsNullOrEmpty(textBoxPrice.Text))
            {
                MessageBox.Show("Заполните цену", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            if (documentBlanks == null || documentBlanks.Count == 0)
            {
                MessageBox.Show("Заполните бланки", "Ошибка", MessageBoxButtons.OK,
               MessageBoxIcon.Error);
                return;
            }
            try
            {
                logic.CreateOrUpdate(new DocumentBindingModel
                {
                    Id = id,

                    DocumentName = textBoxName.Text,
                    Price = Convert.ToDecimal(textBoxPrice.Text),
                    DocumentBlanks = documentBlanks
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

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            LoadData();
        }
    }
}
