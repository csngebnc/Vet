import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'src/environments/environment';

@Injectable({
  providedIn: 'root'
})
export class MedicalRecordService {

  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private toasr: ToastrService) { }

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

  generatePdf(id) {
    this.toasr.warning("A pdf készítése elkeződött, hamarosan elkészül.", "PDF készítése");
    return this.http.get(this.baseUrl + 'records/pdf/' + id, { responseType: 'blob' });
  }
}
