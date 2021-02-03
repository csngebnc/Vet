import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class TreatmentService {
  baseUrl: string = 'https://localhost:44345/api/'

  constructor(private http: HttpClient) { }

  addTreatment(model: any){
    return this.http.post(this.baseUrl + 'treatment', model);
  }

  updateTreatment(model: any){
    return this.http.put(this.baseUrl + 'treatment', model);
  }

  deleteTreatment(id){
    return this.http.delete(this.baseUrl + 'treatment/delete/' + id);
  }

  changeState(id){
    return this.http.put(this.baseUrl + 'treatment/state/' + id, null);
  }

  getTreatmentById(id){
    return this.http.get(this.baseUrl + 'treatment/treatments/' + id);
  }

  getTreatmentByDoctorId(id){
    return this.http.get(this.baseUrl + 'treatment/doctor/' + id);
  }

  getOwnTreatments(){
    return this.http.get(this.baseUrl + 'treatment/my-treatments');
  }
}
