﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Module08.Model;

namespace Module08.Services
{
    public class GradeService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost/pdc50/";

        public GradeService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Grade>> GetStudentGradesAsync(string studentId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Grade>>($"{BaseUrl}get_grades.php?studentId={studentId}");
            return response ?? new List<Grade>();
        }

        public async Task<string> AddGradeAsync(Grade grade)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}add_grade.php", grade);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
