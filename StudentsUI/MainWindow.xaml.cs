using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Net.Http;
using System.Threading;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Data;
using Newtonsoft.Json.Linq;
using static System.Net.WebRequestMethods;
using Newtonsoft.Json;
using System.Net.Http.Json;
using System.Security.Policy;

namespace StudentsUI
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    /// 
    public class Table
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Patronymic { get; set; }
        public string DateExam { get; set; }  
        public int IdSubject { get; set; }
        public int Grade { get; set; }
    }


    public partial class MainWindow : Window
    {
        static string apiUrl = "http://localhost:5018/api/Students/GetAllStudents";
        Table student = new Table();

        public MainWindow()
        {
            InitializeComponent();
            GetTable(apiUrl);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {     
            GetTable(apiUrl);
        }
        private void Subject_Click(object sender, RoutedEventArgs e)
        {
            string Url = "http://localhost:5018/api/Students/GetSubID?subID=" + SubjectIdText.Text;
            GetTable(Url);           
        }
        private void UserId_Click(object sender, RoutedEventArgs e)
        {
            string Url = "http://localhost:5018/api/Students/GetStudentID?studID=" + UserIdText.Text;
            GetTable(Url);
        }
        private void GetTable(string apiUrl)//метод возвращающий таблицу
        {
           
            using (HttpClient client = new HttpClient())
            {
                //делаем запрос к эндпоинту
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(apiUrl);
                request.Method = "GET";

                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        Stream receiveStream = response.GetResponseStream();
                        StreamReader readStream;

                        if (response.CharacterSet == null)
                            readStream = new StreamReader(receiveStream);
                        else
                            readStream = new StreamReader(receiveStream, Encoding.GetEncoding(response.CharacterSet));
                        //получаем строку ответа в формате json 
                        string responseBody = readStream.ReadToEnd();
                        //приводим к нормальному виду json строку
                        JArray arr = JArray.Parse(responseBody);
                        List<Table> students = new List<Table>();

                        //разделяем значения и добавляем в список students
                        foreach (JObject e in arr.Cast<JObject>())
                        {
                            
                            students.Add( new Table() { 
                                ID=Convert.ToInt32(e["id"]),
                                Name = e["name"].ToString(),
                                Surname = e["surname"].ToString(),
                                Patronymic = e["patronymic"].ToString(),
                                DateExam = e["dateExam"].ToString(),
                                IdSubject = Convert.ToInt32(e["idSubject"]),
                                Grade = Convert.ToInt32(e["grade"])
                            });
                        }
                        myDataGrid.ItemsSource = students;

                    }
                }
            }
        }
        private void DeleteStudent_Click(object sender, RoutedEventArgs e)
        {
            string Url = "http://localhost:5018/api/Students/RemoveStudent?id=" + delText.Text;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Url);
            request.Method = "DELETE";
            using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
            {
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    MessageBox.Show("OK");
                }
                else
                {
                    MessageBox.Show("SomethingWrong");
                }
            }
        }
        private async  void PostStudent_Click(object sender, RoutedEventArgs e)
        {
            //cоздаем уникального пользователя и ассоциируем его с таблицей
            student.Name = nameText.Text;
            student.Surname = surnameText.Text;
            student.Patronymic = patronymicText.Text;
            student.DateExam = dateText.Text;
            student.IdSubject = Convert.ToInt32(subIdText.Text);
            student.Grade = Convert.ToInt32(gradeText.Text);

            //загоняем данные под json формат
            JsonContent content = JsonContent.Create(student);

            // отправляем запрос
            using (var client = new HttpClient())
            {
                var response = await client.PostAsync("http://localhost:5018/api/Students/AddStudent", content);
                if(response.StatusCode == HttpStatusCode.OK)
                {
                    MessageBox.Show("OK");
                }
                else
                {
                    MessageBox.Show("SomethingWrong");
                }
            }              

        }
        
    }
}
