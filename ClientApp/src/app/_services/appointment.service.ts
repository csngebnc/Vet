import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class AppointmentService {
  baseUrl: string = 'https://localhost:44345/api/'

  constructor(private http: HttpClient) { }

  bookAppointment(model: FormData){
    return this.http.post(this.baseUrl + 'appointments', model);
  }

  getMyAppointments(){
    return this.http.get(this.baseUrl + 'appointments/my-appointments');
  }

  resignAppointment(id){
    return this.http.put(this.baseUrl + 'appointments/resign/'+id, null);
  }
}
