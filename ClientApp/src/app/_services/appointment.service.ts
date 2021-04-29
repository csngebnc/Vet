import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  bookAppointment(model: FormData) {
    return this.http.post(this.baseUrl + 'appointments', model);
  }

  getMyAppointments() {
    return this.http.get(this.baseUrl + 'appointments/my-appointments');
  }

  resignAppointment(id) {
    return this.http.put(this.baseUrl + 'appointments/resign/' + id, null);
  }

  resignAppointmentByDoctor(id) {
    return this.http.put(this.baseUrl + 'appointments/resign-by-doctor/' + id, null);
  }

  doctorActiveAppointments(id) {
    return this.http.get(this.baseUrl + 'appointments/doctor-active-appointments/' + id);
  }

  doctorInactiveAppointments(id) {
    return this.http.get(this.baseUrl + 'appointments/doctor-inactive-appointments/' + id);
  }

  getDoctorReservedTimes(id, time) {
    let params = new HttpParams()
      .set('time', time.toLocaleString())
      .set('doctorId', id);

    return this.http.get(this.baseUrl + 'appointments/getreserved', { params });
  }

}
