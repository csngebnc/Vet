import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class DoctorService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getDoctors(){
    return this.http.get(this.baseUrl + 'doctors');
  }

  promoteDoctor(email: string){
    return this.http.post(this.baseUrl + 'doctors/promote/' + email, null);
  }

  demoteDoctor(id: string){
    return this.http.post(this.baseUrl + 'doctors/demote/' + id, null);
  }

  addHoliday(model: FormData){
    return this.http.post(this.baseUrl + 'doctors/holiday/add', model);
  }

  updateHoliday(model){
    return this.http.put(this.baseUrl + 'doctors/holiday/update', model);
  }

  deleteHoliday(id){
    return this.http.delete(this.baseUrl + 'doctors/holiday/delete/' + id);
  }

  getHolidayById(id){
    return this.http.get(this.baseUrl + 'doctors/holiday/get/' + id)
  }

  getOwnHolidays(){
    return this.http.get(this.baseUrl + 'doctors/holiday');
  }

  getDoctorHolidays(id){
    return this.http.get(this.baseUrl + 'doctors/' + id + '/holiday');
  }
}
