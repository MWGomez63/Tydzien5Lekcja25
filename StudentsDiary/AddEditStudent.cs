using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {

        private int _studentId;
        private Student _student;
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        public AddEditStudent(int id = 0)
        {
            InitializeComponent();

            _studentId = id;

            GetStudentData();
            
            // wypełnij ComboBox
            Groups groups = new Groups();
            cbGroupID.Items.Clear();
            foreach(string el in groups.groupList)
            {
                cbGroupID.Items.Add(el);
            }

            // Insert`
            if (_studentId == 0)
            {
                tbId.Enabled = false;
                cbGroupID.SelectedIndex = -1;
            }
            else
            {
                string activeCBItem = _student.GroupID;

                int idx = cbGroupID.FindString(activeCBItem);
                cbGroupID.SelectedIndex = idx;
            }

            // aktywna kontrolka Imie
            tbFirstname.Select();
        }


        private void GetStudentData()
        {
            if (_studentId != 0)
            {
                // uwaga Text pochodzi z klasy Form
                Text = "Edytowanie danych ucznia";

                var students = _fileHelper.DeserializeFromFile();
                // LINQ, pobierz mi pierwszego ucznia z listy który ma takie id
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                {
                    throw new Exception("Brak użytkownika o podanym Id");
                }

                FillTextBoxes();
            }
        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstname.Text = _student.FirstName;
            tbLastname.Text = _student.Lastname;
            rtbComments.Text = _student.Comments;
            tbMath.Text = _student.Math;
            tbTechnology.Text = _student.Technology;
            tbPhysics.Text = _student.Physics;
            tbPolishLang.Text = _student.PolishLang;
            tbForeignLang.Text = _student.ForeignLang;

            // ChceckBox
            if (_student.AdditionalActivities)
                cbAA.Checked = true;
            else
                cbAA.Checked = false;

            // ComboBox
            string activeCBItem = _student.GroupID;
            int idx = cbGroupID.FindString(activeCBItem);
            cbGroupID.SelectedIndex = idx;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            // edycja
            if (_studentId != 0)
                // usuwamy tego ucznia z listy
                students.RemoveAll(x => x.Id == _studentId);
            else
                AssignIdToNewStudent(students);

            // tego mi brakowało
            AddNewUserToList(students);

            _fileHelper.SerializeToFile(students);

            Close();
        }

        private void AddNewUserToList(List<Student> students)
        {
            int idx = cbGroupID.SelectedIndex;

            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstname.Text,
                Lastname = tbLastname.Text,
                Comments = rtbComments.Text,
                Math = tbMath.Text,
                Technology = tbTechnology.Text,
                Physics = tbPhysics.Text,
                PolishLang = tbPolishLang.Text,
                ForeignLang = tbForeignLang.Text,
                AdditionalActivities = cbAA.Checked ? true : false,
                GroupID = cbGroupID.Items[idx].ToString()
            };

            students.Add(student);
        }

        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHeighestId = students.OrderByDescending(x => x.Id).FirstOrDefault();
            _studentId = studentWithHeighestId == null ? 1 : studentWithHeighestId.Id + 1;
        }
    }
}
