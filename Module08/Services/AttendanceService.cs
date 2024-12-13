﻿using Module08.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace Module08.Services
{
    public class AttendanceService
    {
        private readonly HttpClient _httpClient;
        private const string BaseUrl = "http://localhost/pdc50/";

        public AttendanceService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<Attendance>> GetStudentAttendanceAsync(string studentId)
        {
            var response = await _httpClient.GetFromJsonAsync<List<Attendance>>($"{BaseUrl}get_attendance.php?studentId={studentId}");
            return response ?? new List<Attendance>();
        }

        public async Task<string> AddAttendanceAsync(Attendance attendance)
        {
            var response = await _httpClient.PostAsJsonAsync($"{BaseUrl}add_attendance.php", attendance);
            return await response.Content.ReadAsStringAsync();
        }
    }
}
