using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Module08.Model;

namespace Module08.Services
{
	public class StudentService
	{
		private readonly HttpClient _httpClient;
		private const string BaseUrl = "http://localhost/pdc50/";

		public StudentService()
		{
			_httpClient = new HttpClient();
		}


        //Get
        public async Task<List<Student>> GetStudentsAsync()
        {
            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<Student>>($"{BaseUrl}get_students.php");
                if (response != null)
                {
                    return response;
                }
                return new List<Student>();
            }
            catch (Exception)
            {
                return new List<Student>();
            }
        }


        //Add
        public async Task<string> AddStudentAsync(Student student)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}add_student.php", new
                {
                    studentID = student.StudentID,
                    fullName = student.FullName,
                    gradeClass = student.GradeClass,
                    contactNo = student.ContactNo,
                    dateOfBirth = student.DateOfBirth,
                    gender = student.Gender,
                    address = student.Address,
                    email = student.Email,
                    emergencyContact = student.EmergencyContact,
                    status = student.Status
                });

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                return $"Error: {ex.Message}";
            }
        }

        //Update
        public async Task<string> UpdateStudentAsync(Student student)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}update_student.php", student);
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }

        //Delete
        public async Task<string> DeleteStudentAsync(string studentId)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}delete_student.php", new { studentID = studentId });
            var result = await response.Content.ReadAsStringAsync();
            return result;
        }
    }
}