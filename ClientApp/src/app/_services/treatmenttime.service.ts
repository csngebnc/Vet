import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class TreatmenttimeService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  addTreatmentTime(model: any){
    return this.http.post(this.baseUrl + 'treatmenttime', model);
  }

  updateTreatmentTime(model: any){
    return this.http.put(this.baseUrl + 'treatmenttime', model);
  }

  deleteTreatmentTime(id){
    return this.http.delete(this.baseUrl + 'treatmenttime/' + id);
  }

  changeState(id){
    return this.http.put(this.baseUrl + 'treatmenttime/state/' + id, null);
  }

  getTreatmentTimeById(id){
    return this.http.get(this.baseUrl + 'treatmenttime/' + id);
  }

  getTreatmentTimeByTreatmentId(id){
    return this.http.get(this.baseUrl + 'treatmenttime/treatment/' + id);
  }
}
