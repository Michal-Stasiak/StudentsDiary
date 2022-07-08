using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace StudentsDiary
{
    public partial class AddEditStudent : Form
    {
        private FileHelper<List<Student>> _fileHelper = new FileHelper<List<Student>>(Program.FilePath);

        private int _studentId;
        private Student _student;

        public AddEditStudent(int id = 0)
        {
            InitializeComponent();
            _studentId = id;

            GetStudentData();
            tbFirstName.Select();


        }

        private void GetStudentData()
        {
            if(_studentId != 0)
            {
                Text = "Edytowanie danych ucznia";
                var students = _fileHelper.DeserializeFromFile();
                _student = students.FirstOrDefault(x => x.Id == _studentId);

                if (_student == null)
                {
                    throw new Exception("Brak użytkowanika od podanym id");
                }

                FillTextBoxes();


            }
        }

        private void FillTextBoxes()
        {
            tbId.Text = _student.Id.ToString();
            tbFirstName.Text = _student.FirstName;
            tbLastName.Text = _student.LastName;
            tbMaths.Text = _student.Maths;
            tbBiology.Text = _student.Biology;
            tbPhysics.Text = _student.Physics;
            tbPolishLang.Text = _student.PolishLang;
            tbForeignLang.Text = _student.ForeignLang;
            rtxComments.Text = _student.Comments;
        }

      

        private void btnCancel_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnConfirm_Click(object sender, EventArgs e)
        {
            var students = _fileHelper.DeserializeFromFile();

            if (_studentId != 0)
            {
                students.RemoveAll(x => x.Id == _studentId);
            }
            else
            {
                AssignIdToNewStudent(students);
            }


            /*var studentId = 0;

            if (studentWithHightestId == null)
            {
                studentId = 1;
            }
            else
            {
                studentId = studentWithHightestId.Id + 1;
            }*/
            AddNewStudentToTheList(students);
            
            _fileHelper.SerializedToFile(students);

            Close();

        }

        private void AddNewStudentToTheList(List<Student> students)
        {
            var student = new Student
            {
                Id = _studentId,
                FirstName = tbFirstName.Text,
                LastName = tbLastName.Text,
                Comments = rtxComments.Text,
                Maths = tbMaths.Text,
                Biology = tbBiology.Text,
                Physics = tbPhysics.Text,
                PolishLang = tbPolishLang.Text,
                ForeignLang = tbForeignLang.Text
            };

            students.Add(student);
        }

        private void AssignIdToNewStudent(List<Student> students)
        {
            var studentWithHightestId = students.OrderByDescending(x => x.Id).FirstOrDefault();
            _studentId = studentWithHightestId == null ? 1 : studentWithHightestId.Id + 1;
        }

       
    }
}
