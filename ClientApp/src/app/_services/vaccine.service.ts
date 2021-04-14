import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class VaccineService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  addVaccine(model: any) {
    return this.http.post(this.baseUrl + 'vaccine', model);
  }

  updateVaccine(model: any) {
    return this.http.put(this.baseUrl + 'vaccine', model);
  }

  deleteVaccine(id) {
    return this.http.delete(this.baseUrl + 'vaccine/' + id);
  }

  getVaccines() {
    return this.http.get(this.baseUrl + 'vaccine/get');
  }

  getVaccineById(id) {
    return this.http.get(this.baseUrl + 'vaccine/get/' + id);
  }

  addVaccineRecord(model: any) {
    return this.http.post(this.baseUrl + 'vaccine/record', model);
  }

  updateVaccineRecord(model: any) {
    return this.http.put(this.baseUrl + 'vaccine/record', model);
  }

  deleteVaccineRecord(id) {
    return this.http.delete(this.baseUrl + 'vaccine/record/' + id);
  }

  getVaccineRecordsByAnimal(id) {
    return this.http.get(this.baseUrl + 'vaccine/record-by-animal/' + id);
  }

  getVaccineRecordById(id) {
    return this.http.get(this.baseUrl + 'vaccine/record-by-id/' + id);
  }



}
