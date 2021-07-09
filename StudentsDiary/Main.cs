using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class Main : Form
    {
        // DELEGATA
        private delegate void DisplayMessage(string message);



        // tak by wyglądało wywołanie zwykłej klasy FileHelper
        //private FileHelper _fileHelper = new FileHelper(_filePath);
        // aby to zadziałało dla klasy generycznej musimy powiedziec na jakim typie chcemy pracować:
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        public Main()
        {
            // Aby ukryć pierwszą kolumnę ze wskaźnikiem rekordu
            // ustawiamy
            // dgvDiary.RowHeadersVisible = false;
            // żeby kolumny zajmowały całe okno, ustawiamy
            // dgvDiary.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;


            // klasa List<> także jest typem generycznym np:
            //var list1 = new List<string>();
            //var list2 = new List<int>();
            //var list3 = new List<Student>();

            InitializeComponent();

            SetComboBoxFilter();

            RefreshDiary();
            SetColumnsHeader();
        }

        private void RefreshDiary()
        {
            int idx = cbFilter.SelectedIndex;

            // Wszyscy
            if (idx == 0)
            {
                var students = _fileHelper.DeserializeFromFile();
                /*
                dgvDiary.DataSource = students;
                */
                dgvDiary.DataSource = students.OrderBy(student => student.Lastname + student.FirstName).ToList();

            }
            else
            {
                string activeCBItem = cbFilter.Items[idx].ToString();

                var students = _fileHelper.DeserializeFromFile();

                dgvDiary.DataSource = students.Where(student => student.GroupID == activeCBItem).OrderBy(student => student.Lastname + student.FirstName).ToList();

                // Pisanie zapytań w języku C# (LINQ)
                // https://docs.microsoft.com/pl-pl/dotnet/csharp/programming-guide/concepts/linq/walkthrough-writing-queries-linq
                //var list2 = (from x in students
                //             where x.GroupID == activeCBItem
                //             orderby x.Lastname, x.FirstName
                //             select x).ToList();
                //dgvDiary.DataSource = list2;
            }
        }

        private void SetColumnsHeader()
        {
            // zmieniamy nagłówki kolumn w Grid
            dgvDiary.Columns[0].HeaderText = "Numer";
            dgvDiary.Columns[1].HeaderText = "Imię";
            dgvDiary.Columns[2].HeaderText = "Nazwisko";
            dgvDiary.Columns[3].HeaderText = "Uwagi";
            dgvDiary.Columns[4].HeaderText = "Matematyka";
            dgvDiary.Columns[5].HeaderText = "Technologia";
            dgvDiary.Columns[6].HeaderText = "Fizyka";
            dgvDiary.Columns[7].HeaderText = "Język polski";
            dgvDiary.Columns[8].HeaderText = "Język obcy";
            dgvDiary.Columns[9].HeaderText = "Zajęcia dodatkowe";
            dgvDiary.Columns[10].HeaderText = "Grupa";
        }
        
        private void btnAdd_Click(object sender, EventArgs e)
        {
            // [1]
            var addEditStudent = new AddEditStudent();
            addEditStudent.FormClosing += AddEditStudent_FormClosing;

            addEditStudent.ShowDialog();
        }

        private void AddEditStudent_FormClosing(object sender, FormClosingEventArgs e)
        {
            // [2]
            RefreshDiary();
        }

        private void AddEditStudent_StudentAdded()
        {
            // throw new NotImplementedException();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            // sprawdzamy czy jakiś wiersz został zaznaczony
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz dane ucznia którego dane chcesz edytować");
                return;
            }

            int ARow = dgvDiary.CurrentCell.RowIndex;

            // daj pierwszy zaznaczony wiersz, a zniego wez z pierwszej kolumny wartość (tutaj ID)
            var addEditStudent = new AddEditStudent(
                Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));

            //var addEditStudent = new AddEditStudent();
            addEditStudent.FormClosing += AddEditStudent_FormClosing;


            addEditStudent.ShowDialog();

            dgvDiary.Rows[0].Selected = false;
            dgvDiary.Rows[ARow].Selected = true;
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            // sprawdzamy czy jakiś wiersz został zaznaczony
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz dane ucznia którego dane chcesz usunąć");
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];

            // interpolacja stringu
            var confirmDelete = MessageBox.Show($"Czy na pewno chesz usunąć ucznia {(selectedStudent.Cells[1].Value.ToString() + " " +  selectedStudent.Cells[2].Value.ToString()).Trim()}", 
                                "Usuwanie ucznia", 
                                 MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                DeleteStudent(Convert.ToInt32(selectedStudent.Cells[0].Value));
                RefreshDiary();
            }
        }

        private void DeleteStudent(int id)
        {
            var students = _fileHelper.DeserializeFromFile();
            students.RemoveAll(x => x.Id == id);
            _fileHelper.SerializeToFile(students);
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void btnAdd_MouseEnter(object sender, EventArgs e)
        {
            //MessageBox.Show("Mouse Enter");
        }


        public void SetComboBoxFilter()
        {
            // wypełnij ComboBox
            Groups groups = new Groups();

            cbFilter.Items.Clear();
            // na pierwszej pozycji "Wszyscy"
            cbFilter.Items.Add("Wszyscy");
            foreach (string el in groups.groupList)
            {
                cbFilter.Items.Add(el);
            }
            cbFilter.SelectedIndex = 0; // "Wszyscy"
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            RefreshDiary();
        }

        private void cbFilter_KeyDown(object sender, KeyEventArgs e)
        {
            /*
            if (e.KeyCode == Keys.Enter)
            {
                RefreshDiary();
            }
            */
        }
    }
}
