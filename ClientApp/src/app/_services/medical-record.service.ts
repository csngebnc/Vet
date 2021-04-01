import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class MedicalRecordService {

  baseUrl: string = 'https://localhost:44345/api/'

  constructor(private http: HttpClient) { }

  addMedicalRecord(model: any) {
    return this.http.post(this.baseUrl + 'records', model);
  }

  updateMedicalRecord(model: any) {
    return this.http.put(this.baseUrl + 'records', model);
  }

  deleteMedicalRecord(id) {
    return this.http.delete(this.baseUrl + 'records/' + id);
  }

  getMedicalRecords() {
    return this.http.get(this.baseUrl + 'records');
  }

  getMedicalRecordById(id) {
    return this.http.get(this.baseUrl + 'records/id/' + id);
  }

  getMedicalRecordByAnimalId(id) {
    return this.http.get(this.baseUrl + 'records/animalid/' + id);
  }

  getMedicalRecordByEmail(email) {
    return this.http.get(this.baseUrl + 'records/email/' + email);
  }

  getMedicalRecordByUserId(id) {
    return this.http.get(this.baseUrl + 'records/userid/' + id);
  }

  getMyMedicalRecords() {
    return this.http.get(this.baseUrl + 'records/my-records');
  }

  deletePhoto(id) {
    return this.http.delete(this.baseUrl + 'records/delete-photo/' + id)
  }

  removeTherapiaFromMedicalRecord(id) {
    return this.http.delete(this.baseUrl + 'records/therapia/' + id)
  }
}
