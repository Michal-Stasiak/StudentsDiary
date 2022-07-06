using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace StudentsDiary
{
    public partial class Main : Form
    {

        private string _filePath = $@"{Environment.CurrentDirectory}\students.txt";

        public Main()
        {


            InitializeComponent();
            var students = DeserializeFromFile();
            dgvDiary.DataSource = students;

            /* var path = $@"{Path.GetDirectoryName(Application.ExecutablePath)}\NowyPlik2.txt";

             if (!File.Exists(path))
             {
                 System.IO.File.Create(path);
             }*/

            //File.WriteAllText(path, "Zostanę programistą!");
            /*  File.AppendAllText(path, "\nJuż niedłgo");

              var text = File.ReadAllText(path);

              MessageBox.Show(text);
              MessageBox.Show("Test", "Tytul", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);*/

            /*  var students = new List<Student>();
              students.Add(new Student() { FirstName = "Jan"});
              students.Add(new Student() { FirstName = "Marek" });
              students.Add(new Student() { FirstName = "Jarek" });

              SerializedToFile(students);*/

          /*  var students = DeserializeFromFile();

            foreach (var item in students)
            {
                MessageBox.Show(item.FirstName);
            }*/

        }

        public void SerializedToFile(List<Student> students)
        {
            var serializer = new XmlSerializer(typeof(List<Student>));

            using (var streamWriter = new StreamWriter(_filePath))
            {
                serializer.Serialize(streamWriter, students);
                streamWriter.Close();
            }

        }

        public List<Student> DeserializeFromFile()
        {
            if (!File.Exists(_filePath))
                return new List<Student>();
            

            var serializer = new XmlSerializer(typeof(List<Student>));

            using (var streamReader = new StreamReader(_filePath))
            {
                var students = (List<Student>)serializer.Deserialize(streamReader);
                streamReader.Close();   
                return students;
            }

        }

        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            var addEditStudent = new AddEditStudent();
            addEditStudent.ShowDialog();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zaznacz ucznia, którego dane chcesz edytować.");
                return;
            }

            var addEditStudent = new AddEditStudent(Convert.ToInt32(dgvDiary.SelectedRows[0].Cells[0].Value));
            addEditStudent.ShowDialog();

        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (dgvDiary.SelectedRows.Count == 0)
            {
                MessageBox.Show("Proszę zanzacz ucznia, którego chcesz usunąć.");
                return;
            }

            var selectedStudent = dgvDiary.SelectedRows[0];

            var confirmDelete = MessageBox.Show($"Czy napewno chcesz usunąć ucznia {(selectedStudent.Cells[1].Value.ToString() + " " + selectedStudent.Cells[2].Value.ToString()).Trim()}", "Usuwanie ucznia", MessageBoxButtons.OKCancel);

            if (confirmDelete == DialogResult.OK)
            {
                var students = DeserializeFromFile();
                students.RemoveAll(x => x.Id == Convert.ToInt32(selectedStudent.Cells[0].Value));
                SerializedToFile(students);
                dgvDiary.DataSource = students;
            }
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            var students = DeserializeFromFile();
            dgvDiary.DataSource = students;
        }
    }
}
